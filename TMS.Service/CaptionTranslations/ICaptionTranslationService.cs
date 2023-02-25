namespace TMS.Service.CaptionTranslations
{
    public partial interface ICaptionTranslationService
    {
        string GetCaptionName(int languageId, string key, string value);
    }
}