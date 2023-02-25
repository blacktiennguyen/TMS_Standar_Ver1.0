using System;
using System.Collections.Generic;

namespace TMS.WebAPP.Models
{
    public class HomePageModel
    {
        public class FunctionModel
        {
            public FunctionModel()
            {
                SubFunctions = new List<FunctionModel>();
            }

            public List<FunctionModel> SubFunctions { get; set; }

            public int Id { get; set; }
            public string Name { get; set; }

            public bool? IsActive { get; set; }

            public string ClassActive { get; set; }

            public Guid? ParentID { get; set; }

            public string CssIcon { get; set; }

            public string Controller { get; set; }

            public string Action { get; set; }

            public int DisplayOrder { get; set; }

            public Guid TranslationID { get; set; }
        }

        public class FunctionPageHeaderModel
        {
            public FunctionPageHeaderModel()
            {
                Languages = new List<LanguageModel>();
                Companys = new List<CompanyModel>();
            }

            public int CompanyCurrentId { get; set; }

            public List<LanguageModel> Languages { get; set; }

            public List<CompanyModel> Companys { get; set; }
        }
    }
}