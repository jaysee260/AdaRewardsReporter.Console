using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ADARewardsReporter.Interfaces;
using CsvHelper;

namespace ADARewardsReporter.Utils
{
    public class ReportWriter : ICsvReportWriter
    {
        private readonly string FilenameTemplate = "RewardsSummary-{0}T{1}.csv";

        public void WriteReport<T>(IEnumerable<T> records)
        {
            var path = BuildPath();
            using var writer = new StreamWriter(path);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(records);
        }

        private string BuildPath()
        {
            var date = DateTime.Now.ToShortDateString().Replace("/", "-");
            var time = DateTime.Now.ToShortTimeString().Replace(":", ".").Replace(" ", string.Empty);
            return string.Format(FilenameTemplate, date, time);
        }
    }
}