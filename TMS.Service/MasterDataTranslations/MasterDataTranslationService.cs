using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Caching;
using System.Web.Configuration;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.MasterDataTranslations
{
    public partial class MasterDataTranslationService : IMasterDataTranslationService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static double CacheMasterDataExpireMinute = Convert.ToInt64(WebConfigurationManager.AppSettings["CacheMasterDataExpireMinute"]);

        #region Fields

        private readonly IRepository<MasterDataTranslation> _masterDataTranslationnRepository;

        #endregion Fields

        #region Ctor

        public MasterDataTranslationService(IRepository<MasterDataTranslation> masterDataTranslationnRepository)
        {
            this._masterDataTranslationnRepository = masterDataTranslationnRepository;
        }

        #endregion Ctor

        public string GetName(int languageID, Guid? translationID)
        {
            try
            {
                var resultName = "";

                using (var db = new TMSContext())
                {
                    var query = db.MasterDataTranslations
                        .Where(x => x.LanguageId == languageID && x.TranslationId == translationID)
                        .FirstOrDefault();
                    resultName = query != null ? query.Name : "";
                }

                //if (translationID != null)
                //{
                //    if (System.Web.HttpContext.Current.Cache[translationID.ToString() + LanguageCurrent.Id] == null)
                //    {
                //        using (var db = new TMSContext())
                //        {
                //            var query = db.MasterDataTranslations
                //                .Where(x => x.LanguageId == languageID && x.TranslationId == translationID)
                //                .FirstOrDefault();
                //            resultName = query != null ? query.Name : "";
                //        }

                //        System.Web.HttpContext.Current.Cache.Add(translationID.ToString() + LanguageCurrent.Id, resultName, null, DateTime.Now.AddMinutes(CacheMasterDataExpireMinute), Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.AboveNormal, null);
                //    }
                //    else
                //    {
                //        resultName = System.Web.HttpContext.Current.Cache[translationID.ToString() + LanguageCurrent.Id] as string;
                //    }
                //}
                //else
                //{
                //    using (var db = new TMSContext())
                //    {
                //        var query = db.MasterDataTranslations
                //            .Where(x => x.LanguageId == languageID && x.TranslationId == translationID)
                //            .FirstOrDefault();
                //        resultName = query != null ? query.Name : "";
                //    }
                //}

                return resultName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return "";
            }
        }

        public void SaveOrUpdate(MasterDataTranslation masterDataTranslation)
        {
            try
            {
                if (masterDataTranslation.Id > 0)
                {
                    using (var db = new TMSContext())
                    {
                        db.Entry(masterDataTranslation).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    _masterDataTranslationnRepository.Insert(masterDataTranslation);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public void Delete(MasterDataTranslation masterDataTranslation)
        {
            try
            {
                _masterDataTranslationnRepository.Delete(masterDataTranslation);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }
    }
}