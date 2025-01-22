using WifeBirthdayApp.Helper;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace WifeBirthdayApp.ViewModel;

public sealed class MainWindowViewModel : ViewModelBase
{
    private readonly GiftRepository _giftRepository = new();

    public ReactiveCommand<string, Unit> PlayCommand { get; private set; }
    
    public ReactiveCommand<Unit, Unit> PlayAgainCommand { get; private set; }

    [Reactive]
    public bool IsGameMode { get; set; }

    [Reactive]
    public VirtualKeyboardViewModel? GameViewModel { get; set; }

    public MainWindowViewModel()
    {
        PlayCommand = ReactiveCommand.CreateFromTask<string>(StartGame);
        PlayAgainCommand = ReactiveCommand.Create(RepeatGame);
    }

    private async Task StartGame(string strGiftIndex)
    {
        IsGameMode = true;

        var giftIndex = Convert.ToInt32(strGiftIndex);
        var gift = _giftRepository.GetGiftById(giftIndex);
        if (gift == null)
        {
            await MessageBox.Show("Что ж, а такого подарка у меня для тебя нет :-(", "ДР жены", MessageBox.MessageBoxButtons.Ok);
            return;
        }

        GameViewModel = new VirtualKeyboardViewModel(gift);
    }

    private void RepeatGame()
    {
        IsGameMode = false;
    }
}
