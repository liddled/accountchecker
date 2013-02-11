using DL.AccountChecker.Framework;

namespace DL.AccountChecker.BusinessLogic
{
    public class IdAllocationManager : IIdAllocationManager
	{
        private readonly IIdAllocationRepository _repository;

        public IdAllocationManager(IIdAllocationRepository repository)
		{
            _repository = repository;
		}

		public long GetNextId(long entityTypeId)
		{
			var idList = GetNextId(1, entityTypeId);
			return idList[0];
		}

		public long[] GetNextId(int idCount, long entityTypeId)
		{
		    return _repository.GetNextIds(idCount, entityTypeId);
		}
    }
}
