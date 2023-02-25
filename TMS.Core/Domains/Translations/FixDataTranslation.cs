using System;

namespace TMS.Core.Domains
{
    public class FixDataTranslation : BaseEntity
    {
        public string Name { get; set; }

        public Guid TranslationId { get; set; }

        public int LanguageId { get; set; }

        public string FunctionTable { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedById { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}