using CloudCash.Client.Modules.MenuItem.Enums;
using CloudCash.Client.Modules.MenuItem.Items.Base;
using CloudCash.Common.Functions;
using CloudCash.Interface.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudCash.Client.Modules.MenuItem.Factory
{
    public static class MenuItemFactory
    {
        public static UnfinishedMenuItem CreateMenuItem(MenuItemType type, MenuItemSize size, IMessenger messenger) => Activator.CreateInstance(type.GetDataClass(), size, messenger) as UnfinishedMenuItem;

        public static List<UnfinishedMenuItem> CreateAllMenuItems(IMessenger messenger, MenuItemSize size)
        {
            List<UnfinishedMenuItem> menuItems = new();

            foreach (var menuItemType in Enum.GetValues(typeof(MenuItemType)).Cast<MenuItemType>().ToList())
            {
                if (menuItemType == MenuItemType.None)
                    continue;

                menuItems.Add(CreateMenuItem(menuItemType, size, messenger));
            }

            return menuItems;
        }
    }
}
