using CloudCash.Client.Modules.MenuItem.Items;
using CloudCash.Common.Attributes;

namespace CloudCash.Client.Modules.MenuItem.Enums
{
    public enum MenuItemType
    {
        None,

        [DataClass(typeof(TablesMenuItem))]
        Tables,

        [DataClass(typeof(ReservationsMenuItem))]
        Reservations,

        [DataClass(typeof(PaymentsMenuItem))]
        Payments,

        [DataClass(typeof(ShiftOpenMenuItem))]
        ShiftOpen,

        [DataClass(typeof(ShiftCloseMenuItem))]
        ShiftClose,

        [DataClass(typeof(ShiftsMenuItem))]
        Shifts,

        [DataClass(typeof(StatisticsMenuItem))]
        Statistics,

        [DataClass(typeof(CustomersMenuItem))]
        Customers,

        [DataClass(typeof(CardsMenuItem))]
        Cards,

        [DataClass(typeof(ProductsMenuItem))]
        Products,

        [DataClass(typeof(UsersMenuItem))]
        Users,

        [DataClass(typeof(SettingsMenuItem))]
        Settings,

        [DataClass(typeof(LogInMenuItem))]
        LogIn,

        [DataClass(typeof(LogOutMenuItem))]
        LogOut,

        [DataClass(typeof(AppCloseMenuItem))]
        AppClose,
    }
}
