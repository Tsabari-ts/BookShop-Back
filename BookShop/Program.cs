using BookShop.BookManager;
using BookShop.Helper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<XmlSerialization>();
builder.Services.AddTransient<BookFileHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var configuration = new ConfigurationBuilder()
    .SetBasePath(app.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{app.Environment.EnvironmentName}.json", optional: true)
    .Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
