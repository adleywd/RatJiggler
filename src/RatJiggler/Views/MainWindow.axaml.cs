using Avalonia.Controls;
using Avalonia.Input;
using RatJiggler.ViewModels;

namespace RatJiggler.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm && vm.IsCapturingHotkey)
        {
            // Ignore modifier-only key presses
            if (e.Key is Key.LeftCtrl or Key.RightCtrl
                or Key.LeftShift or Key.RightShift
                or Key.LeftAlt or Key.RightAlt
                or Key.LWin or Key.RWin)
            {
                return;
            }

            var modifiers = (int)e.KeyModifiers;
            var key = (int)e.Key;

            // Allow function keys without modifiers, require modifiers for all other keys
            var isFunctionKey = e.Key >= Key.F1 && e.Key <= Key.F24;
            if (modifiers == 0 && !isFunctionKey)
            {
                return;
            }

            vm.SetHotkey(modifiers, key);
            e.Handled = true;
            return;
        }

        base.OnKeyDown(e);
    }
}
