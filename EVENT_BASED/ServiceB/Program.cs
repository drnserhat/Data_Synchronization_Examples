using MassTransit;
using MongoDB.Driver;
using ServiceB.Consumers;
using ServiceB.Models.Entities;
using ServiceB.Services;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ServiceB.Services.MongoDBService>();
builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<UpdatePersonNameEventConsumer>();

    cfg.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration["RabbitMQ"]);
        config.ConfigureEndpoints(context);
        config.ReceiveEndpoint(RabbitMQSettings.Person_UpdatedPersonNameEventQueue, e => e.ConfigureConsumer<UpdatePersonNameEventConsumer>(context));
    });
});

#region Harici - mongoDB veritabanýna veri ekleme
using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope();
MongoDBService mongoDBService = scope.ServiceProvider.GetService<MongoDBService>();
var collection = mongoDBService.GetCollection<Employee>();
if (!collection.Find(s => true).Any())
{
    await collection.InsertOneAsync(new() { PersonId = "68adbb4bfe64081426757132", Name = "Serhat", Department = "Software", Salary = 4000 });
    await collection.InsertOneAsync(new() { PersonId = "68adbb4cfe64081426757133", Name = "Kadir", Department = "Dentist", Salary = 5000 });
    await collection.InsertOneAsync(new() { PersonId = "68adbb4cfe64081426757134", Name = "Efraim", Department = "Doctor", Salary = 6000 });
    await collection.InsertOneAsync(new() { PersonId = "68adbb4cfe64081426757135", Name = "Metin", Department = "Backend", Salary = 7000 });
    await collection.InsertOneAsync(new() { PersonId = "68adbb4cfe64081426757136", Name = "Ozan", Department = "Nurse", Salary = 8000 });
    await collection.InsertOneAsync(new() { PersonId = "68adbb4cfe64081426757137", Name = "Yusuf", Department = "Game", Salary = 9000 });

}
#endregion

var app = builder.Build();


app.Run();
