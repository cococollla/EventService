using EventGenerator.WebApi.HostedServices;
using EventGenerator.WebApi.HttpClients.Contacts;
using EventGenerator.WebApi.HttpClients.Implementations;
using EventGenerator.WebApi.Services.Contracts;
using EventGenerator.WebApi.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IEventProcessorClient, EventProcessorClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
});

builder.Services.AddScoped<IEventGeneratorService, EventGeneratorService>();
builder.Services.AddHostedService<EventGeneratorBackgroundService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
