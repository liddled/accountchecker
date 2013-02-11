using System;
using DL.AccountChecker.Domain;

namespace DL.AccountChecker.Model
{
    [Serializable]
    public class AccountSummaryModel
    {
        public Account Account { get; set; }

        public int TransactionCount { get; set; }
        public decimal Balance { get { return Outcome + Income; } }
        public decimal Income { get; set; }
        public decimal Outcome { get; set; }
    }
}
