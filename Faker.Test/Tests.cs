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
        public void When_FakerCreates_Expect_NotNullValues()
        {
            Faker faker = new Faker();
            A a = faker.Create<A>();
            Assert.Multiple(() =>
            {
                Assert.NotNull(a.a);
                Assert.NotZero(a.a.Length);
                Assert.NotZero(a.b);
            });
        }

        [Test]
        public void When_NestedClass_Expect_Initialized2Classes()
        {
            Faker faker = new Faker();
            B b = faker.Create<B>();
            Assert.Multiple(() =>
            {
                Assert.NotNull(b.a);
                Assert.NotZero(b.a.a.Length);
                Assert.NotZero(b.a.b);
            });
        }
    }
}
