using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Options
{
    public class JwtOption
    {
        public const string SectionName = "JWT";

        public string Key { get; set; } = default!;

        public string ValidIssuer { get; set; } = default!;

        public string ValidAudience { get; set; } = default!;

        public int ExpiryInDays { get; set; }

        public int ExpiryInHours { get; set; }
    }
}
