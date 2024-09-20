using EventProcessor.WebApi.BackgroundServices;
using EventProcessor.WebApi.Data;
using EventProcessor.WebApi.Data.Models;
using EventProcessor.WebApi.Services.Contracts;
using EventProcessor.WebApi.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProcessorDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEventProcessorService, EventProcessorService>();
builder.Services.AddHostedService<EventProcessorBackgroundService>();

builder.Services.AddSingleton(Channel.CreateUnbounded<Event>());

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

app.UseAuthorization();

app.MapControllers();

app.Run();
