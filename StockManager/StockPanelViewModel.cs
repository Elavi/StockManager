using StockManager.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using StockManager.Core;
using StockManager.StockCalculations;

namespace StockManager
{
    public class StockPanelViewModel : INotifyPropertyChanged
    {
        private readonly StockCreator stockCreator;
        const int SelectedThickness = 2;
        const int NeutralThickness = 1;
        public StockPanelViewModel()
        {
            this.createEquityCommand = new DelegateCommand(this.CreateEquity);
            this.createBondCommand = new DelegateCommand(this.CreateBond);
            this.addCommand = new DelegateCommand(this.AddStock);
            this.cancelCommand = new DelegateCommand(this.CancelStock);
            this.stockCreator = new StockCreator();
        }

        private void AddStock(object obj)
        {
            if (this.Type != null && this.Price > 0 && this.Quantity != 0)
            {
                var stockTypeElements = stockCollection.Count(a => a.Type == this.Type);
                var stock = this.stockCreator.CreateStock((StockType)this.Type, this.Price, this.Quantity, stockTypeElements);
                this.StockCollection.Add(stock);
                this.RecalculateStockWeigth(this.StockCollection);

                this.ClearStockProperties();
                this.RefreshSummary();
            }
        }

        private void CancelStock(object obj)
        {
            this.ClearStockProperties();
        }

        private void CreateEquity(object obj)
        {
            this.Type = StockType.Equity;
            this.EquityThickness = SelectedThickness;
            this.IsStockCreationVisibile = true;
        }

        private void CreateBond(object obj)
        {
            this.Type = StockType.Bond;
            this.BondThickness = SelectedThickness;
            this.IsStockCreationVisibile = true;
        }
        
        public ObservableCollection<Stock> RecalculateStockWeigth(ObservableCollection<Stock> stockCollection)
        {
            if (stockCollection == null)
                return null;

            foreach (var stock in stockCollection)
            {
                stock.StockWeight = this.stockCreator.GenerateStockWeight(stock.MarketValue, stockCollection);
            }
            return stockCollection;
        }
        

        private void ClearStockProperties()
        {
            this.Type = null;
            this.Price = 0;
            this.Quantity = 0;
            this.IsStockCreationVisibile = false;
            this.EquityThickness = NeutralThickness;
            this.BondThickness = NeutralThickness;
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

        private int equityThickness;

        public int EquityThickness
        {
            get { return equityThickness; }
            set
            {
                equityThickness = value;
                OnPropertyChanged("EquityThickness");
            }
        }

        private int bondThickness;

        public int BondThickness
        {
            get { return bondThickness; }
            set
            {
                bondThickness = value;
                OnPropertyChanged("BondThickness");
            }
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
