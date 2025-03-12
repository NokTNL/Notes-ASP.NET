// "Builder" of our web app
var builder = WebApplication.CreateBuilder(args);

// Instantiate our app object
var app = builder.Build();

// This is the simplest middleware which responds "Hello World" to whatever requests
app.Run(async context =>
{
    await context.Response.WriteAsync("Hello World!");
});

// Run our app
app.Run();
