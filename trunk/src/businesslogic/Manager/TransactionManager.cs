using System.Collections.Generic;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;

namespace DL.AccountChecker.BusinessLogic
{
	public class TransactionManager : ITransactionManager
	{
        private readonly ITransactionRepository _transactionRepository;

        public TransactionManager(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public void Insert(Transaction transaction)
        {
            _transactionRepository.Insert(transaction);
        }

        public void Insert(IList<Transaction> transactions)
        {
            _transactionRepository.Insert(transactions);
        }

        public void Update(Transaction transaction)
        {
            _transactionRepository.Update(transaction);
        }

        public void Update(IList<Transaction> transactions)
        {
            _transactionRepository.Update(transactions);
        }

        public IEnumerable<Transaction> GetAllItems()
        {
            return _transactionRepository.GetAll();
        }

        public IEnumerable<TransactionCategory> GetAllTransactionCategories()
        {
            return _transactionRepository.GetAllTransactionCategories();
        }

        public void Save(TransactionInfo transactionInfo)
        {
            _transactionRepository.Save(transactionInfo);
        }

        public IEnumerable<TransactionInfo> GetAllTransactionInfos()
        {
            return _transactionRepository.GetAllTransactionInfos();
        }
    }
}
