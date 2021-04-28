using CloudCash.Common.Attributes;

namespace CloudCash.Common.Enums
{
    public enum LocalizationStrings
    {
        [LocalizationString("MenuItem/")]
        MenuItem,

        [LocalizationString("Settings/SettingsApp/PartName")]
        SettingsApp,

        [LocalizationString("Settings/SettingsTableCategories/PartName")]
        SettingsTableCategories,

        [LocalizationString("Settings/SettingsProductCategories/PartName")]
        SettingsProductCategories,

        [LocalizationString("Settings/SettingsProducts/PartName")]
        SettingsProducts,

        [LocalizationString("Settings/SettingsTableInfos/PartName")]
        SettingsTableInfos,

        [LocalizationString("Settings/SettingsUsers/PartName")]
        SettingsUsers,

        [LocalizationString("Settings/SettingsApp/DialogCategoryName")]
        DialogCategoryName,

        [LocalizationString("Settings/SettingsApp/PasswordCategoryName")]
        PasswordCategoryName,

        [LocalizationString("Buttons/Ok")]
        OkButton,

        [LocalizationString("Buttons/Cancel")]
        CancelButton,

        [LocalizationString("Buttons/Add")]
        AddButton,

        [LocalizationString("Buttons/Yes")]
        YesButton,

        [LocalizationString("Buttons/No")]
        NoButton,

        [LocalizationString("Buttons/Edit")]
        EditButton,

        [LocalizationString("Buttons/Sign")]
        SignButton,

        [LocalizationString("Buttons/Logout")]
        LogoutButton,

        [LocalizationString("Validation/NullOrEmpty")]
        NullOrEmpty,

        [LocalizationString("Validation/DuplicatedValue")]
        DuplicatedValue,

        [LocalizationString("Validation/ZeroValue")]
        ZeroValue,

        [LocalizationString("Validation/BelowZero")]
        BelowZero,

        [LocalizationString("Validation/NotInserted")]
        NotInserted,

        [LocalizationString("Validation/TooShort")]
        TooShort,

        [LocalizationString("Validation/AreNotSame")]
        AreNotSame,

        [LocalizationString("Validation/PasswordIncorrect")]
        PasswordIncorrect,

        [LocalizationString("Validation/UserNotExist")]
        UserNotExist,

        [LocalizationString("Validation/NickPasswordMismatch")]
        NickPasswordMismatch,

        [LocalizationString("ContentDialog/Remove/Text")]
        ContentDialogRemoveText,

        [LocalizationString("ContentDialog/Remove/Header")]
        ContentDialogRemoveHeader,

        [LocalizationString("ContentDialog/LogOut/Text")]
        ContentDialogLogOutText,

        [LocalizationString("ContentDialog/LogOut/Header")]
        ContentDialogLogOutHeader,

        [LocalizationString("ContentDialog/ConfirmLogOut/Header")]
        ContentDialogConfirmLogOutHeader,

        [LocalizationString("ContentDialog/LogOutAllUsers/Header")]
        ContentDialogLogOutAllUsersHeader,

        [LocalizationString("ContentDialog/LogOutAllUsers/Text")]
        ContentDialogLogOutAllUsersText,

        [LocalizationString("ContentDialog/CloseAppLogOut/Text")]
        ContentDialogCloseAppLogOutText,

        [LocalizationString("ContentDialog/CloseAppLogOut/Header")]
        ContentDialogCloseAppLogOutHeader,

        [LocalizationString("OpenCloseShift/Open/Title/Text")]
        OpenCloseShiftOpenTitle,

        [LocalizationString("OpenCloseShift/Open/Ok/Text")]
        OpenCloseShiftOpenOk,

        [LocalizationString("OpenCloseShift/Open/UserPlaceholder/Text")]
        OpenCloseShiftOpenUserPlaceholder,

        [LocalizationString("OpenCloseShift/Close/Title/Text")]
        OpenCloseShiftCloseTitle,

        [LocalizationString("OpenCloseShift/Close/Ok/Text")]
        OpenCloseShiftCloseOk,

        [LocalizationString("OpenCloseShift/Close/UserPlaceholder/Text")]
        OpenCloseShiftCloseUserPlaceholder,
    }
}
