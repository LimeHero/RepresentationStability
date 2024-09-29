using IntegerMethods;
using Polynomials;
using System.Collections.Concurrent;

namespace RepStability
{
    public class YoungToChar
    {

        /// <summary>
        /// Given a partition part, returns the character polynomial for the family of irreducibles
        /// V(part). 
        /// 
        /// The computation of \chi^\mu_\rho is completed using the previously computed 
        /// character polynomials when possible (stored in the two above lists)
        /// but otherwise using recursive Murnaghan-Nakayama. 
        /// 
        /// Computed using the formula here https://arxiv.org/pdf/2204.10633 
        /// originally from Macdonald example 1.7.14. 
        /// </summary>
        /// <param name="part">partition</param>
        /// <returns>character polynomial for the family of irreducibles V(part)</returns>
        /// <exception cref="Exception"></exception>
        public static CharacterPolynomial PartToCharPoly(Partition part)
        {
            CharacterPolynomial result = new();

            int sizePart = part.Size();
            List<int> part_list = part.GetList();

            for (int i = 0; i <= sizePart; i++)
            {
                // all partitions mu with |mu| = i such that part - mu is a vertical strip,
                // i.e., |part| - i boxes can be added to mu (with never two on the same row)
                // to be equal to k
                List<Partition> all_mu = [];
                // check if k - mu is a vertical strip, if so add it to all_mu
                foreach (Partition rho_part in Partition.AllPartitions(i))
                {
                    List<int> rho = rho_part.GetList();
                    bool valid_mu = true;
                    for (int j = 0; j < rho.Count; j++)
                    {
                        if (rho.Count > part_list.Count)
                        {
                            valid_mu = false;
                            break;
                        }

                        if (part_list[j] - rho[j] >= 2 || part_list[j] - rho[j] < 0)
                        {
                            valid_mu = false;
                            break;
                        }
                    }

                    for (int j = rho.Count; j < part_list.Count; j++)
                    {
                        if (part_list[j] > 1)
                        {
                            valid_mu = false;
                            break;
                        }
                    }

                    if (valid_mu)
                        all_mu.Add(new(rho_part));
                }

                // Compute the coefficients F_\rho^\mu and add it to result
                foreach (Partition rho in Partition.AllPartitions(i))
                {
                    BigRational coef = 0;
                    foreach (Partition mu in all_mu)
                        coef += IrreducibleCharacter(mu, rho);

                    // multiply by (-1)^{|k| - |rho|}
                    if ((sizePart - i) % 2 == 1)
                        coef *= -1;

                    // no need to add the term if coef = 0
                    if (coef == 0)
                        continue;

                    result.Append(rho, coef);
                }
            }
            return result;
        }

        private static readonly Dictionary<Tuple<Partition, Partition>, BigRational> irreducible_character_prev_values = [];
        /// <summary>
        /// Returns the value of the character of the irreducible representation given by mu
        /// evaluated at the conjugacy class of S_n given by the partition rho.
        /// 
        /// Calculated using the *recursive* version of Murnaghan-Nakayama.
        /// https://en.wikipedia.org/wiki/Murnaghan%E2%80%93Nakayama_rule
        /// </summary>
        /// <param name="mu">partition of n</param>
        /// <param name="rho">partition of n</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static BigRational IrreducibleCharacter(Partition mu, Partition rho)
        {
            if (mu.Size() != rho.Size())
                throw new Exception("mu and rho must have the same size");

            Tuple<Partition, Partition> t = new(mu, rho);
            if (irreducible_character_prev_values.ContainsKey(t))
                return irreducible_character_prev_values[t];

            // trivial rep
            if (mu.Size() == 0)
                return 1;

            BigRational rslt = 0;

            List<int> rho_list = rho.GetList();
            List<int> mu_list = mu.GetList();
            int bs_size = rho_list[0];
            // We need to sum over all border strips of mu.
            // All border strips are determined by their upper rightmost entry.
            // This upper rightmost entry must be at the right end of a row of mu,
            // and then it must stay along the farthest right side of the Tableau.
            // So we just try the rightmost entry of each row of mu.
            // The only way this can fail is if the resulting list of boxes is not 
            // a Tableau anymore, i.e., the next row after the border strip is now 
            // longer than the previous.
            for (int start_of_bs = 0; start_of_bs < mu_list.Count; start_of_bs++)
            {
                List<int> bs = [];

                int i = start_of_bs;
                int bs_sum = 0;
                while (bs_sum < bs_size)
                {
                    // not enough room for a bs of given length starting at start_of_bs
                    if (i == mu_list.Count)
                        break;

                    // we only "take" what we need - either take
                    // all of the next row, or just bs_size - bs_sum of the next row,
                    // which is the number of boxes remaining for the border strip
                    if (i == mu_list.Count - 1)
                    {
                        bs.Add(Math.Min(mu_list[^1], bs_size - bs_sum));
                    }
                    else
                    {
                        // this is when resulting diagram fails to be a young diagram:
                        // draw a picture!
                        if (bs_size - bs_sum == mu_list[i] - mu_list[i + 1] + 1)
                            break;

                        bs.Add(Math.Min(mu_list[i] - mu_list[i + 1] + 1, bs_size - bs_sum));
                    }
                    bs_sum += bs[^1];
                    i++;
                }

                // there is no border strip starting at start_of_bs
                if (bs_sum < bs_size)
                    continue;

                int sgn = 1; if ((bs.Count % 2) == 0) sgn = -1;

                // otherwise, delete the border strip from mu, remove rho[0], and recurse
                List<int> new_rho = []; for (int j = 1; j < rho_list.Count; j++) new_rho.Add(rho_list[j]);
                List<int> new_mu = []; for (int j = 0; j < mu_list.Count; j++) new_mu.Add(mu_list[j]);
                for (int j = 0; j < bs.Count; j++) new_mu[j + start_of_bs] -= bs[j];

                rslt += sgn * IrreducibleCharacter(new(new_mu), new(new_rho));
            }


            irreducible_character_prev_values[t] = rslt;
            return rslt;
        }
    }
}
