using ExcelsisDeo.Authentication;
using ExcelsisDeo.Authorization;
using ExcelsisDeo.Interfaces.Endpoints;
using ExcelsisDeo.Persistence;
using ExcelsisDeo.Persistence.Entities;
using ExcelsisDeo.Validation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureValidation();
builder.Services.ConfigurePersistence(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();
builder.Services.RegisterEndpointsHandlers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.RegisterEndpoints();

app.Run();