using CloudCash.BL.DTOs.TableInfo;

namespace CloudCash.Client.Modules.Settings.Messages
{
    public record SelectedTableInfoChangedMsg
    {
        public TableInfoDetailModel SelectedTableInfo;

        public SelectedTableInfoChangedMsg(TableInfoDetailModel tableInfoDetailModel) => SelectedTableInfo = tableInfoDetailModel;
    }
}
