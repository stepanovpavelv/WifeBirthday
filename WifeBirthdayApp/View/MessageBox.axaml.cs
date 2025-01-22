using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using WifeBirthdayApp.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WifeBirthdayApp;

public partial class MessageBox : Window
{
    public interface IMessageBoxLogic
    {
        Task<MessageBoxResult> Show(string text, string title, MessageBoxButtons buttons, Window parent = null, bool textWrapped = false, bool isCardSave = false);
    }

    public static IMessageBoxLogic Logic
    {
        get; set;
    } = new DefaultMessageBoxLogic();

    public enum MessageBoxButtons
    {
        Ok,
        OkCancel,
        YesNo,
        NoYes,
        YesNoCancel,
        CancelContinue
    }

    public enum MessageBoxResult
    {
        Ok,
        Cancel,
        Yes,
        No
    }

    public MessageBox()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
    }

    public static Task<MessageBoxResult> Show(string text, string title, MessageBoxButtons buttons, Window parent = null, bool textWrapped = false, bool isCardSave = false)
    {
        return Logic.Show(text, title, buttons, parent, textWrapped, isCardSave);
    }

    private class DefaultMessageBoxLogic : IMessageBoxLogic
    {
        private string Trimming(string text)
        {
            var strings = text.Split('\n');
            var result = string.Empty;
            foreach (var str in strings)
            {
                if (str.Length > 121)
                {
                    result += str.Substring(0, 121) + "...\n";
                    continue;
                }
                result += str + "\n";
            }
            return result;
        }

        public Task<MessageBoxResult> Show(string text, string title, MessageBoxButtons buttons, Window? parent = null, bool textWrapped = false, bool isCardSave = false)
        {
            parent ??= Application.Current.GetWindows().Any()
                ? Application.Current.GetWindows()?.First()
                : null;

            int currentFocus = 0;

            var msgbox = new MessageBox()
            {
                Title = title
            };

            if (isCardSave)
            {
                var mainGrid = msgbox.FindControl<Grid>("MainGrid");
                var mainStackPanelForText = new StackPanel();
                var mainTextBlock = new TextBlock();

                mainTextBlock.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;

                var splitText = text.Split("@");
                mainTextBlock.Text = splitText[0];
                mainTextBlock.FontSize = 14;

                mainGrid.Children.Add(mainStackPanelForText);
                mainStackPanelForText.Children.Add(mainTextBlock);

                var valuesToText = new Dictionary<string, string>();

                for (int i = 1; i < splitText.Length; i = i + 2)
                {
                    valuesToText.Add(splitText[i], splitText[i + 1]);
                }

                foreach (var value in valuesToText)
                {
                    var stackPanelForText = new StackPanel();
                    stackPanelForText.Orientation = Avalonia.Layout.Orientation.Horizontal;
                    stackPanelForText.Margin = Thickness.Parse("10,0,10,0");

                    var name = new TextBlock();
                    name.Text = value.Key;
                    name.FontSize = 14;

                    var num = new TextBlock();
                    num.Text = value.Value;
                    num.FontSize = 14;
                    num.FontWeight = FontWeight.Bold;

                    stackPanelForText.Children.Add(name);
                    stackPanelForText.Children.Add(num);
                    stackPanelForText.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;

                    mainStackPanelForText.Children.Add(stackPanelForText);
                }
            }
            else
            {
                var textBox = msgbox.FindControl<TextBlock>("Text");
                var textBlock = msgbox.FindControl<TextBlock>("toolTip");
                textBlock.Text = text;
                textBox.Text = Trimming(text);
                textBox.TextWrapping = textWrapped ? TextWrapping.Wrap : TextWrapping.NoWrap;
            }

            var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");

            var res = MessageBoxResult.Ok;
            List<IInputElement> allButtons = new List<IInputElement>();
            void AddButton(string caption, MessageBoxResult r, bool def = false)
            {
                var btn = new Button { Content = caption, Focusable = false };
                btn.Click += (_, __) =>
                {
                    res = r;
                    msgbox.Close();
                };
                buttonPanel.Children.Add(btn);
                if (def)
                    res = r;
                allButtons.Add(btn);
            }

            void KeyDown(object sender, KeyEventArgs args)
            {
                if (allButtons.Count <= 1)
                    return;
                switch (args.Key)
                {
                    case Key.Left:
                        if (currentFocus > 0)
                            currentFocus--;
                        else
                            currentFocus = allButtons.Count - 1;
                        break;
                    case Key.Tab:
                    case Key.Right:
                        if (currentFocus < allButtons.Count - 1)
                            currentFocus++;
                        else
                            currentFocus = 0;
                        break;
                    default: return;
                }
                //Application.Current.FocusManager.Focus(allButtons[currentFocus]);
                //FocusManager.Instance.Focus(allButtons[currentFocus], NavigationMethod.Tab);
            };

            if (buttons == MessageBoxButtons.Ok || buttons == MessageBoxButtons.OkCancel)
            {
                AddButton("Ok", MessageBoxResult.Ok, true);
            }

            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
            {
                AddButton("Да", MessageBoxResult.Yes);
                AddButton("Нет", MessageBoxResult.No, true);
            }

            if (buttons == MessageBoxButtons.NoYes)
            {
                AddButton("Нет", MessageBoxResult.No, true);
                AddButton("Да", MessageBoxResult.Yes);
            }

            if (buttons == MessageBoxButtons.OkCancel
                || buttons == MessageBoxButtons.YesNoCancel
                || buttons == MessageBoxButtons.CancelContinue)
                AddButton("Отмена", MessageBoxResult.Cancel, true);

            if (buttons == MessageBoxButtons.CancelContinue)
                AddButton("Продолжить", MessageBoxResult.Ok, true);

            buttonPanel.KeyDown += KeyDown;

            var tcs = new TaskCompletionSource<MessageBoxResult>();

            msgbox.Closed += (sender, args) =>
            {
                tcs.TrySetResult(res);
                if (parent != null)
                {
                    parent.IsEnabled = true;
                }

                buttonPanel.KeyDown -= KeyDown;
            };

            if (parent != null)
            {
                parent.IsEnabled = false;
                msgbox.ShowDialog(parent);
            }
            else
            {
                msgbox.Show();
            }
            //Application.Current.FocusManager.Focus(allButtons[0]);

            return tcs.Task;
        }
    }
}