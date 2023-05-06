using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.UserService
{
    public interface ICurrentUserInfoService
    {
        public string GetCurrentUserEmail();
    }
}
