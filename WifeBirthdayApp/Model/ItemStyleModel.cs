namespace WifeBirthdayApp.Model;

/// <summary>
/// Модель отображаемой буквы
/// </summary>
public sealed class ItemStyleModel
{
    /// <summary>
    /// Символ
    /// </summary>
    public char Symbol { get; set; }

    /// <summary>
    /// Признак обычного стиля
    /// </summary>
    public bool IsUsualStyle { get; set; } = true;

    /// <summary>
    /// Признак буквы, которая где-то присутствует в слове
    /// </summary>
    public bool IsSomewhereStyle { get; set; }

    /// <summary>
    /// Признак точного попадания
    /// </summary>
    public bool IsEqualStyle { get; set; }
}
