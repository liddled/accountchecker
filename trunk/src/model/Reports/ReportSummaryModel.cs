using System;
using DL.AccountChecker.Domain;

namespace DL.AccountChecker.Model
{
    [Serializable]
    public class ReportSummaryModel
    {
        public Account Account { get; set; }

        public decimal Change { get; set; }
        public decimal ChangePercentage { get; set; }
        public decimal HighestBalance { get; set; }
        public decimal LowestBalance { get; set; }
    }
}
