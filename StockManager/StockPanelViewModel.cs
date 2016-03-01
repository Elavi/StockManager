using StockManager.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Media;

namespace StockManager
{
    public class StockPanelViewModel : INotifyPropertyChanged
    {
        public StockPanelViewModel()
        {
            this.createEquityCommand = new DelegateCommand(this.CreateEquity);
            this.createBondCommand = new DelegateCommand(this.CreateBond);
            this.addCommand = new DelegateCommand(this.AddStock);
            this.cancelCommand = new DelegateCommand(this.CancelStock);
        }

        private void CreateEquity(object obj)
        {
            this.Type = StockType.Equity;
            this.IsStockCreationVisibile = true;
        }

        private void CreateBond(object obj)
        {
            this.Type = StockType.Bond;
            this.IsStockCreationVisibile = true;
        }

        private void AddStock(object obj)
        {
            if (this.Type != null && this.Price > 0 && this.Quantity != 0)
            {
                var stock = this.CreateStock((StockType)this.Type, this.Price, this.Quantity);
                this.StockCollection.Add(stock);
                this.RecalculateStockWeigth(this.StockCollection);
                this.IsStockCreationVisibile = false;
                this.ClearStockProperties();
                this.RefreshSummary();
            }
        }

        private void CancelStock(object obj)
        {
            this.ClearStockProperties();
            this.IsStockCreationVisibile = false;
        }

        public Stock CreateStock(StockType type, double price, int quantity)
        {
            var marketValue = this.GenerateMarketValue(price, quantity);
            var name = this.GenerateName(type, this.StockCollection);
            var stockWeight = 0.0;
            var transactionCost = this.GenerateTransactionCost(type, marketValue);
            var colorName = this.GenerateColor(type, marketValue, transactionCost);
            var stockToAdd = new Stock {
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
        
        public String GenerateName(StockType type, ObservableCollection<Stock> stockCollection)
        {
            if (stockCollection != null)
            {
                var number = this.GenerateNumber(type, stockCollection);
                return type.ToString() + number;
            }
            return String.Empty;
        }

        private int GenerateNumber(StockType type, ObservableCollection<Stock> stockCollection)
        {
            return stockCollection.Count(a => a.Type == type) + 1;
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

        public ObservableCollection<Stock> RecalculateStockWeigth(ObservableCollection<Stock> stockCollection)
        {
            if (stockCollection == null)
                return null;

            foreach (var stock in stockCollection)
            {
                stock.StockWeight = GenerateStockWeight(stock.MarketValue, stockCollection);
            }
            return stockCollection;
        }

        public Brush GenerateColor (StockType type, double marketValue, double transactionCost)
        {
            var brush = Brushes.Black;
            var tolerance = type == StockType.Bond ? 100000 : 200000;
            if (marketValue < 0 || transactionCost > tolerance)
                brush = Brushes.Red;
            return brush;
        }

        private void ClearStockProperties()
        {
            this.Type = null;
            this.Price = 0;
            this.Quantity = 0;
        }

        private void RefreshSummary()
        {
            this.EquityNumber = this.StockCollection.Where(a => a.Type == StockType.Equity).Sum(a => a.Quantity);
            this.BondNumber = this.StockCollection.Where(a => a.Type == StockType.Bond).Sum(a => a.Quantity);
            this.AllNumber = this.StockCollection.Sum (a => a.Quantity);
            this.EquityStockWeight = this.StockCollection.Where( a => a.Type == StockType.Equity).Sum(a => a.StockWeight);
            this.BondStockWeight = this.StockCollection.Where(a => a.Type == StockType.Bond).Sum(a => a.StockWeight);
            this.AllStockWeight = this.StockCollection.Sum(a => a.StockWeight);
            this.EquityMarketValue = this.StockCollection.Where(a => a.Type == StockType.Equity).Sum(a => a.MarketValue);
            this.BondMarketValue = this.StockCollection.Where(a => a.Type == StockType.Bond).Sum(a => a.MarketValue);
            this.AllMarketValue = this.StockCollection.Sum(a => a.MarketValue);
        }

        private StockType? type;
        public StockType? Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }
        
        private double price;
        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        private int equityNumber;
        public int EquityNumber
        {
            get { return equityNumber; }
            set
            {
                equityNumber = value;
                OnPropertyChanged("EquityNumber");
            }
        }

        private int bondNumber;
        public int BondNumber
        {
            get { return bondNumber; }
            set
            {
                bondNumber = value;
                OnPropertyChanged("BondNumber");
            }
        }

        private int allNumber;
        public int AllNumber
        {
            get { return allNumber; }
            set
            {
                allNumber = value;
                OnPropertyChanged("AllNumber");
            }
        }

        private double equityStockWeight;
        public double EquityStockWeight
        {
            get { return equityStockWeight; }
            set
            {
                equityStockWeight = value;
                OnPropertyChanged("EquityStockWeight");
            }
        }

        private double bondStockWeight;
        public double BondStockWeight
        {
            get { return bondStockWeight; }
            set
            {
                bondStockWeight = value;
                OnPropertyChanged("BondStockWeight");
            }
        }
        private double allStockWeight;
        public double AllStockWeight
        {
            get { return allStockWeight; }
            set
            {
                allStockWeight = value;
                OnPropertyChanged("AllStockWeight");
            }
        }

        private double equityMarketValue;
        public double EquityMarketValue
        {
            get { return equityMarketValue; }
            set
            {
                equityMarketValue = value;
                OnPropertyChanged("EquityMarketValue");
            }
        }

        private double bondMarketValue;
        public double BondMarketValue
        {
            get { return bondMarketValue; }
            set
            {
                bondMarketValue = value;
                OnPropertyChanged("BondMarketValue");
            }
        }

        private double allMarketValue;
        public double AllMarketValue
        {
            get { return allMarketValue; }
            set
            {
                allMarketValue = value;
                OnPropertyChanged("AllMarketValue");
            }
        }

        private ObservableCollection<Stock> stockCollection = new ObservableCollection<Stock>();
        public ObservableCollection<Stock> StockCollection
        {
            get { return stockCollection; }
            set
            {
                stockCollection = value;
                OnPropertyChanged("StockCollection");
            }
        }

        private bool isStockCreationVisibile;
        public bool IsStockCreationVisibile
        {
            get { return isStockCreationVisibile; }
            set
            {
                isStockCreationVisibile = value;
                OnPropertyChanged("IsStockCreationVisibile");
            }
        }

        private ICommand createEquityCommand;
        public ICommand CreateEquityCommand
        {
            get { return createEquityCommand; }
            set { createEquityCommand = value; }
        }
        
        private ICommand createBondCommand;
        public ICommand CreateBondCommand
        {
            get { return createBondCommand; }
            set { createBondCommand = value; }
        }
        
        private ICommand addCommand;
        public ICommand AddCommand
        {
            get { return addCommand; }
            set { addCommand = value; }
        }

        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set { cancelCommand = value; }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
