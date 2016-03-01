using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using StockManager.Model;
using System.Windows.Media;

namespace StockManager.Tests
{
    [TestClass]
    public class StockPanelViewModelTests
    {
        [TestMethod]
        public void GenerateName_ForEquity_EmptyCollection()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            var collection = new ObservableCollection<Stock>();

            //Act
            var result = viewModel.GenerateName(StockType.Equity, collection);

            //Assert
            Assert.AreEqual(result, "Equity1");
        }

        [TestMethod]
        public void GenerateName_ForEquity_NotEmptyCollection()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            var collection = new ObservableCollection<Stock>();
            collection.Add(new Stock { Type = StockType.Equity });

            //Act
            var result = viewModel.GenerateName(StockType.Equity, collection);

            //Assert
            Assert.AreEqual(result, "Equity2");
        }

        [TestMethod]
        public void GenerateName_ForBond_EmptyCollection()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            var collection = new ObservableCollection<Stock>();

            //Act
            var result = viewModel.GenerateName(StockType.Bond, collection);

            //Assert
            Assert.AreEqual(result, "Bond1");
        }

        [TestMethod]
        public void GenerateName_ForBond_NotEmptyCollection()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            var collection = new ObservableCollection<Stock>();
            collection.Add(new Stock { Type = StockType.Bond });

            //Act
            var result = viewModel.GenerateName(StockType.Bond, collection);

            //Assert
            Assert.AreEqual(result, "Bond2");
        }

        [TestMethod]
        public void GenerateName_ForEquity_NullCollection()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();

            //Act
            var result = viewModel.GenerateName(StockType.Equity, null);

            //Assert
            Assert.AreEqual(result, string.Empty);
        }
        
        [TestMethod]
        public void GenerateMarketValue_QuantityAboveZero()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            double price = 5.0;
            int quantity = 10;

            //Act
            var result = viewModel.GenerateMarketValue(price, quantity);

            //Assert
            Assert.AreEqual(result, 50.0);
        }

        [TestMethod]
        public void GenerateMarketValue_QuantityBelowZero()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            double price = 5.0;
            int quantity = -10;

            //Act
            var result = viewModel.GenerateMarketValue(price, quantity);

            //Assert
            Assert.AreEqual(result, -50.0);
        }

        [TestMethod]
        public void GenerateStockWeight_AboveZeroMarkedValue_NotEmptyCollection()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            var collection = new ObservableCollection<Stock>();
            collection.Add(new Stock { MarketValue = 1.0 });

            //Act
            var result = viewModel.GenerateStockWeight(1.0, collection);

            //Assert
            Assert.AreEqual(result, 100);
        }

        [TestMethod]
        public void GenerateStockWeight_ZeroMarkedValue_NotEmptyCollection()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            var collection = new ObservableCollection<Stock>();
            collection.Add(new Stock { MarketValue = 0.0 });

            //Act
            var result = viewModel.GenerateStockWeight(0.0, collection);

            //Assert
            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void GenerateStockWeight_NotZeroMarkedValue_EmptyCollection()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();

            //Act
            var result = viewModel.GenerateStockWeight(1.0, null);

            //Assert
            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void RecalculateStockWeigth_NotEmptyCollection()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            var collection = new ObservableCollection<Stock>();
            collection.Add( new Stock { MarketValue = 3.0 });
            collection.Add( new Stock { MarketValue = 2.0 });

            //Act
            var result = viewModel.RecalculateStockWeigth(collection);

            //Assert
            Assert.AreEqual(result[0].StockWeight, 60);
            Assert.AreEqual(result[1].StockWeight, 40);
        }

        [TestMethod]
        public void RecalculateStockWeigth_EmptyCollection()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();

            //Act
            var result = viewModel.RecalculateStockWeigth(null);

            //Assert
            Assert.AreEqual(result, null);
        }
        
        [TestMethod]
        public void GenerateTransactionCost_ForEquity()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            var marketValue = 200.0;

            //Act
            var result = viewModel.GenerateTransactionCost(StockType.Equity, marketValue);

            //Assert
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void GenerateTransactionCost_ForBond()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            var marketValue = 200.0;

            //Act
            var result = viewModel.GenerateTransactionCost(StockType.Bond, marketValue);

            //Assert
            Assert.AreEqual(result, 4);
        }

        [TestMethod]
        public void GenerateColor_MarketValueUnderZero_ReturnsRed()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            var marketValue = -1;
            var transactionCost = 0;

            //Act
            var result = viewModel.GenerateColor(StockType.Equity, marketValue, transactionCost);

            //Assert
            Assert.AreEqual(result, Brushes.Red);
        }

        [TestMethod]
        public void GenerateColor_MarketValueAboveZeroOrZero_ReturnsBlack()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            var marketValue = 5;
            var transactionCost = 0;

            //Act
            var result = viewModel.GenerateColor(StockType.Equity, marketValue, transactionCost);

            //Assert
            Assert.AreEqual(result, Brushes.Black);
        }

        [TestMethod]
        public void GenerateColor_ForEquityAndHighTransactionCost_ReturnsRed()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            var marketValue = 5;
            var transactionCost = 200001;

            //Act
            var result = viewModel.GenerateColor(StockType.Equity, marketValue, transactionCost);

            //Assert
            Assert.AreEqual(result, Brushes.Red);
        }

        [TestMethod]
        public void GenerateColor_ForEBondAndHighTransactionCost_ReturnsRed()
        {
            //Arrange
            var viewModel = new StockPanelViewModel();
            var marketValue = 5;
            var transactionCost = 100001;

            //Act
            var result = viewModel.GenerateColor(StockType.Bond, marketValue, transactionCost);

            //Assert
            Assert.AreEqual(result, Brushes.Red);
        }
    }
}
