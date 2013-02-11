using System;
using System.IO;
using DL.Framework.OFX;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DL.AccountChecker.Test
{
    [TestClass]
    public class OfxTest
    {
        private const string OfxValidNonWellFormedFile = @"Data\OFX\valid-nonwellformed.ofx";
        private const string OfxValidWellFormedFile = @"Data\OFX\valid-wellformed.ofx";

        [TestMethod]
        public void TestValidNonWellFormedOfxFile()
        {
            var ofxDocumentParser = new OFXDocumentParser();
            var ofxDocument = ofxDocumentParser.Import(new FileStream(OfxValidNonWellFormedFile, FileMode.Open));

            Assert.IsNotNull(ofxDocument);
            Assert.IsNotNull(ofxDocument.Account);
            Assert.IsInstanceOfType(ofxDocument.Account, typeof(OFXBankAccount));

            var ofxBankAccount = (OFXBankAccount)ofxDocument.Account;

            Assert.AreEqual("20851360276960", ofxBankAccount.AccountID);
            Assert.AreEqual("492900", ofxBankAccount.BankID);
            Assert.AreEqual(OFXBankAccountType.CHECKING, ofxBankAccount.BankAccountType);
            Assert.AreEqual(OFXAccountType.BANK, ofxDocument.AccountType);
            Assert.AreEqual(1407.95, ofxDocument.Balance.LedgerBalance);
            Assert.AreEqual("GBP", ofxDocument.Currency);
            Assert.IsNotNull(ofxDocument.SignOn);
            Assert.AreEqual(20120519103213, ofxDocument.SignOn.DTServer);
            Assert.AreEqual("ENG", ofxDocument.SignOn.Language);
            Assert.AreEqual(0, ofxDocument.SignOn.StatusCode);
            Assert.AreEqual("INFO", ofxDocument.SignOn.StatusSeverity);
            Assert.AreEqual(new DateTime(2012, 04, 29), ofxDocument.StatementStart.Date);
            Assert.AreEqual(new DateTime(2012, 05, 19), ofxDocument.StatementEnd.Date);
            Assert.IsNotNull(ofxDocument.Transactions);
            Assert.AreEqual(24, ofxDocument.Transactions.Count);
        }

        [TestMethod]
        public void TestValidWellFormedOfxFile()
        {
            var ofxDocumentParser = new OFXDocumentParser();
            var ofxDocument = ofxDocumentParser.Import(new FileStream(OfxValidWellFormedFile, FileMode.Open));

            Assert.IsNotNull(ofxDocument);
            Assert.IsNotNull(ofxDocument.Account);
            Assert.IsInstanceOfType(ofxDocument.Account, typeof(OFXBankAccount));

            var ofxBankAccount = (OFXBankAccount)ofxDocument.Account;

            Assert.AreEqual("40030090006122", ofxBankAccount.AccountID);
            Assert.AreEqual("400300", ofxBankAccount.BankID);
            Assert.AreEqual(OFXBankAccountType.CHECKING, ofxBankAccount.BankAccountType);
            Assert.AreEqual(OFXAccountType.BANK, ofxDocument.AccountType);
            Assert.AreEqual(674.93, ofxDocument.Balance.LedgerBalance);
            Assert.AreEqual("GBP", ofxDocument.Currency);
            Assert.IsNotNull(ofxDocument.SignOn);
            Assert.AreEqual(20120519085001, ofxDocument.SignOn.DTServer);
            Assert.AreEqual("ENG", ofxDocument.SignOn.Language);
            Assert.AreEqual(0, ofxDocument.SignOn.StatusCode);
            Assert.AreEqual("INFO", ofxDocument.SignOn.StatusSeverity);
            Assert.AreEqual(new DateTime(2012, 05, 04), ofxDocument.StatementStart.Date);
            Assert.AreEqual(new DateTime(2012, 05, 19), ofxDocument.StatementEnd.Date);
            Assert.IsNotNull(ofxDocument.Transactions);
            Assert.AreEqual(22, ofxDocument.Transactions.Count);
        }
    }
}
