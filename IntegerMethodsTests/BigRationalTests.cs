using IntegerMethods;

namespace IntegerMethodsTests
{
    [TestClass]
    public class BigRationalTests
    {

        [TestMethod]
        public void Test1()
        {
            BigRational a = 4;
            BigRational b = 20;

            Assert.AreEqual(-5 * a, -1 * b);
        }

        [TestMethod]
        public void Test2()
        {
            BigRational a = new(4, 17);
            BigRational b = new(13, 17);
            Assert.IsTrue(1 == (a + b));
            Assert.IsTrue(new BigRational(9, 17) == (b - a));
        }

        [TestMethod]
        public void Test3()
        {
            Assert.IsTrue(new BigRational(3, 11) < new BigRational(1, 3));
            Assert.IsFalse(new BigRational(3, 11) < new BigRational(3, 11));
        }
    }
}
