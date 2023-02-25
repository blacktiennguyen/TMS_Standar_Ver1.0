using TMS.Core.Domains;

namespace TMS.Service.Users
{
    public partial interface IUserService
    {
        User GetById(int Id);

        #region CheckLogin

        User LoginUser(string userName, string passowrd);

        #endregion CheckLogin
    }
}