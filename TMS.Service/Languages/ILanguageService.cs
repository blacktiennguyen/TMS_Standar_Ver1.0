using System.Collections.Generic;
using TMS.Core.Domains;

namespace TMS.Service.Languages
{
    public partial interface ILanguageService
    {
        Language GetById(int Id);

        IList<Language> GetAll();
    }
}