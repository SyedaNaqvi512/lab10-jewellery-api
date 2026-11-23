using lab10_Jewellery_API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers + OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register the database context (SQL Server LocalDB)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();   // JSON description at /openapi/v1.json
}

app.UseHttpsRedirection();

// Needed so uploaded images can be opened by URL (wwwroot/uploads/...)
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
