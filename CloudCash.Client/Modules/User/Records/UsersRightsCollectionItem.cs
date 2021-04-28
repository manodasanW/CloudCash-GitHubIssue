using CloudCash.Common.Enums;

namespace CloudCash.Client.Modules.User.Records
{
    public record UsersRightsCollectionItem
    {
        public Right Right { get; set; }

        public bool IsSelected { get; set; }

        public UsersRightsCollectionItem(Right right) : this(right, false) { }

        public UsersRightsCollectionItem(Right right, bool isSelected)
        {
            Right = right;
            IsSelected = isSelected;
        }
    }
}
