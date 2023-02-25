using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TMS.WebAPP.Models.AccountModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "*")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*")]
        public string Password { get; set; }

        public List<LanguageModel> Languages { get; set; }
    }
}