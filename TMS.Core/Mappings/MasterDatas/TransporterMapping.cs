using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Core.Mappings.MasterDatas
{
    public class TransporterMapping : EntityTypeConfiguration<Transporter>
    {
        public TransporterMapping()
        {
            this.ToTable("MasterData_Transporters");
            this.HasKey(a => a.Id);
            this.Ignore(a => a.NameLL);
        }
    }
}