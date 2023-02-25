using System;
using System.Linq;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.CaptionTranslations
{
    public partial class CaptionTranslationService : ICaptionTranslationService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<CaptionTranslation> _captionTranslationRepository;

        #endregion Fields

        #region Ctor

        public CaptionTranslationService(IRepository<CaptionTranslation> captionTranslationRepository)
        {
            this._captionTranslationRepository = captionTranslationRepository;
        }

        #endregion Ctor

        public string GetCaptionName(int languageId, string key, string value)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var captionValue = db.CaptionTranslations
                        .Where(x => x.LanguageId == languageId && x.CaptionKey == key)
                        .FirstOrDefault();

                    return captionValue?.CaptionValue;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }
    }
}