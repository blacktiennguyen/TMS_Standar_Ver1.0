using System;

namespace TMS.Core.Domains
{
    public class MessageInfoTranslation : BaseEntity
    {
        public string MsgCode { get; set; }

        public string MsgContent { get; set; }

        public int LanguageId { get; set; }

        public string Remark { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedById { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}