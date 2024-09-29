# Representation Stability Computations


#

Below we briefly describe each project file.

## Integer Methods

Defines the ```Partition``` class which stores a partition of n as a non-decreasing list of positive integers whose sum is equal to n. Defines the ```BigRational``` class which handles rational number arithmetic and stores the numerator and denominator as [BigIntegers](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger?view=net-8.0) to avoid overflow errors.

## Polynomials

Defines the ```LaurentPolynomial```, ```Polynomial```, and ```CharacterPolynomial``` classes. Note that character polynomials are stored as rational linear combinations in the [binomial basis](https://arxiv.org/pdf/2001.04112#page=4).


## RepStability

Defines ```YoungToChar.PartToCharPoly``` which takes in a partition $\lambda$ and returns the character polynomial $\chi^\lambda$ of the family of irreducibles $V(\lambda)$ based on example 1.7.14 in [Macdonald](https://math.berkeley.edu/~corteel/MATH249/macdonald.pdf#page=100). Note that while computing $\chi^\lambda$ we determine the character table of $S_n$ for all $n \leq |\lambda|$ via the recursive [Murnaghan-Nakayama rule](https://en.wikipedia.org/wiki/Murnaghan%E2%80%93Nakayama_rule). 

This project also contains ```PolyStatistics.CharPolyToPowSeries``` which takes as input a characteristic polynomial $\chi$ and returns the convergent polynomial statistic $\lim_{n\to\infty}q^{-n}\sum_{f \in \Conf_n(\F_q)} \chi(f)$ as a formal power series in $z = q^{-1}$ via equation 2.11 from [Chen](https://arxiv.org/pdf/1603.03931#page=11). Note that we use extensive memoization
