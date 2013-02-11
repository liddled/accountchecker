using System;
using DL.Framework.Common;

namespace DL.AccountChecker.Domain
{
    [Serializable]
    public class TransactionCategory : DomainObject<TransactionCategory>
    {
        public long TransactionId { get; set; }
        public long CategoryId { get; set; }

        public override IComparable Key
        {
            get { return String.Format("{0}.{1}", TransactionId, CategoryId); }
        }
    }
}
