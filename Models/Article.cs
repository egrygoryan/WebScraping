namespace WebScrapping.Models;

public sealed class Article(
    string origin,
    string author,
    string title,
    string description,
    DateTime publishedDate)
{
    public string Origin { get; } = origin;
    public string Author { get; } = author;
    public string Title { get; } = title;
    public string Description { get; } = description;
    public DateTime PublishedDate { get; } = publishedDate;
    public Tag Originality { get; private set; } = Tag.New;

    public void RemoveTag()
    {
        if (Originality != 0)
        {
            Originality = default;
        }
    }
    public override bool Equals(object? obj) => Equals(obj as Article);
    private bool Equals(Article? article) => article is not null && article.Title == Title;
    public override int GetHashCode() => string.GetHashCode(Title);
}
