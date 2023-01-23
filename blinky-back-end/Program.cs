var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddPolicy(name: "PolicyOne", 
    policy => {
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new() {Title="Spike Api", Version="v1"});});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/test", () => "Testing");

app.UseCors("PolicyOne");

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint(
    "/swagger/v1/swagger.json", "v1"
));

app.Run();
