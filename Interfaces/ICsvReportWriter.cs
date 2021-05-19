using System.Collections.Generic;

namespace ADARewardsReporter.Interfaces
{
    public interface ICsvReportWriter
    {
        void WriteReport<T>(IEnumerable<T> records);
    }
}