using System;

namespace CloudCash.Common.MessagesBase
{
    public record ItemBaseMsg
    {
        public Type ItemType { get; set; }

        public ItemBaseMsg(Type itemType) => ItemType = itemType;
    }
}
