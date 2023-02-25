namespace TMS.WebAPP.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }

        public int UserNumber { get; set; }

        public string Remark { get; set; }
    }
}