namespace WebScrapping.DTO;

public sealed record FilterNewArticlesResponse(
    string Origin,
    string Author,
    string Title,
    string Description,
    string PublishedDate,
    string Originality
);
