using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace PolynomialLibrary
{
	public static class GenericArithmetic<T>
	{
		public static T MinusOne = ConvertImplementation<int, T>.Convert(-1);
		public static T Zero = ConvertImplementation<int, T>.Convert(0);
		public static T One = ConvertImplementation<int, T>.Convert(1);
		public static T Two = ConvertImplementation<int, T>.Convert(2);

		private static Func<T, T, T> _addFunction = null;
		private static Func<T, T, T> _subtractFunction = null;
		private static Func<T, T, T> _multiplyFunction = null;
		private static Func<T, T, T> _divideFunction = null;
		private static Func<T, T, T> _moduloFunction = null;
		//private static Func<T, int, T> _powerFunction = null;
		private static Func<T, T, T> _powerFunction = null;

		private static Func<T, T, bool> _greaterthanFunction = null;
		private static Func<T, T, bool> _equalFunction = null;

		private static Func<T, T> _sqrtFunction = null;
		private static Func<T, int, T, T> _modpowFunction = null;
		private static Func<T, T> _truncateFunction = null;
		private static Func<string, T> _parseFunction = null;
		private static Func<T, double, double> _logFunction = null;
		private static Func<T, byte[]> _tobytesFunction = null;

		public static T Convert<U>(U value)
		{
			if (IsComplexValueType(typeof(T)))
			{
				return (T)((object)new Complex((double)System.Convert.ChangeType(value, typeof(double)), 0d));
			}
			return ConvertImplementation<U, T>.Convert(value);
		}

		public static T Add(T a, T b)
		{
			if (_addFunction == null)
			{
				_addFunction = CreateGenericBinaryFunction(ExpressionType.Add);
			}
			return _addFunction.Invoke(a, b);
		}

		public static T Subtract(T a, T b)
		{
			if (_subtractFunction == null)
			{
				_subtractFunction = CreateGenericBinaryFunction(ExpressionType.Subtract);
			}
			return _subtractFunction.Invoke(a, b);
		}

		public static T Multiply(T a, T b)
		{
			if (_multiplyFunction == null)
			{
				_multiplyFunction = CreateGenericBinaryFunction(ExpressionType.Multiply);
			}
			return _multiplyFunction.Invoke(a, b);
		}

		public static T Divide(T a, T b)
		{
			if (_divideFunction == null)
			{
				_divideFunction = CreateGenericBinaryFunction(ExpressionType.Divide);
			}
			return _divideFunction.Invoke(a, b);
		}

		public static T Modulo(T a, T b)
		{
			if (_moduloFunction == null)
			{
				_moduloFunction = CreateGenericBinaryFunction(ExpressionType.Modulo);
			}
			return _moduloFunction.Invoke(a, b);
		}

		public static T Power(T a, int b)
		{
			return Power(a, ConvertImplementation<int, T>.Convert(b));
		}

		public static T Power(T a, T b)
		{
			if (_powerFunction == null)
			{
				_powerFunction = CreatePowerFunction();
			}
			return _powerFunction.Invoke(a, b);
		}

		public static T Negate(T a)
		{
			return Multiply(a, MinusOne);
		}

		public static T Increment(T a)
		{
			return Add(a, One);
		}

		public static T Decrement(T a)
		{
			return Subtract(a, One);
		}

		public static bool GreaterThan(T a, T b)
		{
			if (IsComplexValueType(typeof(T)))
			{
				return ComplexComparisonInternal(a, b, ExpressionType.GreaterThan);
			}
			if (_greaterthanFunction == null)
			{
				_greaterthanFunction = CreateGenericComparisonFunction(ExpressionType.GreaterThan);
			}
			return _greaterthanFunction.Invoke(a, b);
		}

		public static bool LessThan(T a, T b)
		{
			return !(GreaterThan(a, b) || Equal(a, b));
		}

		public static bool GreaterThanOrEqual(T a, T b)
		{
			return (GreaterThan(a, b) || Equal(a, b));
		}

		public static bool LessThanOrEqual(T a, T b)
		{
			return (LessThan(a, b) || Equal(a, b));
		}

		public static bool Equal(T a, T b)
		{
			if (_equalFunction == null)
			{
				_equalFunction = CreateGenericComparisonFunction(ExpressionType.Equal);
			}
			return _equalFunction.Invoke(a, b);
		}

		public static bool NotEqual(T a, T b)
		{
			return !Equal(a, b);
		}

		public static T SquareRoot(T input)
		{
			Type typeFromHandle = typeof(T);
			if (IsArithmeticValueType(typeFromHandle) && (typeFromHandle != typeof(double)))
			{
				return (T)System.Convert.ChangeType(GenericArithmetic<double>.SquareRoot(System.Convert.ToDouble(input)), typeFromHandle);
			}

			if (_sqrtFunction == null)
			{
				_sqrtFunction = CreateSqrtFunction();
			}
			return _sqrtFunction.Invoke(input);
		}

		public static T ModPow(T value, int exponent, T modulus)
		{
			if (_modpowFunction == null)
			{
				_modpowFunction = CreateModPowFunction();
			}
			return _modpowFunction.Invoke(value, exponent, modulus);
		}

		public static T Truncate(T input)
		{
			if (_truncateFunction == null)
			{
				_truncateFunction = CreateTruncateFunction();
			}
			return _truncateFunction.Invoke(input);
		}

		public static T Parse(string input)
		{
			if (_parseFunction == null)
			{
				_parseFunction = CreateParseFunction();
			}
			return _parseFunction.Invoke(input);
		}

		public static T Max(T left, T right)
		{
			if (GreaterThanOrEqual(left, right))
			{
				return left;
			}
			return right;
		}

		public static T Abs(T input)
		{
			return (!GreaterThanOrEqual(input, Zero)) ? Negate(input) : input;
		}

		public static T Sign(T input)
		{
			if (GreaterThan(input, Zero))
			{
				return One;
			}
			else if (LessThan(input, Zero))
			{
				return MinusOne;
			}
			return Zero;
		}

		public static T DivRem(T dividend, T divisor, out T remainder)
		{
			T rem = Modulo(dividend, divisor);
			remainder = rem;
			return Divide(dividend, divisor);
		}

		public static T Clone(T obj)
		{
			var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(serialized);
		}

		public static T GCD(IEnumerable<T> array)
		{
			return array.Aggregate(GCD);
		}

		public static T GCD(T left, T right)
		{
			T absLeft = Abs(left);
			T absRight = Abs(right);
			while (NotEqual(absLeft, Zero) && NotEqual(absRight, Zero))
			{
				if (GreaterThan(absLeft, absRight))
				{
					absLeft = Modulo(absLeft, absRight);
				}
				else
				{
					absRight = Modulo(absRight, absLeft);
				}
			}
			return Max(absLeft, absRight);
		}

		public static double Log(T value, double baseValue)
		{
			Type typeFromHandle = typeof(T);
			if (IsArithmeticValueType(typeFromHandle) && typeFromHandle != typeof(double))
			{
				return GenericArithmetic<double>.Log(System.Convert.ToDouble(value), baseValue);
			}
			if (_logFunction == null)
			{
				_logFunction = CreateLogFunction();
			}
			return _logFunction.Invoke(value, baseValue);
		}

		public static byte[] ToBytes(T input)
		{
			if (_tobytesFunction == null)
			{
				_tobytesFunction = CreateToBytesFunction();
			}
			return _tobytesFunction.Invoke(input);
		}

		private static bool IsArithmeticValueType(Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = type.GetGenericArguments()[0];
			}
			if (!type.IsEnum)
			{
				TypeCode typeCode = Type.GetTypeCode(type);
				if ((uint)(typeCode - 7) <= 8u)
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsComplexValueType(Type type)
		{
			return (type == typeof(Complex));
		}

		private static T SquareRootInternal(T input)
		{
			if (Equal(input, Zero)) { return Zero; }

			T n = Zero;
			T p = Zero;
			T low = Zero;
			T high = Abs(input);

			while (GreaterThan(high, Increment(low)))
			{
				n = Divide(Add(high, low), Two);
				p = Multiply(n, n);

				if (LessThan(input, p)) { high = n; }
				else if (GreaterThan(input, p)) { low = n; }
				else { break; }
			}
			return Equals(input, p) ? n : low;
		}

		private static T ModPowInternal(T value, int exponent, T modulus)
		{
			T power = Power(value, exponent);
			return Modulo(power, modulus);
		}

		private static int ConvertBigIntegerToInt(BigInteger value)
		{
			return (int)value;
		}

		private static bool ComplexComparisonInternal(T left, T right, ExpressionType operationType)
		{
			if (!IsComplexValueType(typeof(T)))
			{
				throw new Exception("T must be of type: Complex.");
			}

			Complex? l = left as Complex?;
			Complex? r = right as Complex?;
			if (!l.HasValue || !r.HasValue)
			{
				throw new Exception("Could not cast parameters to type: Complex.");
			}

			double lft = Complex.Abs(l.Value);
			double rght = Complex.Abs(r.Value);

			if (operationType == ExpressionType.GreaterThan)
			{
				return (lft > rght);
			}
			else if (operationType == ExpressionType.Equal)
			{
				return (lft == rght);
			}
			/*
			else if (operationType == ExpressionType.LessThan)			{	return (lft < rght);	}
			else if (operationType == ExpressionType.GreaterThanOrEqual){	return (lft >= rght);	}
			else if (operationType == ExpressionType.LessThanOrEqual)	{	return (lft <= rght);	}			
			else if (operationType == ExpressionType.NotEqual)			{	return (lft != rght);	}
			*/
			else
			{
				throw new NotSupportedException($"Not a comparison expression type: {Enum.GetName(typeof(ExpressionType), operationType)}.");
			}
		}

		private static class ConvertImplementation<TFrom, TTo>
		{
			private static Func<TFrom, TTo> _convertFunction = null;

			public static TTo Convert(TFrom value)
			{
				if (_convertFunction == null)
				{
					_convertFunction = CreateConvertFunction();
				}
				return _convertFunction.Invoke(value);
			}

			private static Func<TFrom, TTo> CreateConvertFunction()
			{
				ParameterExpression value = Expression.Parameter(typeof(TFrom), "value");
				Expression convert = Expression.Convert(value, typeof(TTo));
				Func<TFrom, TTo> result = Expression.Lambda<Func<TFrom, TTo>>(convert, value).Compile();
				return result;
			}
		}

		private static Func<T, T, T> CreateGenericBinaryFunction(ExpressionType operationType)
		{
			ParameterExpression left = Expression.Parameter(typeof(T), "left");
			ParameterExpression right = Expression.Parameter(typeof(T), "right");

			BinaryExpression operation = null;
			if (operationType == ExpressionType.Add)
			{
				operation = Expression.Add(left, right);
			}
			else if (operationType == ExpressionType.Subtract)
			{
				operation = Expression.Subtract(left, right);
			}
			else if (operationType == ExpressionType.Multiply)
			{
				operation = Expression.Multiply(left, right);
			}
			else if (operationType == ExpressionType.Divide)
			{
				operation = Expression.Divide(left, right);
			}
			else if (operationType == ExpressionType.Modulo)
			{
				operation = Expression.Modulo(left, right);
			}
			else if (operationType == ExpressionType.Power)
			{
				operation = Expression.Power(left, right);
			}
			else
			{
				throw new NotSupportedException($"ExpressionType not supported: {Enum.GetName(typeof(ExpressionType), operationType)}.");
			}

			Func<T, T, T> result = Expression.Lambda<Func<T, T, T>>(operation, left, right).Compile();
			return result;
		}

		private static Func<T, T> CreateGenericUnaryFunction(ExpressionType operationType)
		{
			ParameterExpression value = Expression.Parameter(typeof(T), "value");

			UnaryExpression operation = null;
			if (operationType == ExpressionType.Negate)
			{
				operation = Expression.Negate(value);
			}
			else
			{
				throw new NotSupportedException($"ExpressionType not supported: {Enum.GetName(typeof(ExpressionType), operationType)}.");
			}

			Func<T, T> result = Expression.Lambda<Func<T, T>>(operation, value).Compile();
			return result;
		}

		private static Func<T, T, bool> CreateGenericComparisonFunction(ExpressionType operationType)
		{
			ParameterExpression left = Expression.Parameter(typeof(T), "left");
			ParameterExpression right = Expression.Parameter(typeof(T), "right");

			BinaryExpression comparison = null;
			if (operationType == ExpressionType.GreaterThan)
			{
				comparison = Expression.GreaterThan(left, right);
			}
			else if (operationType == ExpressionType.Equal)
			{
				comparison = Expression.Equal(left, right);
			}
			/*
			else if (operationType == ExpressionType.LessThan)			{	comparison = Expression.LessThan(left, right);			}
			else if (operationType == ExpressionType.GreaterThanOrEqual){	comparison = Expression.GreaterThanOrEqual(left, right);}
			else if (operationType == ExpressionType.LessThanOrEqual)	{	comparison = Expression.LessThanOrEqual(left, right);	}			
			else if (operationType == ExpressionType.NotEqual)			{	comparison = Expression.NotEqual(left, right);			}
			*/
			Func<T, T, bool> result = Expression.Lambda<Func<T, T, bool>>(comparison, left, right).Compile();
			return result;
		}

		private static Func<T, T> CreateSqrtFunction()
		{
			MethodInfo method;
			Type typeFromHandle = typeof(T);
			if (IsArithmeticValueType(typeFromHandle))
			{
				method = typeof(Math).GetMethod("Sqrt", BindingFlags.Static | BindingFlags.Public);
			}
			else
			{
				method = typeFromHandle.GetMethod("Sqrt", BindingFlags.Static | BindingFlags.Public);
			}

			if (method == null)
			{
				return SquareRootInternal;
			}

			ParameterExpression parameter = Expression.Parameter(typeFromHandle, "input");
			MethodCallExpression methodCall = Expression.Call(method, parameter);
			Func<T, T> result = Expression.Lambda<Func<T, T>>(methodCall, parameter).Compile();
			return result;
		}

		private static Func<T, int, T, T> CreateModPowFunction()
		{
			Type typeFromHandle = typeof(T);

			MethodInfo method = typeFromHandle.GetMethod("ModPower", BindingFlags.Static | BindingFlags.Public);
			if (method == null)
			{
				return ModPowInternal;
			}

			ParameterExpression val = Expression.Parameter(typeFromHandle, "value");
			ParameterExpression exp = Expression.Parameter(typeFromHandle, "exponent");
			ParameterExpression mod = Expression.Parameter(typeFromHandle, "modulus");
			MethodCallExpression methodCall = Expression.Call(method, val, exp, mod);
			Func<T, int, T, T> result = Expression.Lambda<Func<T, int, T, T>>(methodCall, val, exp, mod).Compile();
			return result;
		}

		/*
		private static Func<T, int, T> CreatePowerFunction()
		{
			ParameterExpression baseVal = Expression.Parameter(typeof(T), "baseValue");
			ParameterExpression exp = Expression.Parameter(typeof(int), "exponent");

			MethodInfo method = null;

			Type typeFromHandle = typeof(T);
			if (IsArithmeticValueType(typeFromHandle))
			{
				method = typeof(Math).GetMethod("Pow", BindingFlags.Static | BindingFlags.Public);
			}
			else
			{
				method = typeFromHandle.GetMethod("Pow", BindingFlags.Static | BindingFlags.Public);
			}

			if (method == null)
			{
				throw new NotSupportedException($"Cannot find public static method 'Pow' for type of {typeFromHandle.FullName}.");
			}

			MethodCallExpression methodCall = Expression.Call(method, baseVal, exp);
			Func<T, int, T> result = Expression.Lambda<Func<T, int, T>>(methodCall, baseVal, exp).Compile();
			return result;
		}
		*/

		private static Func<T, T, T> CreatePowerFunction()
		{
			Type typeOfT = typeof(T);

			ParameterExpression baseVal = Expression.Parameter(typeOfT, "baseValue");
			ParameterExpression exp = Expression.Parameter(typeOfT, "exponent");

			MethodInfo method = null;
			Expression methodCall = null;

			Type typeFromHandle = typeof(T);
			if (IsArithmeticValueType(typeFromHandle))
			{
				method = typeof(Math).GetMethod("Pow", BindingFlags.Static | BindingFlags.Public);

				Type returnType = method.ReturnType;

				ParameterInfo baseParameterInfo = method.GetParameters()[0];
				ParameterInfo expParameterInfo = method.GetParameters()[1];

				Expression baseParameterExpression = ConvertIfNeeded(baseVal, baseParameterInfo.ParameterType);
				Expression exponentParameterExpression = ConvertIfNeeded(exp, expParameterInfo.ParameterType);

				Expression methodCallExpression = Expression.Call(method, baseParameterExpression, exponentParameterExpression);

				methodCall = ConvertIfNeeded(methodCallExpression, typeOfT);
			}
			else
			{
				var methods = typeFromHandle.GetMethods(BindingFlags.Static | BindingFlags.Public);
				var powMethods = methods.Where(mi => mi.Name == "Pow").ToList();
				method = powMethods.FirstOrDefault();

				if (method != null)
				{
					Expression convertExpression = null;
					if (typeOfT == typeof(BigInteger))
					{
						convertExpression = Expression.Convert(exp, typeof(int), typeof(GenericArithmetic<T>).GetMethod("ConvertBigIntegerToInt", BindingFlags.Static | BindingFlags.NonPublic));
					}
					else
					{
						convertExpression = exp;
					}

					methodCall = Expression.Call(method, baseVal, convertExpression);
				}
			}

			if (method == null || methodCall == null)
			{
				throw new NotSupportedException($"Cannot find public static method 'Pow' for type of {typeFromHandle.FullName}.");
			}

			Func<T, T, T> result = Expression.Lambda<Func<T, T, T>>(methodCall, baseVal, exp).Compile();
			return result;
		}

		private static Func<T, T> CreateTruncateFunction()
		{
			MethodInfo method = null;
			Type typeFromHandle = typeof(T);

			if (typeFromHandle == typeof(double) || typeFromHandle == typeof(decimal))
			{
				method = typeof(Math).GetMethod("Truncate", new Type[] { typeof(T) });
			}
			else
			{
				method = typeFromHandle.GetMethod("Truncate", new Type[] { typeof(T) });
			}

			if (method == null)
			{
				return new Func<T, T>((arg) => arg);
			}

			ParameterExpression parameter = Expression.Parameter(typeFromHandle, "input");
			MethodCallExpression methodCall = Expression.Call(method, parameter);
			Func<T, T> result = Expression.Lambda<Func<T, T>>(methodCall, parameter).Compile();
			return result;
		}

		private static Func<string, T> CreateParseFunction()
		{
			Type typeFromHandle = typeof(T);

			MethodInfo[] methods = typeFromHandle.GetMethods(BindingFlags.Static | BindingFlags.Public);
			var filteredMethods = methods.Where(mi => mi.Name == "Parse" && mi.GetParameters().Count() == 1);

			MethodInfo method = null;
			if (typeFromHandle == typeof(Complex))
			{
				method = typeof(HelperMethods).GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
			}
			else
			{
				method = filteredMethods.FirstOrDefault();//GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
			}

			if (method == null)
			{

				throw new NotSupportedException($"Cannot find public static method 'Parse' for type of {typeFromHandle.FullName}.");
			}

			ParameterExpression parameter = Expression.Parameter(typeof(string), "input");
			MethodCallExpression methodCall = Expression.Call(method, parameter);
			Func<string, T> result = Expression.Lambda<Func<string, T>>(methodCall, parameter).Compile();
			return result;
		}

		private static Func<T, double, double> CreateLogFunction()
		{
			MethodInfo[] methods = null;

			Type typeFromHandle = typeof(T);
			if (IsArithmeticValueType(typeFromHandle))
			{
				methods = typeof(Math).GetMethods(BindingFlags.Static | BindingFlags.Public);
			}
			else
			{
				methods = typeFromHandle.GetMethods(BindingFlags.Static | BindingFlags.Public);
			}

			var filteredMethods = methods.Where(mi => mi.Name == "Log" && mi.GetParameters().Count() == 2);

			MethodInfo method = filteredMethods.FirstOrDefault();
			if (method == null)
			{
				throw new NotSupportedException($"No such method 'Log' on type {typeFromHandle.FullName}.");
			}

			ParameterExpression val = Expression.Parameter(typeFromHandle, "value");
			ParameterExpression baseVal = Expression.Parameter(typeof(double), "baseValue");
			MethodCallExpression methodCall = Expression.Call(method, val, baseVal);
			Func<T, double, double> result = Expression.Lambda<Func<T, double, double>>(methodCall, val, baseVal).Compile();
			return result;
		}

		private static Func<T, byte[]> CreateToBytesFunction()
		{
			Type typeFromHandle = typeof(T);
			if (IsArithmeticValueType(typeFromHandle))
			{
				MethodInfo method = typeof(BitConverter).GetMethod("GetBytes", BindingFlags.Static | BindingFlags.Public);
				ParameterExpression parameter = Expression.Parameter(typeFromHandle, "input");
				MethodCallExpression methodCall = Expression.Call(method, parameter);
				return Expression.Lambda<Func<T, byte[]>>(methodCall, parameter).Compile();
			}
			else
			{
				MethodInfo method = typeFromHandle.GetMethod("ToByteArray", BindingFlags.Public);
				if (method == null)
				{
					throw new NotSupportedException($"Cannot find suitable method to convert instance of type {typeFromHandle.FullName} to an array of bytes.");
				}
				MethodCallExpression methodCall = Expression.Call(method);
				return Expression.Lambda<Func<T, byte[]>>(methodCall).Compile();
			}
		}

		private static Expression ConvertIfNeeded(Expression valueExpression, Type targetType)
		{
			Type expressionType = null;
			if (valueExpression.NodeType == ExpressionType.Parameter)
			{
				expressionType = ((ParameterExpression)valueExpression).Type;
			}
			else if (valueExpression.NodeType == ExpressionType.Call)
			{
				expressionType = ((MethodCallExpression)valueExpression).Method.ReturnType;
			}

			if (expressionType != targetType)
			{
				return Expression.Convert(valueExpression, targetType);
			}
			return valueExpression;
		}
	}
}
