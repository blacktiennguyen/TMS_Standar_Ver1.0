using System;
using TMS.Core.Domains.FIXs;

namespace TMS.Service.FIXs
{
    public partial interface ILengthTypeService
    {
        LengthType GetById(int Id);
    }
}