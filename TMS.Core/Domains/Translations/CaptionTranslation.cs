namespace TMS.Core.Domains
{
    public class CaptionTranslation : BaseEntity
    {
        public string CaptionKey { get; set; }

        public string CaptionValue { get; set; }

        public int LanguageId { get; set; }

        public string FunctionName { get; set; }
    }
}