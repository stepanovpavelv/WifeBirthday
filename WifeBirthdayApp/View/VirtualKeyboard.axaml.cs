using Avalonia.Controls;
using Avalonia.ReactiveUI;
using WifeBirthdayApp.ViewModel;

namespace WifeBirthdayApp.View;

public partial class VirtualKeyboard : ReactiveUserControl<VirtualKeyboardViewModel>
{
    public VirtualKeyboard()
    {
        InitializeComponent();

        this.MainCurrentInput.KeyUp += MainCurrentInput_KeyUp;
    }

    private void MainCurrentInput_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (sender is TextBox textBox && !string.IsNullOrEmpty(textBox.Text))
        {
            textBox.Text = textBox.Text.ToUpper();
        }
    }
}