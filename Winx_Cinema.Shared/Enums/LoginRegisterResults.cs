using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winx_Cinema.Shared.Enums
{
    public enum LoginRegisterResults
    {
        Ok,
        SomethingWentWrong,
        IncorrectPassword,
        IncorrectEmail,
        EmailIsExist,
        UserNameIsExist
    }
}
