using FateExplorer.FreeDiceCupViMo;
using FateExplorer.RollLogic;
using FateExplorer.Shared;
using NUnit.Framework;
using System;
using System.Linq;

namespace UnitTests.ViewModel
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
            Assert.That(MaxLen, Is.EqualTo(resultQueueViMo.Count));
            if (MaxLen > 1)
                Assert.That("_2", Is.EqualTo(resultQueueViMo.Peek().Name));
            else
                Assert.That("_99", Is.EqualTo(resultQueueViMo.Peek().Name));
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
            Assert.That(Decreased, Is.EqualTo(resultQueueViMo.Count));
            Assert.That($"_{Start - Decreased + 1}", Is.EqualTo(resultQueueViMo.Peek().Name));
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
            Assert.That(MaxLen, Is.EqualTo(resultQueueViMo.Count));
            int ri = MaxLen;
            foreach (var item in resultQueueViMo.Reverse())
            {
                Assert.That($"_{ri}", Is.EqualTo(item.Name));
                ri--;
            }

        }
    }
}
