using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace IntegerMethods
{
    /// <summary>
    /// Stores a partition of n in two ways - as a positive list of integers
    /// and in cycle form. For instance, the partition
    ///          13 = 5 + 2 + 2 + 2 + 1 + 1
    /// Is represented by (Part) [5, 2, 2, 2, 1, 1] and (Cycles) [2, 3, 0, 0, 1]
    /// 
    /// Note that the empty partition is represented by Part = [], Cycles = [].
    /// 
    /// We can 
    /// 
    /// This class implements a simple hash function which allows
    /// for storing arbitrary types T indexed by a permutation,
    /// which is useful for memoization.
    /// </summary>
    public class Partition
    {
        private List<int>? Part = null;
        private List<int>? Cycles = null;

        /// <summary>
        /// Creates new partition with the given list, as long as 
        /// part is a non-positive list of positive integers.
        /// 
        /// If cycles = true, initializes the new Partition via its cycles, in
        /// which case part must be a list of nonnegative integers.
        /// </summary>
        /// <param name="part">Either decreasing list of positive integers if cycles = true or list of positive integers if cycles = false</param>
        public Partition(List<int> part, bool cycles = false)
        {
            if (cycles)
            {
                foreach (int i in part)
                    if (i < 0)
                        throw new ArgumentException("Cycles must all be nonnegative");

                Cycles = new(part);
                return;
            }
            for (int i = 1; i < part.Count; i++)
            {
                if (part[i] > part[i - 1])
                    throw new ArgumentException("Partition must non-decreasing");

            }
            if (part.Count > 0 && part[^1] < 0)
                throw new ArgumentException("Partition must be of non-negative integers");

            Part = new(part);


        }

        /// <summary>
        /// Initializes a partition from a list of the number of each cycle. For instance,
        /// {(1, 3), (4, 2)} -> [4 + 4 + 1 + 1 + 1].
        /// </summary>
        /// <param name="numEachCycle"></param>
        public Partition(List<Tuple<int, int>> numEachCycle)
        {
            Part = [];

            for (int i = 0; i < numEachCycle.Count; i++)
                for (int j = 0; j < numEachCycle[i].Item2; j++)
                    Part.Add(numEachCycle[i].Item1);

            Part.Sort();
            Part.Reverse();

            if (Part.Count > 0 && Part[^1] <= 0)
                throw new ArgumentException("Cycle numbers must be positive");
        }

        /// <summary>
        /// Creates a copy of the partition _perm
        /// </summary>
        /// <param name="p"></param>
        public Partition(Partition part) : this(part.GetList())
        {

        }

        /// <summary>
        /// Creates empty partition.
        /// </summary>
        public Partition()
        {
            Part = [];
            Cycles = [];
        }

        /// <summary>
        /// Returns what integer this is a partition of. For instance,
        /// K = [4 + 3 + 1] is a partition of 8, so K.Size() = 8.
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            Part ??= new(GetList());

            return Part.Sum();
        }

        /// <summary>
        /// Compares left and right first by size and then (reverse) lexigraphically. 
        /// For instance, the following are ordered least to greatest.
        /// 
        /// [3] < [4] < [3, 1] < [1, 1, 1, 1] < [5]
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(Partition left, Partition right)
        {
            if (left.Size() != right.Size())
                return left.Size() < right.Size();

            left.Part ??= new(left.GetList());
            right.Part ??= new(right.GetList());

            int i = 0;
            while (i < left.Part.Count && i < right.Part.Count)
            {
                if (left.Part[i] != right.Part[i])
                    return left.Part[i] > right.Part[i];

                i++;
            }

            // in this case, they are equal
            return false;
        }

        /// <summary>
        /// Compares left and right first by size and then (reverse) lexigraphically. 
        /// For instance, the following are ordered least to greatest.
        /// 
        /// [3] < [4] < [3, 1] < [1, 1, 1, 1] < [5]
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(Partition left, Partition right)
        {
            return right < left;
        }

        /// <summary>
        /// Returns this partition as a non-increasing list of positive integers
        /// </summary>
        /// <returns></returns>
        public List<int> GetList()
        {
            if (Part is not null)
                return new(Part);

            Part = new(LoadList());
            return new(Part);
        }

        /// <summary>
        /// Returns this partition in cycle form
        /// </summary>
        /// <returns></returns>
        public List<int> GetCycles()
        {
            if (Cycles is not null)
                return new(Cycles);

            Cycles = new(LoadCycles());
            return new(Cycles);
        }

        /// <summary>
        /// Loads Perm from null from the values of Cycles
        /// </summary>
        private List<int> LoadList()
        {
            if (Cycles is null)
                throw new Exception("How are they both null?");

            List<int> l = [];
            for (int i = Cycles.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < Cycles[i]; j++)
                    l.Add(i + 1);
            }

            return l;
        }

        /// <summary>
        /// Loads Cycles from null from the values of Perm
        /// </summary>
        private List<int> LoadCycles()
        {
            if (Part is null)
                throw new Exception("How are they both null?");

            if (Part.Count == 0)
                return [];

            List<int> cycles = [];
            int j = Part.Count - 1;

            for (int i = 1; i <= Part[0]; i++)
            {
                cycles.Add(0);
                if (Part[j] > i)
                    continue;

                while (j >= 0 && Part[j] == i)
                {
                    cycles[i - 1]++;
                    j--;
                }
            }

            return cycles;
        }

        /// <summary>
        /// Returns this partition as a string in the form [4, 2, 1, 1]
        /// </summary>
        /// <returns></returns>
        override public string ToString()
        {
            Part ??= new(GetList());

            if (Part.Count == 0)
            {
                return "[]";
            }

            string s = "[";
            for (int j = 0; j < Part.Count - 1; j++)
            {
                s += Part[j] + " , ";
            }
            s += (Part[^1] + "]");
            return s;
        }


        /// <summary>
        /// Returns this partition as a string in the form [4s2s1s1] where
        /// s is the string spacing.
        /// </summary>
        /// <param name="spacing"></param>
        /// <returns></returns>
        public string ToStringWithCharSpacing(string spacing)
        {
            Part ??= new(GetList());

            if (Part.Count == 0)
            {
                return "[]";
            }

            string s = "[";
            for (int j = 0; j < Part.Count - 1; j++)
            {
                s += Part[j] + spacing;
            }
            s += (Part[^1] + "]");
            return s;
        }

        /// <summary>
        /// Returns true if and only if obj is of type Permutation,
        /// and their permutations match.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != this.GetType())
                return false;

            List<int> castedObjPerm = ((Partition)obj).GetList();
            List<int> myPerm = GetList();

            if (myPerm.Count != castedObjPerm.Count)
                return false;

            for (int i = 0; i < myPerm.Count; i++)
                if (myPerm[i] != castedObjPerm[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Returns a simple hash based *solely* on perm.
        /// Notice that this is a pure hash function so long as all the permutations
        /// are of 16 or less.
        /// 
        /// Returns perm[0] + perm[1]*2^4 + perm[2]*2^8 + ... + perm[^1]*2^(perm.Count * 4)
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            Part ??= new(LoadList());

            if (Part.Count == 0)
                return -1;

            int rslt = 0;
            for (int i = 0; i < Part.Count; i++)
                rslt += Part[i] << (i * 4);

            return rslt;
        }


        // Memoization for CoefOfPowerSeries
        private static readonly Dictionary<Tuple<int, int>, BigRational> coefOfPSValues = [];
        /// <summary>
        /// Returns the coefficient of z^l in (z - z^2 + ...)^j, which is given by
        /// (-1)^{l - j}(l - 1 C l -j)
        /// </summary>
        /// <param name="l"></param>
        /// <param name="j"></param>
        public static BigRational CoefOfPowerSeries(int l, int j)
        {
            Tuple<int, int> t = new(l, j);
            if (coefOfPSValues.TryGetValue(t, out BigRational? value))
                return value;

            int sgn = ((l - j) % 2 == 1) ? -1 : 1;
            BigInteger coef = sgn * Util.BinomialCoef(l - 1, l - j);

            coefOfPSValues[t] = coef;
            return coef;
        }

        /// <summary>
        /// Returns the number of partitions of n
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static BigInteger Partitions(BigInteger n)
        {
            if (n < 0)
                return 0;

            //using the recurrence relation, we must solve for partitions(n-1), ... partitions(0),
            //so it is just as fast to call the iterator of partitions.
            BigInteger i = 0;
            foreach (BigInteger part in IterPartitions())
            {
                if (i == n)
                    return part;
                i++;
            }

            //this should never be reached
            return 0;
        }

        /// <summary>
        /// Returns the partitions of n as n increments starting at n = 0.
        /// I.e., returns p(0), p(1), ...
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IEnumerable<BigInteger> IterPartitions()
        {
            List<BigInteger> partitionValues = [1];
            yield return 1;
            //note that this is indexed so pentagonal[i] = the (i+1)th pentagonal number
            List<int> pentNums = [1];
            while (true)
            {
                if (pentNums[^1] < partitionValues.Count)
                    pentNums.Add((int)Util.Pentagonal(pentNums.Count + 1));

                BigInteger part = 0;
                int j = 0;
                foreach (int k in pentNums)
                {
                    //this is an alternating series + + - - ...
                    if (k > partitionValues.Count)
                        break;
                    if (j % 4 == 0 || j % 4 == 1)
                        part += partitionValues[^k];
                    else
                        part -= partitionValues[^k];
                    j++;
                }

                partitionValues.Add(part);
                yield return part;
            }

        }

        /// <summary>
        /// Returns all the partitions that sum to n in a nice order: (n), (n-1, 1), (n-2, 2), (n-2, 1, 1), ...
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Partition> AllPartitions(int n)
        {
            if (n < 0)
                yield break;

            if (n == 0)
            {
                yield return new();
                yield break;
            }

            List<int> vals = [];
            vals.Add(n);

            int sum = n;
            while (vals[0] > 0)
            {
                int k = Math.Min(n - sum, vals[^1]);
                if (k > 0)
                {
                    vals.Add(k);
                    sum += k;
                    continue;
                }

                if (sum == n)
                    yield return new(vals);

                vals[^1]--;
                sum--;
                for (int j = vals.Count - 1; j > 0; j--)
                {
                    if (vals[j] <= 0)
                    {
                        sum -= vals[j];
                        vals.RemoveAt(j);

                        vals[j - 1]--;
                        sum--;
                    }
                    else
                        break;
                }
            }

            yield break;
        }

        /// <summary>
        /// Iterates through all partitions of l[0], l[1], ... l[-1], where l is a list of integers.
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static IEnumerable<List<Partition>> AllPartitionLists(List<int> l)
        {
            foreach (int b in l) if (b < 0) yield break;
            if (l.Count <= 0)
                yield break;

            if (l.Count == 1)
            {
                foreach (Partition part in AllPartitions(l[0]))
                    yield return new() { part };

                yield break;
            }

            foreach (Partition part in AllPartitions(l[^1]))
            {
                List<int> nextl = []; // remaining terms
                for (int i = 0; i < l.Count - 1; i++)
                    nextl.Add(l[i]);

                foreach (List<Partition> nextlparts in AllPartitionLists(nextl))
                {
                    List<Partition> parts = []; // copy nextlparts
                    foreach (Partition term in nextlparts)
                        parts.Add(new(term));

                    parts.Add(part);

                    yield return parts;
                }
            }

            yield break;
        }

        /// <summary>
        /// Returns all partitions of n into k positive integers. l is the least integer in the 
        /// partition, and is used for recursion.
        /// 
        /// Code adapted from https://stackoverflow.com/questions/18503096/python-integer-partitioning-with-given-k-partitions
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static IEnumerable<Partition> KPartitions(int n, int k, int l = 1)
        {
            if (k < 1)
                yield break;
            if (k == 1)
            {
                if (n >= l)
                    yield return new(new List<int>() { n });
                yield break;
            }

            for (int i = l; i <= n; i++)
            {
                foreach (Partition result in KPartitions(n - i, k - 1, i))
                {
                    List<int> recurs = result.GetList();
                    recurs.Add(i);
                    yield return new(recurs);
                }
            }
        }

    }
}
