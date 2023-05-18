using System;
using System.Linq;

namespace GildedRoseKata.Extensions;

public static class ItemExtensions
{
    
    private static readonly string[] _SpecialProducts = new string[]
    {
        ItemNames.AgedBrie,
        ItemNames.BackstagePasses,
        ItemNames.SulfurasHandOfRagnaros
    };

    private static readonly string[] _LegendaryProducts = new string[]
    {
        ItemNames.SulfurasHandOfRagnaros
    };
    public static Item Clone(this Item item) => new()
    {
        Name = item.Name,
        Quality = item.Quality,
        SellIn = item.SellIn
    };

    public static void Update(this Item item, Action<Item> action) => action(item);

    public static void IncreaseQuantity(this Item item, int increase = GildedRose.DefaultNormalProductIncrease) => item.Update(item => item.Quality = item.Quality + increase);
    public static void DecreaseQuantity(this Item item, int decrease = GildedRose.DefaultNormalProductDecrease) => item.Update(item => item.Quality = item.Quality - decrease);
    public static void DecreaseSellIn(this Item item, int decrease = GildedRose.DefaultSellInDecrease) => item.Update(item => item.SellIn = item.SellIn - decrease);
    
    public static bool IsSellByDateWithLessThan(this Item item, int days) =>  item.SellIn < days;
    public static bool IsProductSellByDatePassed(this Item item) => item.IsSellByDateWithLessThan(0);

    public static bool IsProductSellByDatePassed(this Item item, string itemName) =>
        item.IsSellByDateWithLessThan(0) && item.Name.Equals(itemName);
    
    public static bool IsSpecialProduct(this Item item) => _SpecialProducts.Contains(item.Name);
    public static bool IsLegendaryProduct(this Item item) => _LegendaryProducts.Contains(item.Name);
    public static bool IsNormalProduct(this Item item) => !item.IsSpecialProduct() && item.Quality > 0;
    public static bool IsConjuredProduct(this Item item) => item.Name.StartsWith("Conjured")  && item.Quality > 0;

}