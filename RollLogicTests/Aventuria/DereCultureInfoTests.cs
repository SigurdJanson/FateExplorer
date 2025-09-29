using Aventuria;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Aventuria
{
    [TestFixture()]
    public class DereCultureInfoTests
    {
        [Test(), Ignore("Class not implemented, yet")]
        public void DereCultureInfoTest()
        {
            // 

            // act

            // assert
            Assert.Fail();
        }

        [Test(), Ignore("Class not implemented, yet")]
        public void DereCultureInfoTest1()
        {
            DereCultureInfo ci = new("Testonia");
            Debug.Print(ci.DisplayName);
            Debug.Print(ci.DereCountryCodes[0]);
            Debug.Print(ci.DateTimeFormat.ToString());
            Assert.Fail();
        }

        [Test(), Ignore("Class not implemented, yet")]
        public void DereCultureInfoTest2()
        {
            Assert.Fail();
        }

        [Test(), Ignore("Class not implemented, yet")]
        public void DereCultureInfoTest3()
        {
            Assert.Fail();
        }
    }
}