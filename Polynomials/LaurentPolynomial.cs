using IntegerMethods;

namespace Polynomials
{
    /// <summary>
    /// A Laurent polynomial is similar to a polynomial, but may contain negative coefficients.
    /// 
    /// Laurent Polynomials will contain two local variables: a list of coefficients (coefs), and an integer
    /// that represents the least degree of terms of the polynomial (lead). For instance,
    /// z^{-3} + z^{-1} - 2 - 2*z is represented as:
    /// 
    /// coefs = [1, 0, 1, -2, -2]
    /// lead = -3
    /// </summary>
    public class LaurentPolynomial
    {
        /// <summary>
        /// Must always have at least one element
        /// </summary>
        public List<BigRational> Coefs { private set; get; }

        /// <summary>
        /// The least degree term of the polynomial
        /// </summary>
        public int Lead { private set; get; }

        /// <summary>
        /// Constructs the zero polynomial
        /// </summary>
        public LaurentPolynomial()
        {
            Lead = 0;
            Coefs = [new BigRational()];
        }

        /// <summary>
        /// Constructs constant polynomial
        /// </summary>
        public LaurentPolynomial(BigRational c)
        {
            Lead = 0;
            Coefs = new List<BigRational>() { c };
        }

        /// <summary>
        /// Copy constuctor
        /// </summary>
        public LaurentPolynomial(LaurentPolynomial p) : this(p.Lead, p.Coefs)
        {
        }

        /// <summary>
        /// Constructor from a normal polynomial
        /// </summary>
        public LaurentPolynomial(Polynomial p) : this(0, p.Coefs)
        {
        }

        /// <summary>
        /// Cast from a Polynomial to LaurentPolynomial
        /// </summary>
        /// <param name="b"></param>
        public static explicit operator LaurentPolynomial(Polynomial p) => new(p);

        /// <summary>
        /// Constructs a laurent polynomial with the given coefficients and leading degree.
        /// </summary>
        /// <param name="coefs"></param>
        public LaurentPolynomial(int leadingDeg, List<BigRational> coefss)
        {
            Lead = leadingDeg;
            Coefs = new(coefss);

            RemoveLeadingTrailingZeroes();
        }

        /// <summary>
        /// Returns the order of this laurent polynomial (highest degree term)
        /// </summary>
        /// <returns></returns>
        public int Degree()
        {
            return Coefs.Count - 1 + Lead;
        }

        /// <summary>
        /// Returns the coefficient at the given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BigRational this[int index]
        {
            get => GetCoefAt(index);
        }

        /// <summary>
        /// Helper for indexing
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BigRational GetCoefAt(int index)
        {
            if (index > Degree())
                return 0;

            if (index < Lead)
                return 0;

            return Coefs[index - Lead];
        }

        /// <summary>
        /// Adds together two laurent polynomials
        /// </summary>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static LaurentPolynomial operator +(LaurentPolynomial f, LaurentPolynomial g)
        {
            // so WLOG, f.lead < g.lead
            if (g.Lead < f.Lead)
                return g + f;

            List<BigRational> coeffs = new(f.Coefs);

            while (coeffs.Count + f.Lead - 1 < g.Lead)
                coeffs.Add(new(0));

            int lead_dif = g.Lead - f.Lead; int g_count = g.Coefs.Count; int upp_bound = coeffs.Count - lead_dif;
            for (int i = 0; i < g_count; i++)
            {
                if (i >= upp_bound)
                    coeffs.Add(g.Coefs[i]);
                else
                    coeffs[i + lead_dif] += g.Coefs[i];
            }

            return new(f.Lead, coeffs);
        }

        /// <summary>
        /// Subtracts the coefficients of two laurent polynomials
        /// </summary>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static LaurentPolynomial operator -(LaurentPolynomial f, LaurentPolynomial g)
        {
            return f + (-1) * g;
        }

        /// <summary>
        /// Adds a constant term to a laurent polynomial
        /// </summary>
        /// <param name="p"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static LaurentPolynomial operator +(LaurentPolynomial p, BigRational k)
        {
            return p + new LaurentPolynomial(k);
        }

        /// <summary>
        /// Subtracts a constant term from a laurent polynomial.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static LaurentPolynomial operator -(LaurentPolynomial p, BigRational k)
        {
            return p + new LaurentPolynomial(-1 * k);
        }

        /// <summary>
        /// Scalar multiplication by BigRational c
        /// </summary>
        /// <param name="c"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static LaurentPolynomial operator *(BigRational c, LaurentPolynomial p)
        {

            List<BigRational> coeffs = new(p.Coefs);
            for (int i = 0; i < coeffs.Count; i++)
                coeffs[i] *= c;

            return new LaurentPolynomial(p.Lead, coeffs);
        }

        /// <summary>
        /// Scalar multiplication by BigRational c
        /// </summary>
        /// <param name="p"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static LaurentPolynomial operator *(LaurentPolynomial p, BigRational c)
        {
            return c * p;
        }

        /// <summary>
        /// Multiplies two laurent polynomials together 
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static LaurentPolynomial operator *(LaurentPolynomial f, LaurentPolynomial g)
        {
            int lead = f.Lead + g.Lead;
            List<BigRational> coeffs = new(f.Coefs.Count + g.Coefs.Count);
            for (int i = 0; i < (f.Coefs.Count + g.Coefs.Count); i++)
                coeffs.Add(new BigRational());

            for (int i = 0; i < f.Coefs.Count; i++)
                for (int j = 0; j < g.Coefs.Count; j++)
                    coeffs[i + j] += f.Coefs[i] * g.Coefs[j];

            return new LaurentPolynomial(lead, coeffs);
        }

        /// <summary>
        /// Multiplies the polynomial so the leading term has coefficient 1
        /// </summary>
        public void Normalize()
        {
            BigRational r = Coefs[^1];

            for (int i = 0; i < Coefs.Count; i++)
                Coefs[i] /= r;
        }

        /// <summary>
        /// If they share all the same coefficients, returns true.
        /// Assumes f,g are well formed: they have no leading zero terms.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static bool operator ==(LaurentPolynomial f, LaurentPolynomial g)
        {
            if (f.Coefs.Count != g.Coefs.Count)
                return false;

            if (f.Lead != g.Lead)
                return false;

            for (int i = 0; i < f.Coefs.Count; i++)
                if (f.Coefs[i] != g.Coefs[i])
                    return false;

            return true;
        }

        /// <summary>
        /// If they share all the same coefficients, returns true.
        /// Assumes f,g are well formed: they have no leading zero terms.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static bool operator !=(LaurentPolynomial f, LaurentPolynomial g)
        {
            return !(f == g);
        }

        /// <summary>
        /// Returns whether this LaurentPolynomial equals obj
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is not LaurentPolynomial)
                return false;

            return (LaurentPolynomial)obj == this;
        }

        /// <summary>
        /// Default hash function
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns this laurent polynomial as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string output = "";
            bool firstTerm = true;
            for (int i = 0; i < Coefs.Count; i++)
            {
                if (Coefs[i].A == 0)
                    continue;

                if (firstTerm)
                {
                    firstTerm = false;
                    output += TermToString(i, Coefs[i], false);
                    continue;
                }

                output += " + " + TermToString(i, Coefs[i], false);
            }

            if (output.Equals(""))
                return "0";

            return output;
        }

        /// <summary>
        /// Formally inverts the variable. For instance,
        /// 
        /// -z^{-2} + 2 + z^3  ->  z^{-3} + 2 - z^2
        /// </summary>
        /// <returns></returns>
        public LaurentPolynomial FormallyInvert()
        {
            LaurentPolynomial polynomial = new(this);
            polynomial.Coefs.Reverse();
            polynomial.Lead = -Degree();

            return polynomial;
        }

        /// <summary>
        /// Formally raises z to the ith power. For instance with i = 2,
        /// 
        /// -z^{-2} + 2 + z^3  ->  -z^{-4} + 2 + z^6
        /// </summary>
        /// <returns></returns>
        public LaurentPolynomial RaiseZToIthPower(int i)
        {
            if (i == 0)
            {
                BigRational sum = 0; foreach (BigRational a in Coefs) sum += a;
                return new(sum);
            }

            if (i == 1)
                return new LaurentPolynomial(this);

            if (i <= 0)
                return this.FormallyInvert().RaiseZToIthPower(-i);


            List<BigRational> new_coefs = new();

            for (int j = 0; j < Coefs.Count; j++)
            {
                new_coefs.Add(Coefs[j]);
                for (int k = 1; k < i; k++)
                    new_coefs.Add(0);
            }

            return new LaurentPolynomial(Lead * i, new_coefs);

        }

        /// <summary>
        /// Returns this laurent polynomial as a string (largest degree terms first)
        /// </summary>
        /// <returns></returns>
        public string ToRevString(bool LaTeX = false)
        {
            string output = "";
            bool firstTerm = true;
            for (int i = Coefs.Count - 1; i >= 0; i--)
            {
                if (Coefs[i].A == 0)
                    continue;

                if (firstTerm)
                {
                    firstTerm = false;
                    output += TermToString(i, Coefs[i], LaTeX);
                    continue;
                }

                if (Coefs[i] > 0)
                    output += " + ";
                else
                    output += " - ";
                output += TermToString(i, Coefs[i] < 0 ? -1 * Coefs[i] : Coefs[i], LaTeX);
            }

            if (output.Equals(""))
                return "0";

            return output;
        }

        /// <summary>
        /// Expresses c*q^{lead+i} as a string
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private string TermToString(int i, BigRational c, bool LaTeX)
        {
            if (c == new BigRational())
                return "";

            if (i + Lead == 0)
                return c.ToString();

            if (c == new BigRational(1, 1))
            {
                if (LaTeX)
                    return "z^{" + (Lead + i) + "}";
                return "z^" + (Lead + i);
            }

            if (LaTeX)
                return c.ToString() + "z^{" + (Lead + i) + "}";
            return c.ToString() + "*z^" + (Lead + i);
        }

        /// <summary>
        /// Returns whether this polynomial is zero or not
        /// </summary>
        /// <returns></returns>
        public bool IsZero()
        {
            return Coefs.Count == 1 && Coefs[0].A == 0;
        }

        /// <summary>
        /// Returns this laurent polynomial, but with all terms z^k with k > n removed
        /// </summary>
        public void RoundToNthDegree(int n)
        {
            while (Degree() > n && Coefs.Count > 0)
            {
                Coefs.RemoveAt(Coefs.Count - 1);
            }

            RemoveLeadingTrailingZeroes();
        }

        /// <summary>
        /// Returns this laurent polynomial, but with all terms z^k with k </ n removed
        /// </summary>
        public void RoundLeadToN(int n)
        {
            while (Lead < n && Coefs.Count > 0)
            {
                Coefs.RemoveAt(0);
                Lead++;
            }

            RemoveLeadingTrailingZeroes();
        }

        /// <summary>
        /// Returns this laurentpolynomial only containing the highest N degree terms of the laurentpolynomial
        /// </summary>
        /// <param name="n"></param>
        public void LeadingNTerms(int n)
        {
            if (n < 0)
                n = 0;

            for (int i = Coefs.Count - n - 1; i >= 0; i--)
            {
                Coefs.RemoveAt(i);
                Lead++;
            }

            //so it is well formed
            RemoveLeadingTrailingZeroes();
        }

        /// <summary>
        /// Returns a new laurentpolynomial only containing the lowest N degree terms of the laurentpolynomial
        /// </summary>
        /// <param name="n"></param>
        public void FirstNTerms(int n)
        {
            while (Coefs.Count > n)
                Coefs.RemoveAt(n);

            //so it is well formed
            RemoveLeadingTrailingZeroes();
        }

        /// <summary>
        /// Ensures this laurent polynomial is properly formed.
        /// </summary>
        private void RemoveLeadingTrailingZeroes()
        {
            for (int i = Coefs.Count - 1; i > 0; i--)
            {
                if (Coefs[i].A == 0)
                {
                    Coefs.RemoveAt(i);
                }
                else
                    break;
            }

            for (int i = 0; i < Coefs.Count; i++)
            {
                if (Coefs[i].A == 0)
                {
                    Coefs.RemoveAt(i);
                    Lead++;
                    i--;
                }
                else
                    break;
            }
            if (Coefs.Count == 0)
                Coefs.Add(new BigRational());
        }
    }
}
