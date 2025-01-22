using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System.Collections.Generic;
using System.Linq;

namespace WifeBirthdayApp.Extensions;

public static class WindowExtensions
{
    public static IEnumerable<Window> GetWindows(this Application app)
    {
        return ((ClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).Windows;
    }

    public static T GetWindowByType<T>(this Application app) where T : Window
    {
        return app.GetWindows().FirstOrDefault(x => x is T) as T;
    }

    public static Window GetMainWindow(this Application app)
    {
        return (app.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime)?.MainWindow;
    }
}
