using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Core.Mappings.MasterDatas
{
    public class ItemUnitMapping : EntityTypeConfiguration<ItemUnit>
    {
        public ItemUnitMapping()
        {
            this.ToTable("MasterData_ItemUnits");
            this.HasKey(a => a.Id);
            this.Ignore(a => a.NameLL);
        }
    }
}