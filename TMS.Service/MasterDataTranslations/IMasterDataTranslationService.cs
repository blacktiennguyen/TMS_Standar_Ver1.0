using System;
using TMS.Core.Domains;

namespace TMS.Service.MasterDataTranslations
{
    public partial interface IMasterDataTranslationService
    {
        string GetName(int languageID, Guid? translationID);

        void SaveOrUpdate(MasterDataTranslation masterDataTranslation);

        void Delete(MasterDataTranslation masterDataTranslation);
    }
}