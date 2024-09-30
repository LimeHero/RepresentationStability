# Representation Stability Computations

Computes the multiplicty of a family of irreducibles in the stable cohomology of complex configuration space. Borrowing notation from [Farb](https://arxiv.org/abs/1404.4065), the function ```YoungToPoly``` returns $d_i(\lambda)$ for $0 \leq i \leq maxDegree$ as the coefficients of a formal power series in $z$. 


For an interactive lookup table and complete .csv file of results visit [my website](https://www.math.ucla.edu/~emilg/repstab.html). For implementation details and background, see [my paper](https://TODO).

# Code Details

The code was prepared targeting .NET 8.0 in Visual Studio 2022. ```MainMethod/Main.cs``` is the execution point and contains a helper method for saving the computational output to ```results.csv```. Note that the code has been optimized through memoization to compute ```YoungToPoly``` for all $|\lambda| \leq n$ for some $n$. In particular, once $\text{YoungToPoly}(\lambda)$ has been computed for some $\lambda$, it is fast to compute $\text{YoungToPoly}(\lambda')$ for any $|\lambda'| < |\lambda|$. 

Below we briefly describe each project file.

## Integer Methods

Defines the ```Partition``` class which stores a partition of $n$ as a non-decreasing list of positive integers whose sum is equal to $n$. We implement a simple hash function for memoization. Defines the ```BigRational``` class which handles rational number arithmetic and stores the numerator and denominator as [BigIntegers](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger?view=net-8.0) to avoid overflow errors.

## Polynomials

Defines the ```LaurentPolynomial```, ```Polynomial```, and ```CharacterPolynomial``` classes. Note that character polynomials are stored as rational linear combinations in the [binomial basis](https://arxiv.org/pdf/2001.04112#page=4).


## RepStability

Defines ```YoungToChar.PartToCharPoly``` which takes in a partition $\lambda$ and returns the character polynomial $\chi^\lambda$ of the family of irreducibles $V(\lambda)$ based on example 1.7.14 in [Macdonald](https://math.berkeley.edu/~corteel/MATH249/macdonald.pdf#page=100). Note that while computing $\chi^\lambda$ we determine the character table of $S_n$ for all $n \leq |\lambda|$ via the recursive [Murnaghan-Nakayama rule](https://en.wikipedia.org/wiki/Murnaghan%E2%80%93Nakayama_rule). 

This project also contains ```PolyStatistics.CharPolyToPowSeries``` which takes as input a characteristic polynomial $\chi$ and returns the convergent polynomial statistic $\lim_{n\to\infty}q^{-n}\sum_{f \in \text{Conf}_n(\mathbb{F}_q)} \chi(f)$ as a formal power series in $z = q^{-1}$ via equation 2.11 from [Chen](https://arxiv.org/pdf/1603.03931#page=11). We memoize wherever possible for performance.
