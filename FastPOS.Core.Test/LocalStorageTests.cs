using Microsoft.VisualStudio.TestTools.UnitTesting;
using FastPOS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPOS.Core.Tests
{
    [TestClass]
    public class LocalStorageTests
    {
        private static IDictionary<string, ProductInfo> Products { get; set;}
        
        private LocalStorage LocalStorage { get; set; }


        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            Products = new Dictionary<string, ProductInfo>
            {
                { "Entel10", new ProductInfo("Entel 10", 10m) },
                { "Entel100", new ProductInfo("Entel 100", 100m) },
                { "Entel20", new ProductInfo("Entel 20", 20m) },
                { "Entel50", new ProductInfo("Entel 50", 50m) },
            };
        }


        [TestInitialize]
        public void TestInitialize()
        {
            LocalStorage = new LocalStorage();
        }


        [DataTestMethod]
        [DataRow("Entel10", 10)]
        [DataRow("Entel100", 100)]
        [DataRow("Entel50", 50)]
        public void AddItemsToLocalStorage(string product, int quantity)
        {
            //Act
            LocalStorage.AddProduct(Products[product], quantity);

            //Assert
            Assert.AreEqual(1, LocalStorage.ProductStockItems.Count);
            Assert.AreEqual(quantity, LocalStorage.ProductStockItems.ElementAt(0).Quantity);
        }

        [DataTestMethod]
        [DataRow("Entel10", 10)]
        [DataRow("Entel100", 100)]
        [DataRow("Entel50", 50)]
        public void AddItemsToLocalStorageMultipleTimes(string product, int quantity)
        {
            //Act
            LocalStorage.AddProduct(Products[product], quantity);
            LocalStorage.AddProduct(Products[product], quantity);

            //Assert
            Assert.AreEqual(1, LocalStorage.ProductStockItems.Count);
            Assert.AreEqual(quantity * 2, LocalStorage.ProductStockItems.ElementAt(0).Quantity);
        }

        [DataTestMethod]
        [DataRow("Entel10", 10)]
        [DataRow("Entel100", 100)]
        [DataRow("Entel50", 50)]
        public void DecreaseItemsFromLocalStorage(string product, int quantity)
        {
            //Act
            LocalStorage.AddProduct(Products[product], quantity);
            LocalStorage.DecreaseProduct(Products[product], quantity / 2);

            //Assert
            Assert.AreEqual(1, LocalStorage.ProductStockItems.Count);
            Assert.AreEqual(quantity - (quantity / 2), LocalStorage.ProductStockItems.ElementAt(0).Quantity);
        }

        [DataTestMethod]
        [DataRow("Entel10", 10)]
        [DataRow("Entel100", 100)]
        [DataRow("Entel50", 50)]
        public void DecreaseAllItemsFromSalesOrder_DoesNotMakeItemDisapear(string product, int quantity)
        {
            //Act
            LocalStorage.AddProduct(Products[product], quantity);
            LocalStorage.DecreaseProduct(Products[product], quantity / 2);
            LocalStorage.DecreaseProduct(Products[product], quantity / 2);

            //Assert
            Assert.AreEqual(1, LocalStorage.ProductStockItems.Count);
            Assert.AreEqual(0, LocalStorage.ProductStockItems.ElementAt(0).Quantity);
        }
    }
}