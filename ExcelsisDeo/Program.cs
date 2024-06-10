using System.Text.Json.Serialization;
using ExcelsisDeo.Authentication;
using ExcelsisDeo.Authorization;
using ExcelsisDeo.Interfaces.Endpoints;
using ExcelsisDeo.Persistence;
using ExcelsisDeo.Persistence.Entities;
using ExcelsisDeo.Validation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var ExcelsisDeoOrigins = "_excelsisDeoOrigins"; 

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: ExcelsisDeoOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowCredentials().AllowAnyMethod();
            policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowCredentials().AllowAnyMethod();
        });
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
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

app.UseCors(ExcelsisDeoOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.RegisterEndpoints();

app.Run();