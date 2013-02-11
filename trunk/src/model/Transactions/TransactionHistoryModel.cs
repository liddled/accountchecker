using System;
using System.Collections.Generic;
using DL.AccountChecker.Domain;
using DL.Framework.Common;

namespace DL.AccountChecker.Model
{
    [Serializable]
    public class TransactionHistoryModel
    {
        public Account Account { get; set; }

        public BusDate Date { get; set; }
        public DateView DateView { get; set; }
        public IList<Category> Categories { get; set; }

        public int TransactionCount { get; set; }
        public decimal Net { get { return Income + Outcome; } }
        public decimal Income { get; set; }
        public decimal Outcome { get; set; }

        public IList<Transaction> Transactions { get; set; }
    }
}
