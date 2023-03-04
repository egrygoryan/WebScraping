var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/v1/crawler/scrape/{site}", async ([FromQuery] string site, [FromServices] IDataScrapeService service) =>
{
    var url = UriBuilderConfiguration.ConfigureUri(site);

    var config = Configuration.Default.WithDefaultLoader();
    var context = BrowsingContext.New(config);
    var document = await context.OpenAsync(url);

    return document is null 
        ? Results.NotFound(new { Message = "Provided resource is not found" }) 
        : Results.Ok(service.GetScrappedData(document));
});

app.Run();