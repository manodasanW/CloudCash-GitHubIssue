using CloudCash.BL.DTOs.Payments;
using CloudCash.BL.DTOs.Products;
using CloudCash.BL.DTOs.Tables;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.ModelBase;
using System;

namespace CloudCash.BL.DTOs.Sells
{
    public record SellDetailModel : ListModelBase
    {
        public TableListModel Table { get; set; } = new();

        public byte Discount { get; set; }

        public ProductListModel Product { get; set; } = new();

        public int Count { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public long SpendedCash { get => Product.Price * Count; }

        public PaymentListModel Payment { get; set; } = new();

        public override void CheckValues()
        {
            if (Table is null)
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(Table))),
                    Localization.GetLocalizedString(LocalizationStrings.NotInserted));

            if (Product is null)
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(Product))),
                    Localization.GetLocalizedString(LocalizationStrings.NotInserted));

            if (Count is 0)
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(Count))),
                    Localization.GetLocalizedString(LocalizationStrings.ZeroValue));

            if (Discount > 100)
                throw new Common.Exceptions.ValidationException(
                    Localization.GetLocalizedString(GetPropertyStringValue(nameof(Count))),
                    Localization.GetLocalizedString(LocalizationStrings.ZeroValue)); // todo

            // todo
        }

        public SellDetailModel CopyToDetail() => new()
        {
            ID = ID,
            Product = Product,
            Discount = Discount,
            Table = Table
        };
    }
}
