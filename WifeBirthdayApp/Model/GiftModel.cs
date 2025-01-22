namespace WifeBirthdayApp.Model;

/// <summary>
/// Модель подарка.
/// </summary>
public sealed class GiftModel(int id, string name)
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public int Id { get; init; } = id;

    /// <summary>
    /// Наименование.
    /// </summary>
    public string Name { get; init; } = name;
}
