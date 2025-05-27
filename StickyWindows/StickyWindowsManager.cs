using System.Windows;
using System.Windows.Input;

namespace StickyWindows
{
    public static class StickyWindowsManager
    {
        private static readonly List<Window> RegisteredWindows = new();

        private static readonly Dictionary<Window, Location> Locations = new();

        public static void Register(Window window)
        {
            if (!RegisteredWindows.Contains(window))
            {
                RegisteredWindows.Add(window);
                window.Loaded += Window_Loaded;
                window.LocationChanged += Window_LocationChanged;
                window.Closed += Window_Closed;
            }
        }

        private static void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Window window)
            {
                Locations.Add(window, new Location(window.Left, window.Top));
            }
        }

        private static void Window_Closed(object? sender, EventArgs e)
        {
            if (sender is Window window)
            {
                Locations.Remove(window);
            }
        }

        private static void Window_LocationChanged(object? sender, EventArgs e)
        {
            if (sender is Window window)
            {
                if (window == Application.Current.MainWindow && window.IsActive)
                {
                    RegisteredWindows.DragAllWindows(window);
                }
                else
                {
                    window.StickToTheLeftOf(RegisteredWindows.GetClosestToTheRightOf(window));
                    window.StickToTheRightOf(RegisteredWindows.GetClosestToTheLeftOf(window));
                    window.StickToTheTopOf(RegisteredWindows.GetClosestToTheBottomOf(window));
                    window.StickToTheBottomOf(RegisteredWindows.GetClosestToTheTopOf(window));
                }
            }
        }

        #region IEnumerable<Window> Extensions

        private static void DragAllWindows(this IEnumerable<Window> windows, Window activeWindow)
        {
            if (Locations.TryGetValue(activeWindow, out Location previousLocation))
            {
                var newLocation = new Location(activeWindow.Left, activeWindow.Top);

                if (!previousLocation.Equals(newLocation))
                {
                    var diffLeft = newLocation.Left - previousLocation.Left;
                    var diffTop = newLocation.Top - previousLocation.Top;

                    if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    {
                        foreach (var window in windows)
                        {
                            if (window == activeWindow)
                                continue;

                            window.Left += diffLeft;
                            window.Top += diffTop;
                        }
                    }

                    Locations[activeWindow] = newLocation;
                }
            }
        }

        private static Window GetClosestToTheTopOf(this IEnumerable<Window> windows, Window window)
        {
            return windows.FirstOrDefault(x => Math.Abs(window.Top - x.GetBottom()) < 10);
        }

        private static Window GetClosestToTheBottomOf(this IEnumerable<Window> windows, Window window)
        {
            return windows.FirstOrDefault(x => Math.Abs(x.Top - window.GetBottom()) < 10);
        }

        private static Window GetClosestToTheLeftOf(this IEnumerable<Window> windows, Window window)
        {
            return windows.FirstOrDefault(x => Math.Abs(window.Left - x.GetRight()) < 10);
        }

        private static Window GetClosestToTheRightOf(this IEnumerable<Window> windows, Window window)
        {
            return windows.FirstOrDefault(x => Math.Abs(x.Left - window.GetRight()) < 10);
        }

        #endregion

        #region Sticky Extensions

        private static void StickToTheTopOf(this Window window, Window nearWindow)
        {
            if (nearWindow != null)
                window.Top = nearWindow.Top - window.Height + 5;
        }

        private static void StickToTheBottomOf(this Window window, Window nearWindow)
        {
            if (nearWindow != null)
                window.Top = nearWindow.GetBottom() - 5;
        }

        private static void StickToTheLeftOf(this Window window, Window nearWindow)
        {
            if (nearWindow != null)
                window.Left = nearWindow.Left - window.Width + 10;
        }

        private static void StickToTheRightOf(this Window window, Window nearWindow)
        {
            if (nearWindow != null)
                window.Left = nearWindow.GetRight() - 10;
        }

        private static double GetRight(this Window window)
        {
            return window.Left + window.Width;
        }

        private static double GetBottom(this Window window)
        {
            return window.Top + window.Height;
        }

        #endregion
    }
}