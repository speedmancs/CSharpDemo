using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedFeaturesDemo
{

    public class PriceChangedEventArgs : EventArgs
    {
        public readonly decimal LastPrice;
        public readonly decimal NewPrice;

        public PriceChangedEventArgs(decimal lastPrice, decimal newPrice)
        {
            LastPrice = lastPrice; NewPrice = newPrice;
        }
    }

    public class Stock
    {
        string symbol;
        decimal price;

        public Stock(string symbol) { this.symbol = symbol; }

        public event EventHandler<PriceChangedEventArgs> PriceChanged;

        protected virtual void OnPriceChanged(PriceChangedEventArgs e)
        {
            PriceChanged?.Invoke(this, e);
        }

        public void Test()
        {
            PriceChanged?.Invoke(this, new PriceChangedEventArgs(1,1));
        }

        public decimal Price
        {
            get { return price; }
            set
            {
                if (price == value) return;
                decimal oldPrice = price;
                price = value;
                OnPriceChanged(new PriceChangedEventArgs(oldPrice, price));
            }
        }
    }

    class EventDemo
    {
        static void Main()
        {
            Stock stock = new Stock("THPW");
            stock.Price = 27.10M;
            // Register with the PriceChanged event
            stock.PriceChanged += stock_PriceChanged;
            stock.PriceChanged += stock_PriceChanged2;
            // stock.Test();
            stock.Price = 31.59M;
        }

        static void stock_PriceChanged(object sender, PriceChangedEventArgs e)
        {
            if ((e.NewPrice - e.LastPrice) / e.LastPrice > 0.1M)
                Console.WriteLine("Alert, 10% stock price increase!");
            Console.WriteLine("aaaa");

        }
        static void stock_PriceChanged2(object sender, PriceChangedEventArgs e)
        {
            Console.WriteLine("HHH");
        }
    }
}
