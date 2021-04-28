using CloudCash.BL.DTOs.TableInfo;

namespace CloudCash.Client.Modules.TableInfo.Messages
{
    public record UpdateTableInfoMsg
    {
        public TableInfoDetailModel TableInfoData;

        public UpdateTableInfoMsg(TableInfoDetailModel tableInfoData) => TableInfoData = tableInfoData;
    }
}
