using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CloudCash.Client.Modules.Sell.Controls
{
    public sealed partial class TableSellSplit : UserControl
    {
        public TableSellSplit()
        {
            this.InitializeComponent();
        }

        public int GetSellSplitCount()
        {
            return (int)SplitCount.Value;
        }
    }
}
