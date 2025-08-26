using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using ServiceA.Models.Entities;
using ServiceA.Services;
using Shared;
using Shared.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ServiceA.Services.MongoDBService>();
builder.Services.AddMassTransit(cfg =>
{

    cfg.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration["RabbitMQ"]);
        config.ConfigureEndpoints(context);
    });
});
#region Harici - mongoDB veritabanýna veri ekleme
using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope();
MongoDBService mongoDBService = scope.ServiceProvider.GetService<MongoDBService>();
var collection = mongoDBService.GetCollection<Person>();
if (!collection.Find(s => true).Any())
{
    await collection.InsertOneAsync(new() { Name = "Serhat" });
    await collection.InsertOneAsync(new() { Name = "Kadir" });
    await collection.InsertOneAsync(new() { Name = "Efraim" });
    await collection.InsertOneAsync(new() { Name = "Metin" });
    await collection.InsertOneAsync(new() { Name = "Ozan" });
    await collection.InsertOneAsync(new() { Name = "Yusuf" });

}
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/{id}/{newName}", async (
    [FromRoute] string id, [FromRoute] string newName, MongoDBService mongoDBService,IPublishEndpoint publishEndpoint
    ) =>
{
    var persons = mongoDBService.GetCollection<Person>();
    Person person = await (await persons.FindAsync(s => s.Id == ObjectId.Parse(id))).FirstOrDefaultAsync();
    person.Name = newName;
    await persons.FindOneAndReplaceAsync(p => p.Id == ObjectId.Parse(id), person);
   UpdatePersonNameEvent updatePersonNameEvent = new()
   {
       PersonId = person.Id.ToString(),
       NewName = newName
   };
    await publishEndpoint.Publish(updatePersonNameEvent);
  
});
app.Run();
