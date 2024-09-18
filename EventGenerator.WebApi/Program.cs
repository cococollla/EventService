using EventGenerator.WebApi.HostedServices;
using EventGenerator.WebApi.HttpClients;
using EventGenerator.WebApi.Services;

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
