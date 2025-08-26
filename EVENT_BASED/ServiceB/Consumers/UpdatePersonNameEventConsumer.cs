using MassTransit;
using MongoDB.Driver;
using ServiceB.Models.Entities;
using ServiceB.Services;
using Shared.Events;
using System.Collections.Concurrent;

namespace ServiceB.Consumers
{
    public class UpdatePersonNameEventConsumer : IConsumer<UpdatePersonNameEvent>
    {
        readonly MongoDBService _mongoDBService;
        public UpdatePersonNameEventConsumer(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }
        public async Task Consume(ConsumeContext<UpdatePersonNameEvent> context)
        {
            var employees = _mongoDBService.GetCollection<Employee>();
            Employee employee = await (await employees.FindAsync(s => s.PersonId == context.Message.PersonId)).FirstOrDefaultAsync();
            employee.Name = context.Message.NewName;
            await employees.FindOneAndReplaceAsync(p => p.Id == employee.Id, employee);
        }
    }
}
