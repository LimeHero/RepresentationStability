# RepresentationStabilityComputations
Code base to calculate limiting multiplicites of families of irreducible representations in the cohomology of complex configuration space. 
In "MainMethod.cs", there are a number of useful computations contained in different code blocks. 

The primary function of the code is provided by the function YoungToPoly(part, smallestDegree), which given a partition of n (a list of decreasing positive integers
with sum n) returns an infinite power series in $q^{-1}$ representing the stable multiplicity of the family of irreducible partitions given by "part" in the
cohomology of complex configuration space. For instance, the empty partition [] returns the stable multiplicity of the trivial representation, and the partition [1]
of 1 returns the stable multiplicity of the standard representation. See https://math.uchicago.edu/~farb/papers/Pn-FqT.pdf for an introduction to representation
stability and what this power series represents. 
The infinite power series is expanded out to the optional "smallestDegree" term, so it will be cut off at (and including) $q^\text{-smallestDegree}$.

# Abstract:
Representation stability was introduced to study mathematical structures which stabilize when viewed from a representation theoretic framework. 
The instance of representation stability studied in this project is that of ordered complex configuration space, denoted $PConf_n(\mathbb{C})$:

$$PConf_n(\mathbb{C}) := \\{ (x_1, x_2, \dots, x_n) \in \mathbb{C}^n \ | \ x_i \neq x_j \\}$$

$PConf_n(\mathbb{C})$ has a natural $S_n$ action by permuting its coordinates which gives the cohomology groups $H^i(PConf_n(\mathbb{C});\mathbb{Q})$ the structure of an $S_n$ representation. 
The cohomology of $PConf_n(\mathbb{C})$ \textit{stabilizes} as $n$ tends toward infinity when viewed as a family of $S_n$ representations. From previous work, there is an 
explicit description for $H^i(PConf_n(\mathbb{C});\mathbb{Q})$ as a direct sum of induced representations for any $i, n$, but this description does not explain the behavior of 
families of irreducible representations as $n\to\infty$. We implement an algorithm which, given a Young Tableau, computes the cohomological degrees where the 
corresponding family of irreducible representations appears stably as $n\to\infty$. Previously, these values were known for only a few Young Tableaus and cohomological 
degrees. Using this algorithm, results have been found for all Young Tableau with up to 8 boxes and certain Tableau with more, which has led us to conjectures based on 
the data collected.


