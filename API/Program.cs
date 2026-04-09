using API.Middleware;
using API.Services;
using Application.Contracts;
using Application.UseCases.Employees;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IGetAllEmployeesUseCase, GetAllEmployeesUseCase>();
builder.Services.AddScoped<IGetEmployeeByIdUseCase, GetEmployeeByIdUseCase>();
builder.Services.AddScoped<ICreateEmployeeUseCase, CreateEmployeeUseCase>();
builder.Services.AddScoped<IUpdateEmployeeUseCase, UpdateEmployeeUseCase>();
builder.Services.AddScoped<IDeleteEmployeeUseCase, DeleteEmployeeUseCase>();

var jwtKey = builder.Configuration["Jwt:Key"] ?? "dev-secret-key-change-in-production";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "StaffTrackerManager";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "StaffTrackerManager";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
