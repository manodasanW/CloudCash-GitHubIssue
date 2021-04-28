using CloudCash.BL.DTOs.Products;
using CloudCash.BL.DTOs.Sells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CloudCash.Client.Modules.ProductSell.Classes
{
    public record ProductSellsModel : INotifyPropertyChanged
    {
        public int SellCount => Sells?.Count ?? 0;

        public ProductListModel Product { get; }

        public List<SellDetailModel> Sells { get; private set; }

        public byte Discount => Sells.FirstOrDefault()?.Discount ?? 0;

        public uint SpendedCash => (uint)Math.Round(Product.Price * SellCount * ((100 - Discount) / 100.0), 0, MidpointRounding.AwayFromZero);

        public bool IsEmpty => Sells.Count == 0;

        public ProductSellsModel(ProductListModel product) => Product = product;

        public ProductSellsModel(ProductListModel product, List<SellDetailModel> sells) : this(product) => Sells = sells;

        public ProductSellsModel(SellDetailModel sell) : this(sell.Product) => Sells = new()
        {
            sell
        };

        public void AddSell(SellDetailModel sell)
        {
            Sells.Add(sell);

            NotifyUpdateAll();
        }

        public SellDetailModel RemoveSell()
        {
            var sellToDelete = Sells.First();

            if (!IsEmpty)
                Sells.Remove(sellToDelete);

            NotifyUpdateAll();

            return sellToDelete;
        }

        public void RemoveAllSells()
        {
            if (!IsEmpty)
                Sells.Clear();

            NotifyUpdateAll();
        }

        public void SetSells(List<SellDetailModel> sells)
        {
            Sells.Clear();
            Sells.AddRange(sells);

            NotifyUpdateAll();
        }

        private void NotifyUpdateAll()
        {
            OnPropertyChanged(nameof(Sells));
            OnPropertyChanged(nameof(Discount));
            OnPropertyChanged(nameof(SellCount));
            OnPropertyChanged(nameof(SpendedCash));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string caller = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
    }
}
