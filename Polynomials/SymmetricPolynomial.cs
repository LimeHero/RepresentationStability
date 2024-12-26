using IntegerMethods;
using System.Numerics;

namespace Polynomials
{
    /// <summary>
    /// Class representing a symmetric polynomial in an arbitrary number of variables (n -> infty)
    /// Every symmetric polynomial is represented by the coefficients 
    /// of each symmetric term (i.e., a_0 + a_1 sum t_i, a_2 sum t_it_j, a_3 sum t_i^2, ...)
    /// For instance, sum t_i represents the formal sum (t_1 + t_2 + t_3 + ... )
    /// 
    /// We assume that the symmetric polynomials in this class are symmetric in infinite variables,
    /// i.e., elements of the ring of symmetric functions.
    /// This detail is mainly significant for symmetric polynomial multiplication.
    /// </summary>
    public class SymmetricPolynomial
    {
        // this stores the ordering of the monomials for the coefficients of all symmetric polys
        public static List<List<int>> monomial { private set; get; } = new List<List<int>>();

        // list of the coefficients of the symmetric poly
        public List<BigRational> coefs { private set; get; }

        /// <summary>
        /// Constructs the zero polynomial
        /// </summary>
        public SymmetricPolynomial()
        {
            coefs = new List<BigRational>();
        }

        /// <summary>
        /// Constructs the constant polynomial
        /// </summary>
        public SymmetricPolynomial(int c)
        {
            coefs = new List<BigRational>() { new BigRational(c, 1) };
        }

        /// <summary>
        /// Constructs the constant polynomial
        /// </summary>
        public SymmetricPolynomial(BigRational c)
        {
            coefs = new List<BigRational>() { c };
        }

        /// <summary>
        /// Basic constructor for a symmetric polynomial
        /// </summary>
        /// <param name="coeffs"></param>
        /// <param name="dim"></param>
        public SymmetricPolynomial(List<int> coeffs)
        {
            //copy coeffs into coef without trailing zeroes
            coefs = new List<BigRational>();
            for (int i = 0; i < coeffs.Count; i++)
            {
                coefs.Add(new BigRational(coeffs[i], 1));
            }
            for (int i = coeffs.Count - 1; i >= 0; i--)
            {
                if (coeffs[i] == 0)
                {
                    coefs.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }

            //update the monomial ordering, so we have all the coefficients of this poly
            if (coefs.Count > monomial.Count)
                UpdateMonomialList(coefs.Count);
        }

        /// <summary>
        /// Basic constructor for a symmetric polynomial
        /// </summary>
        /// <param name="coeffs"></param>
        /// <param name="dim"></param>
        public SymmetricPolynomial(List<BigRational> coeffs)
        {
            //copy coeffs into coef without trailing zeroes
            coefs = new List<BigRational>(coeffs);

            for (int i = coeffs.Count - 1; i >= 0; i--)
            {
                if (coeffs[i].A == 0)
                    coefs.RemoveAt(i);
                else
                    break;
            }

            //update the monomial ordering, so we have all the coefficients of this poly
            if (coefs.Count > monomial.Count)
                UpdateMonomialList(coefs.Count);
        }

        /// <summary>
        /// Copy constructor for symmetric polynomials
        /// </summary>
        /// <param name="coeffs"></param>
        /// <param name="dim"></param>
        public SymmetricPolynomial(SymmetricPolynomial p) : this(p.coefs)
        { }

        /// <summary>
        /// Returns the kth power sum polynomial p_k, which is sum{t^k}
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static SymmetricPolynomial Power(int k)
        {
            int n = FindMonomialIndex(new List<int>() { k });
            List<BigRational> coef = new List<BigRational>();
            for (int i = 0; i < n; i++)
                coef.Add(new BigRational());

            coef.Add(1);
            return new SymmetricPolynomial(coef);
        }

        /// <summary>
        /// Returns e_k, the kth elementary symmetric polynomial. 
        /// Recall, e_k = sum t_1t_2...t_k
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static SymmetricPolynomial Elementary(int k)
        {
            List<int> ones = new List<int>();
            for (int i = 0; i < k; i++)
                ones.Add(1);

            int n = FindMonomialIndex(ones);
            List<BigRational> coef = new List<BigRational>();
            for (int i = 0; i < n; i++)
                coef.Add(new BigRational());

            coef.Add(1);
            return new SymmetricPolynomial(coef);
        }

        /// <summary>
        /// Returns p'_k, defined as: 1/k * sum_{d|k} mu(k/d)*p_d for all d|k.
        /// p_d refers to the power sum polynomial, Power(d). mu(d) is the
        /// moebius function on d. 
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static SymmetricPolynomial PowerPrime(int k)
        {
            List<int> kFactors = new List<int>();
            for (int i = 1; i <= k; i++)
                if (k % i == 0)
                    kFactors.Add(i);

            SymmetricPolynomial output = new SymmetricPolynomial();
            foreach (int d in kFactors)
            {
                output += new BigRational(Util.Moebius(k / d), 1) * Power(d);
            }

            return new BigRational(1, k) * output;
        }

        /// <summary>
        /// Calculate p choose k: (p)(p-1)...(p-k+1)/(k!)
        /// 
        /// Returns 1 if k <= 0
        /// </summary>
        /// <param name="p"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static SymmetricPolynomial Choose(SymmetricPolynomial p, int k)
        {
            if (k <= 0)
                return new SymmetricPolynomial(1);

            SymmetricPolynomial output = new SymmetricPolynomial(p);
            for (int i = 1; i < k; i++)
                output *= new BigRational(1, i + 1) * (p - i);

            return output;
        }

        /// <summary>
        /// Updates the monomial list to have k terms and all sym polynomials of each degree if ByTerms = true
        /// Updates the monomial list to contain all sym polynomials of degree <= k if ByTerms = false
        /// </summary>
        /// <param name="k"></param>
        private static void UpdateMonomialList(int k, bool ByTerms = true)
        {
            monomial = new List<List<int>>() { new List<int> { } };
            foreach (List<int> l in CoefTermsEnumerator(k, ByTerms))
                monomial.Add(l);
        }

        /// <summary>
        /// Returns a string version of this polynomial
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (coefs.Count == 0)
                return "0";

            string s = "" + coefs[0];
            for (int i = 1; i < coefs.Count; i++)
            {
                if (coefs[i].A == 0)
                    continue;

                s += " + ";

                if (coefs[i] != new BigRational(1, 1))
                    s += coefs[i] + "*";
                for (int j = 0; j < monomial[i].Count; j++)
                {
                    s += "t^" + monomial[i][j];
                }
            }
            return s;
        }

        /// <summary>
        /// We have an array that stores the coefficients of the sym polynomial for every monomial.
        /// This method returns the powers of those monomials. For instance, t_1t_2^2 + ... t_it_j^2 
        /// is represented as (1, 2) by output from this method, and has coefficient c[5].
        /// If ByTerms is set to true, 
        /// r is simply the number of terms we compute for the monomial listing.
        /// 
        /// If ByTerms is false,
        /// r is the maximum n we compute to. In other words, we are iterating through all
        /// symmetric polynomials of degree r or less. 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private static IEnumerable<List<int>> CoefTermsEnumerator(int r, bool ByTerms = true)
        {
            //the current sum of the partition
            int n = 1;
            //the number of l's we've returned so far (start at 1, since we don't return (0))
            int k = 1;
            while (true)
            {
                if (!ByTerms && n > r)
                    yield break;

                if (ByTerms && k >= r)
                    yield break;

                foreach (Partition l in Partition.AllPartitions(n))
                {
                    k++;
                    yield return l.GetList();
                }
                n++;
            }
        }

        /// <summary>
        /// Returns true if and only if all their coefficients are the same - i.e., the two polyomials
        /// are exactly the same.
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static bool operator ==(SymmetricPolynomial p_1, SymmetricPolynomial p_2)
        {
            if (p_1.coefs.Count != p_2.coefs.Count)
                return false;

            for (int i = 0; i < p_1.coefs.Count; i++)
                if (p_1.coefs[i] != p_2.coefs[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Override equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (!(obj is SymmetricPolynomial))
                return false;

            return (SymmetricPolynomial)obj == this;
        }

        /// <summary>
        /// Default object HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns true if and only if p_1, p_2 have any differing coefficient.
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static bool operator !=(SymmetricPolynomial p_1, SymmetricPolynomial p_2)
        {
            if (p_1.coefs.Count != p_2.coefs.Count)
                return false;

            for (int i = 0; i < p_1.coefs.Count; i++)
                if (p_1.coefs[i] != p_2.coefs[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Adds two symmetric polynomials together
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static SymmetricPolynomial operator +(SymmetricPolynomial p_1, SymmetricPolynomial p_2)
        {
            List<BigRational> coeffs = new List<BigRational>(p_2.coefs);
            for (int i = 0; i < p_1.coefs.Count; i++)
            {
                if (i >= coeffs.Count)
                    coeffs.Add(new BigRational());
                coeffs[i] += p_1.coefs[i];
            }

            return new SymmetricPolynomial(coeffs);
        }

        /// <summary>
        /// Adds two symmetric polynomials together
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static SymmetricPolynomial operator +(SymmetricPolynomial p, BigRational q)
        {

            List<BigRational> coeffs = new List<BigRational>(p.coefs);

            if (coeffs.Count == 0)
                coeffs.Add(new BigRational());

            coeffs[0] += q;

            return new SymmetricPolynomial(coeffs);
        }

        /// <summary>
        /// Adds two symmetric polynomials together
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static SymmetricPolynomial operator -(SymmetricPolynomial p_1, SymmetricPolynomial p_2)
        {

            List<BigRational> coeffs = new List<BigRational>(p_1.coefs);
            for (int i = 0; i < p_2.coefs.Count; i++)
            {
                if (i < coeffs.Count)
                    coeffs.Add(new BigRational());
                coeffs[i] -= p_2.coefs[i];
            }

            return new SymmetricPolynomial(coeffs);
        }

        /// <summary>
        /// Adds two symmetric polynomials together
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static SymmetricPolynomial operator -(SymmetricPolynomial p, BigRational q)
        {

            List<BigRational> coeffs = new List<BigRational>(p.coefs);

            if (coeffs.Count == 0)
                coeffs.Add(new BigRational());

            coeffs[0] -= q;

            return new SymmetricPolynomial(coeffs);
        }

        /// <summary>
        /// Scalar multiplication of a polynomial
        /// </summary>
        /// <param name="k"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static SymmetricPolynomial operator *(int k, SymmetricPolynomial p)
        {
            List<BigRational> coeffs = new List<BigRational>(p.coefs);

            for (int i = 0; i < coeffs.Count; i++)
            {
                coeffs[i] *= k;
            }

            return new SymmetricPolynomial(coeffs);
        }

        /// <summary>
        /// Scalar multiplication of a polynomial
        /// </summary>
        /// <param name="k"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static SymmetricPolynomial operator *(BigRational k, SymmetricPolynomial p)
        {
            List<BigRational> coeffs = new List<BigRational>(p.coefs);

            for (int i = 0; i < coeffs.Count; i++)
            {
                coeffs[i] *= k;
            }

            return new SymmetricPolynomial(coeffs);
        }

        /// <summary>
        /// Multiplies two symmetric polynomials (with degree tending toward infinity) together 
        /// </summary>
        /// <param name="p_1"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public static SymmetricPolynomial operator *(SymmetricPolynomial p, SymmetricPolynomial q)
        {
            SymmetricPolynomial output = new SymmetricPolynomial();
            for (int i = 0; i < p.coefs.Count; i++)
            {
                for (int j = 0; j < q.coefs.Count; j++)
                {
                    if (p.coefs[i].A != 0 && q.coefs[j].A != 0)
                        output += p.coefs[i] * q.coefs[j] * MultiplyMonomials(monomial[i], monomial[j]);
                }
            }

            return output;
        }

        /// <summary>
        /// Multiplies the monomials p, q. These polynomials are identified
        /// by their powers, represented as a term in monomials.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        private static SymmetricPolynomial MultiplyMonomials(List<int> p, List<int> q)
        {
            // this is the faster version - roughly 2x faster
            if (p.Count < q.Count) return MultiplyMonomials(q, p);

            //which choices for t_i are valid for the next term of p
            List<bool> allowed = new List<bool>();
            for (int i = 0; i < q.Count; i++)
                allowed.Add(true);
            allowed.Add(true);

            return MultiplyMonomialHelper(p, 0, new List<int>(q), allowed, q, -1);
        }

        /// <summary>
        /// Helper method for multiplyMonomials function. 
        /// 
        /// We fix the variables t_1,t_2,t_q.Count and then multiply by different monomial terms of p. 
        /// If some monomial term is of the form t_{i_1}^{p_1}*...*t_{i_r}^{p_r}, this amounts to picking the
        /// values i_1,i_2, ... i_r. We pick the values i_1,...,i_r inductively starting with i_1. 
        /// 
        /// Additionally, we only are interested in monomial terms which are "well formed", which means that
        /// if some t_i is present in the monomial, then all t_j with j <= i are also present in the monomial.
        /// 
        /// The "allowed" array is an array of booleans which keeps track of which values some i_k have already
        /// been assigned to. The allowed array is also convenient to make sure that the resulting monomial is well formed.
        /// 
        /// Once all values of i_1,...,i_r have been assigned properly, we must calculate the coefficient of the
        /// resulting monomial, given by the list q[]. Let us then calculate the total number of terms in the product p*q
        /// which have a monomial which is symmetric to the monomial q[] and has variables in t_1,t_2,...,t_q.Count. 
        /// There are MultinomialOfPartition(originalQ, q.Count) 
        /// total ways of choosing the originalQ variables from t_1,t_2,...,t_q.Count.
        /// There are MultinomialOfPartition(newVars, q.Count - originalQ.Count)
        /// total ways of choosing newVars.Count remaining variables from the remaining q.Count-originalQ.Count variables.
        /// 
        /// Then, we divide by the total number of monomials of the form q[] that can be chosen in t_1,t_2,...,t_q.Count
        /// variables, which is equal to MultinomialOfPartition(q, q.Count), which gives us the desired coefficient.
        /// <param name="p"></param>
        /// <param name="index"></param>
        /// <param name="q"></param>
        /// <param name="allowed"></param>
        /// <param name="originalQ"></param>
        /// <param name="prevChoice"></param>
        /// <returns></returns>
        private static SymmetricPolynomial MultiplyMonomialHelper(List<int> p, int index, List<int> q, List<bool> allowed,
            List<int> originalQ, int prevChoice)
        {
            //return the monomial associated with q
            if (p.Count <= index)
            {
                //there is an ordering imposed as we set t_i with i == q.Count below (adding variables from p to the end of the partition)
                //to correct this ordering, we multiply by the number of ways to choose these variables
                List<int> newVars = new List<int>();
                for (int j = originalQ.Count; j < q.Count; j++)
                    newVars.Add(q[j]);

                //sort so q is a partition
                q.Sort();
                q.Reverse();
                int n = FindMonomialIndex(q);

                List<BigRational> coef = new();

                BigRational r = new(MultinomialOfPartition(originalQ, q.Count) *
                    MultinomialOfPartition(newVars, q.Count - originalQ.Count), MultinomialOfPartition(q, q.Count));

                for (int j = 0; j < n; j++)
                    coef.Add(new BigRational());
                coef.Add(r);

                return new SymmetricPolynomial(coef);
            }

            SymmetricPolynomial output = new();

            //set the variable with power p[index] to t_i
            for (int i = 0; i < allowed.Count; i++)
            {
                if (!allowed[i])
                    continue;

                if (index > 0 && p[index - 1] == p[index] && i < prevChoice)
                    continue;

                List<int> _q = new List<int>(q);

                List<bool> _allowed = new List<bool>(allowed);
                _allowed[i] = false;
                if (i == _allowed.Count - 1)
                {
                    _allowed.Add(true);
                    _q.Add(0);
                }
                _q[i] += p[index];

                output += MultiplyMonomialHelper(p, index + 1, _q, _allowed, originalQ, i);
            }

            return output;
        }

        /// <summary>
        /// Given a monomial in partition form, this function returns the index i such that monomial[i] = q
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static int FindMonomialIndex(List<int> q)
        {
            if (q.Count == 0 || q[0] == 0)
                return 0;

            if (monomial.Count == 0 || ComparePartitions(monomial[monomial.Count - 1], q) < 0)
            {
                int qSum = 0;
                foreach (int k in q)
                    qSum += k;
                UpdateMonomialList(qSum, false);
            }

            int a = 0;
            int b = monomial.Count;
            //our current guess for q's index
            int g;

            while (a < b)
            {
                g = (a + b) / 2;
                int c = ComparePartitions(monomial[g], q);
                if (c == 0)
                    return g;

                if (c < 0)
                    a = g;

                if (c > 0)
                    b = g;
            }

            throw new Exception("Could not find this partition :((((");
        }

        /// <summary>
        /// Returns 0 if p == q, -1 if p < q, and 1 if p > q.
        /// p < q is defined as p occurring before q in monomial. 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static int ComparePartitions(List<int> p, List<int> q)
        {
            //if one has higher sum, it clearly has priority
            int pSum = 0;
            foreach (int k in p)
                pSum += k;
            int qSum = 0;
            foreach (int k in q)
                qSum += k;

            if (pSum > qSum)
                return 1;
            if (pSum < qSum)
                return -1;

            for (int i = 0; i < p.Count; i++)
            {
                if (p[i] < q[i])
                    return 1;
                if (p[i] > q[i])
                    return -1;
            }

            return 0;
        }

        /// <summary>
        /// Returns the multinomial coefficient: n choose ((# of 1's in p), (# of 2's in p), ...)
        /// We assume that p is sorted, and n >= p.Count.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static BigInteger MultinomialOfPartition(List<int> p, int n)
        {
            if (p.Count == 0)
                return 1;

            BigInteger result = Util.Factorial(n) / Util.Factorial(n - p.Count);
            int k = 1;
            for (int i = 1; i < p.Count; i++)
            {
                if (p[i] == p[i - 1])
                    k++;
                else
                {
                    result /= Util.Factorial(k);
                    k = 1;
                }
            }
            result /= Util.Factorial(k);

            return result;
        }
    }
}
