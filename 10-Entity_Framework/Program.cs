using _10_Entity_Framework.DbContexts;
using _10_Entity_Framework.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
})
.AddNewtonsoftJson();
// Register our DbContext here
// This will call our contructor in CityInfoContext which receives the options we define below
builder.Services.AddDbContext<CityInfoContext>(
    // Store the DB at a local path (at root directory)
    dbContextOptions => dbContextOptions.UseSqlite("Data Source=CityInfo.db")
);
// Add our repository as a service, which will be injected as a dependency in classes that use it (e.g. Controllers).
// It has to be per request so we use `AddScoped`
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();

