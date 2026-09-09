using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Errors
{
    public sealed record ValidationError(string Code, string Message) : Error(Code, Message);
}
