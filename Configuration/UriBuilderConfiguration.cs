namespace WebScrapping.Configuration;

public static class UriBuilderConfiguration
{
    public static string ConfigureUri(string url)
    {
        UriBuilder uriBuilder = new UriBuilder(url);
        uriBuilder.Scheme = Uri.UriSchemeHttps;
        uriBuilder.Port = -1;

        return uriBuilder.Uri.ToString();
    }
}