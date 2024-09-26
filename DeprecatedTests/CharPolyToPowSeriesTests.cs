using IntegerMethods;
using Polynomials;
using Deprecated;
using RepStability;

namespace DeprecatedTests
{
    [TestClass]
    public class SymmPolyToValuesTests
    {
        [TestMethod]
        public void SymToChoose1()
        {
            SymmetricPolynomial ex = SymmetricPolynomial.Elementary(2) - SymmetricPolynomial.Elementary(1) + 1;
            SymmetricPolynomial ex2 = YoungToSymmPoly.WedgeToSymmetricPolynomial(2);

            Assert.AreEqual(ex, ex2);
        }

        [TestMethod]
        public void SymToChoose2()
        {
            SymmetricPolynomial ex = SymmetricPolynomial.Elementary(2) * SymmetricPolynomial.Elementary(1)
                + SymmetricPolynomial.Elementary(5) * SymmetricPolynomial.Power(4) + 4;

            SymmetricPolynomial ex2 = CharPolyToPowSeries.ChooseBasisToSymmetric(CharPolyToPowSeries.SymmetricInChooseBasis(ex));
            Assert.IsTrue(ex == ex2);
        }

        [TestMethod]
        public void SymToChoose3()
        {
            SymmetricPolynomial ex = SymmetricPolynomial.Elementary(2) - SymmetricPolynomial.Elementary(1) + 1;

            SymmetricPolynomial ex2 = CharPolyToPowSeries.ChooseBasisToSymmetric(CharPolyToPowSeries.SymmetricInChooseBasis(ex));
            Assert.IsTrue(ex == ex2);
        }

        [TestMethod]
        public void CyclicToPolynomialTest1()
        {
            Tuple<List<List<Tuple<int, int>>>, List<BigRational>> allTerms = new(new List<List<Tuple<int, int>>>()
            { new List<Tuple<int, int>>() { new Tuple<int,int>(1,1) } }, new() { 1 });
            // (1 C 1) to polynomial
            // (simplest case)

            LaurentPolynomial result = PolyStatistic.CharPolyToPowSeries(new(allTerms.Item1, allTerms.Item2), -5);

            // 1 - 2q^{-1} + 2q^{-2} - 2q^{-3} + ...
            LaurentPolynomial expected = new LaurentPolynomial(-5, new List<BigRational>() { -2, 2, -2, 2, -2, 1 });

            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        public void CyclicToPolynomialTest2()
        {
            Tuple<List<List<Tuple<int, int>>>, List<BigRational>> allTerms = new(new()
            { new() { new(1, 1) } , new() { new(1, 2) } }, new() { 1, 1 });
            // (1 C 1) + (1 C 2) to polynomial

            LaurentPolynomial result = PolyStatistic.CharPolyToPowSeries(new(allTerms.Item1, allTerms.Item2), -6);

            LaurentPolynomial expected = new LaurentPolynomial(-6, new List<BigRational>() { 14, -12, 10, -8, 6, -4, new(3, 2) });

            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        public void CyclicToPolynomialTest3()
        {
            Tuple<List<List<Tuple<int, int>>>, List<BigRational>> allTerms = new(new()
            { new() { new(1, 1), new(1, 2) } }, new() { 1, 1 });
            // (1 C 1) * (1 C 2) to polynomial

            // 
            LaurentPolynomial result = PolyStatistic.CharPolyToPowSeries(new(allTerms.Item1, allTerms.Item2), -8);

            LaurentPolynomial expected = new LaurentPolynomial(-8, new List<BigRational>() {new (145, 2), new(-113, 2), new(85, 2), new(-61, 2),
                new(41,2), new(-25,2), new(13,2), new(-5,2), new(1,2) });

            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        public void CyclicToPolynomialTest4()
        {
            Tuple<List<List<Tuple<int, int>>>, List<BigRational>> allTerms = new(new()
            { new() { new(2, 1) } }, new() { 1 });
            // (2 C 1) to polynomial

            LaurentPolynomial result = PolyStatistic.CharPolyToPowSeries(new(allTerms.Item1, allTerms.Item2), -7);
            LaurentPolynomial expected = new(-7, new() { 1, 0, -1, 0, 1, 0, -1, new(1, 2) });

            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        public void CyclicToPolynomialTest5()
        {
            Tuple<List<List<Tuple<int, int>>>, List<BigRational>> allTerms = new(new()
            { new() { new(2, 1), new(1,2) } }, new() { 1 });
            // (2 C 1) * (1 C 2) to polynomial

            LaurentPolynomial result = PolyStatistic.CharPolyToPowSeries(new(allTerms.Item1, allTerms.Item2), -5);
            LaurentPolynomial expected = new(-5, new() { new(-21, 4), new(17, 4), new(-15, 4), new(11, 4), new(-5, 4), new(1, 4) });

            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        public void CyclicToPolynomialTest6()
        {
            Tuple<List<List<Tuple<int, int>>>, List<BigRational>> allTerms = new(new()
            { new() { new(4, 2), new(3, 3) }, new() { new Tuple<int, int>(6, 2) }, new() { new(2, 5) } }, new() { 4, -27, 6 });
            // differently computed ending terms are the same

            LaurentPolynomial result1 = PolyStatistic.CharPolyToPowSeries(new(allTerms.Item1, allTerms.Item2), -10);
            LaurentPolynomial result2 = PolyStatistic.CharPolyToPowSeries(new(allTerms.Item1, allTerms.Item2), -15);

            result2.RoundToNthDegree(-10);

            Assert.IsTrue(result1 == result2);
        }
    }
}