using Microsoft.EntityFrameworkCore;
using trip_app.OutputFolder;
using trip_app.OutputFolder.Middlewares;
using trip_app.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterInfraServices();
builder.Services.RegisterApplicationServices(); 

// // Configure DbContext with connection string from appsettings.json
// builder.Services.AddDbContext<ApbdContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddScoped<ITripRepository, TripRepository>();

var app = builder.Build();
app.UseMiddleware<ExceptionHandingMiddleware>();

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