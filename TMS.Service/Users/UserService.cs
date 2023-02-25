using System;
using System.Linq;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.Users
{
    public partial class UserService : IUserService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<User> _userRepository;

        #endregion Fields

        #region Ctor

        public UserService(IRepository<User> userRepository)
        {
            this._userRepository = userRepository;
        }

        #endregion Ctor

        public User GetById(int Id)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var user = db.Users
                        .Where(x => x.Id == Id)
                        .FirstOrDefault();

                    return user;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        #region CheckLogin

        public User LoginUser(string userName, string passowrd)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var user = db.Users.Where(x => x.UserName == userName && x.Password == passowrd && x.IsActive)
                                .FirstOrDefault();

                    return user;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        #endregion CheckLogin
    }
}