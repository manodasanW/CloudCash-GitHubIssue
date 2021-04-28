using CloudCash.Interface.BL;

namespace CloudCash.Interface.Common
{
    public interface IUserControlViewModelBase
    {
        IListModelBase GetData();

        void CheckData();

        void DoBeforeCheck();
    }
}
