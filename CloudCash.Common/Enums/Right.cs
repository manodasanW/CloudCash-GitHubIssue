using System;

namespace CloudCash.Common.Enums
{
    [Flags]
    public enum Right : long
    {
        None = 0,
        BasicCommands = 1 << 0,
        ShiftOpen = 1 << 1,
        ShiftClose = 1 << 2,
        ShowSettings = 1 << 3
    }
}
