using Polynomials;
using IntegerMethods;

namespace PolynomialTests
{
    [TestClass]
    internal class CharacterPolynomialTests
    {
        [TestMethod]
        public void CharPolyConstructorTest()
        {
            CharacterPolynomial p = new();
            CharacterPolynomial q = new(4);
            CharacterPolynomial r = new(new List<Partition>() { new(new List<int>() { 4, 1 }) }, [new(2, 4)]);

            Assert.IsTrue(p.Coefs.Count == 0);

            Assert.IsTrue(q.Terms.Count == 1 && q.Terms[0].Equals(new Partition(new List<int>())));
            Assert.IsTrue(r.Coefs.Count == r.Terms.Count);
        }

        [TestMethod]
        public void EvaluateTest()
        {
            List<Partition> partitions = new() { new(new List<int>() { 2, 1 }), 
                new(new List<int>() { 1 }) };

            List<BigRational> coefs = new() { 2, -2 };

            CharacterPolynomial p = new(partitions, coefs);
            Partition mu = new(new List<int> { 3, 2, 1 });
            Assert.IsTrue(p.Evaluate(mu) == 0);
            mu = new(new List<int> { 2, 1, 1, 1, 1 });
            Assert.IsTrue(p.Evaluate(mu) == -6);
        }
    }
}
