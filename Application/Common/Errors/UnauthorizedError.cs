using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Errors
{
    public sealed record UnauthorizedError(string Code , string Message) : Error(Code,Message)
    {
    }
}
