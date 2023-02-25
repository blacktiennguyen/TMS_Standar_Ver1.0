using System;
using System.Collections.Generic;
using System.Linq;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.Languages
{
    public partial class LanguageService : ILanguageService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<Language> _languageRepository;

        #endregion Fields

        #region Ctor

        public LanguageService(IRepository<Language> languageRepository)
        {
            this._languageRepository = languageRepository;
        }

        #endregion Ctor

        public Language GetById(int Id)
        {
            var language = new Language();
            try
            {
                using (var db = new TMSContext())
                {
                    language = db.Languages
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();

                    return language;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public IList<Language> GetAll()
        {
            var languages = new List<Language>();
            try
            {
                using (var db = new TMSContext())
                {
                    languages = db.Languages
                        .Where(x => x.IsActive == true)
                        .OrderBy(x => x.DisplayOrder)
                        .ToList();

                    return languages;
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