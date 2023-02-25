using System;
using System.Linq;
using System.Web.Caching;
using System.Web.Configuration;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.FixDataTranslations
{
    public partial class FixDataTranslationService : IFixDataTranslationService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static double CacheFixDataExpireMinute = Convert.ToInt64(WebConfigurationManager.AppSettings["CacheFixDataExpireMinute"]);

        #region Fields

        private readonly IRepository<FixDataTranslation> _fixDataTranslationnRepository;

        #endregion Fields

        #region Ctor

        public FixDataTranslationService(IRepository<FixDataTranslation> fixDataTranslationnRepository)
        {
            this._fixDataTranslationnRepository = fixDataTranslationnRepository;
        }

        #endregion Ctor

        public string GetName(int languageID, Guid translationID)
        {
            try
            {
                var resultName = "";

                if (System.Web.HttpContext.Current.Cache[translationID.ToString() + LanguageCurrent.Id] == null)
                {
                    using (var db = new TMSContext())
                    {
                        var query = db.FixDataTranslations
                            .Where(x => x.LanguageId == languageID && x.TranslationId == translationID)
                            .FirstOrDefault();
                        resultName = query != null ? query.Name : null;
                    }

                    System.Web.HttpContext.Current.Cache.Add(translationID.ToString() + LanguageCurrent.Id, resultName, null, DateTime.Now.AddMinutes(CacheFixDataExpireMinute), Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.AboveNormal, null);
                }
                else
                {
                    resultName = System.Web.HttpContext.Current.Cache[translationID.ToString() + LanguageCurrent.Id] as string;
                }

                return resultName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }
    }
}