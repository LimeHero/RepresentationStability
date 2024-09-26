using System.Numerics;

namespace IntegerMethods
{
    public static class Util
    {
        /// <summary>
        /// Prints a list in a nice format
        /// </summary>
        /// <param name="list"></param>s
        public static void PrintList<T>(List<T> list)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("[]");
                return;
            }

            Console.Write("[");
            for (int j = 0; j < list.Count - 1; j++)
            {
                Console.Write(list[j] + ", ");
            }
            Console.WriteLine(list[^1] + "]");
        }

        /// <summary>
        /// Returns the number of integers less than a that are relatively prime to it.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static BigInteger Phi(BigInteger a)
        {
            List<BigInteger> primes = PrimeFactorize(a);
            BigInteger p = 1;
            BigInteger output = 1;
            foreach (BigInteger k in primes)
            {
                if (p < k)
                {
                    p = k;
                    output *= (p - 1);
                }
                else
                {
                    output *= p;
                }
            }

            return output;
        }

        /// <summary>
        /// checks if the given integer is prime
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool IsPrime(BigInteger a)
        {
            if (a <= 1)
                return false;
            if (a == 2)
                return true;
            if (a % 2 == 0)
                return false;
            for (BigInteger i = 3; i * i <= a; i += 2)
            {
                if (BigInteger.Remainder(a, i) == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a list of the prime factors of a sorted smallest to largest
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static List<BigInteger> PrimeFactorize(BigInteger a)
        {
            if (a <= 1)
                return new();
            if (a == 2)
                return new() { 2 };

            List<BigInteger> output = new();
            //so a will not be modified
            BigInteger n = a;
            while (n % 2 == 0)
            {
                output.Add(2);
                n /= 2;
            }

            for (BigInteger i = 3; i * i <= n; i += 2)
            {
                while (n % i == 0)
                {
                    output.Add(i);
                    n /= i;
                }
            }

            if (n > 1)
                output.Add(n);

            return output;
        }

        /// <summary>
        /// Returns the closest BigInteger square root to n.
        /// </summary>
        /// <returns></returns>
        public static BigInteger Sqrt(BigInteger x)
        {
            if (x < 0)
                throw new ArgumentException("Can't have negative sqrt");

            BigInteger bigX = x;
            BigInteger sqrt = 0;

            int len = bigX.ToString().Length;
            BigInteger nextDig = 1;
            for (int i = 0; i < len / 2; i++)
            {
                nextDig *= 10;
            }

            while (nextDig > 0)
            {
                while ((sqrt + nextDig) * (sqrt + nextDig) <= bigX)
                    sqrt += nextDig;

                nextDig /= 10;
            }

            return sqrt;
        }

        /// <summary>
        /// Returns the sqrt of x to n digits.
        /// </summary>
        /// <typeparam name=""></typeparam>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static List<int> Sqrt(BigInteger x, BigInteger n)
        {
            if (x < 0)
                throw new ArgumentException("Can't take negative square root");

            BigInteger bigX = x;
            BigInteger sqrt = 0;

            List<int> digitList = new() { 0 };

            int len = bigX.ToString().Length;
            BigInteger nextDig = 1;
            for (int i = 0; i < len / 2; i++)
            {
                nextDig *= 10;
                digitList[0]++;
            }

            while (digitList.Count <= n && nextDig > 0)
            {
                while ((sqrt + nextDig) * (sqrt + nextDig) <= bigX)
                    sqrt += nextDig;

                nextDig /= 10;
            }

            List<int> intDigs = Digits(sqrt);
            for (int i = intDigs.Count - 1; i >= 0; i--)
                digitList.Add(intDigs[i]);



            while (digitList.Count <= n)
            {
                sqrt *= 10;
                bigX *= 100;
                while ((sqrt + 1) * (sqrt + 1) <= bigX)
                    sqrt++;

                digitList.Add((int)(sqrt % 10));
            }

            return digitList;
        }

        /// <summary>
        /// Returns n choose k
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static BigRational Choose(int n, int k)
        {
            if (n < 0 || k < 0)
                return 0;

            if (n < k)
                return 0;

            BigInteger num = 1;
            for (BigInteger a = n; a > n - k; a--)
                num *= a;

            BigInteger denom = 1;
            for (BigInteger a = 1; a <= k; a++)
                denom *= a;

            return new BigRational(num, denom);
        }

        /// <summary>
        /// Returns n choose k. n must be nonnegative and 0 <= k <= n.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static BigInteger BinomialCoef(int n, int k)
        {
            if (n < 0)
                throw new ArgumentException("n must be positive");

            if (k < 0 || k > n)
                throw new ArgumentException("k must be between 0 and n");

            if (k > n / 2 + 1) return BinomialCoef(n, n - k);

            BigInteger rslt = 1;
            for (int i = n; i > n-k; i--)
                rslt *= i;

            rslt /= Factorial(k);
            return rslt;
        } 

        /// <summary>
        /// Returns n! / (k[0]! k[1]! ... )
        /// k must be a list of positive integers with sum less then or equal to n
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static BigRational MultinomialCoef(int n, List<int> k)
        {
            if (n < 0)
                return 0;

            foreach (int i in k)
                if (i < 0)
                    return 0;

            if (n < k.Sum())
                return 0;

            BigInteger num = 1;
            for (BigInteger a = 1; a <= n; a++)
                num *= a;

            BigInteger denom = 1;
            foreach (int i in k)
                for (BigInteger a = 1; a <= i; a++)
                    denom *= a;

            return new BigRational(num, denom);
        }

        /// <summary>
        /// Takes a list of digits (as given in the previous function) and returns the corresponding decimal.
        /// </summary>
        /// <param name="digitList"></param>
        /// <returns></returns>
        public static string DecimalToString(List<int> digitList)
        {
            if (digitList.Count < 2)
                throw new ArgumentException("Gotta have more digits");
            int place = digitList[0];
            string dec = "";
            //handles negatives
            if (digitList[1] < 0)
                dec += "-";

            //adds zeros to the decimal
            if (place < -1)
                dec += "0.";
            for (int j = -1; j > place; j--)
                dec += "0";

            //populates the string with digits
            for (int i = 1; i < digitList.Count; i++)
            {
                if (place == -1)
                    dec += ".";

                dec += Math.Abs(digitList[i]);
                place--;
            }

            //if its a long integer, populates with zeros
            while (place >= 0)
            {
                place--;
                dec += "0";
            }

            return dec;
        }

        /// <summary>
        /// Returns whether N is a square number or not.
        /// </summary>
        /// <returns></returns>
        public static bool IsSquare(BigInteger n)
        {
            BigInteger sqrt = Sqrt(n);
            return sqrt * sqrt == n;
        }

        /// <summary>
        /// Returns the gcd of a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            if (a < b)
                return GCD(b, a);

            if (a % b == 0)
            {
                return b;
            }

            return GCD(b, a % b);
        }

        /// <summary>
        /// returns the kth digit of n. returns 0 if k > length_n, throws an exception of
        /// n or k are not positive.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int KthDigit(BigInteger n, int k)
        {
            if (n < 0 || k <= 0)
            {
                throw new ArgumentException(n + ", " + k);
            }
            return (int)BigInteger.Remainder(BigInteger.Divide(n, BigInteger.Pow(10, k - 1)), 10);
        }

        /// <summary>
        /// returns the kth binary digit of n. returns 0 if k > length_n, throws an exception of
        /// n or k are not positive.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int NthBinaryDigit(BigInteger n, int k)
        {
            if (n < 0 || k < 0)
            {
                throw new ArgumentException("cant be negative in NthBinaryDigit!");
            }
            if (n == 0)
                return 0;
            return (int)((n / Pow(2, k - 1)) % 2);
        }

        /// <summary>
        /// Returns a^b, as an integer. For b<=0, returns 1.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger Pow(int a, int b)
        {
            BigInteger output = 1;
            for (int i = 0; i < b; i++)
            {
                output *= a;
            }

            return output;
        }


        /// <summary>
        /// Returns factorial of the given number
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static BigInteger Factorial(int a)
        {
            if (a < 0)
            {
                throw new ArgumentException("Factorial only makes sense with nonnegative #s");
            }

            BigInteger output = 1;
            for (int i = 1; i <= a; i++)
            {
                output *= i;
            }

            return output;
        }

        /// <summary>
        ///  Returns the digits of n in order, such that outputput[0] is the ones digit, and
        /// outputput[1] is the tens.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static List<int> Digits(BigInteger n)
        {
            int size = 1;
            BigInteger k = 10;
            while (n >= k)
            {
                size++;
                k *= 10;
            }

            List<int> output = new();
            for (int i = 1; i <= size; i++)
            {
                output.Add(KthDigit(n, i));
            }

            return output;
        }

        /// <summary>
        /// Takes an array of digits (lowest term first) and returns the corresponding big integer
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static BigInteger DigitsToBigInt(List<int> arr)
        {
            BigInteger output = 0;
            for (int i = 0; i < arr.Count; i++)
            {
                output += arr[i] * BigInteger.Pow(10, i);
            }
            return output;
        }

        /// <summary>
        /// Returns the digits of n in order, such that output[0] is the ones digit, and so on, in binary
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static List<int> BinaryDigits(long n)
        {
            int size = 1;
            long k = 2;
            while (n >= k)
            {
                size++;
                k *= 2;
            }

            List<int> output = new();
            for (int i = 1; i <= size; i++)
            {
                output.Add(NthBinaryDigit(n, i));
            }

            return output;
        }

        /// <summary>
        /// Returns true if and only if a and b are comprised of the same digits (they are permutations of one another)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool SameDigits(BigInteger a, BigInteger b)
        {
            List<int> diga = Digits(a);
            List<int> digb = Digits(b);

            //to compare diga, digb
            //has 10 indices, each with the number of the ith digit
            //so digs[0] = 2 suggests there are 2 0's
            List<int> digs = new();
            for (int i = 0; i < 10; i++)
                digs.Add(0);

            foreach (int k in diga)
                digs[k]++;

            foreach (int k in digb)
                digs[k]--;

            foreach (int k in digs)
                if (k != 0)
                    return false;

            return true;
        }

        /// <summary>
        /// Given a permutation L of [1,2,...,n], returns the sign of L
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public static int Sgn(List<int> L)
        {
            int count = 0;
            for (int i = 0; i < L.Count; i++)
                for (int j = i + 1; j < L.Count; j++)
                    if (L[i] > L[j]) count++;
            return 1 - 2 * (count % 2);
        }

        /// <summary>
        /// Returns all permutations of n elements, as permutations of the list [0, 1, ..., n-1]
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IEnumerable<Partition> Permutations(int n)
        {
            List<int> L = new();
            for (int i = 0; i < n; i++)
                L.Add(i);

            foreach (List<int> l in Util.PermutationsOfList(L))
                yield return new(l);

            yield break;
        }


        /// <summary>
        /// Returns all permutations of the list L
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> PermutationsOfList<T>(List<T> L)
        {
            if (L.Count <= 1)
            {
                yield return new List<T>(L);
                yield break;
            }

            for (int i = 0; i < L.Count; i++)
            {
                List<T> A = new(L);
                A.RemoveAt(i);
                foreach (List<T> k in PermutationsOfList(A))
                {
                    k.Insert(0, L[i]);
                    yield return k;
                }
            }
            yield break;
        }


        /// <summary>
        /// Returns the k'th pentagonal number, given by m*(3*m - 1)/2
        /// as m = 0, 1, -1, 2, -2, ...
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static int Pentagonal(int k)
        {
            if (k <= 0)
                return 0;

            int m;
            if (k % 2 == 1)
                m = (k + 1) / 2;
            else
                m = -k / 2;
            return m * (3 * m - 1) / 2;
        }


        /// <summary>
        /// Inverse of PartitionToNumCycles
        /// </summary>
        /// <param name="cycles"></param>
        /// <returns></returns>
        public static List<int> NumCyclesToPartition(List<int> cycles)
        {
            List<int> part = new();

            for (int i = cycles.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < cycles[i]; j++)
                {
                    part.Add(i + 1);
                }
            }

            return part;
        }

        /// <summary>
        /// Given a binomial term of the form (x_1 C a_1)(x_2 C a_2) ... (x_r C a_r)
        /// Converts the "cycle form" [a_1, a_2, ..., a_r] to a partition.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public static List<int> BinomialTermToPartition(List<Tuple<int, int>> term)
        {
            List<int> cycles = new();
            for (int i = 0; i < term.Count; i++)
            {
                while (cycles.Count < term[i].Item1)
                    cycles.Add(0);

                cycles[term[i].Item1 - 1] = term[i].Item2;
            }

            return NumCyclesToPartition(cycles);
        }

        /// <summary>
        /// Returns the number of partitions of n into primes
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static int PrimePartitions(int n)
        {
            if (n <= 1)
                return 0;

            List<int> primes = new() { 2 };
            for (int i = 3; i <= n; i++)
                if (IsPrime((BigInteger)i))
                    primes.Add(i);

            int count = 0;

            List<int> vals = new();
            vals.Add(primes.Count - 1);

            int sum = primes[^1];
            while (vals[0] >= 0)
            {
                if (sum < n)
                {
                    int k = vals[^1];
                    vals.Add(k);
                    sum += primes[k];
                    if (sum < n)
                        continue;
                }

                if (sum == n)
                    count++;

                sum -= primes[vals[^1]];
                vals[^1]--;
                if (vals[^1] >= 0)
                {
                    sum += primes[vals[^1]];
                    continue;
                }

                for (int j = vals.Count - 1; j > 0; j--)
                {
                    if (vals[j] <= 0)
                    {
                        vals.RemoveAt(j);

                        sum -= primes[vals[j - 1]];
                        vals[j - 1]--;
                        if (vals[j - 1] >= 0)
                        {
                            sum += primes[vals[j - 1]];
                            break;
                        }
                    }
                    else
                        break;
                }
            }
            return count;
        }

        /// <summary>
        /// Returns \mu(n) (the mobeius function), which returns 0 if n is divisible by a square,
        /// and -1^{# prime divisors of n} otherwise. 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int Moebius(BigInteger n)
        {
            if (n <= 0)
                return 0;

            List<BigInteger> factors = PrimeFactorize(n);
            for (int i = 0; i < factors.Count - 1; i++)
            {
                if (factors[i] == factors[i + 1])
                    return 0;
            }

            return 1 - 2 * (factors.Count % 2);
        }

    }
}
