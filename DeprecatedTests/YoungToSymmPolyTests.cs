using IntegerMethods;
using Polynomials;
using Deprecated;
using RepStability;

namespace DeprecatedTests
{
    [TestClass]
    public class YoungToSymmTests
    {
        [TestMethod]
        public void WedgeToSymmPolyTest()
        {
            SymmetricPolynomial ex = SymmetricPolynomial.Elementary(4) -
                SymmetricPolynomial.Elementary(3) + SymmetricPolynomial.Elementary(2) - SymmetricPolynomial.Elementary(1) + 1;

            Assert.IsTrue(ex == YoungToSymmPoly.WedgeToSymmetricPolynomial(4));
        }

        [TestMethod]
        public void WedgeToPolynomialTest1()
        {
            LaurentPolynomial result = PolyStatistic.CharPolyToPowSeries(YoungToSymmPoly.WedgeToSymmetricPolynomial(1), -6);

            LaurentPolynomial expected = new(-6, new List<BigRational>() { 2, -2, 2, -2, 2, -1 });

            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        public void TwoRowsTest2()
        {
            List<int> tableau = new List<int> { 7, 6 };

            Tuple<List<List<Tuple<int, int>>>, List<BigRational>> terms = YoungToSymmPoly.FrobPartToCharPoly(new List<int> { tableau[1] });

            foreach (List<int> part in Util.AllPartitions(tableau.Sum()))
            {
                List<int> cycles = Util.PartitionToNumCycles(part);

                Assert.IsTrue(RepTheoryAlgs.FrobeniusFormula(tableau, cycles) ==
                    RepTheoryAlgs.ChooseFormToCharacter(terms, cycles));
            }
        }

        [TestMethod]
        public void ThreeRowsTest1()
        {
            List<int> tableau = new List<int> { 4, 2, 1 };

            Tuple<List<List<Tuple<int, int>>>, List<BigRational>> terms =
                YoungToSymmPoly.FrobPartToCharPoly(new List<int> { tableau[1], tableau[2] });

            foreach (List<int> part in Util.AllPartitions(tableau.Sum()))
            {
                List<int> cycles = Util.PartitionToNumCycles(part);

                Assert.IsTrue(RepTheoryAlgs.FrobeniusFormula(tableau, cycles) ==
                    RepTheoryAlgs.ChooseFormToCharacter(terms, cycles));
            }
        }

        [TestMethod]
        public void ThreeRowsTest2()
        {
            List<int> tableau = new List<int> { 8, 5, 3 };

            Tuple<List<List<Tuple<int, int>>>, List<BigRational>> terms =
                YoungToSymmPoly.FrobPartToCharPoly(new List<int> { tableau[1], tableau[2] });

            foreach (List<int> part in Util.AllPartitions(tableau.Sum()))
            {
                List<int> cycles = Util.PartitionToNumCycles(part);

                Assert.IsTrue(RepTheoryAlgs.FrobeniusFormula(tableau, cycles) ==
                    RepTheoryAlgs.ChooseFormToCharacter(terms, cycles));
            }
        }

        [TestMethod]
        public void ThreeRowsTest3()
        {
            Tuple<List<List<Tuple<int, int>>>, List<BigRational>> terms =
                YoungToSymmPoly.FrobPartToCharPoly(new List<int> { 1, 1 });

            Tuple<List<List<Tuple<int, int>>>, List<BigRational>> expterms =
                CharPolyToPowSeries.SymmetricInChooseBasis(YoungToSymmPoly.WedgeToSymmetricPolynomial(2));

            // since terms may not be reduced (i.e., same terms combined)
            // easier to just evaluate each on some cycles and this will ensure they are the same
            foreach (List<int> part in Util.AllPartitions(8))
            {
                List<int> cycles = Util.PartitionToNumCycles(part);

                Assert.IsTrue(RepTheoryAlgs.ChooseFormToCharacter(terms, cycles)
                    == RepTheoryAlgs.ChooseFormToCharacter(expterms, cycles));
            }
        }

        [TestMethod]
        public void YoungDiagramToChooseTest4()
        {
            LaurentPolynomial expseries =
                CharPolyToPowSeries.SymmPolyToPowerSeries(YoungToSymmPoly.WedgeToSymmetricPolynomial(5));

            Tuple<List<List<Tuple<int, int>>>, List<BigRational>> terms =
                YoungToSymmPoly.FrobPartToCharPoly(new() { 1, 1, 1, 1, 1 });

            LaurentPolynomial result = CharPolyToPowSeries.CyclicPolynomialBasisToPolynomial(terms, -10);

            Assert.IsTrue(result == expseries);
        }
    }
}
