using System;

namespace TMS.Service.FixDataTranslations
{
    public partial interface IFixDataTranslationService
    {
        string GetName(int languageId, Guid translationId);
    }
}