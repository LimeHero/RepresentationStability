using Polynomials;
using IntegerMethods;

namespace PolynomialTests
{
    [TestClass]
    public class PolynomialTests
    {
        [TestMethod]
        public void PolynomialTest1()
        {
            Polynomial poly1 = new (4);
            Polynomial poly2 = new (5);

            Assert.IsTrue(poly1.Coefs[0] * poly2.Coefs[0] == new BigRational(20, 1));
            Assert.IsTrue(poly1.Coefs[0] / poly2.Coefs[0] == new BigRational(4, 5));
        }

        [TestMethod]
        public void PolynomialTest2()
        {
            Polynomial poly1 = new Polynomial(new List<int> { 4, 1, 0, 1 });
            Polynomial poly2 = new Polynomial(new List<BigRational> { 1, -1 });

            Polynomial diff = poly1 - poly2;
            Polynomial prod = poly1 * poly2;
            Assert.IsTrue(diff == new Polynomial(new List<int> { 3, 2, 0, 1 }));
            Assert.IsTrue(prod == new Polynomial(new List<int> { 4, -3, -1, 1, -1 }));
        }

        [TestMethod]
        public void PolynomialDivTest1()
        {
            Polynomial poly1 = new Polynomial(new List<BigRational> { -2, 1, 0, 1 });
            Polynomial poly2 = new Polynomial(new List<BigRational> { 1, -1 });

            Tuple<Polynomial, Polynomial> divis = poly1 / poly2;
            Polynomial quot = divis.Item1;
            Polynomial rem = divis.Item2;
            Assert.IsTrue(quot == new Polynomial(new List<BigRational> { -2, -1, -1 }));
            Assert.IsTrue(rem == new Polynomial(0));
        }

        [TestMethod]
        public void PolynomialDivTest2()
        {
            Polynomial poly1 = new Polynomial(new List<int> { 2, 1, 0, 1 });
            Polynomial poly2 = new Polynomial(new List<BigRational> { 1, -1 });

            Tuple<Polynomial, Polynomial> divis = poly1 / poly2;
            Polynomial quot = divis.Item1;
            Polynomial rem = divis.Item2;
            Assert.IsTrue(quot == new Polynomial(new List<int> { -2, -1, -1 }));
            Assert.IsTrue(rem == new Polynomial(4));
        }

        [TestMethod]
        public void PolynomialGCDTest1()
        {
            Polynomial poly1 = new Polynomial(new List<int> { -2, 1, 0, 1 });
            Polynomial poly2 = new Polynomial(new List<BigRational> { 1, -1 });

            Assert.IsTrue(Polynomial.GCD(poly1, poly2) == ((-1) * poly2));
        }


        [TestMethod]
        public void PolynomialMoebiusSumTest1()
        {
            Polynomial moebSum = Polynomial.MoebiusSum(4);

            Assert.IsTrue(4 * moebSum == new Polynomial(new List<BigRational> { 0, 0, -1, 0, 1 }));
        }


        [TestMethod]
        public void PolynomialMoebiusSumTest2()
        {
            Polynomial moebSum = Polynomial.MoebiusSum(12);

            Assert.IsTrue(12 * moebSum == new Polynomial(new List<BigRational> { 0, 0, 1, 0, -1, 0, -1, 0, 0, 0, 0, 0, 1 }));
        }
    }
}