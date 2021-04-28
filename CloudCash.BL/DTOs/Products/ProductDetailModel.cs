using System.Collections.ObjectModel;
using System.Linq;

namespace CloudCash.BL.DTOs.Products
{
    public record ProductDetailModel : ProductListModel
    {
        public void CheckValues(ObservableCollection<ProductDetailModel> tableInfos) => base.CheckValues(new(tableInfos.Cast<ProductListModel>().ToList()));
    }
}
