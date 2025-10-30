using Backend.Domain;
using Backend.Ports;
using Backend.Repository;
using Backend.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddScoped<ILightService, LightService>();
builder.Services.AddScoped<ILightRepository, LightRepository>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IJsonFileHandler<Light>, JsonFileHandler<Light>>();
builder.Services.AddScoped<IJsonFileHandler<ApiKey>, JsonFileHandler<ApiKey>>();
builder.Services.AddScoped<IJsonFileHandler<Group>, JsonFileHandler<Group>>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

