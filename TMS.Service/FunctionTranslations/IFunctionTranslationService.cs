using System;

namespace TMS.Service.FunctionTranslations
{
    public partial interface IFunctionTranslationService
    {
        string GetName(int languageID, Guid translationID);
    }
}