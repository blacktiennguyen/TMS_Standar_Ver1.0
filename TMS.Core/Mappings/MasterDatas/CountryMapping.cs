using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Core.Mappings.MasterDatas
{
    public class CountryMapping : EntityTypeConfiguration<Country>
    {
        public CountryMapping()
        {
            this.ToTable("MasterData_Countries");
            this.HasKey(a => a.Id);
        }
    }
}