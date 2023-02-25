using System;

namespace TMS.Service.MasterDataTranslations
{
    public partial interface IMasterDataTranslationService
    {
        string GetName(int languageID, Guid? translationID);
    }
}