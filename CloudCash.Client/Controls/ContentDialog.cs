using CloudCash.Common.Exceptions;
using CloudCash.Common.Functions;
using CloudCash.Common.MVVM;
using CloudCash.Interface.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CloudCash.Client.Controls
{
    public class ContentDialog : Microsoft.UI.Xaml.Controls.ContentDialog
    {
        // Insp
        //public bool DoBeforeCheckSwitch { get; set; } = true;
        //public bool DoCheckValuesSwitch { get; set; } = true;

        public ContentDialog() : base()
        {
            Closing += ContentDialog_Closing;
        }

        private void ContentDialog_Closing(Microsoft.UI.Xaml.Controls.ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            Focus(FocusState.Pointer);

            var content = Content.CheckType<UserControl>(true);

            if (args.Result is ContentDialogResult.Primary && content is not null)
            {
                //if (DoBeforeCheckSwitch)
                DoBeforeCheck(content.DataContext);

                //if (DoCheckValuesSwitch)
                args.Cancel = !CheckValues(content.DataContext);
            }
        }

        private bool CheckValues(object dataContext)
        {
            var dataCon = dataContext.CheckType<ViewModelBase>();
            var dataMiner = dataContext.CheckType<IUserControlViewModelBase>();
            bool result = false;

            try
            {
                dataMiner.CheckData();
            }
            catch (ValidationException e)
            {
                dataCon.ErrorMessage = e.ValidationErrorMessage;
                dataCon.ErrorHeader = e.PropertyName;

                return false;
            }

            dataCon.ErrorMessage = null;
            return true;
        }

        private void DoBeforeCheck(object dataContext)
        {
            var dataMiner = dataContext.CheckType<IUserControlViewModelBase>();

            dataMiner.DoBeforeCheck();
        }
    }
}
