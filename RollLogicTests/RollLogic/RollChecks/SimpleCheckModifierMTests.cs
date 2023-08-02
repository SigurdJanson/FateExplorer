using FateExplorer.RollLogic;
using FateExplorer.Shared;
using Moq;
using NUnit.Framework;
using System;

namespace UnitTests.RollLogic
{
    [TestFixture]
    public class SimpleCheckModifierMTests
    {
        private MockRepository mockRepository;



        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private SimpleCheckModificatorM CreateSimpleCheckModifierM(int Value)
        {
            return new SimpleCheckModificatorM(new Modifier(Value));
        }

    }
}
