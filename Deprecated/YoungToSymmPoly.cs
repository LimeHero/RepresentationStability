using IntegerMethods;
using Polynomials;

namespace Deprecated
{
    public class YoungToSymmPoly
    {

        /// <summary>
        /// Computes the character of the representation defined by the given tableau (descending
        /// list of positive integers) evaluated on the given conjugacy class. The cycleType should
        /// be a list of positive integers [i_1, i_2, ..., i_n] such that the cycle type has
        /// i_j j-cycles. Used for testing.
        /// </summary>
        /// <param name="tableau"></param>
        /// <param name="cycles"></param>
        /// <returns></returns>
        public static BigRational FrobeniusFormula(List<int> tableau, List<int> cycles)
        {
            if (tableau.Count == 0 || tableau[0] == 0)
                return 1;

            SymmetricPolynomial prod = new(1);

            for (int i = 0; i < cycles.Count; i++)
                for (int j = 0; j < cycles[i]; j++)
                    prod *= SymmetricPolynomial.Power(i + 1);

            List<int> l = new();
            for (int i = 0; i < tableau.Count; i++)
            {
                l.Add(tableau[i] + tableau.Count - i - 1);
            }

            BigRational rslt = 0;
            // we add each term of the discriminant bit by bit - start with (-x_2)*(-x_3)*...
            // and each binary digit corresponds to a choice of a term in the discriminant
            int numterms = (tableau.Count * (tableau.Count - 1)) / 2;
            int power2 = (int)Util.Pow(2, numterms);

            for (int i = 0; i < power2; i++)
            {
                List<int> currentterm = new(l);
                List<int> bindig = Util.BinaryDigits(i);


                while (bindig.Count < numterms)
                    bindig.Add(0);

                int k = 0;
                int pm1 = 1;
                for (int n = 0; n < tableau.Count; n++)
                {
                    for (int m = 0; m < n; m++)
                    {
                        if (bindig[k] == 0)
                        {
                            pm1 *= -1;
                            currentterm[n] -= 1;
                        }
                        else
                            currentterm[m] -= 1;

                        k++;
                    }
                }

                currentterm.Sort();
                currentterm.Reverse();
                if (currentterm[^1] < 0)
                    continue;

                while (currentterm[^1] == 0)
                    currentterm.RemoveAt(currentterm.Count - 1);

                int index = SymmetricPolynomial.FindMonomialIndex(currentterm);
                if (index >= prod.coefs.Count)
                    continue;

                rslt += prod.coefs[index] * pm1;
            }

            return rslt;
        }

        /// <summary>
        /// Returns the symmetric polynomial e_n - e_{n-1} + e_{n-2} - ... +- 1
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static SymmetricPolynomial WedgeToSymmetricPolynomial(int n)
        {
            SymmetricPolynomial p = new();
            int pm1 = 1;
            for (int i = n; i >= 1; i--)
            {
                p += pm1 * SymmetricPolynomial.Elementary(i);
                pm1 *= -1;
            }
            p += new SymmetricPolynomial(pm1);

            return p;
        }


        private static readonly Dictionary<Partition, CharacterPolynomial> frob_prev_computations = new();
        /// <summary>
        /// Returns the characteristic polynomial (in choose basis) of the family of irreducible representations
        /// given by young diagrams of the form (n, k[0], k[1], ... k[^1]) for n -> infty.
        /// 
        /// Computed using Frobenius character formula. Slower in almost all cases, but if k.Count
        /// is small can outperform PartToCharPoly.
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static CharacterPolynomial FrobPartToCharPoly(List<int> k)
        {
            // special cases. Note that we do separate computation if
            // k = (1, 1, 1, ..., 1)
            {
                bool allones = true;
                for (int i = 0; i < k.Count; i++)
                {
                    if (k[i] <= 0)
                        throw new Exception("(YoungToPoly) No negative numbers!");

                    if (i > 0 && k[i] > k[i - 1])
                        throw new Exception("(YoungToPoly) Must be nonincreasing sequence!");

                    if (k[i] != 1)
                        allones = false;
                }

                if (allones)
                    return CharPolyToPowSeries.SymmetricInChooseBasis(WedgeToSymmetricPolynomial(k.Count));
            }

            CharacterPolynomial result = new();

            List<Tuple<int, List<int>>> discriminant = new();
            // we need not add the x_0 coefficient, since tending toward infinity
            // we compute the discriminant binomials by taking determinant of vandermont matrix

            // we have to compute this sgn_change to account for the fact that we have the determinant
            // as \prod_{i<j} (x_i - x_j) in Frobenius formula, but as \prod_{i<j} (x_j - x_i) for det Vandermont matrix
            int sgn_change = ((k.Count * (k.Count + 1) / 2) % 2) == 0 ? 1 : -1;
            foreach (Partition part in Util.Permutations(k.Count + 1))
            {
                List<int> L = part.GetList();
                List<int> currentterm = new() { }; for (int j = 0; j <= k.Count; j++) currentterm.Add(L[j]);
                discriminant.Add(new(Util.Sgn(L) * sgn_change, currentterm));
            }

            // iterate through all terms of the discriminant
            foreach (Tuple<int, List<int>> term in discriminant)
            {
                // the integers l in Frobenius formula
                List<int> l = new(k);
                for (int i = 0; i < l.Count; i++)
                {
                    l[i] -= term.Item2[i + 1];
                    l[i] += l.Count - 1 - i;
                }

                l.Sort();
                l.Reverse();
                if (l[^1] < 0)
                    continue;

                //memoization lookup
                if (frob_prev_computations.ContainsKey(new Partition(l)))
                {
                    result += term.Item1 * frob_prev_computations[new Partition(l)];
                    continue;
                }

                // the result for this term of discriminant
                CharacterPolynomial this_term_result = new();

                // All partitions of l[0], l[1], ... l[^1]
                foreach (List<Partition> parts in Partition.AllPartitionLists(l))
                {
                    // Change to cycle format of partitions
                    List<List<int>> cycles = new();
                    foreach (Partition part in parts) cycles.Add(part.GetCycles());

                    // make all partitions the same size for ease of implementation
                    int maxterm = parts[0].GetList()[0];
                    for (int i = 1; i < parts.Count; i++)
                        if (parts[i].GetList()[0] > maxterm)
                            maxterm = parts[i].GetList()[0];
                    foreach (List<int> cycle in cycles)
                        while (cycle.Count < maxterm)
                            cycle.Add(0);

                    List<Tuple<int, int>> choosePoly = new(); // next term added
                    BigRational coef = 1; // coefficient of determinant term (\pm 1)

                    for (int i = 0; i < maxterm; i++)
                    {
                        // Finding the (x_i C j_i) term
                        int totalsum = 0;
                        foreach (List<int> cycle in cycles)
                            totalsum += cycle[i];

                        // j_i = 0, so skip
                        if (totalsum == 0)
                            continue;

                        // i + 1, since we index from 1 in the variables x_1, ... x_n for character polynomial
                        choosePoly.Add(new(i + 1, totalsum));

                        // (multinomial) coefficient
                        List<int> ithterms = new();
                        foreach (List<int> cycle in cycles)
                            ithterms.Add(cycle[i]);
                        coef *= Util.MultinomialCoef(totalsum, ithterms);
                    }

                    this_term_result.Append(new(choosePoly), coef);
                }

                // memoization saving
                frob_prev_computations[new Partition(l)] = this_term_result;

                result += term.Item1 * this_term_result;
            }

            return new CharacterPolynomial(result.Terms, result.Coefs);
        }
    }
}
