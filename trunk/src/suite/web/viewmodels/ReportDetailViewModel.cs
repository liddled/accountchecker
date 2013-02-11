namespace DL.AccountChecker.Suite.Web
{
    using System;
    using System.Collections.Generic;

    using DL.Framework.Common;
    using DL.AccountChecker.Domain;

    public class ReportDetailViewModel
    {
        public Account Account { get; set; }
        public IDictionary<BusDate, Candlestick> Balances { get; set; }

        public BusDate FromDate { get; set; }
        public BusDate ToDate { get; set; }

        public decimal StartBalance { get; set; }
        public decimal EndBalance { get; set; }

        public decimal Change { get; set; }
        public decimal ChangePercentage { get; set; }

        public BusDate HighestBalanceDate { get; set; }
        public decimal HighestBalance { get; set; }
        public BusDate LowestBalanceDate { get; set; }
        public decimal LowestBalance { get; set; }
    }
}