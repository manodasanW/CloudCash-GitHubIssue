using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Common.Functions;

namespace CloudCash.Client.Modules.MenuItem.Items.Base
{
    public abstract class LogInOutMenuItemBase : UnfinishedMenuItem
    {
        public LogInOutMenuItemBase(string icon, string title, int widgetLinesNumber, MenuItemSize size, Messenger messenger) : base(icon, title, widgetLinesNumber, size, messenger) { }
    }
}
