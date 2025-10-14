using Aventuria;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Aventuria
{
    [TestFixture()]
    public class DereCultureInfoTests
    {
        [SetUp]
        public void SetUp()
        {
            string customCultureName = "en-Middenrealm";
        }


        [Test(), Ignore("Class not implemented, yet")]
        public void DereCultureInfoTest()
        {
            // 

            // act

            // assert
            Assert.Fail();
        }

        [Test(), Culture("de-DE")]//, Ignore("Class not implemented, yet")
        public void DereCultureInfoTest1()
        {
            DereCultureInfo ci = new("ForestFolk", CultureInfo.CurrentCulture.Name);
            Debug.Print(ci.DisplayName);
            Debug.Print(ci.DateTimeFormat.ToString());
            Assert.Pass();
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