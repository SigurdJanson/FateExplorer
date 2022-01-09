using FateExplorer.RollLogic;
using FateExplorer.Shared;
using FateExplorer.FreeDiceCupViMo;
using NUnit.Framework;
using System;
using System.Linq;

namespace RollLogicTests.ViewModel
{
    [TestFixture]
    public class EueuqMaxTests
    {
        [Test]
        public void Enqueue_ExceedMaxCount_LengthIsFixedtoMaxLen([Values(1, 2, 5, 10)] int MaxLen)
        {
            // Arrange
            var resultQueueViMo = new EueuqMax<RollResultViMo>(MaxLen);
            for (int i = 0; i < MaxLen; i++)
            {
                RollResultViMo item = new($"_{i + 1}", new int[1] { i }, CupType.Single);
                resultQueueViMo.Enqueue(item);
            }
            Assume.That(resultQueueViMo.Count, Is.EqualTo(MaxLen));

            // Act
            resultQueueViMo.Enqueue(new($"_{99}", new int[1] { 99 }, CupType.Single));

            // Assert
            Assert.AreEqual(MaxLen, resultQueueViMo.Count);
            if (MaxLen > 1)
                Assert.AreEqual("_2", resultQueueViMo.Peek().Name);
            else
                Assert.AreEqual("_99", resultQueueViMo.Peek().Name);
        }


        [Test]
        [TestCase(2, 1)]
        [TestCase(5, 2)]
        [TestCase(10, 5)]
        public void Enqueue_DecreaseMaxCount_LengthIsReducedToFit(int Start, int Decreased)
        {
            // Arrange
            var resultQueueViMo = new EueuqMax<RollResultViMo>(Start);
            for (int i = 0; i < Start; i++)
            {
                RollResultViMo item = new($"_{i + 1}", new int[1] { i }, CupType.Single);
                resultQueueViMo.Enqueue(item);
            }
            Assume.That(resultQueueViMo.Count, Is.EqualTo(Start));

            // Act
            resultQueueViMo.MaxCount = Decreased;

            // Assert
            Assert.AreEqual(Decreased, resultQueueViMo.Count);
            Assert.AreEqual($"_{Start-Decreased+1}", resultQueueViMo.Peek().Name);
        }



        [Test]
        public void Enqueue_Reverse_GetReversed([Values(2, 5, 10)] int MaxLen)
        {
            // Arrange
            var resultQueueViMo = new EueuqMax<RollResultViMo>(MaxLen);
            for (int i = 0; i < MaxLen; i++)
            {
                RollResultViMo item = new($"_{i + 1}", new int[1] { i }, CupType.Single);
                resultQueueViMo.Enqueue(item);
            }
            Assume.That(resultQueueViMo.Count, Is.EqualTo(MaxLen));

            // Act
            // Assert
            Assert.AreEqual(MaxLen, resultQueueViMo.Count);
            int ri = MaxLen;
            foreach (var item in resultQueueViMo.Reverse())
            {
                Assert.AreEqual($"_{ri}", item.Name);
                ri--;
            }
                
        }
    }
}
