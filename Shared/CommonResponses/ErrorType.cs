using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CommonResponses
{
    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        UnAuthorized = 3,
        Forbidden = 4,
        InvalidCredential = 5,
         

    }
}
