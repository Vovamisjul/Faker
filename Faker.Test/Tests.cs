using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faking;

namespace FakingTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void When_FakerCreates_Expect_NotNull()
        {
            Faker faker = new Faker();
            A a = faker.Create<A>();
            Assert.NotNull(a);
        }
    }
}
