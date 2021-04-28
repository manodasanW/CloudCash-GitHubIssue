using CloudCash.BL.DTOs.Products;

namespace CloudCash.Client.Modules.ProductSelector.Messages
{
    public record AddProductToTableMsg
    {
        public ProductListModel NewItemToSell { get; }

        public AddProductToTableMsg(ProductListModel newItemToSell) => NewItemToSell = newItemToSell;
    }
}
