namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ITranslationService
    {
        Task<string> TranslateToLeetSpeakAsync(string originalText);
    }
}
