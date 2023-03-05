namespace WebScrapping.Services;

public sealed class ValidateService : IValidateService
{
    private readonly HttpClient _httpClient;
    public ValidateService(HttpClient httpClient) => _httpClient = httpClient;
    public async Task<bool> ValidateStatusCode(string url)
    {
        var response = await _httpClient.GetAsync(url);

        return response.IsSuccessStatusCode;
    }
}