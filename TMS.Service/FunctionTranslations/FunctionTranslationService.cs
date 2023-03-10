using System;
using System.Linq;
using System.Web.Caching;
using System.Web.Configuration;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.FunctionTranslations
{
    public partial class FunctionTranslationService : IFunctionTranslationService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static double CacheFunctionDataExpireMinute = Convert.ToInt64(WebConfigurationManager.AppSettings["CacheFunctionDataExpireMinute"]);

        #region Fields

        private readonly IRepository<FunctionTranslation> _functionTranslationRepository;

        #endregion Fields

        #region Ctor

        public FunctionTranslationService(IRepository<FunctionTranslation> functionTranslationRepository)
        {
            this._functionTranslationRepository = functionTranslationRepository;
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
                        var query = db.FunctionTranslations
                            .Where(x => x.LanguageId == languageID && x.TranslationId == translationID)
                            .FirstOrDefault();
                        resultName = query != null ? query.Name : null;
                    }

                    System.Web.HttpContext.Current.Cache.Add(translationID.ToString() + LanguageCurrent.Id, resultName, null, DateTime.Now.AddMinutes(CacheFunctionDataExpireMinute), Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.AboveNormal, null);
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