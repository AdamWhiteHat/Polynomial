# Polynomial
A univariate, sparse, integer polynomial class. That is, a polynomial in only one indeterminate, X, that only tracks terms with non-zero coefficients, and all coefficients are BigInteger integers. 

**NOTE: All arithmetic is done symbolically. That means the result a arithmetic operation on two polynomials, returns another polynomial, not some integer that is the result of evaluating said polynomials.**


#


### Generic Arithmetic Types
   * I created an implementation that can perform symbolic polynomial arithmetic on generic numeric types. All polynomial arithmetic is performed on this generic type, allowing BigInteger to be swapped out for Complex, Decimal, Double, BigComplex, BigDecimal, BigRational, Int32, Int64 and more! Check it out on my [GenericArithmetic-Expression branch](https://github.com/AdamWhiteHat/Polynomial/tree/GenericArithmetic-Expression).


#


### BigInteger Polynomial

* Supports **symbolic** univariate polynomial arithmetic including:
   * Addition
   * Subtraction
   * Multiplication
   * Division
   * Modulus
   * Exponentiation   
   * GCD of polynomials
   * Irreducibility checking
   * Polynomial evaluation by assigning to the invariant (X in this case) a value.
   * All numbers use BigInteger integers, for arbitrarily large numbers.

#


### Polynomial Rings over a Finite Field

* **Polynomial.Field** supports all of the above arithmetic operations, but on a polynomial ring over a finite field!
   * What this effectively means in less-technical terms is that the polynomial arithmetic is performed in the usual way, but the result is then taken modulus two things: A BigInteger integer and another polynomial:
      * Modulus an integer: All the polynomial coefficients are reduced modulus this integer.
      * Modulus a polynomial: The whole polynomial is reduced modulus another, smaller, polynomial. This notion works much the same as regular modulus; The modulus polynomial, lets call it g, is declared to be zero, and so every multiple of g is reduced to zero. You can think of it this way (although this is not how its actually carried out): From a large polynomial, g is repeatedly subtracted from that polynomial until it cant subtract g anymore without going past zero. The result is a polynomial that lies between 0 and g. Just like regular modulus, the result is always less than your modulus, or zero if the first polynomial was an exact multiple of the modulus.
      * Effectively forms a quotient ring
   
* You can instantiate a polynomial in various ways:
   * From a string
      * This is the most massively-useful way and is the quickest way to start working with a particular polynomial you had in mind.
   * From its roots
      * Build a polynomial that has as its roots, all of the numbers in the supplied array. If you want multiplicity of roots, include that number in the array multiple times.
   * From the base-m expansion of a number
      * Given a large number and a radix (base), call it m, a polynomial will be generated that is that number represented in the number base m.
   

* Other methods of interest that are related to, but not necessarily performed on a polynomial:
   * Eulers Criterion
   * Legendre Symbol and Legendre Symbol Search
   * Tonelli-Shanks
   * Chinese Remainder Theorem
   


