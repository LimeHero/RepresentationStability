using IntegerMethods;

namespace Polynomials
{
    /// <summary>
    /// A simple class which stores a character polynomial in the basis
    /// of the form (X_1 C a_1) ... (X_r C a_r) for C representing "choose"
    /// and a_1 nonnegative integers. See https://arxiv.org/pdf/2001.04112
    /// the "binomial basis", definition 2.2.
    /// 
    /// 'terms' and 'coefficients' MUST be the same length (and are). 
    /// </summary>
    public class CharacterPolynomial
    {
        public List<Partition> Terms { private set; get; }
        public List<BigRational> Coefs { private set; get; }

        /// <summary>
        /// Constructs a new character polynomial
        /// </summary>
        /// <param name="_terms"></param>
        /// <param name="_coefs"></param>
        /// <exception cref="ArgumentException"></exception>
        public CharacterPolynomial(List<List<Tuple<int, int>>> _terms, List<BigRational> _coefs)
        {
            if (_terms.Count != _coefs.Count) throw new ArgumentException("Terms and Coefs must have same length!");

            Terms = [];
            Coefs = new(_coefs);

            for (int i = 0; i < _coefs.Count; i++)
                Terms.Add(new(_terms[i]));

        }

        /// <summary>
        /// Constructs a new character polynomial
        /// </summary>
        /// <param name="_terms"></param>
        /// <param name="_coefs"></param>
        public CharacterPolynomial(List<Partition> _terms, List<BigRational> _coefs)
        {
            Terms = new(_terms);
            Coefs = new(_coefs);
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="p"></param>
        public CharacterPolynomial(CharacterPolynomial p) : this(p.Terms, p.Coefs)
        {
        }

        /// <summary>
        /// Sets the character polynomial to be equal to r
        /// </summary>
        /// <param name="r"></param>
        public CharacterPolynomial(BigRational r)
        {
            Terms = [];
            Coefs = [];

            Terms.Add(new());
            Coefs.Add(r);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CharacterPolynomial()
        {
            Terms = [];
            Coefs = [];
        }

        /// <summary>
        /// Returns this character polynomial evaluated at
        /// the permutation cycles, i.e., the permutation 
        /// with cycles[i] number of i cycles.
        /// 
        /// DO NOT pass in a partition stored in the standard way as
        /// non-decreasing list of ints.
        /// I.e., pass in [1, 0, 1, 0] for [3, 1].
        /// </summary>
        /// <param name="cycles"></param>
        /// <returns></returns>
        public BigRational Evaluate(Partition part)
        {
            List<int> cycles = part.GetCycles();

            BigRational rslt = 0;
            for (int i = 0; i < Terms.Count; i++)
            {
                BigRational nextterm = Coefs[i];

                List<int> term_cycles = Terms[i].GetCycles();
                for (int j = 0; j < term_cycles.Count; j++)
                {
                    while (cycles.Count < j)
                        cycles.Add(0);

                    nextterm *= Util.Choose(cycles[j], term_cycles[j]);
                }

                rslt += nextterm;
            }

            return rslt;
        }

        /// <summary>
        /// Adds the given term and coefficient to the characteristic polynomial
        /// </summary>
        /// <param name="term"></param>
        /// <param name="coef"></param>
        public void Append(Partition term, BigRational coef)
        {
            Terms.Add(new(term));
            Coefs.Add(coef);
        }

        /// <summary>
        /// Multiplies each coefficient of p by r
        /// </summary>
        /// <param name="p"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static CharacterPolynomial operator *(CharacterPolynomial p, BigRational r)
        {
            CharacterPolynomial q = new(p);
            for (int i = 0; i < q.Coefs.Count; i++)
            {
                q.Coefs[i] *= r;
            }

            return q;
        }

        /// <summary>
        /// Multiplies each coefficient of p by r
        /// </summary>
        /// <param name="p"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static CharacterPolynomial operator *(BigRational r, CharacterPolynomial p)
        {
            CharacterPolynomial q = new(p);
            for (int i = 0; i < q.Coefs.Count; i++)
            {
                q.Coefs[i] *= r;
            }

            return q;
        }

        /// <summary>
        /// Simply appends the coefficients and terms of both p and q.
        /// (i.e., NOT efficient. We could make this way more efficient if we wanted)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static CharacterPolynomial operator +(CharacterPolynomial p, CharacterPolynomial q)
        {
            CharacterPolynomial rslt = new(p);

            for (int i = 0; i < q.Terms.Count; i++)
            {
                rslt.Terms.Add(new(q.Terms[i]));
                rslt.Coefs.Add(q.Coefs[i]);
            }

            return rslt;
        }


        /// <summary>
        /// Returns the character polynomial as a string. Works better 
        /// if you have already cleared out any zero terms (which is not done automatically
        /// for performance reasons).
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Terms.Count == 0)
                return "0";

            string s = "";
            s += TermToString(Terms[0], Coefs[0]);

            for (int i = 1; i < Coefs.Count; i++)
                s += " + " + TermToString(Terms[i], Coefs[i]);

            return s;
        }

        /// <summary>
        /// Sends an individual term (product of choose polys) to a string
        /// helper function for LinComboToString
        /// </summary>
        /// <param name="term"></param>
        /// <param name="coef"></param>
        /// <returns></returns>
        private static string TermToString(Partition term, BigRational coefficient)
        {
            if (term.Size() == 0)
                return coefficient.ToString();

            string s = coefficient.ToString();
            List<int> cycles = term.GetCycles();

            for (int i = 0; i <= cycles.Count; i++)
            {
                if (cycles[i] == 0) continue;

                s += " * (p'_" + i + " C " + cycles[i] + ")";
            }

            return s;
        }
    }
}
