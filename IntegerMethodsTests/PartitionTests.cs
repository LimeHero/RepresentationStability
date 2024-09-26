using IntegerMethods;

namespace IntegerMethodsTests
{
    [TestClass]
    public class PartitionTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            List<int> l = [8, 2, 1, 1];
            List<int> cycs = [ 2, 1, 0, 0, 0, 0, 0, 1 ];
            List<Tuple<int, int>> l2 = [new(8, 1), new(2, 1), new(1, 2)];

            Assert.AreEqual(new Partition(l), new Partition(cycs, true));
            Assert.AreEqual(new Partition(l), new Partition(l2));

            List<int> _cycs = new Partition(l).GetCycles();

            for (int i = 0; i < _cycs.Count; i++)
            {
                Assert.AreEqual(cycs[i], _cycs[i]);
            }
        }

        [TestMethod]
        public void OrderTest()
        {
            List<int> l = [3];
            Partition p1 = new(l);
            l = [4];
            Partition p2 = new(l);
            l = [3, 1];
            Partition p3 = new(l);
            l = [2, 1, 1];
            Partition p4 = new(l);
            l = [5];
            Partition p5 = new(l);

            Assert.IsTrue(p1 < p2);
            Assert.IsTrue(p2 < p3);
            Assert.IsTrue(p3 < p4);
            Assert.IsTrue(p2 < p4);
            Assert.IsTrue(p4 < p5);
            Assert.IsTrue(p4 > p2);

            Assert.IsFalse(p2 < new Partition(p2));
            Assert.IsFalse(p3 < new Partition(p3));
        }

        [TestMethod]
        public void OrderTest2()
        {
            Partition p = new();

            foreach (Partition q in Partition.AllPartitions(6))
            {
                Assert.IsTrue(p < q);
                p = new(q);
            }
        }
    }
}
