using Coravel;
using WebScrapping.CustomMiddleware;
using WebScrapping.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices();
builder.Services.AddScheduler();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.AddScheduler();

app.MapPost("/api/v1/crawler/scrape", ScrapeEndpoint.ScrapeArticle);
app.MapPost("/api/v1/crawler/blog/scrape", ScrapeEndpoint.ScrapeBlog);
app.MapPost("/api/v1/blog/save", StoreEndpoint.StoreBlog);
app.MapGet("/api/v1/articles/filter", ArticlesEndpoint.GetNewArticles);

app.Run();