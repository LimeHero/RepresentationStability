using IntegerMethods;
using Polynomials;

namespace Deprecated
{
    public class CharPolyToPowSeries
    {

        /// <summary>
        /// This method represents the given symmetric polynomial p (viewed as a function
        /// on S_n by p(sigma) = p(e_1, ..., e_n) where e_1, ... e_n are the eigenvalues
        /// of the matrix representation of sigma) as a character polynomial in the
        /// usual binomial basis.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static CharacterPolynomial SymmetricInChooseBasis(SymmetricPolynomial p)
        {
            List<List<Tuple<int, int>>> terms = new();
            List<BigRational> coefficients = new();

            SymmetricPolynomial q = new(p.coefs);
            //We subtract terms from q until we get 0
            while (q.coefs.Count > 0)
            {
                if (q.coefs.Count == 1)
                {
                    coefficients.Add(q.coefs[0]);
                    terms.Add(new List<Tuple<int, int>>());
                    break;
                }
                coefficients.Add(q.coefs[^1]);

                // if the partition is (5 2 2 1)
                // we want to subtract the term (p'_5 C 1)(p'_2 C 2)(p'_1 C 1)
                // so for each value we see how many times it appears, and proceed
                List<Tuple<int, int>> nextTerm = new();
                List<int> part = SymmetricPolynomial.monomial[q.coefs.Count - 1];
                int lastK = part[0];
                int j = 0;
                foreach (int k in part)
                {
                    if (k == lastK)
                        j++;
                    else
                    {
                        nextTerm.Add(new Tuple<int, int>(lastK, j));
                        j = 1;
                    }

                    lastK = k;
                }
                nextTerm.Add(new Tuple<int, int>(lastK, j));

                //subtract this term from q, and then we proceed
                SymmetricPolynomial prod = new(1);
                foreach (Tuple<int, int> t in nextTerm)
                {
                    SymmetricPolynomial productTerm = SymmetricPolynomial.Choose(SymmetricPolynomial.PowerPrime(t.Item1), t.Item2);
                    prod *= SymmetricPolynomial.Choose(SymmetricPolynomial.PowerPrime(t.Item1), t.Item2);
                }
                //we want to make sure this has the right coefficient to cancel out the term we want
                BigRational matchingCoef = new BigRational(1, 1) / prod.coefs[^1];
                prod = coefficients[^1] * matchingCoef * prod;

                coefficients[^1] *= matchingCoef;

                terms.Add(nextTerm);
                q -= prod;
            }

            return new CharacterPolynomial(terms, coefficients);
        }

        /// <summary>
        /// The inverse of SymmetricInChooseBasis - writes the given character polynomial
        /// in terms of the elementary symmetric polynomials (interpreted as a function
        /// on S_n by the above).
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static SymmetricPolynomial ChooseBasisToSymmetric(CharacterPolynomial P)
        {
            SymmetricPolynomial sym = new();

            for (int i = 0; i < P.Terms.Count; i++)
            {
                List<int> nextTerm = P.Terms[i].GetCycles();
                SymmetricPolynomial prod = new(P.Coefs[i]);
                for (int j = 0; j < nextTerm.Count; j++) { 
                    if (nextTerm[j] == 0) continue;
                    SymmetricPolynomial productTerm = SymmetricPolynomial.Choose(SymmetricPolynomial.PowerPrime(j+1), nextTerm[j]);
                    prod *= productTerm;
                }

                sym += prod;
            }

            return sym;
        }

    }
}
