using Dna;
using Dna.AspNet;
using Mc.Api.Server;

var builder = WebApplication.CreateBuilder(args);

// Configure DnaFramework
builder.WebHost.UseDnaFramework(construct =>
{
    // Add configuration
    construct.AddConfiguration(builder.Configuration);
});

// Add services to the container.

// Add ApplicationDbContext
builder.Services.ConfigureDbContext(builder.Configuration)
    .ConfigureCors()
    .AddCloudinary(builder.Configuration)
    .AddDomainServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations
app.ApplyMigrations();

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
