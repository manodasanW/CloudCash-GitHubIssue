using CloudCash.Client.Modules.MenuItem.Enums;

namespace CloudCash.Client.Modules.MenuItem.Messages
{
    public record UpdateMenuItemMsg
    {
        public MenuItemType ItemType { get; set; }

        public string NewWidgetLine1Text { get; set; }

        public string NewWidgetLine2Text { get; set; }

        public bool IsEnabled { get; set; }

        public UpdateMenuItemMsg(MenuItemType itemType, string newWidgetLine1Text, string newWidgetLine2Text, bool isEnabled)
        {
            ItemType = itemType;
            NewWidgetLine1Text = newWidgetLine1Text;
            NewWidgetLine2Text = newWidgetLine2Text;
            IsEnabled = isEnabled;
        }
    }
}
