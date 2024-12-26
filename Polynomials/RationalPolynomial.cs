using IntegerMethods;
using Polynomials;

namespace Polynomials
{
    /// <summary>
    /// Ratio of polynomials of the form a/b, for a, b instances of the Polynomial class -
    /// i.e., they are polynomials in a single variable with rational (BigRational) coefficients.
    /// </summary>
    class RationalPolynomial
    {
        //fraction is a/b
        public Polynomial A { get; private set; }
        public Polynomial B { get; private set; }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="p"></param>
        public RationalPolynomial(RationalPolynomial p) : this(new Polynomial(p.A), new Polynomial(p.B))
        {
        }

        /// <summary>
        /// Default initializer
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public RationalPolynomial(Polynomial a, Polynomial b)
        {
            if (b.IsZero())
                throw new ArgumentException("NO zero denominator");

            this.A = new Polynomial(a);
            this.B = new Polynomial(b);

            if (a.IsZero())
                this.B = new Polynomial(1);

            //first term should be positive
            if (b.Coefs[b.Degree()].A < 0)
            {
                this.B *= -1;
                this.A *= -1;
            }
        }

        /// <summary>
        /// Initalizes as zero
        /// </summary>
        public RationalPolynomial() : this(new Polynomial(), new Polynomial(1))
        {
        }

        /// <summary>
        /// Adds two RationalPolynomials
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static RationalPolynomial operator +(RationalPolynomial m, RationalPolynomial n)
        {
            return new RationalPolynomial(m.A * n.B + m.B * n.A, n.B * m.B);
        }

        /// <summary>
        /// Subtracts two RationalPolynomials
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static RationalPolynomial operator -(RationalPolynomial m, RationalPolynomial n)
        {
            return new RationalPolynomial(m.A * n.B - m.B * n.A, n.B * m.B);
        }

        /// <summary>
        /// Multiplies two RationalPolynomials
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static RationalPolynomial operator *(RationalPolynomial m, RationalPolynomial n)
        {
            return new RationalPolynomial(m.A * n.A, m.B * n.B);
        }

        /// <summary>
        /// Multiplies an int and RationalPolynomial
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static RationalPolynomial operator *(int m, RationalPolynomial n)
        {
            return new RationalPolynomial(m * n.A, n.B);
        }

        /// <summary>
        /// Multiplies an int and RationalPolynomial
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static RationalPolynomial operator *(RationalPolynomial m, int n)
        {
            return new RationalPolynomial(n * m.A, m.B);
        }

        /// <summary>
        /// Divides two RationalPolynomials
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static RationalPolynomial operator /(RationalPolynomial m, RationalPolynomial n)
        {
            return new RationalPolynomial(m.A * n.B, m.B * n.A);
        }

        /// <summary>
        /// Returns the string version of this rational number
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (B.Coefs.Count == 0 && B.Coefs[0].A == 1)
                return "" + A;
            return "(" + A + ") / (" + B + ")";
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool operator ==(RationalPolynomial m, RationalPolynomial n)
        {
            return m.A * n.B == n.A * m.B;
        }

        /// <summary>
        /// Not equal operator
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool operator !=(RationalPolynomial m, RationalPolynomial n)
        {
            return m.A * n.B != n.A * m.B;
        }

        /// <summary>
        /// Returns this == obj
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is not RationalPolynomial)
                return false;

            return (RationalPolynomial)obj == this;
        }

        /// <summary>
        /// Default HashCode from Object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Divides the numerator and denominator by their GCD.
        /// Also divides by the leading term of the numerator, so it is monic.
        /// </summary>
        public void Reduce()
        {
            Polynomial GCD = Polynomial.GCD(A, B);

            //remainder should be zero!
            A = (A / GCD).Item1;
            B = (B / GCD).Item1;

            BigRational r = new BigRational(1, 1) / A.Coefs[A.Degree()];

            A = r * A;
            B = r * B;
        }
    }
}