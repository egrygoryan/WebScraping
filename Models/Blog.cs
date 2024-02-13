namespace WebScrapping.Models;

public sealed class Blog(string url)
{
    public string Url { get; } = url;
    public bool IsStored { get; private set; }

    public void Store() => IsStored = true;
    public override bool Equals(object? obj) => Equals(obj as Blog);
    private bool Equals(Blog? blog) => blog is not null && blog.Url == Url;
    public override int GetHashCode() => string.GetHashCode(Url);
}
