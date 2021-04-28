using CloudCash.Client.MVVM;
using CloudCash.Client.Modules.AppWindow.Views;
using CloudCash.Client.Modules.Login.Classes;
using CloudCash.Client.Modules.Shift.Classes;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace CloudCash.Client
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public LoggedUsersManager LoggedUsersManager { get; set; }
        public ShiftsManager ShiftsManager { get; set; }

        public new static App Current { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            //Suspending += OnSuspending;
            UnhandledException += App_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            
            Current = this;
        }

        public static VMLocator GetVMLocator()
        {
            return (VMLocator)Current.Resources["VMLocator"];
        }

        public static void LoadManagers(XamlRoot xamlRoot)
        {
            Current.LoggedUsersManager = new(GetVMLocator(), xamlRoot);
            Current.ShiftsManager = new(GetVMLocator(), xamlRoot);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            //CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            m_window = new AppWindow
            {
                ExtendsContentIntoTitleBar = true,
                Title = Package.Current.DisplayName
            };
            m_window.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            // Save application state and stop any background activity
        }

        private Window m_window;
    }
}
