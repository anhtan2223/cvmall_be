using Framework.Core.Abstractions;

namespace Application.Core.Contracts
{
    public class TimesheetRequestPaged : IRequestPaged
    {
        public int page { get; set; } = 1;

        public int size { get; set; } = 10;

        public string? sort { get; set; }

        public string? search { get; set; }

        public int month { get; set; }

        public int year { get; set; }

        public IList<string>? branchFilters { get; set; }

        public IList<string>? groupFilters { get; set; }
        
        public IList<int>? stateFilters { get; set; }

    }
}
