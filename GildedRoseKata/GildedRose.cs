using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GildedRoseKata.Extensions;
using Xunit;

namespace GildedRoseKata
{
    public class GildedRose
    {
        private readonly int _defaultMaxQuality = 50;
        private const int _daysBackstagePassesDecreaseByTwo = 11;
        private readonly int _daysBackstagePassesDecreaseByThree = 6;

        IList<Item> Items;
        public GildedRose(IList<Item> Items)
        {
            this.Items = Items;
        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                UpdateItem(item);
            }
        }

        private void UpdateItem(Item item)
        {
            UpdateProductQuality(item);

            if (!item.IsLegendaryProduct())
                item.DecreaseSellIn();

            UpdatePassedItem(item);
        }

        private void UpdateProductQuality(Item item)
        {
            if (item.IsNormalProduct())
                item.DecreaseQuantity();

            if (IsSpecialProductWithGoodQuality(item))
                item.IncreaseQuantity();

            UpdateBackstagePasses(item);
        }

        private bool IsSpecialProductWithGoodQuality(Item item)
        {
            return item.IsSpecialProduct() && item.Quality < _defaultMaxQuality;
        }

        private void UpdateBackstagePasses(Item item)
        {
            if (!IsValidBackstagePasses(item)) 
                return;
            
            if(item.IsSellByDateWithLessThan(_daysBackstagePassesDecreaseByTwo))
                item.IncreaseQuantity();

            if (item.IsSellByDateWithLessThan(_daysBackstagePassesDecreaseByThree))
                item.IncreaseQuantity();
        }

        private bool IsValidBackstagePasses(Item item) => item.Name == ItemNames.BackstagePasses && item.Quality < _defaultMaxQuality;
        private void UpdatePassedItem(Item item)
        {
            if (item.IsProductSellByDatePassed() && item.Name == ItemNames.BackstagePasses)
                item.Quality = 0;

            if (item.IsProductSellByDatePassed() && item.Name == ItemNames.AgedBrie && item.Quality < _defaultMaxQuality)
                item.IncreaseQuantity();

            if (item.IsProductSellByDatePassed() && !item.IsSpecialProduct() && item.Quality > 0)
                item.DecreaseQuantity();
        }
        
    }
}
