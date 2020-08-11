using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastPOS.Core.Tests
{
    [TestClass]
    public class SalesOrderTests
    {
        private static IDictionary<string, ProductInfo> Products { get; set; }

        private SalesOrder SalesOrder { get; set; }


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
            SalesOrder = new SalesOrder(new Location(Country.BOLIVIA, City.COCHABAMBA));
        }

        [DataTestMethod]
        [DataRow("Entel10", 10)]
        [DataRow("Entel100", 100)]
        [DataRow("Entel50", 50)]
        public void AddItemsToSalesOrder(string product, int quantity)
        {
            //Act
            SalesOrder.AddProduct(Products[product], quantity);

            //Assert
            Assert.AreEqual(1, SalesOrder.ProductStockItems.Count);
            Assert.AreEqual(quantity, SalesOrder.ProductStockItems.ElementAt(0).Quantity);
        }

        [DataTestMethod]
        [DataRow("Entel10", 10)]
        [DataRow("Entel100", 100)]
        [DataRow("Entel50", 50)]
        public void AddItemsToSalesOrderMultipleTimes(string product, int quantity)
        {
            //Act
            SalesOrder.AddProduct(Products[product], quantity);
            SalesOrder.AddProduct(Products[product], quantity);

            //Assert
            Assert.AreEqual(1, SalesOrder.ProductStockItems.Count);
            Assert.AreEqual(quantity * 2, SalesOrder.ProductStockItems.ElementAt(0).Quantity);
        }

        [DataTestMethod]
        [DataRow("Entel10", 10)]
        [DataRow("Entel100", 100)]
        [DataRow("Entel50", 50)]
        public void DecreaseItemsFromSalesOrder(string product, int quantity)
        {
            //Act
            SalesOrder.AddProduct(Products[product], quantity);
            SalesOrder.DecreaseProduct(Products[product], quantity / 2);

            //Assert
            Assert.AreEqual(1, SalesOrder.ProductStockItems.Count);
            Assert.AreEqual(quantity - (quantity / 2), SalesOrder.ProductStockItems.ElementAt(0).Quantity);
        }

        [DataTestMethod]
        [DataRow("Entel10", 10)]
        [DataRow("Entel100", 100)]
        [DataRow("Entel50", 50)]
        public void DecreaseAllItemsFromSalesOrder_MakeItemDisapear(string product, int quantity)
        {
            //Act
            SalesOrder.AddProduct(Products[product], quantity);
            SalesOrder.DecreaseProduct(Products[product], quantity / 2);
            SalesOrder.DecreaseProduct(Products[product], quantity / 2);

            //Assert
            Assert.AreEqual(0, SalesOrder.ProductStockItems.Count);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMultipleProductData), DynamicDataSourceType.Method)]
        public void SalesOrderSubTotal_ReturnCorrectCalculations((string name, int quantity)[] productsToAdd,
                                                                 (decimal expectedSubtotal, decimal, decimal) expectedValues)
        {
            //Act   
            foreach (var (name, quantity) in productsToAdd)
            {
                SalesOrder.AddProduct(Products[name], quantity);
            }


            //Assert
            Assert.AreEqual(expectedValues.expectedSubtotal, SalesOrder.GetSubTotal());
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMultipleProductData), DynamicDataSourceType.Method)]
        public void SalesOrderTaxes_ReturnCorrectCalculations((string name, int quantity)[] productsToAdd,
                                                              (decimal, decimal expectedTaxes, decimal) expectedValues)
        {
            //Act   
            foreach (var (name, quantity) in productsToAdd)
            {
                SalesOrder.AddProduct(Products[name], quantity);
            }


            //Assert
            Assert.AreEqual(expectedValues.expectedTaxes, SalesOrder.GetTaxes());
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMultipleProductData), DynamicDataSourceType.Method)]
        public void SalesOrderTotal_ReturnCorrectCalculations((string name, int quantity)[] productsToAdd,
                                                              (decimal, decimal, decimal expectedTotal) expectedValues)
        {
            //Act   
            foreach (var (name, quantity) in productsToAdd)
            {
                SalesOrder.AddProduct(Products[name], quantity);
            }


            //Assert
            Assert.AreEqual(expectedValues.expectedTotal, SalesOrder.GetTotal());
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPaymentChangeData), DynamicDataSourceType.Method)]
        public void SalerOrderReturnCorrectChange_OnPaymentReceived(decimal amountToPay, decimal expectedChange)
        {
            //Prepare
            foreach (var item in Products.Values)
            {
                SalesOrder.AddProduct(item, 1);
            }

            //Act
            Money change = SalesOrder.AddRecievedAmount(amountToPay);


            //Assert
            Assert.AreEqual(expectedChange, change);
        }

        [TestMethod]
        public void SalerOrderFinish_Succesfully()
        {
            //Prepare
            foreach (var item in Products.Values)
            {
                SalesOrder.AddProduct(item, 1);
            }
            SalesOrder.AddRecievedAmount(203.4m);

            //Act
            SalesOrder.CompleteSalesOrder();

            //Assert
            Assert.AreEqual(SalesOrderStatus.Fulfilled, SalesOrder.Status);
        }

        [TestMethod]
        public void SalerOrderFinish_Unsuccesfully()
        {
            //Prepare
            foreach (var item in Products.Values)
            {
                SalesOrder.AddProduct(item, 1);
            }
            SalesOrder.AddRecievedAmount(10m);

            //Act 
            void completeSalerOrder() => SalesOrder.CompleteSalesOrder();

            //Assert
            Assert.ThrowsException<InvalidOperationException>(() => completeSalerOrder());
        }

        private static IEnumerable<object[]> GetPaymentChangeData()
        {
            yield return new object[] { 203.4m, 0m };
            yield return new object[] { 250m, 46.6m };
            yield return new object[] { 100m, 0m };
        }

        private static IEnumerable<object[]> GetMultipleProductData()
        {
            yield return new object[] { new (string, int)[] { ("Entel10", 20), ("Entel100", 1), ("Entel20", 3) }, (360m, 46.8m, 406.8m) };
            yield return new object[] { new (string, int)[] { ("Entel10", 1), ("Entel100", 1), ("Entel20", 3) }, (170m, 22.1m, 192.1m) };
        }
    }
}