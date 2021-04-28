using CloudCash.BL.DTOs.Customers;
using CloudCash.BL.DTOs.Sells;
using CloudCash.Common.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CloudCash.BL.DTOs.Payments
{
    public record PaymentDetailModel : PaymentListModel
    {
        public ICollection<SellDetailModel> Sells { get; set; } = new ObservableCollection<SellDetailModel>();

        public CustomerListModel Customer { get; set; } = new();

        public byte Discount { get; set; }

        public PaymentType PaymentType { get; set; } = PaymentType.Cash;

        public string Note { get; set; } = string.Empty;
    }
}
