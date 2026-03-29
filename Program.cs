using Microsoft.EntityFrameworkCore;
using pp_back_codex.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dbContext.Database.EnsureCreated();
}

app.MapGet("/", () => "Hello World!");

app.MapGet("/companies", async (ApplicationContext dbContext) =>
    await dbContext.Companies
        .OrderBy(company => company.Id)
        .ToListAsync());

app.Run();
