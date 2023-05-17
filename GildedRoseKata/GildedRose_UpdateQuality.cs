using FluentAssertions;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GildedRoseKata
{
    /// <summary>
    /// Test naming convention recommendation:
    /// https://ardalis.com/unit-test-naming-convention/
    /// </summary>
    public class GildedRose_UpdateQuality
    {
        [Fact]
        public void DoesNothingGivenSulfuras()
        {
            //Arrange
            var initialQuality = 80;
            var items = new List<Item> {
                                new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = initialQuality},

            };

            //Act
            ExecuteUpdateQuality(items);

            //Assert
            ThenFirstItemIsEqualTo(items, initialQuality);
        }

        [Fact]
        public void GivenAPassedItem_ThenDegradesTwiceAsFast()
        {
            //Arrange
            var initialQuality = 80;
            var expectedValue = 78;
            var items = new List<Item> {
                new Item {Name = "Random Item", SellIn = 0, Quality = initialQuality},
            };

            //Act
            ExecuteUpdateQuality(items);

            //Assert
            ThenFirstItemIsEqualTo(items, expectedValue);
        }

        [Fact]
        public void GivenAgedBrie_ThenIncreaseTheQualityWithOne()
        {
            //Arrange
            var initialQuality = 40;
            var expectedValue = 41;
            var items = new List<Item> {
                new Item {Name = ItemNames.AgedBrie, SellIn = 10, Quality = initialQuality},
            };

            //Act
            ExecuteUpdateQuality(items);

            //Assert
            ThenFirstItemIsEqualTo(items, expectedValue);
        }


        [Fact]
        public void GivenBackstagePasses_With10DaysOrLess_ThenQualityIncreaseByTwo()
        {
            //Arrange
            var initialQuality = 40;
            var expectedValue = 42;
            var items = new List<Item> {
                new Item {Name = ItemNames.BackstagePasses, SellIn = 10, Quality = initialQuality},
            };

            //Act
            ExecuteUpdateQuality(items);

            //Assert
            ThenFirstItemIsEqualTo(items, expectedValue);
        }

        [Fact]
        public void GivenBackstagePasses_With5DaysOrLess_ThenQualityIncreaseByThree()
        {
            //Arrange
            var initialQuality = 40;
            var expectedValue = 43;
            var items = new List<Item> {
                new Item {Name = ItemNames.BackstagePasses, SellIn = 5, Quality = initialQuality},
            };

            //Act
            ExecuteUpdateQuality(items);

            //Assert
            ThenFirstItemIsEqualTo(items, expectedValue);
        }

        [Fact]
        public void GivenBackstagePasses_WithoutDaysLess_ThenDropsQualityToZero()
        {

            //Arrange
            var initialQuality = 40;
            var expectedValue = 0;
            var items = new List<Item> {
                new Item {Name = ItemNames.BackstagePasses, SellIn = 0, Quality = initialQuality},
            };

            //Act
            ExecuteUpdateQuality(items);

            //Assert
            ThenFirstItemIsEqualTo(items, expectedValue);
        }

        [Fact]
        public void GivenAPassedItemWithQualityZero_QualityIsNoNegative()
        {
            //Arrange
            var initialQuality = 0;
            var expectedValue = 0;
            var items = new List<Item> {
                new Item {Name = "Quality Zero Item", SellIn = 0, Quality = initialQuality},
            };

            //Act
            ExecuteUpdateQuality(items);

            //Assert
            ThenFirstItemIsEqualTo(items, expectedValue);
        }

        [Fact]
        public void GivenItemsThatIncreaseQuality_QualityIsNeverMoreThanFifty()
        {
            //Arrange
            var initialQuality = 50;
            var items = new List<Item> {
                new Item {Name = ItemNames.AgedBrie, SellIn = 10, Quality = initialQuality},
                new Item {Name = ItemNames.BackstagePasses, SellIn =5, Quality = initialQuality},
            };

            //Act
            ExecuteUpdateQuality(items);

            //Assert
            items.Should().NotContain(item => item.Quality>50);
        }

        #region PrivateMethods

        private void ExecuteUpdateQuality(List<Item> items)
        {
            var gildedRose = new GildedRose(items);
            gildedRose.UpdateQuality();
        }

        private void ThenFirstItemIsEqualTo(List<Item> items, int expectedValue)
        {
            var item = items.First();
            item.Quality.Should().Be(expectedValue);
        }

        #endregion

    }
}
