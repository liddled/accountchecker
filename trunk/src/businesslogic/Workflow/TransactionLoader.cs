using System;
using System.IO;
using System.Linq;
using Common.Logging;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;
using DL.Framework.OFX;
using DL.Framework.Services;

namespace DL.AccountChecker.BusinessLogic
{
    public class TransactionLoader : FileLoader
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IMessagePublisher _messagePublisher;
        private readonly string _cacheDestination;

        public TransactionLoader(IMessagePublisher messagePublisher, string cacheDestination, string inputFolder, string processedFolder, string fileFilter)
            : base(inputFolder, processedFolder, fileFilter)
        {
            _messagePublisher = messagePublisher;
            _cacheDestination = cacheDestination;
        }

        private static OFXDocument ParseOFXDocument(string file)
        {
            var ofxParser = new OFXDocumentParser();
            return ofxParser.Import(new FileStream(file, FileMode.Open));
        }

        public override void FileCreated(object sender, FileSystemEventArgs e)
        {
            OFXDocument ofxDocument = null;
            try
            {
                ofxDocument = ParseOFXDocument(e.FullPath);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Unable to map file ({0}) to OFX document.", ex, e.FullPath);
            }

            try
            {
                if (ofxDocument == null || ofxDocument.Transactions.Count == 0)
                    return;

                Account account = null;
                ServiceProxyManager.Use<IGuiService>(EndPoints.GuiService,
                    proxy => { account = proxy.GetAccount(ofxDocument.Account.AccountID); });

                if (account == null)
                {
                    Log.ErrorFormat("Unable to find account with AccountId {0}", ofxDocument.Account.AccountID);
                    return;
                }

                Log.DebugFormat("Processing {0} transactions for account {1}", ofxDocument.Transactions.Count, account.AccountId);

                var transactionList = ofxDocument.Transactions.OrderBy(t => t.TransactionID).Select(t => new Transaction
                {
                    UniqueId = t.TransactionID,
                    AccountId = account.AccountId,
                    Date = new BusDate(t.Date),
                    Description = t.Description,
                    Amount = t.Amount,
                    Status = Status.Active
                }).ToList();

                var message = new BinaryMessage(SerializeHelper.ObjectToByteArray(transactionList));
                _messagePublisher.Send(_cacheDestination, message);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error in transaction loader", ex);
            }
            finally
            {
                var processedFile = Path.Combine(_processedFolder, String.Format("transactions.{0:yyyyMMddhhmmss}.ofx", DateTime.Now));
                Log.DebugFormat("Moving processed transaction file to {0}", processedFile);
                File.Move(e.FullPath, processedFile);
            }
        }
    }
}
