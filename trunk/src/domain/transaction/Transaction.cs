using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DL.Framework.Common;

namespace DL.AccountChecker.Domain
{
	public class TransactionMetaData
	{
        [Required]
        [DisplayName("Account")]
        public object AccountId { get; set; }

        [DisplayName("Unique Id")]
        public object UniqueId { get; set; }

        [Required]
        [DisplayName("Date")]
        public object Date { get; set; }

        [Required]
        [DisplayName("Description")]
        public object Description { get; set; }

        [Required]
        [RegularExpression(@"^(-)?\d+(\.\d\d)?$", ErrorMessage = "Amount must be a valid money amount.")]
        [DisplayName("Amount")]
        public object Amount { get; set; }

        [UIHint("CategoryList")]
        [DisplayName("Categories")]
        public object Categories { get; set; }
	}

    [Serializable]
    [MetadataType(typeof(TransactionMetaData))]
    public class Transaction : DomainObject<Transaction>
    {
        public long TransactionId { get; set; }
        public string UniqueId { get; set; }
        public long AccountId { get; set; }
        public BusDate Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public IList<Category> Categories { get; set; }
        public TransactionInfo Info { get; set; }

        public override IComparable Key
        {
            get { return TransactionId; }
        }

        public Transaction()
        {
            Categories = new List<Category>();
            Info = new TransactionInfo();
        }
    }
}
