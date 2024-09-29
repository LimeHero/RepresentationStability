using IntegerMethods;

namespace Polynomials
{
    /// <summary>
    /// Polynomial class in a single variable, with BigRational coefficients.
    /// </summary>
    public class Polynomial
    {
        /// <summary>
        /// Must always have at least one element
        /// </summary>
        public List<BigRational> Coefs { private set; get; }

        /// <summary>
        /// Constructs the zero polynomial
        /// </summary>
        public Polynomial()
        {
            Coefs = [new BigRational()];
        }

        /// <summary>
        /// Constructs constant polynomial
        /// </summary>
        public Polynomial(BigRational c)
        {
            Coefs = [c];
        }

        /// <summary>
        /// Copy constuctor
        /// </summary>
        public Polynomial(Polynomial p) : this(p.Coefs)
        {
        }

        /// <summary>
        /// Constructs a polynomial with the given coefficients.
        /// </summary>
        /// <param name="coefs"></param>
        public Polynomial(List<BigRational> coefss)
        {
            Coefs = new(coefss);
            for (int i = Coefs.Count - 1; i > 0; i--)
            {
                if (Coefs[i].A == 0)
                {
                    Coefs.RemoveAt(i);
                }
                else
                    return;
            }

            if (Coefs.Count == 0)
                Coefs.Add(new BigRational());
        }

        /// <summary>
        /// Constructs a polynomial with the given coefficients.
        /// </summary>
        /// <param name="coefs"></param>
        public Polynomial(List<int> coefss)
        {
            Coefs = new List<BigRational>();
            for (int i = 0; i < coefss.Count; i++)
                Coefs.Add(new BigRational(coefss[i], 1));

            for (int i = Coefs.Count - 1; i > 0; i--)
            {
                if (Coefs[i].A == 0)
                    Coefs.RemoveAt(i);
                else
                    break;
            }

            if (Coefs.Count == 0)
                Coefs.Add(new BigRational());
        }

        /// <summary>
        /// Returns the order of this polynomial (highest degree term)
        /// Returns coefs.Count - 1.
        /// </summary>
        /// <returns></returns>
        public int Degree()
        {
            return Coefs.Count - 1;
        }

        /// <summary>
        /// Adds the coefficients of two polnyomials
        /// </summary>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static Polynomial operator +(Polynomial f, Polynomial g)
        {
            List<BigRational> coeffs = new List<BigRational>(g.Coefs);
            for (int i = 0; i < f.Coefs.Count; i++)
            {
                if (i >= coeffs.Count)
                    coeffs.Add(new BigRational());
                coeffs[i] += f.Coefs[i];
            }

            return new Polynomial(coeffs);
        }

        /// <summary>
        /// Subtracts the coefficients of two polnyomials
        /// </summary>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static Polynomial operator -(Polynomial f, Polynomial g)
        {
            List<BigRational> coeffs = new List<BigRational>(f.Coefs);
            for (int i = 0; i < g.Coefs.Count; i++)
            {
                if (i >= coeffs.Count)
                    coeffs.Add(new BigRational());
                coeffs[i] -= g.Coefs[i];
            }

            return new Polynomial(coeffs);
        }

        /// <summary>
        /// Subtracts a constant term from a polynomial.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static Polynomial operator -(Polynomial f, BigRational k)
        {
            Polynomial output = new Polynomial(f);

            output.Coefs[0] -= k;

            return output;
        }

        /// <summary>
        /// Adds a constant term to a polynomial
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static Polynomial operator +(Polynomial p, BigRational q)
        {

            List<BigRational> coeffs = new List<BigRational>(p.Coefs);

            if (coeffs.Count == 0)
                coeffs.Add(new BigRational());

            coeffs[0] += q;

            return new Polynomial(coeffs);
        }

        /// <summary>
        /// Scalar multiplication by BigRational c
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static Polynomial operator *(BigRational c, Polynomial p)
        {

            List<BigRational> coeffs = new List<BigRational>(p.Coefs);
            for (int i = 0; i < coeffs.Count; i++)
                coeffs[i] *= c;

            return new Polynomial(coeffs);
        }

        /// <summary>
        /// Scalar multiplication by int c
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static Polynomial operator *(int c, Polynomial p)
        {

            List<BigRational> coeffs = new List<BigRational>(p.Coefs);
            for (int i = 0; i < coeffs.Count; i++)
                coeffs[i] *= c;

            return new Polynomial(coeffs);
        }

        /// <summary>
        /// Scalar multiplication by int c
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static Polynomial operator *(Polynomial p, int c)
        {

            List<BigRational> coeffs = new List<BigRational>(p.Coefs);
            for (int i = 0; i < coeffs.Count; i++)
                coeffs[i] *= c;

            return new Polynomial(coeffs);
        }

        /// <summary>
        /// Multiplies two polynomials together 
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static Polynomial operator *(Polynomial f, Polynomial g)
        {
            List<BigRational> coeffs = new List<BigRational>();
            for (int i = 0; i < (f.Degree() + 1) + (g.Degree() + 1); i++)
                coeffs.Add(new BigRational());

            for (int i = 0; i < f.Coefs.Count; i++)
                for (int j = 0; j < g.Coefs.Count; j++)
                    coeffs[i + j] += f.Coefs[i] * g.Coefs[j];

            return new Polynomial(coeffs);
        }

        /// <summary>
        /// Computes the division f/g: f = qg + r. Returns the tuple of polynomials. Item1 is the quotient,
        /// Item2 is the remainder, so we return (q, r).
        /// 
        /// Note: Not very computationally efficient.
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static Tuple<Polynomial, Polynomial> operator /(Polynomial f, Polynomial g)
        {
            if (g.IsZero())
                throw new Exception("CANT DIVIDE BY ZERO!");
            Polynomial r = new Polynomial(f.Coefs);

            //q is not actually a well formed polynomial, but it doesn't matter here
            //when q is returned, it is well formed
            Polynomial q = new Polynomial();
            while (q.Coefs.Count < r.Degree() - g.Degree() + 1)
                q.Coefs.Add(new BigRational());

            while (r.Degree() >= g.Degree() && !r.IsZero())
            {
                q.Coefs[r.Degree() - g.Degree()] = r.Coefs[r.Coefs.Count - 1] / g.Coefs[g.Coefs.Count - 1];

                r = f - q * g;
            }

            return new Tuple<Polynomial, Polynomial>(new Polynomial(q.Coefs), r);
        }

        /// <summary>
        /// Returns the monic GCD of a,b.
        /// 
        /// Never returns 0: returns 1 if a and b are relatively prime.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Polynomial GCD(Polynomial a, Polynomial b)
        {
            if (a.Degree() == 0 && b.Degree() == 0)
                return new Polynomial(1);

            if (a.Degree() < b.Degree())
                return GCD(b, a);

            if (b.IsZero())
            {
                Polynomial _a = new(a);
                _a.Normalize();
                return _a;
            }

            Tuple<Polynomial, Polynomial> div = a / b;
            return GCD(b, div.Item2);
        }

        /// <summary>
        /// Multiplies the polynomial so the leading term has coefficient 1 (monic)
        /// </summary>
        public void Normalize()
        {
            BigRational r = Coefs[Degree()];

            for (int i = 0; i <= Degree(); i++)
                Coefs[i] /= r;
        }

        // for memoization
        private static Dictionary<Tuple<int, int>, Polynomial> prev_choose_sum_results = new();
        /// <summary>
        /// Returns Choose(MoebiusSum(item1), item2), but memoized for performance
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Polynomial ChooseMoebiusSum(int item1, int item2)
        {
            if (item2 < 0 || item1 < 0)
                throw new ArgumentException("Both values should be nonnegative integers");
            Tuple<int, int> t = new(item1, item2);
            if (prev_choose_sum_results.ContainsKey(t)) return prev_choose_sum_results[t];

            if (item2 == 0)
                return new Polynomial(1);

            if (item2 == 1)
                return MoebiusSum(item1);

            Polynomial p = MoebiusSum(item1);

            prev_choose_sum_results[t] = new BigRational(1, item2) *
                ChooseMoebiusSum(item1, item2 - 1) *
                (p - new Polynomial(item2 - 1))
                ;
            return prev_choose_sum_results[t];

        }

        /// <summary>
        /// Returns (p*(p-1)*...(p-k+1))/k!
        /// </summary>
        /// <param name="p"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Polynomial Choose(Polynomial p, int k)
        {
            Polynomial output = new Polynomial(p);

            for (int i = 1; i < k; i++)
            {
                output *= p - new BigRational(i, 1);
            }

            for (int i = 1; i <= k; i++)
                output = new BigRational(1, i) * output;

            return output;
        }

        // for memoization
        private static Dictionary<int, Polynomial> moebius_sum_prev_results = new();
        /// <summary>
        /// Returns (1/k)*\sum{d|k} (\moebius(k/d) q^d). Memoized for performance
        /// </summary>
        /// <param name="p"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Polynomial MoebiusSum(int k)
        {
            if (k <= 0)
                return new Polynomial(1);

            if (moebius_sum_prev_results.ContainsKey(k))
                return moebius_sum_prev_results[k];

            List<int> kFactors = new List<int>();
            for (int i = 1; i <= k; i++)
                if (k % i == 0)
                    kFactors.Add(i);

            Polynomial output = new Polynomial();
            foreach (int d in kFactors)
            {
                output += new BigRational(Util.Moebius(k / d), 1) * Power(d);
            }

            moebius_sum_prev_results[k] = new BigRational(1, k) * output;

            return moebius_sum_prev_results[k];
        }

        /// <summary>
        /// Returns q^d
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Polynomial Power(int d)
        {
            List<int> coeffs = new List<int>();
            for (int i = 0; i < d; i++)
                coeffs.Add(0);

            coeffs.Add(1);

            return new Polynomial(coeffs);
        }

        /// <summary>
        /// If they share all the same coefficients, returns true.
        /// Assumes f,g are well formed: they have no leading zero terms.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static bool operator ==(Polynomial f, Polynomial g)
        {
            if (f.Degree() != g.Degree())
                return false;

            for (int i = 0; i <= f.Degree(); i++)
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
        public static bool operator !=(Polynomial f, Polynomial g)
        {
            return !(f == g);
        }

        /// <summary>
        /// Same as ==
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is not Polynomial)
                return false;

            return (Polynomial)obj == this;
        }

        /// <summary>
        /// Default HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns this polynomial as a string
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
                    output += TermToString(i, Coefs[i]);
                    continue;
                }

                output += " + " + TermToString(i, Coefs[i]);
            }

            if (output.Equals(""))
                return "0";

            return output;
        }

        /// <summary>
        /// Expresses c*q^i as a string
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private string TermToString(int i, BigRational c)
        {
            if (c == new BigRational())
                return "";

            if (i == 0)
                return c.ToString();

            if (c == new BigRational(1, 1))
                return "q^" + i;
            if (c == new BigRational(-1, 1))
                return "-q^" + i;
            return c.ToString() + "*q^" + i;
        }

        /// <summary>
        /// Returns whether this polynomial is zero or not
        /// </summary>
        /// <returns></returns>
        public bool IsZero()
        {
            return Degree() == 0 && Coefs[0].A == 0;
        }
    }
}
