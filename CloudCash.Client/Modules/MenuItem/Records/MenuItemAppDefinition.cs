using CloudCash.Client.Modules.MenuItem.Enums;

namespace CloudCash.Client.Modules.MenuItem.Records
{
    public record MenuItemAppDefinition
    {
        public MenuItemType Type { get; set; }

        public MenuItemSize Size { get; set; }

        public MenuItemAppDefinition(MenuItemType type, MenuItemSize size)
        {
            Type = type;
            Size = size;
        }
    }
}
