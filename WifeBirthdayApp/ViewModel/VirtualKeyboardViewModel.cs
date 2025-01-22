using WifeBirthdayApp.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WifeBirthdayApp.ViewModel;

public sealed class VirtualKeyboardViewModel : ViewModelBase
{
    private readonly string _rightAnswer;

    [Reactive]
    public ObservableCollection<VirtualKeyboardLayoutRuViewModel> Answers { get; set; }

    [Reactive]
    public string CurrentAnswer { get; set; } = string.Empty;

    [Reactive]
    public string GiftText { get; set; } = string.Empty;

    public ICommand CheckAnswerCommand { get; set; }

    public VirtualKeyboardViewModel(GiftModel gift)
    {
        Answers = [];
        _rightAnswer = gift.Name;
        CheckAnswerCommand = ReactiveCommand.CreateFromTask(CheckAnswer);
        SetGiftText(gift);
    }

    private async Task CheckAnswer()
    {
        if (!await ValidateCurrentAnswer())
        {
            return;
        }

        Answers.Add(new VirtualKeyboardLayoutRuViewModel(CurrentAnswer, _rightAnswer));
        
        CurrentAnswer = string.Empty;
    }

    private async Task<bool> ValidateCurrentAnswer()
    {
        if (string.IsNullOrEmpty(CurrentAnswer))
        {
            await MessageBox.Show("Так, жена. Соберись. Мы собираемся угадывать подарок или нет?", "ДР жены", MessageBox.MessageBoxButtons.Ok);
            return false;
        }

        if (CurrentAnswer.Length > 5)
        {
            await MessageBox.Show("Серьезно?! Ты хочешь, чтобы в подарке было так много букав? Я на такое бы не рассчитывал!", "ДР жены", MessageBox.MessageBoxButtons.Ok);
            return false;
        }

        if (CurrentAnswer.Any(x => x < 'А' || x > 'Я'))
        {
            await MessageBox.Show("Чуяло моё сердечко, что ты будешь баловаться. Смотри. Вводить надо только буквы русского алфавита. Договор?", "ДР жены", MessageBox.MessageBoxButtons.Ok);
            return false;
        }

        return true;
    }

    private void SetGiftText(GiftModel gift)
    {
        GiftText = $"Пробуем угадать подарок номер {gift.Id}!";
    }
}