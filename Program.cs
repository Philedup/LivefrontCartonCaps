using LivefrontCartonCaps.MockData;
using LivefrontCartonCaps.Models.Config;
using LivefrontCartonCaps.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUserProfileRepository, MockUserProfileRepository>();
builder.Services.AddSingleton<IUserReferralRepository, MockUserReferralRepository>();
builder.Services.AddScoped<IReferralService, ReferralService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.Configure<ReferralSettings>(
    builder.Configuration.GetSection("ReferralSettings"));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddSwaggerGen(options =>
{
    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Enable support for Swagger response/request examples
    options.ExampleFilters();
    options.SupportNonNullableReferenceTypes();
    options.MapType<string>(() => new OpenApiSchema { Type = "string", Format = "text" });
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
