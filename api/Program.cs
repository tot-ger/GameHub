using GameHub.API;
using GameHub.GomokuEngine;
using GameHub.GomokuEngine.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGomokuGameEngine();
builder.Services.AddSignalR();

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors(builder => builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(origin => true) // allow any origin
        .AllowCredentials());
}

app.MapHub<GomokuHub>("/gomokuHub");

app.MapGet("/", () => "Hello World!");

app.Run();
