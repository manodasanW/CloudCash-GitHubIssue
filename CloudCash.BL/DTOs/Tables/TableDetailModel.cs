using CloudCash.BL.DTOs.Payments;
using CloudCash.BL.DTOs.Sells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CloudCash.BL.DTOs.Tables
{
    public record TableDetailModel : TableListModel
    {
        public DateTime EndDateTime { get; set; } = DateTime.MinValue;

        public ICollection<SellDetailModel> Sells { get; set; } = new ObservableCollection<SellDetailModel>();

        public ICollection<PaymentDetailModel> Payments { get; set; } = new ObservableCollection<PaymentDetailModel>();
    }
}
