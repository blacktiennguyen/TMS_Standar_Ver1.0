using System;
using System.Linq;
using TMS.Core;
using TMS.Core.Domains.FIXs;

namespace TMS.Service.FIXs
{
    public partial class LengthTypeService : ILengthTypeService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<LengthType> _lengthTypeRepository;

        #endregion Fields

        #region Ctor

        public LengthTypeService(IRepository<LengthType> lengthTypeRepository)
        {
            this._lengthTypeRepository = lengthTypeRepository;
        }

        #endregion Ctor

        public LengthType GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var lengthType = db.LengthTypes
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();

                    return lengthType;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }
    }
}