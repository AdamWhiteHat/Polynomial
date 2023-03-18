# Polynomial
A univariate, sparse, integer polynomial class. That is, a polynomial in only one indeterminate, X, that only tracks terms with non-zero coefficients, and all coefficients are BigInteger integers. 

**NOTE: All arithmetic is done symbolically. That means the result a arithmetic operation on two polynomials, returns another polynomial, not some integer that is the result of evaluating said polynomials.**


#


## Generic Arithmetic Polynomial Types
I've written a number of other polynomial implementations and numeric types catering to various specific scenarios. [See this heading for more information](README.md#other-polynomial-projects-matharithmetic-libraries-and-numeric-types).


#


## BigInteger Polynomial

* Supports **symbolic** univariate polynomial arithmetic including:
   * Addition
   * Subtraction
   * Division
   * Multiplication
   * Modulus
   * Factoring
   * Derivatives
   * Exponentiation
   * GCD of polynomials
   * Functional composition
   * Irreducibility checking
   * Polynomial evaluation by assigning values to the indeterminates
   * Numeric values are of type BigInteger, so it support polynomials that evaluate to arbitrarily large numbers
   * While all coefficients must be integers, it does support evaluating the polynomial with real and complex indeterminates, returning a real or complex result

#


## Polynomial Rings over a Finite Field

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
   



# Other mathy projects & numeric types

I've written a number of other polynomial implementations and numeric types catering to various specific scenarios. Depending on what you're trying to do, another implementation of this same library might be more appropriate. All of my polynomial projects should have feature parity, where appropriate[^1].

[^1]: For example, the ComplexPolynomial implementation may be missing certain operations (namely: Irreducibility), because such a notion does not make sense or is ill defined in the context of complex numbers).

* [Polynomial](https://github.com/AdamWhiteHat/Polynomial). The original. A univariate polynomial that uses System.Numerics.BigInteger as the indeterminate type.
* [GenericPolynomial](https://github.com/AdamWhiteHat/GenericPolynomial) allows the indeterminate to be of an arbitrary type, as long as said type implements operator overloading. This is implemented by dynamically, at run time, calling the operator overload methods using Linq.Expressions and reflection.
* [CSharp11Preview.GenericMath.Polynomial](https://github.com/AdamWhiteHat/CSharp11Preview.GenericMath.Polynomial) allows the indeterminate to be of an arbitrary type, but this version is implemented using C# 11's new Generic Math via static virtual members in interfaces.
>
* [MultivariatePolynomial](https://github.com/AdamWhiteHat/MultivariatePolynomial). A multivariate polynomial (meaning more than one indeterminate, e.g. 2*X*Y^2) which uses BigInteger as the type for the indeterminates.
* [GenericMultivariatePolynomial](https://github.com/AdamWhiteHat/GenericMultivariatePolynomial). As above, but allows the indeterminates to be of [the same] arbitrary type. GenericMultivariatePolynomial is to MultivariatePolynomial what GenericPolynomial is to Polynomial, and indeed is implemented using the same strategy as GenericPolynomial (i.e. dynamic calling of the operator overload methods at runtime using Linq.Expressions and reflection).
>
* [ComplexPolynomial](https://github.com/AdamWhiteHat/ComplexPolynomial). Same idea as Polynomial, but using the System.Numerics.Complex class instead of System.Numerics.BigInteger.
* [ComplexMultivariatePolynomial](https://github.com/AdamWhiteHat/ComplexMultivariatePolynomial). Same idea as MultivariatePolynomial, but using the System.Numerics.Complex class instead of System.Numerics.BigInteger.
>
* [BigDecimal](https://github.com/AdamWhiteHat/BigDecimal) - An arbitrary precision, base-10 floating point number class.
* [BigRational](https://github.com/AdamWhiteHat/BigRational) encodes a numeric value as an Integer + Fraction
* [BigComplex](https://github.com/AdamWhiteHat/BigComplex) - Essentially the same thing as System.Numerics.Complex but uses a System.Numerics.BigInteger type for the real and imaginary parts instead of a double.
>
* [IntervalArithmetic](https://github.com/AdamWhiteHat/IntervalArithmetic). Instead of representing a value as a single number, interval arithmetic represents each value as a mathematical interval, or range of possibilities, [a,b], and allows the standard arithmetic operations to be performed upon them too, adjusting or scaling the underlying interval range as appropriate. See [Wikipedia's article on Interval Arithmetic](https://en.wikipedia.org/wiki/Interval_arithmetic) for further information.
* [GNFS](https://github.com/AdamWhiteHat/GNFS) - A C# reference implementation of the General Number Field Sieve algorithm for the purpose of better understanding the General Number Field Sieve algorithm.

