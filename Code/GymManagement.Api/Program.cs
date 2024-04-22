using GymManagement.Api.Endpoints.Subscriptions;
using GymManagement.Api.Endpoints.Weather;
using GymManagement.Application;
using GymManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddApplication()
    .AddInfrastructure();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add endpoints
app.AddGettingWeatherEndpoint()
    .AddCreatingSubscriptionEndpoint()
    .AddGettingSubscriptionEndpoint()
    .AddDeletingSubscriptionEndpoint();


app.Run();