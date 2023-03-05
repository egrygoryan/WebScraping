namespace WebScrapping.Services;

interface IValidateService
{
    Task<bool> ValidateStatusCode(string url);
}