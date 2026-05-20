using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Enums
{
    public enum ProjectTaskStatus
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4
    }

    public enum ProjectTaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }
}
