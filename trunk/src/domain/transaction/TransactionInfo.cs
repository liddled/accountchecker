using System;
using DL.Framework.Common;

namespace DL.AccountChecker.Domain
{
    [Serializable]
    public class TransactionInfo : DomainObject<TransactionInfo>
    {
        public long TransactionId { get; set; }
        public bool Excluded { get; set; }

        public override IComparable Key
        {
            get { return TransactionId; }
        }
    }
}
