using Polynomials;
using IntegerMethods;

namespace PolynomialTests
{
    [TestClass]
    public class LaurentPolynomialTests
    {
        [TestMethod]
        public void LaurentPolynomialTest1()
        {
            LaurentPolynomial poly1 = new LaurentPolynomial(4);
            LaurentPolynomial poly2 = new LaurentPolynomial(5);

            Assert.IsTrue(poly1.Coefs[0] * poly2.Coefs[0] == new BigRational(20, 1));
            Assert.IsTrue(poly1.Coefs[0] / poly2.Coefs[0] == new BigRational(4, 5));
        }

        [TestMethod]
        public void LaurentPolynomialTest2()
        {
            LaurentPolynomial poly1 = new LaurentPolynomial(-1, new List<BigRational> { 4, 1, 0, 1 });
            LaurentPolynomial poly2 = new LaurentPolynomial(1, new List<BigRational> { 1, -1 });

            LaurentPolynomial diff = poly1 - poly2;
            LaurentPolynomial prod = poly1 * poly2;
            Assert.IsTrue(diff == new LaurentPolynomial(-1, new List<BigRational> { 4, 1, -1, 2 }));
            Assert.IsTrue(prod == new LaurentPolynomial(0, new List<BigRational> { 4, -3, -1, 1, -1 }));
        }

        [TestMethod]
        public void LaurentPolynomialTest3()
        {
            LaurentPolynomial poly1 = new LaurentPolynomial(-2, new List<BigRational> { -4, 1, 0, -7 });
            LaurentPolynomial poly2 = new LaurentPolynomial(-1, new List<BigRational> { 1, -2, 4 });

            LaurentPolynomial prod = poly1 * poly2;
            Assert.IsTrue(prod == new LaurentPolynomial(-3, new List<BigRational> { -4, 9, -18, -3, 14, -28 }));
        }

        [TestMethod]
        public void LaurentPolynomialIndexTest()
        {
            LaurentPolynomial poly1 = new LaurentPolynomial(-2, new List<BigRational> { -4, 1, 0, -7 });
            LaurentPolynomial poly2 = new LaurentPolynomial(-1, new List<BigRational> { 1, -2, 4 });

            Assert.IsTrue(poly1[-2] == -4);
            Assert.IsTrue(poly1[-3] == 0);
            Assert.IsTrue(poly1[1] == -7);
            Assert.IsTrue(poly1[4] == 0);
            Assert.IsTrue((poly1 + poly2)[-1] == 2);
        }

        [TestMethod]
        public void LaurentRoundTest1()
        {
            LaurentPolynomial poly = new LaurentPolynomial(-3, new List<BigRational> { 0, 0, 1, 1, 2, 384823942 });

            poly.RoundLeadToN(1);

            Assert.IsTrue(poly == new LaurentPolynomial(1, new List<BigRational> { 2, 384823942 }));
        }

        [TestMethod]
        public void LaurentRoundTest2()
        {
            LaurentPolynomial poly = new LaurentPolynomial(-3, new List<BigRational> { 0, 0, 1, 1, 2, 384823942 });

            poly.LeadingNTerms(2);

            Assert.IsTrue(poly == new LaurentPolynomial(1, new List<BigRational> { 2, 384823942 }));
        }

        [TestMethod]
        public void LaurentRoundTest3()
        {
            LaurentPolynomial poly = new LaurentPolynomial(-3, new List<BigRational> { 0, 0, 1, 1, 2, 384823942 });

            poly.FirstNTerms(2);

            Assert.IsTrue(poly == new LaurentPolynomial(-1, new List<BigRational> { 1, 1 }));
        }
    }
}
