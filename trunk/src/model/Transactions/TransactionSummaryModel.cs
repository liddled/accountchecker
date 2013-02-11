using System;
using DL.AccountChecker.Domain;

namespace DL.AccountChecker.Model
{
    [Serializable]
    public class TransactionSummaryModel
    {
        public Account Account { get; set; }

        public decimal StartBalance { get; set; }
        public decimal CurrentBalance { get; set; }

        public int TransactionCount { get; set; }
        public decimal Net { get { return Income + Outcome; } }
        public decimal Income { get; set; }
        public decimal Outcome { get; set; }
    }
}
