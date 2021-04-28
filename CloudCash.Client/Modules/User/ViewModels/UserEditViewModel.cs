using CloudCash.BL.DTOs.Users;
using CloudCash.Client.Modules.User.Records;
using CloudCash.Common.Enums;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CloudCash.Client.Modules.User.ViewModels
{
    public class UserEditViewModel : UserControlViewModelBase<UserDetailModel>
    {
        private ObservableCollection<UserListModel> _users;

        #region Props

        private bool _showPasswordInsert;
        public bool ShowPasswordInsert
        {
            get => _showPasswordInsert;
            set
            {
                _showPasswordInsert = value;
                OnPropertyChanged();
            }
        }

        private string _oldPassword;
        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                _oldPassword = value;
                OnPropertyChanged();
            }
        }

        private bool _allowUpdateUserData;
        public bool AllowUpdateUserData
        {
            get => _allowUpdateUserData;
            set
            {
                _allowUpdateUserData = value;
                OnPropertyChanged();
            }
        }

        private bool _allowUpdateRights;
        public bool AllowUpdateRights
        {
            get => _allowUpdateRights;
            set
            {
                _allowUpdateRights = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<UsersRightsCollectionItem> _allRights = new();
        public ObservableCollection<UsersRightsCollectionItem> AllRights
        {
            get => _allRights;
            set
            {
                _allRights = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Interface

        public UserEditViewModel(IMessenger messenger, ObservableCollection<UserListModel> users, UserDetailModel data = null,
            bool allowUpdateUserData = true, bool allowUpdateRights = true) : base(messenger, data)
        {
            _users = users;
            AllowUpdateUserData = allowUpdateUserData;
            AllowUpdateRights = allowUpdateRights;

            ShowPasswordInsert = data is null;
            LoadRights();

            if (data is not null)
                SetUsedRights();
        }

        public override void CheckData() => Data.CheckValues(new(_users.Select(x => UserDetailModel.ConvertToUserDetailModel(x)).ToList()), ShowPasswordInsert);

        public override void DoBeforeCheck()
        {
            base.DoBeforeCheck();

            Data.Rights = AllRights.Where(x => x.IsSelected).Select(x => x.Right).ToList().EncodeRights();
            Data.DoBeforeCheck();
        }

        #endregion

        #region Private

        private void SetUsedRights()
        {
            var usedRights = Data.Rights.DecodeRights();

            foreach (var usedRight in usedRights)
            {
                UsersRightsCollectionItem right = AllRights.FirstOrDefault(x => x.Right == usedRight);

                if (right is not null)
                    right.IsSelected = true;
            }

            OnPropertyChanged(nameof(AllRights));
        }

        private void LoadRights()
        {
            foreach (var right in Enum.GetValues(typeof(Right)).Cast<Right>().ToList())
            {
                if (right is Right.None)
                    continue;

                AllRights.Add(new UsersRightsCollectionItem(right));
            }
        }

        #endregion
    }
}
