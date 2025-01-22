namespace WifeBirthdayApp.Model;

/// <summary>
/// Типы отображения символов
/// </summary>
public enum LetterVisualType
{
    /// <summary>
    /// Нет совпадения
    /// </summary>
    None = 0,

    /// <summary>
    /// На какой-то позиции есть
    /// </summary>
    Somewhere,

    /// <summary>
    /// Точное совпадение позиции буквы
    /// </summary>
    Equal
}