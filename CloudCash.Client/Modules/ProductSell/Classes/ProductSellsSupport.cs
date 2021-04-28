using CloudCash.BL.DTOs.Products;
using CloudCash.BL.DTOs.Sells;
using CloudCash.Common.Functions;
using System.Collections.Generic;
using System.Linq;

namespace CloudCash.Client.Modules.ProductSell.Classes
{
    public static class ProductSellsSupport
    {
        public static List<ProductSellsModel> ConvertSellsToProductSells(List<SellDetailModel> sells)
        {
            var res = new List<ProductSellsModel>();

            foreach (var sell in sells)
            {
                var productSell = res.FirstOrDefault(x => x.Product.ID == sell.Product.ID && x.Discount == sell.Discount);

                if (productSell is null)
                    res.Add(new(sell));
                else
                    productSell.AddSell(sell);
            }

            return res;
        }

        public static void AddSellToProductSell(this object productSells, SellDetailModel sell)
        {
            var productSellsCollection = productSells.CheckType<ICollection<ProductSellsModel>>();
            var productSell = productSellsCollection.FirstOrDefault(x => x.Product.ID == sell.Product.ID && x.Discount == sell.Discount);

            if (productSell is null)
                productSellsCollection.Add(new(sell));
            else
                productSell.AddSell(sell);
        }

        public static void AddSellsToProductSell(this object productSells, ProductListModel newItemToSell, List<SellDetailModel> sells)
        {
            var productSellsCollection = productSells.CheckType<ICollection<ProductSellsModel>>();
            var productSell = productSellsCollection.FirstOrDefault(x => x.Product.ID == newItemToSell.ID && x.Discount == sells.First().Discount);

            foreach (var sell in sells)
            {
                if (productSell is null)
                    productSellsCollection.Add(new(sell));
                else
                    productSell.AddSell(sell);
            }
        }

        public static void RemoveSellFromProductSell(this object productSells, SellDetailModel sell)
        {
            var productSellsCollection = productSells.CheckType<ICollection<ProductSellsModel>>();
            var productSell = productSellsCollection.FirstOrDefault(x => x.Product.ID == sell.Product.ID && x.Discount == sell.Discount);

            productSell.RemoveSell();

            if (productSell.IsEmpty)
                productSellsCollection.Remove(productSell);
        }

        public static void RemoveAllSellsFromProductSell(this object productSells, SellDetailModel referenceSell)
        {
            var productSellsCollection = productSells.CheckType<ICollection<ProductSellsModel>>();
            var productSell = productSellsCollection.FirstOrDefault(x => x.Product.ID == referenceSell.Product.ID && x.Discount == referenceSell.Discount);

            productSell.RemoveAllSells();
            productSellsCollection.Remove(productSell);
        }
    }
}
