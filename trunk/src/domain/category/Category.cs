using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DL.Framework.Common;

namespace DL.AccountChecker.Domain
{
	public class CategoryMetaData
	{
        [StringLength(30)]
        [DisplayName("Category")]
        public object CategoryName { get; set; }
	}

    [Serializable]
    [MetadataType(typeof(CategoryMetaData))]
	public class Category : DomainObject<Category>
	{
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }

        public override IComparable Key
        {
            get { return CategoryId; }
        }
    }
}
