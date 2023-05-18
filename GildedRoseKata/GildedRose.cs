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
               var updated = UpdateItem(item);
               
               item.Quality = updated.Quality;
               item.SellIn = updated.SellIn;
            }
        }

        private Item UpdateItem(Item item)
        {
            var updatedItem = item.Clone();
            UpdateProductQuality(updatedItem);

            if (!updatedItem.IsLegendaryProduct())
                updatedItem.DecreaseSellIn();

            UpdatePassedItem(updatedItem);

            return updatedItem;
        }

        private void UpdateProductQuality(Item item)
        {
            if (item.IsNormalProduct())
                item.DecreaseQuantity();

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
            if (IsNormalProductPassed(item))
                item.DecreaseQuantity();
            
            else if (item.IsProductSellByDatePassed(ItemNames.BackstagePasses))
                item.Quality = 0;

            else if (item.IsProductSellByDatePassed(ItemNames.AgedBrie) && HasValidQuantity(item))
                item.IncreaseQuantity();
        }
        
        private bool IsSpecialProductWithValidQuality(Item item) => item.IsSpecialProduct() && HasValidQuantity(item);
        private bool IsValidBackstagePasses(Item item) => item.Name == ItemNames.BackstagePasses && HasValidQuantity(item);
        private bool HasValidQuantity(Item item)=> item.Quality < _defaultMaxQuality;
        private bool IsNormalProductPassed(Item item) => 
            item.IsProductSellByDatePassed() && !item.IsSpecialProduct() && item.Quality > 0;
        
    }
}
