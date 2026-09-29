using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.DTOs
{
    public sealed record TaskResponseDto(Guid PublicId, string TaskName, string TaskDescription, DateTimeOffset StartDate, DateTimeOffset EndDate, string TaskStatus, List<EmployeeDto> Employees)
    {
    }
}
