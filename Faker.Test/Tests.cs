using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faking;
using static System.Net.Mime.MediaTypeNames;

namespace FakingTest
{
    [TestFixture]
    public class Tests
    {
        Faker faker;
        [SetUp]
        public void SetUp()
        {
            faker = new Faker();

        }
        [Test]
        public void When_FakerCreates_Expect_NotNullValues()
        {
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
            B b = faker.Create<B>();
            Assert.Multiple(() =>
            {
                Assert.NotNull(b.a);
                Assert.NotZero(b.a.a.Length);
                Assert.NotNull(b.a.b);
            });
        }

        [Test]
        public void When_RecursionClasses_Expect_NotStackOverflow()
        {
            D d = faker.Create<D>();
            Assert.Multiple(() =>
            {
                Assert.NotNull(d.c);
                Assert.NotNull(d.c.d);
            });
        }

        [Test]
        public void When_Generics_Expect_FillIt()
        {
            E e = faker.Create<E>();

            Assert.Multiple(() =>
            {
                Assert.NotNull(e.alist);
                Assert.AreNotEqual(e.alist[0], e.alist[1]);
            });
        }

        [Test]
        public void When_ClassWithConstructor_Expect_FillIt()
        {
            F f = faker.Create<F>();

            Assert.Multiple(() =>
            {
                Assert.NotNull(f.a);
                Assert.NotNull(f.a.a);
                Assert.NotZero(f.a.b);
                Assert.NotZero(f.b);
            });
        }

        [Test]
        public void When_Abstract_Expect_null()
        {
            var a = faker.Create<Encoding>();
            Assert.Multiple(() =>
            {
                Assert.Null(a);
            });
        }

        [Test]
        public void When_LinkedList_Expect_FillIt()
        {
            G g = faker.Create<G>();
            Assert.Multiple(() =>
            {
                Assert.NotNull(g);
                Assert.NotNull(g.kek);
                Assert.NotZero(g.kek.Count);
            });
        }

        [Test]
        public void When_ClassFromDll_Expect_FakeIt()
        {
            H h = faker.Create<H>();
            Assert.Multiple(() =>
            {
                Assert.NotZero(h.b);
                Assert.NotNull(h.time);
                Assert.NotZero(h.time.Hour);
                Assert.NotZero(h.time.Minute);
                Assert.NotZero(h.time.Second);
                Assert.NotZero(h.time.Millisecond);
            });
        }

        [Test]
        public void When_NotSupportedType_Expect_Zero()
        {
            I i = faker.Create<I>();
            Assert.Multiple(() =>
            {
                Assert.Zero(i.a);
            });
        }
    }
}
