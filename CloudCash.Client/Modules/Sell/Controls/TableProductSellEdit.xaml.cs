using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CloudCash.Client.Modules.Sell.Controls
{
    public sealed partial class TableProductSellEdit : UserControl
    {
        public TableProductSellEdit(byte defaultDiscount, int defaultSellCount)
        {
            this.InitializeComponent();

            Discount.Value = defaultDiscount;
            SellCount.Value = defaultSellCount;
        }

        public (byte Discount, int Count) GetInsertedValues()
        {
            return ((byte)Discount.Value, (int)SellCount.Value);
        }
    }
}
