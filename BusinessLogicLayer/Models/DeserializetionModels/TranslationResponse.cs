namespace BusinessLogicLayer.Models.DeserializetionModels
{
    public class TranslationResponse
    {
        public Success success { get; set; } = new Success();
        public Contents contents { get; set; } = new Contents();
    }
}
