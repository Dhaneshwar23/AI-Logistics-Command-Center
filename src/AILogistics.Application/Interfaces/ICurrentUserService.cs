using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.Interfaces
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        int? Role {  get; }
        int? CustomerId {  get; }
        bool IsAuthenticated { get; }
    }
}
