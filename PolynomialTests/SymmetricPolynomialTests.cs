
using IntegerMethods;
using Polynomials;

namespace PolynomialTests
{
    [TestClass]
    public class SymmetricPolynomialTests
    {
        [TestMethod]
        public void ElementarySymmetricTest()
        {
            SymmetricPolynomial elt = SymmetricPolynomial.Elementary(5);

            List<int> term = SymmetricPolynomial.monomial[elt.coefs.Count - 1];

            Assert.IsTrue(elt.coefs[elt.coefs.Count - 1] == 1);
            Assert.IsTrue(term.Count == 5);
            for (int i = 0; i < term.Count; i++)
            {
                Assert.IsTrue(term[i] == 1);
            }
        }

        [TestMethod]
        public void PowerSymmetricTest()
        {
            SymmetricPolynomial elt = SymmetricPolynomial.Power(5);

            List<int> term = SymmetricPolynomial.monomial[elt.coefs.Count - 1];

            Assert.IsTrue(elt.coefs[elt.coefs.Count - 1] == 1);
            Assert.IsTrue(term.Count == 1);
            Assert.IsTrue(term[0] == 5);
        }

        [TestMethod]
        public void PowerPrimeSymmetricTest()
        {
            SymmetricPolynomial elt = SymmetricPolynomial.PowerPrime(4);

            Assert.IsTrue(4 * elt == new SymmetricPolynomial(new List<int> { 0, 0, -1, 0, 0, 0, 0, 1 }));
        }

        [TestMethod]
        public void SymmetricPolynomialChooseTest()
        {
            SymmetricPolynomial elt = new SymmetricPolynomial(new List<int> { 1, 1 });

            elt = SymmetricPolynomial.Choose(elt, 3);
            //(t_i + 1)(t_i)(t_i - 1)/6
            Assert.IsTrue(6 * elt == new SymmetricPolynomial(new List<int> { 0, -1, 0, 0, 1, 3, 6 }));
        }

        [TestMethod]
        public void SymmetricPolynomialTest1()
        {
            SymmetricPolynomial elt1 = new SymmetricPolynomial(new List<BigRational> { new(2, 7), new(-1, 2) });
            SymmetricPolynomial elt2 = new SymmetricPolynomial(new List<BigRational> { new(-4, 3), 7 });

            SymmetricPolynomial expected = new(new List<BigRational>() { new(-8, 21), new(8, 3), new(-7, 2), -7 });
            //(t_i + 1)(t_i)(t_i - 1)
            Assert.IsTrue(elt1 * elt2 == expected);
        }

        [TestMethod]
        public void SymmetricPolynomialTest2()
        {
            SymmetricPolynomial elt = new(new List<int> { 2, 1 });

            SymmetricPolynomial expected = new(new List<int> { 4, 4, 1, 2 });
            Assert.IsTrue((elt * elt) == expected);
        }

        [TestMethod]
        public void SymmetricPolynomialTest3() // Winston was here! if u delete this comment im gonna b mad
        {
            // heinous computation by hand

            // (1 - 2a^2 + abc + a^4)(a + ab + a^3)
            SymmetricPolynomial elt1 = new SymmetricPolynomial(new List<int> { 1, 0, -2, 0, 0, 0, 1, 1 });
            SymmetricPolynomial elt2 = new SymmetricPolynomial(new List<int> { 0, 1, 0, 1, 1 });

            // = (a) + (- 2a^3 - 2a^2b) + (a^2bc + 4abcd) + (a^5 + a^4b)
            // + (ab) + (-2a^3b - 2a^2bc) + (a^2b^2c + 3a^2bcd + 10abcde) + (a^5b + a^4bc)
            // + (a^3) + (-2a^5 - 2a^3b^2) + (a^4bc + a^3bcd) + (a^7 + a^4b^3)

            // = 0 + a + 0a^2 + ab - a^3 - 2a^2b + 0abc + 0a^4 - 2a^3b + 0a^2b^2 - a^2bc + 4abcd
            // - a^5 + a^4b - 2a^3b^2 + 0a^3bc + a^2b^2c + 3a^2bcd + 10abcde + 0a^6 + a^5b + 0a^4b^2 + 2a^4bc
            // + 0a^3b^3 + 0a^3b^2c + a^3bcd + 0 + 0 + 0 + 0 + a^7 + 0 + 0 + 0 + a^4b^3
            Assert.IsTrue(elt1 * elt2 == new SymmetricPolynomial(new List<int> { 0, 1, 0, 1, -1, -2, 0, 0, -2, 0,
                -1, 4, -1, 1, -2, 0, 1, 3, 10, 0, 1, 0, 2, 0, 0,
                1, 0, 0, 0, 0, 1, 0, 0, 0, 1}));
        }
    }
}
