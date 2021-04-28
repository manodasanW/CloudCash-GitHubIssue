using CloudCash.BL.DTOs.Sells;

namespace CloudCash.Client.Modules.Table.Messages
{
    public record SellAddedToTableMsg
    {
        public long TableId { get; }

        public SellDetailModel AddedSell { get; }

        public SellAddedToTableMsg(long tableId, SellDetailModel addedSell)
        {
            TableId = tableId;
            AddedSell = addedSell;
        }
    }
}
