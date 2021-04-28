using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Interface.Common;

namespace CloudCash.Client.Modules.MenuItem.Items.Base
{
    public abstract class ShiftMenuItemBase : UnfinishedMenuItem
    {
        public ShiftMenuItemBase(string icon, string title, int widgetLinesNumber, MenuItemSize size, IMessenger messenger) : base(icon, title, widgetLinesNumber, size, messenger) { }
    }
}
