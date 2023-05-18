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
        private readonly int _daysBackstagePassesDecreaseByTwo = 11;
        private readonly int _daysBackstagePassesDecreaseByThree = 6;

        public const int DefaultNormalProductDecrease = 1;
        public const int DefaultSellInDecrease = 1;
        public const int DefaultNormalProductIncrease = 1;
        public const int DefaultConjuredProductDecrease = DefaultNormalProductDecrease * 2;
        
        IList<Item> Items;
        public GildedRose(IList<Item> Items)
        {
            this.Items = Items;
        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
               var updated = GetUpdatedItem(item);
               
               item.Quality = updated.Quality;
               item.SellIn = updated.SellIn;
            }
        }

        private Item GetUpdatedItem(Item item)
        {
            var updatedItem = item.Clone();
            UpdateProductQuality(updatedItem);

            if (!updatedItem.IsLegendaryProduct())
                updatedItem.DecreaseSellIn();
            
            if(updatedItem.IsProductSellByDatePassed())
                UpdatePassedItem(updatedItem);

            return updatedItem;
        }

        private void UpdateProductQuality(Item item)
        {
            DecreaseProductQuality(item);
            
            if (IsSpecialProductWithValidQuality(item))
                item.IncreaseQuantity();

            UpdateBackstagePasses(item);
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
        
        private void UpdatePassedItem(Item item)
        {
            DecreaseProductQuality(item);
            
            if (item.IsProductSellByDatePassed(ItemNames.BackstagePasses))
                item.Quality = 0;

            else if (item.IsProductSellByDatePassed(ItemNames.AgedBrie) && HasValidQuality(item))
                item.IncreaseQuantity();
        }
        
        private void DecreaseProductQuality(Item item)
        {
            if (item.IsConjuredProduct())
                item.DecreaseQuantity(DefaultConjuredProductDecrease);
            else if (item.IsNormalProduct())
                item.DecreaseQuantity();
        }
        private bool IsSpecialProductWithValidQuality(Item item) => item.IsSpecialProduct() && HasValidQuality(item);
        private bool IsValidBackstagePasses(Item item) => item.Name == ItemNames.BackstagePasses && HasValidQuality(item);
        private bool HasValidQuality(Item item)=> item.Quality < _defaultMaxQuality;
        private bool IsNormalProductPassed(Item item) => 
            item.IsProductSellByDatePassed() && !item.IsSpecialProduct() && item.Quality > 0;
        
    }
}
