/*using System;
using System.Collections.Generic;
using System.Linq;
using DL.AccountChecker.BusinessLogic;
using DL.AccountChecker.Domain;
using DL.Framework.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DL.AccountChecker.Test
{
    [TestClass]
    public class TransactionLoaderTest
    {
        private CategoryManager _categoryManager;

        [TestInitialize]
        public void Initialize()
        {
            var mockCategoryRepository = new Mock<CategoryRepository>();
            mockCategoryRepository.Setup(m => m.LoadAll()).Returns(new List<Category>
            {
                new Category(1) { CategoryName = "fox", Status = Status.Active },
                new Category(1) { CategoryName = "quick", Status = Status.Active },
                new Category(1) { CategoryName = "the", Status = Status.Active },
                new Category(1) { CategoryName = "random", Status = Status.Active },
                new Category(1) { CategoryName = "invalid", Status = Status.Inactive },
            });

            _categoryManager = new CategoryManager(mockCategoryRepository.Object);
            _categoryManager.LoadAll();
        }

        [TestMethod]
        public void UpdateCategoriesWithNoExistingCategories()
        {
            var transaction = new Transaction(1)
            {
                Description = "The quick brown fox"
            };

            TransactionLoader.UpdateCategories(_categoryManager, transaction);

            Assert.IsNotNull(transaction.Categories);
            Assert.AreEqual(3, transaction.Categories.Count);
        }

        [TestMethod]
        public void UpdateCategoriesWithExistingCategories()
        {
            var transaction = new Transaction(1)
            {
                Description = "The quick brown fox",
                Categories = new List<Category>
                {
                    new Category(1)
                    {
                        CategoryName = "not-existing"
                    }
                }
            };

            TransactionLoader.UpdateCategories(_categoryManager, transaction);

            Assert.IsNotNull(transaction.Categories);
            Assert.AreEqual(4, transaction.Categories.Count);
        }

        [TestMethod]
        public void UpdateCategoriesUnionWithExistingCategories()
        {
            var transaction = new Transaction(1)
            {
                Description = "The quick brown fox",
                Categories = new List<Category>
                {
                    new Category(1)
                    {
                        CategoryName = "fox",
                    }
                }
            };

            TransactionLoader.UpdateCategories(_categoryManager, transaction);

            Assert.IsNotNull(transaction.Categories);
            Assert.AreEqual(3, transaction.Categories.Count);
        }
    }
}*/
