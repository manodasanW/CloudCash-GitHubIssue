using System;

namespace CloudCash.Client.Modules.MainPage.Messages
{
    public record NavigateToMsg
    {
        public Type NavigateToType { get; }

        public NavigateToMsg(Type navigateToType) => NavigateToType = navigateToType;
    }
}
