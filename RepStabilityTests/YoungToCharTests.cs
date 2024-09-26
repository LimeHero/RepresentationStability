using IntegerMethods;
using Polynomials;
using RepStability;

namespace RepStabilityTests
{
    [TestClass]
    public class YoungToCharTests
    {
        [TestMethod]
        public void Test1()
        {
            Partition p = new(new List<int> { 1 });

            CharacterPolynomial rslt = YoungToChar.PartToCharPoly(p);

            Assert.IsTrue(rslt.Coefs.Count == 2);
            Partition mu = new(new List<int> { 4, 3, 2, 2, 2, 2, 1, 1, 1 });
            Assert.IsTrue(rslt.Evaluate(mu) == 2);
            mu = new(new List<int> { 4, 3, 2, 2, 2, 2 });
            Assert.IsTrue(rslt.Evaluate(mu) == -1);
        }

        [TestMethod]
        public void Test2()
        {
            Partition p = new(new List<int> { 1, 1 });

            CharacterPolynomial rslt = YoungToChar.PartToCharPoly(p);
            // i dont have an equals function for char polys but it should be equal to
            // (X_1 C 2) - X_1 - X_2 + 1

            Assert.IsTrue(rslt.Coefs.Count == 4);
            Partition mu = new(new List<int> { 4, 3, 2, 2, 2, 2, 1, 1, 1 });
            Assert.IsTrue(rslt.Evaluate(mu) == -3);
            mu = new(new List<int> { 4, 3, 1, 1, 1, 1, 1, 1, 1 });
            Assert.IsTrue(rslt.Evaluate(mu) == 15);
        }
    }
}
