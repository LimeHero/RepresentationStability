using System.Numerics;

namespace IntegerMethods
{
    /// <summary>
    /// Class that represents rational numbers, with BigInteger support, as A/B
    /// </summary>
    public class BigRational
    {
        //fraction is A/B
        public BigInteger A { get; private set; }
        public BigInteger B { get; private set; }

        /// <summary>
        /// Default initializer as a/b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public BigRational(BigInteger a, BigInteger b)
        {
            if (b == 0)
                throw new ArgumentException("No zero denominator allowed");
            if (b < 0)
            {
                b *= -1;
                a *= -1;
            }
            this.A = a;
            this.B = b;
            if (a == 0)
                this.B = 1;

            Reduce();
        }

        public BigRational(BigInteger a) : this(a, 1)
        {
        }

        /// <summary>
        /// Initalizes as zero
        /// </summary>
        public BigRational() : this(0, 1)
        {
        }

        /// <summary>
        /// Cast from a BigRational to BigInteger
        /// </summary>
        /// <param name="b"></param>
        public static implicit operator BigRational(BigInteger q) => new(q, 1);

        /// <summary>
        /// Cast from a BigRational to integer
        /// </summary>
        /// <param name="b"></param>
        public static implicit operator BigRational(int q) => new(q, 1);

        /// <summary>
        /// Adds two BigRationals
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static BigRational operator +(BigRational m, BigRational n)
        {
            return new BigRational(m.A * n.B + m.B * n.A, n.B * m.B);
        }

        /// <summary>
        /// Subtracts two BigRationals
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static BigRational operator -(BigRational m, BigRational n)
        {
            return new BigRational(m.A * n.B - m.B * n.A, n.B * m.B);
        }

        /// <summary>
        /// Multiplies two BigRationals
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static BigRational operator *(BigRational m, BigRational n)
        {
            return new BigRational(m.A * n.A, m.B * n.B);
        }

        /// <summary>
        /// Multiplies an int and BigRational
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static BigRational operator *(int m, BigRational n)
        {
            return new BigRational(m * n.A, n.B);
        }

        /// <summary>
        /// Multiplies an int and BigRational
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static BigRational operator *(BigRational m, int n)
        {
            return new BigRational(n * m.A, m.B);
        }

        /// <summary>
        /// Divides two BigRationals
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static BigRational operator /(BigRational m, BigRational n)
        {
            return new BigRational(m.A * n.B, m.B * n.A);
        }

        /// <summary>
        /// Returns absolute value of the big rational
        /// </summary>
        /// <returns></returns>
        public BigRational Abs()
        {
            return new BigRational(A < 0 ? -A : A, B < 0 ? -B : B);
        }

        /// <summary>
        /// Returns the string version of this rational number
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (B == 1)
                return "" + A;
            return "" + A + "/" + B;
        }

        /// <summary>
        /// Less than operator
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool operator <(BigRational m, BigRational n)
        {
            return m.A * n.B < n.A * m.B;
        }

        /// <summary>
        /// Greater than operator
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool operator >(BigRational m, BigRational n)
        {
            return !(m < n);
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool operator ==(BigRational m, BigRational n)
        {
            return m.A * n.B == n.A * m.B;
        }

        /// <summary>
        /// Not equal operator
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool operator !=(BigRational m, BigRational n)
        {
            return m.A * n.B != n.A * m.B;
        }

        /// <summary>
        /// Default HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (new Tuple<BigInteger, BigInteger>(A, B)).GetHashCode();
        }

        /// <summary>
        /// Returns this fraction to reduced form - i.e., numerator 
        /// and denominator are relatively prime
        /// </summary>
        private void Reduce()
        {
            if (A == 0)
            {
                B = 1;
                return;
            }

            // Probably a really fast implementation of gcd
            BigInteger d = BigInteger.GreatestCommonDivisor(A, B);

            A /= d;
            B /= d;
        }

        /// <summary>
        /// Returns a decimal representation of this fraction, to n digits.
        /// The first value in the digits list is the place of the first digit:
        /// i.e., if the decimal representation is .0045623, the first value of the list
        /// is -3, since the first digit is .004 = 4*10^{-3}
        /// If the decimal rep. is 78.432, the first value is 1.
        /// 
        /// Use 
        /// </summary>
        /// <returns></returns>
        public List<int> ToDecimal(int n)
        {
            if (A == 0)
            {

                List<int> digList = [];
                for (int i = 0; i <= n; i++)
                {
                    digList.Add(0);
                }

                return digList;
            }
            List<int> digitList = [];
            //the first value of digitlist - see function description
            int exponentOffset = 0;
            BigInteger offset = 1;

            BigInteger r = BigInteger.Abs(A);
            if (r < B)
            {
                while (r * offset <= B)
                {
                    offset *= 10;
                    exponentOffset--;
                }
            }
            else
            {
                while (B * offset * 10 < r)
                {
                    offset *= 10;
                    exponentOffset++;
                }
            }

            digitList.Add(exponentOffset);

            BigInteger offsetB = B;
            if (exponentOffset < 0)
            {
                r = offset * r;
            }
            else
            {
                offsetB = offset * B;
            }
            for (int i = 0; i < n; i++)
            {
                int q = (int)(r / offsetB);
                r -= offsetB * q;

                digitList.Add(q);
                r *= 10;
                exponentOffset--;
            }

            if (A < 0)
                digitList[1] *= -1;
            return digitList;
        }

        /// <summary>
        /// Returns whether the object is an equal BigRational
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool Equals(object? obj)
        {
            if (obj is not BigRational)
                return false;

            return ((BigRational)obj) == this;
        }
    }
}
