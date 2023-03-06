using System;
using System.Linq;
using System.Web.Caching;
using System.Web.Configuration;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.CaptionTranslations
{
    public partial class CaptionTranslationService : ICaptionTranslationService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static double CacheCaptionDataExpireMinute = Convert.ToInt64(WebConfigurationManager.AppSettings["CacheCaptionDataExpireMinute"]);

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
                var captionValue = "";

                if (System.Web.HttpContext.Current.Cache[key + LanguageCurrent.Id] == null)
                {
                    using (var db = new TMSContext())
                    {
                        var getCaptionValue = db.CaptionTranslations
                            .Where(x => x.LanguageId == languageId && x.CaptionKey == key)
                            .FirstOrDefault();

                        captionValue = getCaptionValue?.CaptionValue ?? "";
                    }

                    System.Web.HttpContext.Current.Cache.Add(key + LanguageCurrent.Id, captionValue, null, DateTime.Now.AddMinutes(CacheCaptionDataExpireMinute), Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.AboveNormal, null);
                }
                else
                {
                    captionValue = System.Web.HttpContext.Current.Cache[key + LanguageCurrent.Id] as string;
                }

                return captionValue;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return "";
            }
        }
    }
}