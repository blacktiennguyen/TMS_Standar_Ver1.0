using System;
using System.Collections.Generic;
using System.Linq;
using TMS.Core;
using TMS.Core.Domains.FIXs;

namespace TMS.Service.FIXs
{
    public partial class WeightTypeService : IWeightTypeService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<WeightType> _weightTypeRepository;

        #endregion Fields

        #region Ctor

        public WeightTypeService(IRepository<WeightType> weightTypeRepository)
        {
            this._weightTypeRepository = weightTypeRepository;
        }

        #endregion Ctor

        public WeightType GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var weightType = db.WeightTypes
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();

                    return weightType;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public List<WeightType> GetAlls()
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var weightTypes = db.WeightTypes
                        .ToList();

                    return weightTypes;
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