using IntegerMethods;
using Polynomials;
using RepStability;

namespace RepStabilityTests
{
    [TestClass]
    public class PolyStatisticsTests
    {
        [TestMethod]
        public void ChenResultsTest1()
        {
            Partition part = new(new List<int> { 1 });
            LaurentPolynomial rslt = PolyStatistic.CharPolyToPowSeries(
                YoungToChar.PartToCharPoly(part), 10);

            Polynomial expectedrslt = new(new List<int>() { 0, -1, 2, -2, 2, -2, 2, -2, 2, -2, 2 }); 
            Assert.IsTrue(rslt == new LaurentPolynomial(expectedrslt));

        }

        [TestMethod]
        public void ChenResultsTest2()
        {
            Partition part = new(new List<int> { 1, 1 });
            LaurentPolynomial rslt = PolyStatistic.CharPolyToPowSeries(
                YoungToChar.PartToCharPoly(part), 10);

            Polynomial expectedrslt = new(new List<int>() { 0, 0, 2, -5, 6, -7, 10, -13, 14, -15, 18 });
            Assert.IsTrue(rslt == new LaurentPolynomial(expectedrslt));
        }


        [TestMethod]
        public void ChenResultsTest3()
        {
            Partition part = new(new List<int> { 2 });
            LaurentPolynomial rslt = PolyStatistic.CharPolyToPowSeries(
                YoungToChar.PartToCharPoly(part), 10);

            Polynomial expectedrslt = new(new List<int>() { 0, -1, 2, -3, 6, -9, 10, -11, 14, -17, 18 });
            Assert.IsTrue(rslt == new LaurentPolynomial(expectedrslt));

        }
    }
}
