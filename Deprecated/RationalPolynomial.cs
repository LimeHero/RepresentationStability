using IntegerMethods;
using Polynomials;

namespace Deprecated
{
    /// <summary>
    /// Ratio of polynomials of the form a/b, for a, b instances of the Polynomial class -
    /// i.e., they are polynomials in a single variable with rational (BigRational) coefficients.
    /// </summary>
    class RationalPolynomial
    {
        //fraction is a/b
        public Polynomial a { get; private set; }
        public Polynomial b { get; private set; }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="p"></param>
        public RationalPolynomial(RationalPolynomial p) : this(new Polynomial(p.a), new Polynomial(p.b))
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

            this.a = new Polynomial(a);
            this.b = new Polynomial(b);

            if (a.IsZero())
                this.b = new Polynomial(1);

            //first term should be positive
            if (b.coefs[b.Degree()].A < 0)
            {
                this.b *= -1;
                this.a *= -1;
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
            return new RationalPolynomial(m.a * n.b + m.b * n.a, n.b * m.b);
        }

        /// <summary>
        /// Subtracts two RationalPolynomials
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static RationalPolynomial operator -(RationalPolynomial m, RationalPolynomial n)
        {
            return new RationalPolynomial(m.a * n.b - m.b * n.a, n.b * m.b);
        }

        /// <summary>
        /// Multiplies two RationalPolynomials
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static RationalPolynomial operator *(RationalPolynomial m, RationalPolynomial n)
        {
            return new RationalPolynomial(m.a * n.a, m.b * n.b);
        }

        /// <summary>
        /// Multiplies an int and RationalPolynomial
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static RationalPolynomial operator *(int m, RationalPolynomial n)
        {
            return new RationalPolynomial(m * n.a, n.b);
        }

        /// <summary>
        /// Multiplies an int and RationalPolynomial
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static RationalPolynomial operator *(RationalPolynomial m, int n)
        {
            return new RationalPolynomial(n * m.a, m.b);
        }

        /// <summary>
        /// Divides two RationalPolynomials
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static RationalPolynomial operator /(RationalPolynomial m, RationalPolynomial n)
        {
            return new RationalPolynomial(m.a * n.b, m.b * n.a);
        }

        /// <summary>
        /// Returns the string version of this rational number
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (b.coefs.Count == 0 && b.coefs[0].A == 1)
                return "" + a;
            return "(" + a + ") / (" + b + ")";
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool operator ==(RationalPolynomial m, RationalPolynomial n)
        {
            return m.a * n.b == n.a * m.b;
        }

        /// <summary>
        /// Not equal operator
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool operator !=(RationalPolynomial m, RationalPolynomial n)
        {
            return m.a * n.b != n.a * m.b;
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
            Polynomial GCD = Polynomial.GCD(a, b);

            //remainder should be zero!
            a = (a / GCD).Item1;
            b = (b / GCD).Item1;

            BigRational r = new BigRational(1, 1) / a.coefs[a.Degree()];

            a = r * a;
            b = r * b;
        }
    }
}
