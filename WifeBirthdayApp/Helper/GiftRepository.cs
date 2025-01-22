using WifeBirthdayApp.Model;
using System.Collections.Generic;

namespace WifeBirthdayApp.Helper;

public sealed class GiftRepository
{
    private readonly Dictionary<int, string> _gifts;

    public GiftRepository()
    {
        _gifts = new Dictionary<int, string>
        {
            { 1, "ЦВЕТЫ" },
            { 2, "ПОЛЕТ" },
            { 3, "КОФИЙ" }
        };
    }

    public GiftModel? GetGiftById(int id)
    {
        if (_gifts.TryGetValue(id, out var giftName))
        {
            return new GiftModel(id, giftName);
        }

        return null;
    }
}
