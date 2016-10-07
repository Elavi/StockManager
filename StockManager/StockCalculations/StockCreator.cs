using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using StockManager.Model;

namespace StockManager.StockCalculations
{
    public class StockCreator
    {
        public Stock CreateStock(StockType type, double price, int quantity, int stockTypeElements)
        {
            var marketValue = this.GenerateMarketValue(price, quantity);
            var name = this.GenerateName(type, stockTypeElements);
            var stockWeight = 0.0;
            var transactionCost = this.GenerateTransactionCost(type, marketValue);
            var colorName = this.GenerateColor(type, marketValue, transactionCost);
            var stockToAdd = new Stock
            {
                Type = type,
                Name = name,
                Price = price,
                Quantity = quantity,
                MarketValue = marketValue,
                TransactionCost = transactionCost,
                StockWeight = stockWeight,
                ColorName = colorName
            };
            return stockToAdd;
        }

        public String GenerateName(StockType type, int stockTypeElements)
        {
            if (stockTypeElements >= 0)
            {
                var number = stockTypeElements + 1;
                return type.ToString() + number;
            }
            throw new InvalidDataException();
        }

        public double GenerateMarketValue(double price, int quantity)
        {
            if (price <= 0 && quantity == 0)
                return 0;
            return price * quantity;
        }

        public double GenerateTransactionCost(StockType type, double marketValue)
        {
            return type == StockType.Bond ? marketValue * 0.02 : marketValue * 0.005;
        }

        public double GenerateStockWeight(double marketValue, ObservableCollection<Stock> stockCollection)
        {
            if (stockCollection == null)
                return 0;

            var sum = stockCollection.Sum(a => a.MarketValue);
            if (sum != 0)
                return marketValue / sum * 100;
            else
                return 0;
        }

        public Brush GenerateColor(StockType type, double marketValue, double transactionCost)
        {
            var brush = Brushes.Black;
            var tolerance = type == StockType.Bond ? 100000 : 200000;
            if (marketValue < 0 || transactionCost > tolerance)
                brush = Brushes.Red;
            return brush;
        }
    }
}
