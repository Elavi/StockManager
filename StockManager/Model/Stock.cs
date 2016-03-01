using System;
using System.ComponentModel;
using System.Windows.Media;

namespace StockManager.Model
{
    public class Stock : INotifyPropertyChanged
    {
        public StockType Type { get; set; }
        public String Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double MarketValue { get; set; }
        public double TransactionCost { get; set; }
        public Brush ColorName { get; set; }

        private double stockWeight;
        public double StockWeight
        {
            get { return stockWeight; } 
            set
            {
                stockWeight = value;
                OnPropertyChanged("StockWeight");
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
