namespace DL.AccountChecker.Framework
{
    public interface IIdAllocationManager
    {
        long GetNextId(long entityTypeId);
        long[] GetNextId(int idCount, long entityTypeId);
    }
}
