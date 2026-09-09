using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Errors
{
    public abstract record Error(string Code, string Message);// code is not status code as this related to Business not api
}
