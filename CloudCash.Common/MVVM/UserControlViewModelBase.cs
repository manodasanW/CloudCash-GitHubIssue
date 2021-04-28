using CloudCash.Common.ModelBase;
using CloudCash.Interface.BL;
using CloudCash.Interface.Common;

namespace CloudCash.Common.MVVM
{
    public abstract class UserControlViewModelBase<T> : ViewModelBase, IUserControlViewModelBase where T : ListModelBase, new()
    {
        protected T _data = new();
        public T Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

        public UserControlViewModelBase(IMessenger messenger, T data) : base(messenger)
        {
            if (data is not null)
                _data = data;
        }

        public IListModelBase GetData() => Data;

        public abstract void CheckData();

        public virtual void DoBeforeCheck() { }
    }
}
