namespace DL.AccountChecker.Framework
{
    public interface IIdAllocationRepository
    {
        long[] GetNextIds(int idCount, long entityTypeId);
    }
}
