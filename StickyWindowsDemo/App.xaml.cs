using System.Windows;
using StickyWindows;

namespace StickyWindowsDemo
{
    public partial class App : Application
    {
        private MainWindow _mainWindow;
        private SecondaryWindow _secondaryWindow;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _mainWindow = new MainWindow();
            _secondaryWindow = new SecondaryWindow();

            StickyWindowsManager.Register(_mainWindow);
            StickyWindowsManager.Register(_secondaryWindow);

            _mainWindow.Show();
            _secondaryWindow.Left = _mainWindow.Left + _mainWindow.Width + 1;
            _secondaryWindow.Top = _mainWindow.Top;
            _secondaryWindow.Show();
        }
    }
}