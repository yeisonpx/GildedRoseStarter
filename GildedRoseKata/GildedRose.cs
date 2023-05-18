using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace GildedRoseKata
{
    public class GildedRose
    {
        private readonly string[] _specialProducts = new string[]
        {
            ItemNames.AgedBrie,
            ItemNames.BackstagePasses,
            ItemNames.SulfurasHandOfRagnaros
        };

        IList<Item> Items;
        public GildedRose(IList<Item> Items)
        {
            this.Items = Items;
        }

        public void UpdateQuality()
        {
            for (var i = 0; i < Items.Count; i++)
            {
                var isSpecialProduct = _specialProducts.Contains(Items[i].Name);
                var isNormalProduct = !isSpecialProduct && Items[i].Quality > 0;
                if (isNormalProduct)
                    Items[i].Quality--;

                var isSpecialProductWithGoodQuality = isSpecialProduct && Items[i].Quality < 50;
                if (isSpecialProductWithGoodQuality)
                    Items[i].Quality++;

                var isValidBackstagePasses = Items[i].Name == "Backstage passes to a TAFKAL80ETC concert" && Items[i].Quality < 50;
                if (isValidBackstagePasses && Items[i].SellIn < 11)
                    Items[i].Quality++;
                
                if (isValidBackstagePasses && Items[i].SellIn < 6)
                    Items[i].Quality++;
                
                if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
                    Items[i].SellIn--;

                var isProductSellByDatePassed = Items[i].SellIn < 0;
                if (isProductSellByDatePassed && Items[i].Name == "Backstage passes to a TAFKAL80ETC concert")
                    Items[i].Quality = 0;

                if (isProductSellByDatePassed && Items[i].Name == "Aged Brie" &&  Items[i].Quality < 50)
                    Items[i].Quality++;
            
                if (isProductSellByDatePassed && !isSpecialProduct && Items[i].Quality > 0)
                    Items[i].Quality--;
            }
        }


    }
}
