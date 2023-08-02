using FateExplorer.Shared;
using Moq;
using NUnit.Framework;
using System;

namespace UnitTests.Shared;

[TestFixture]
public class WeightedListTests
{
    private MockRepository mockRepository;



    [SetUp]
    public void SetUp()
    {
        this.mockRepository = new MockRepository(MockBehavior.Strict);


    }

    private static WeightedList<int> CreateWeightedListInt()
    {
        return new WeightedList<int>();
    }


    [Test]
    public void WeightedListConstructor_ShouldCreateEmptyList()
    {
        var list = CreateWeightedListInt();
        Assert.AreEqual(0, list.Count);
    }


    [Test]
    public void Add_ShouldIncreaseCountAndAddItem()
    {
        var list = CreateWeightedListInt();
        list.Add(1, 2);

        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(1, list[0]);
    }

    [Test]
    public void Insert_ShouldInsertItemAtCorrectIndex()
    {
        // Arrange
        var list = CreateWeightedListInt();
        list.Add(4, 1);
        list.Add(3, 1);

        // Act
        list.Insert(1, 2, 1);

        // Assert
        Assert.AreEqual(3, list.Count);
        Assert.AreEqual(2, list[1]);
    }


    [Test]
    public void Random_ShouldReturnItemFromList()
    {
        // Arrange
        var list = CreateWeightedListInt();
        list.Add(1, 1);
        list.Add(2, 1);

        // Act
        int result = list.Random();

        // Assert
        Assert.IsTrue(list.Contains(result));
    }


    [Test]
    public void Random_ShouldReturnBrokenWeight()
    {
        // Arrange
        var list = CreateWeightedListInt();
        list.Add(1, 0);
        list.Add(2, 0);
        list.Add(3, 0.4f);
        list.Add(4, 0);
        list.Add(5, 0);

        // Act
        int result = list.Random();

        // Assert
        Assert.That(result, Is.EqualTo(3));
        Assert.That(list.TrueCount, Is.EqualTo(0));
    }



    [Test]
    public void Random_OnlyReturnValuesGreaterZero([Values(1, 2, 3)]int NonZeroIndex)
    {
        // Arrange
        var list = CreateWeightedListInt();
        list.Add(1, NonZeroIndex == 1 ? 1 : 0);
        list.Add(2, NonZeroIndex == 2 ? 1 : 0);
        list.Add(3, NonZeroIndex == 3 ? 1 : 0);

        // Act
        int result = list.Random();

        // Assert
        Assert.That(result, Is.EqualTo(NonZeroIndex));
    }


    [Test]
    public void Compress_ShouldRemoveItemsWithZeroWeight()
    {
        // Arrange
        var list = CreateWeightedListInt();
        list.Add(1, 1);
        list.Add(2, 0);

        // Act
        list.Compress();

        // Assert
        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(1, list[0]);
    }



    [Test]
    public void IndexOf_ShouldReturnIndexOfItem([Values(0, 1, 2)] int Index)
    {
        // Arrange
        var list = CreateWeightedListInt();
        list.Add(0, 1);
        list.Add(1, 1);
        list.Add(2, 1);

        // Act
        int result = list.IndexOf(Index);

        Assert.AreEqual(Index, result);
    }



    [Test]
    [TestCase(0, ExpectedResult = false)]
    [TestCase(2, ExpectedResult = false)]
    [TestCase(4, ExpectedResult = false)]
    [TestCase(6, ExpectedResult = false)]
    [TestCase(1, ExpectedResult = true)]
    [TestCase(3, ExpectedResult = true)]
    [TestCase(5, ExpectedResult = true)]
    public bool Contains_ShouldReturnTrueIfItemInList(int ToFind)
    {
        // Arrange
        var list = CreateWeightedListInt();
        list.Add(1, 1);
        list.Add(3, 1);
        list.Add(5, 1);

        // Act
        bool exists = list.Contains(ToFind);

        //Assert.IsTrue(exists);
        return exists;
    }



    [Test]
    public void RemoveAt_ShouldRemoveItemAtIndex()
    {
        // Arrange
        var list = CreateWeightedListInt();
        list.Add(1, 1);
        list.Add(2, 1);

        list.RemoveAt(1);

        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(1, list[0]);
    }


    [Test]
    public void Remove_ShouldRemoveItem()
    {
        // Arrange
        var list = CreateWeightedListInt();
        list.Add(1, 1);
        list.Add(2, 1);

        bool success = list.Remove(1);

        Assert.IsTrue(success);
        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(2, list[0]);
    }


    [Test]
    public void Remove_ShouldReturnFalseIfItemNotInList()
    {
        // Arrange
        var list = CreateWeightedListInt();
        bool success = list.Remove(1);

        Assert.IsFalse(success);
        Assert.AreEqual(0, list.Count);
    }



    [Test]
    [TestCase(2)]
    [TestCase(0)] // Edge case
    public void Clear_LeavesAnEmptyArray(int Count)
    {
        // Arrange
        var weightedList = CreateWeightedListInt();
        for(int i = 0; i < Count; i++)
            weightedList.Add(i, 1);
        Assume.That(weightedList.Count, Is.EqualTo(Count));

        // Act
        weightedList.Clear();

        // Assert
        Assert.That(weightedList.Count, Is.EqualTo(0));
        this.mockRepository.VerifyAll();
    }



    //[Test]
    //public void CopyTo_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var weightedList = this.CreateWeightedListInt();
    //    T[] array = null;
    //    int arrayIndex = 0;

    //    // Act
    //    weightedList.CopyTo(
    //        array,
    //        arrayIndex);

    //    // Assert
    //    Assert.Fail();
    //    this.mockRepository.VerifyAll();
    //}

    //[Test]
    //public void GetEnumerator_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var weightedList = this.CreateWeightedListInt();

    //    // Act
    //    var result = weightedList.GetEnumerator();

    //    // Assert
    //    Assert.Fail();
    //    this.mockRepository.VerifyAll();
    //}
}
