using System;
using DL.AccountChecker.Domain;
using DL.Framework.Common;

namespace DL.AccountChecker.Framework
{
	public static class TransactionFilter
	{
        public static Func<Transaction, bool> WithDateBetween(long accountId, BusDate initialDate, DateView dateView, Status status)
        {
            var startDate = BusDate.GetDate(initialDate, dateView);
            var endDate = BusDate.GetNextDate(initialDate, dateView);

            return t => t.AccountId == accountId &&
                (dateView == DateView.All || (t.Date.Sort8Date >= startDate.Sort8Date && t.Date.Sort8Date < endDate.Sort8Date)) &&
                (status == Status.All || t.Status == status);
        }

        public static Func<Transaction, bool> WithDateBetween(long accountId, BusDate startDate, BusDate endDate, Status status)
        {
            return t => t.AccountId == accountId &&
                ((startDate == BusDate.Null || t.Date.Sort8Date >= startDate.Sort8Date) && (endDate == BusDate.Null || t.Date.Sort8Date < endDate.Sort8Date)) &&
                (status == Status.All || t.Status == status);
        }
	}
}
