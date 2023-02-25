using System;

namespace TMS.Core.SessionEntity
{
    [Serializable]
    public class SessionLanguageCurrent
    {
        public int LanguageId { get; set; }

        public string LanguageName { get; set; }

        public string IconLanguage { get; set; }
    }
}