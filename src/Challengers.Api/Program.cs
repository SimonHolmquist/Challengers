using Microsoft.OpenApi.Models; // Add this namespace for OpenAPI support

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer(); // Required for OpenAPI
builder.Services.AddSwaggerGen(c => // Add Swagger generation
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Challengers API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Challengers API v1"));
}

app.UseHttpsRedirection();

app.Run();