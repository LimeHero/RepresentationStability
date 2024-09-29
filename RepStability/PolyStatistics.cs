using IntegerMethods;
using Polynomials;
using System.Collections.Concurrent;

namespace RepStability
{
    /// <summary>
    /// A static class which contains the functions necessary to TODO
    /// </summary>
    public class PolyStatistic
    {
        // Stores previous results of 
        // The second integer here is storing the maxDegree previously computed.
        // If we lower the maxDegree from the previous computation, we have to recompute it.
        private static readonly Dictionary<Partition, Tuple<LaurentPolynomial, int>> prevResults = [];
        /// <summary>
        /// Given a CharacterPolynomial chi, computes the polynomial statistic 
        /// \lim_{n\to\infty} q^{-n} \sum_{f \in Conf_n(\F_q)} P(f)
        /// expressed as a formal power series in z = q^{-1} with coefficients computed accurately out to z^maxDegree
        /// 
        /// This is given by the following formula:
        /// (x_1 C j_1)(x_2 C j_2)...(x_n C j_n) -> (1 - z) *
        /// \prod_{k = 1}^n ((Polynomial.MoebiusSum(i_k, j_k) C j_k).FormallyInvert() (z^{i_k} - z^{2i_k} + ...)^{j_k}
        /// </summary>
        /// <param name="allTerms"></param>
        /// <param name="maxDegree"></param>
        /// <returns></returns>
        public static LaurentPolynomial CharPolyToPowSeries(CharacterPolynomial chi, int maxDegree)
        {
            List<Partition> terms = chi.Terms;
            List<BigRational> coefs = chi.Coefs;

            LaurentPolynomial output = new();

            for (int i = 0; i < terms.Count; i++)
            {
                LaurentPolynomial nextTerm = new(1);

                // memoization (checking if already computed)
                Partition part = terms[i];
                if (prevResults.TryGetValue(part, out Tuple<LaurentPolynomial, int>? value))
                {
                    if (maxDegree <= value.Item2)
                    {
                        nextTerm = coefs[i] * value.Item1;
                        nextTerm.RoundToNthDegree(maxDegree);
                        output += nextTerm;

                        continue;
                    }
                }

                List<int> cycles = part.GetCycles();
                for (int j = 0; j < cycles.Count; j++)
                {
                    if (cycles[j] == 0)
                        continue;

                    // memoization on individual binomial terms
                    {
                        List<int> termList = []; for (int a = 0; a < cycles[j]; a++) termList.Add(j + 1);
                        Partition _part = new(termList);
                        if (prevResults.ContainsKey(_part))
                        {
                            if (maxDegree <= prevResults[_part].Item2)
                            {
                                nextTerm *= prevResults[_part].Item1;
                                nextTerm.RoundToNthDegree(maxDegree);

                                continue;
                            }
                        }
                    }

                    //the choose portion of the expression
                    LaurentPolynomial nextProd = (LaurentPolynomial)Polynomial.ChooseMoebiusSum(j + 1, cycles[j]);

                    nextProd = nextProd.FormallyInvert();

                    // the power series part of the expression
                    nextProd = MultByPowerSeries(nextProd, maxDegree, new(j + 1, cycles[j]));

                    nextTerm *= nextProd;
                }

                // saving memoization
                prevResults[part] = new(nextTerm, maxDegree);

                output += nextTerm * coefs[i];
            }

            // multiply by 1 - z
            output *= new LaurentPolynomial(0, [1, -1]);
            output.RoundToNthDegree(maxDegree);

            return output;
        }


        /// <summary>
        /// Multiplies poly by the power series (z^{k} - z^{2k} + z^{3k} - ...)^j times.
        /// Computes the coefficients of the resulting power series accurately to degree maxDegree.
        /// </summary>
        /// <param name="poly"></param>
        /// <param name="maxDegree"></param>
        /// <param name="powerSeries"></param>
        /// <returns></returns>
        private static LaurentPolynomial MultByPowerSeries(LaurentPolynomial poly, int maxDegree, Tuple<int, int> powerSeries)
        {
            int i = powerSeries.Item1;
            int j = powerSeries.Item2;

            // we must compute coefficients of (z^{i} - z^{2i} + ... )^j up to maxDegree - poly.lead
            // thus maximal coefficient of (z - z^2 + ... )^j needed is (maxDegree - poly.lead) / i.
            int maxCoefNeeded = (maxDegree - poly.Lead) / i;

            LaurentPolynomial powSeries = new();
            for (int l = j; l <= maxCoefNeeded; l++)
            {
                // coefficient of z^{l} in (z^{1} - z^{2} + ... )^j
                BigRational coef = Partition.CoefOfPowerSeries(l, j);


                powSeries += new LaurentPolynomial(l, [coef]);
            }

            // formally raise pow_series to ith power
            powSeries = powSeries.RaiseZToIthPower(i);
            LaurentPolynomial output = powSeries * poly;
            output.RoundToNthDegree(maxDegree);
            return output;
        }


    }
}
