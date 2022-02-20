using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.UseOcelot();

app.MapGet("/", () => "STR Phone Book - API Gateway");

app.Run();
