using WifeBirthdayApp.Model;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace WifeBirthdayApp.ViewModel;

public sealed class VirtualKeyboardLayoutRuViewModel : ViewModelBase
{
    [Reactive]
    public ItemStyleModel First { get; set; }
    
    [Reactive]
    public ItemStyleModel Second { get; set; }

    [Reactive]
    public ItemStyleModel Third { get; set; }

    [Reactive]
    public ItemStyleModel Fourth { get; set; }

    [Reactive]
    public ItemStyleModel Fifth { get; set; }

    public VirtualKeyboardLayoutRuViewModel(string answer, string rightAnswer)
    {
        var letterVisuals = answer.Select(
           (symbol, index) => new
           {
               key = index,
               value = (rightAnswer[index] == symbol) ? LetterVisualType.Equal :
                       (rightAnswer.Contains(symbol) ? LetterVisualType.Somewhere : LetterVisualType.None)
           })
           .ToDictionary(k => k.key, k => k.value);

        First = GetItemStyleForSymbol(answer, letterVisuals, 0);
        Second = GetItemStyleForSymbol(answer, letterVisuals, 1);
        Third = GetItemStyleForSymbol(answer, letterVisuals, 2);
        Fourth = GetItemStyleForSymbol(answer, letterVisuals, 3);
        Fifth = GetItemStyleForSymbol(answer, letterVisuals, 4);
    }

    private ItemStyleModel GetItemStyleForSymbol(string answer, Dictionary<int, LetterVisualType> letterVisuals, int index) =>
        new ItemStyleModel
        {
            Symbol = answer[index],
            IsUsualStyle = letterVisuals[index] == LetterVisualType.None,
            IsSomewhereStyle = letterVisuals[index] == LetterVisualType.Somewhere,
            IsEqualStyle = letterVisuals[index] == LetterVisualType.Equal
        };
}