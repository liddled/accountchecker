using DL.Framework.Common;

namespace DL.AccountChecker.Domain
{
    public enum EEntityType
    {
        [TextRep(ETextRepType.Db, "ACCOUNT")]
        Account = 1,

        [TextRep(ETextRepType.Db, "TRANSACTION")]
        Transaction = 2,

        [TextRep(ETextRepType.Db, "CATEGORY")]
        Category = 3
    }
}
