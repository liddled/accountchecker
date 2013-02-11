using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DL.Framework.Common;

namespace DL.AccountChecker.Domain
{
	public class AccountMetaData
	{
        [Required]
        [StringLength(30)]
        [DisplayName("Bank Name")]
		public object BankName { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("Bank Code")]
        public object BankCode { get; set; }

        [StringLength(15)]
        [DisplayName("Card Number")]
        public object CardNumber { get; set; }

        [Required]
        [StringLength(5)]
        [DisplayName("Locale")]
        public object Locale { get; set; }
	}

    [Serializable]
    [MetadataType(typeof(AccountMetaData))]
    public class Account : DomainObject<Account>
    {
        public long AccountId { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string CardNumber { get; set; }
        public string Locale { get; set; }

        public override IComparable Key
        {
            get { return AccountId; }
        }

        public override int CompareTo(object obj)
        {
            var item = obj as Account;
            if (item == null) return -1;
            return BankName.CompareTo(item.BankName);
        }
    }
}
