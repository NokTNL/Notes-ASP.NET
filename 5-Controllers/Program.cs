// Controllers deals with requests and apply relavany business logic based on that. Go to `Controllers` for implementations
// DTOs (Data transfer objects) deals with shape of input / output, including validations. Go to `Models` for implementations.

/**
 * Builder setup
 */ 
var builder = WebApplication.CreateBuilder(args);

// Builder service for enabling Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Builder service for enabling Controllers
// !!! Do NOT use `AddMvc` here as that will add services that we don't need (e.g. those for the vuew layer for showing HTML)
builder.Services.AddControllers(options =>
{
    // Content Negotiation
    options.ReturnHttpNotAcceptable = true; // This will return 406 (Not Acceptable) when incoming request's `Accept` header specifies unexpected formats (e.g. application/xml)
})
// Needed for using JSON Document, replacing the default builtin JsonDeserializer
// See `Controllers` for using it in PATCH endpoints.
.AddNewtonsoftJson();

var app = builder.Build();

/**
 * Middlewares
 */
// Shows Swagger UI at `/swagger` when Environment is "Development"
// - This depends on the profile in the launch settings. So for this project if we launch the "https" profile (which is using the "Production" environment), we will not see the Swagger UI
// - Order matters when adding middlewares. If this is added after a catch-all "Hello World" middleware at the very bottom, every request will return "Hello World" regardless
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Add the Controllers to the app
app.MapControllers();

app.Run();

