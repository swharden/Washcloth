using System;
using System.Linq;
using Washcloth.DataStructures;

namespace Washcloth
{
	/// <summary>Root type of the static functional methods in Towel.</summary>
	public static partial class Statics
	{
		#region Keywords

		/// <summary>Stepper was not broken.</summary>
		public const StepStatus Continue = StepStatus.Continue;
		/// <summary>Stepper was broken.</summary>
		public const StepStatus Break = StepStatus.Break;
		/// <summary>The left operand is less than the right operand.</summary>
		public const CompareResult Less = CompareResult.Less;
		/// <summary>The left operand is equal to the right operand.</summary>
		public const CompareResult Equal = CompareResult.Equal;
		/// <summary>The left operand is greater than the right operand.</summary>
		public const CompareResult Greater = CompareResult.Greater;
		/// <summary>The default case in a Switch statement (true).</summary>
		public const SwitchSyntax.Keyword Default = SwitchSyntax.Keyword.Default;

		#endregion

		#region Switch

		/// <summary>Syntax sugar Switch statements.</summary>
		/// <param name="possibleActions">The possible actions of the Switch statement.</param>
		public static void Switch(params (SwitchSyntax.Condition, Action)[] possibleActions) =>
			SwitchSyntax.Do(possibleActions);

		/// <summary>Syntax sugar Switch statements.</summary>
		/// <typeparam name="T">The generic type parameter to the Switch statement.</typeparam>
		/// <param name="value">The value argument of the Switch statement.</param>
		/// <returns>The delegate for the Switch statement.</returns>
		public static SwitchSyntax.ParamsAction<SwitchSyntax.Condition<T>, Action> Switch<T>(T value) =>
			SwitchSyntax.Do<T>(value);

		/// <summary>Definitions for Switch syntax.</summary>
		public static class SwitchSyntax
		{
			/// <summary>Delegate with params intended to be used with the Switch syntax.</summary>
			public delegate void ParamsAction<A, B>(params (A, B)[] values);

			internal static ParamsAction<Condition<T>, Action> Do<T>(T value) =>
				possibleActions =>
				{
					foreach (var possibleAction in possibleActions)
					{
						if (possibleAction.Item1.Resolve(value))
						{
							possibleAction.Item2();
							return;
						}
					}
				};

			internal static void Do(params (Condition Condition, Action Action)[] possibleActions)
			{
				foreach (var possibleAction in possibleActions)
				{
					if (possibleAction.Condition)
					{
						possibleAction.Action();
						return;
					}
				}
			}

			/// <summary>Intended to be used with Switch syntax.</summary>
			public enum Keyword
			{
				/// <summary>The default keyword for the the Switch syntax.</summary>
				Default,
			}

			/// <summary>Represents the result of a conditional expression inside Switch syntax.</summary>
			/// <typeparam name="T">The generic type of the Switch condition for equality checks.</typeparam>
			public abstract class Condition<T>
			{
				/// <summary>Resolves the condition to a bool.</summary>
				public abstract bool Resolve(T b);
				/// <summary>Casts a <typeparamref name="T"/> to a bool using an equality check.</summary>
				public static implicit operator Condition<T>(T value) => new Value<T>(a: value);
				/// <summary>Uses the bool as the condition result.</summary>
				public static implicit operator Condition<T>(bool result) => new Bool<T> { Result = result, };
				/// <summary>Converts a keyword to a condition result (for "Default" case).</summary>
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0060 // Remove unused parameter
				public static implicit operator Condition<T>(Keyword keyword) => new Default<T>();
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0079 // Remove unnecessary suppression
			}

			internal class Value<T> : Condition<T>
			{
				/// <summary>The value of this condition for an equality check.</summary>
				internal T A;
				public override bool Resolve(T b) => Equate(A, b);
				public Value(T a)
				{
					A = a;
				}
			}

			internal class Bool<T> : Condition<T>
			{
				internal bool Result;
				public override bool Resolve(T b) => Result;
			}

			internal class Default<T> : Condition<T>
			{
				public override bool Resolve(T b) => true;
			}

			/// <summary>Represents the result of a conditional expression inside Switch syntax.</summary>
			public abstract class Condition
			{
				/// <summary>Resolves the condition to a bool.</summary>
				public abstract bool Resolve();
				/// <summary>Uses the bool as the condition result.</summary>
				public static implicit operator Condition(bool result) => new Bool { Result = result, };
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0060 // Remove unused parameter
				/// <summary>Converts a keyword to a condition result (for "Default" case).</summary>
				public static implicit operator Condition(Keyword keyword) => new Default();
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0079 // Remove unnecessary suppression
				/// <summary>Converts a condition to a bool using the Resolve method.</summary>
				public static implicit operator bool(Condition condition) => condition.Resolve();
			}

			internal class Bool : Condition
			{
				internal bool Result;
				public override bool Resolve() => Result;
			}

			internal class Default : Condition
			{
				public override bool Resolve() => true;
			}
		}

		#endregion

		#region Chance

#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0075 // Simplify conditional expression
#pragma warning disable IDE0060 // Remove unused parameter

		/// <summary>Allows chance syntax with "using static Towel.Syntax;".</summary>
		/// <example>25% Chance</example>
		public static ChanceSyntax Chance => default;

		/// <summary>Struct that allows percentage syntax that will be evaluated at runtime.</summary>
		public struct ChanceSyntax
		{
			/// <summary>The random algorithm currently being used by chance syntax.</summary>
			public static Random Algorithm = new Random();

			/// <summary>Creates a chance from a percentage that will be evaluated at runtime.</summary>
			/// <param name="percentage">The value of the percentage.</param>
			/// <param name="chance">The chance syntax struct object.</param>
			/// <returns>True if the the chance hits. False if not.</returns>
			public static bool operator %(double percentage, ChanceSyntax chance) =>
				percentage < 0d ? throw new ArgumentOutOfRangeException(nameof(chance)) :
				percentage > 100d ? throw new ArgumentOutOfRangeException(nameof(chance)) :
				percentage is 100d ? true :
				percentage is 0d ? false :
				Algorithm.NextDouble() < percentage / 100d;
		}

#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0075 // Simplify conditional expression
#pragma warning restore IDE0079 // Remove unnecessary suppression
#pragma warning restore CA2211 // Non-constant fields should not be visible

		#endregion

		#region Inequality

		/// <summary>Used for inequality syntax.</summary>
		/// <typeparam name="T">The generic type of elements the inequality is being used on.</typeparam>
		public struct Inequality<T>
		{
			internal bool Cast;
			internal T A;

			/// <summary>Contructs a new <see cref="Inequality{T}"/>.</summary>
			/// <param name="a">The initial value of the running inequality.</param>
			public static implicit operator Inequality<T>(T a) =>
				new Inequality<T>()
				{
					Cast = true,
					A = a,
				};
			/// <summary>Adds a greater than operation to a running inequality.</summary>
			/// <param name="a">The current running inequality and left hand operand.</param>
			/// <param name="b">The value of the right hand operand of the greater than operation.</param>
			/// <returns>A running inequality with the additonal greater than operation.</returns>
			public static OperatorValidated.Inequality<T> operator >(Inequality<T> a, T b) =>
				!a.Cast ? throw new InequalitySyntaxException() :
				new OperatorValidated.Inequality<T>(Compare(a.A, b) == Greater, b);
			/// <summary>Adds a less than operation to a running inequality.</summary>
			/// <param name="a">The current running inequality and left hand operand.</param>
			/// <param name="b">The value of the right hand operand of the less than operation.</param>
			/// <returns>A running inequality with the additonal less than operation.</returns>
			public static OperatorValidated.Inequality<T> operator <(Inequality<T> a, T b) =>
				!a.Cast ? throw new InequalitySyntaxException() :
				new OperatorValidated.Inequality<T>(Compare(a.A, b) == Less, b);
			/// <summary>Adds a greater than or equal operation to a running inequality.</summary>
			/// <param name="a">The current running inequality and left hand operand.</param>
			/// <param name="b">The value of the right hand operand of the greater than or equal operation.</param>
			/// <returns>A running inequality with the additonal greater than or equal operation.</returns>
			public static OperatorValidated.Inequality<T> operator >=(Inequality<T> a, T b) =>
				!a.Cast ? throw new InequalitySyntaxException() :
				new OperatorValidated.Inequality<T>(Compare(a.A, b) != Less, b);
			/// <summary>Adds a less than or equal operation to a running inequality.</summary>
			/// <param name="a">The current running inequality and left hand operand.</param>
			/// <param name="b">The value of the right hand operand of the less than or equal operation.</param>
			/// <returns>A running inequality with the additonal less than or equal operation.</returns>
			public static OperatorValidated.Inequality<T> operator <=(Inequality<T> a, T b) =>
				!a.Cast ? throw new InequalitySyntaxException() :
				new OperatorValidated.Inequality<T>(Compare(a.A, b) != Greater, b);
			/// <summary>Adds an equal operation to a running inequality.</summary>
			/// <param name="a">The current running inequality and left hand operand.</param>
			/// <param name="b">The value of the right hand operand of the equal operation.</param>
			/// <returns>A running inequality with the additonal equal operation.</returns>
			public static OperatorValidated.Inequality<T> operator ==(Inequality<T> a, T b) =>
				!a.Cast ? throw new InequalitySyntaxException() :
				new OperatorValidated.Inequality<T>(Equate(a.A, b), b);
			/// <summary>Adds an inequal operation to a running inequality.</summary>
			/// <param name="a">The current running inequality and left hand operand.</param>
			/// <param name="b">The value of the right hand operand of the inequal operation.</param>
			/// <returns>A running inequality with the additonal inequal operation.</returns>
			public static OperatorValidated.Inequality<T> operator !=(Inequality<T> a, T b) =>
				!a.Cast ? throw new InequalitySyntaxException() :
				new OperatorValidated.Inequality<T>(Inequate(a.A, b), b);

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
			/// <summary>This member is not intended to be invoked.</summary>
			/// <returns>This member is not intended to be invoked.</returns>
			
			public override string ToString() => throw new InequalitySyntaxException();
			/// <summary>This member is not intended to be invoked.</summary>
			/// <param name="obj">This member is not intended to be invoked.</param>
			/// <returns>This member is not intended to be invoked.</returns>
			
			public override bool Equals(object? obj) => throw new InequalitySyntaxException();
			/// <summary>This member is not intended to be invoked.</summary>
			/// <returns>This member is not intended to be invoked.</returns>
			
			public override int GetHashCode() => throw new InequalitySyntaxException();
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
		}

		/// <summary>Helper type for inequality syntax. Contains an Inequality type that has been operator validated.</summary>
		public static partial class OperatorValidated
		{
			/// <summary>Used for inequality syntax.</summary>
			/// <typeparam name="T">The generic type of elements the inequality is being used on.</typeparam>
			public struct Inequality<T>
			{
				internal readonly bool Result;
				internal readonly T A;

				internal Inequality(bool result, T a)
				{
					Result = result;
					A = a;
				}
				/// <summary>Converts this running inequality into the result of the expression.</summary>
				/// <param name="inequality">The inequality to convert into the result of the expression.</param>
				public static implicit operator bool(Inequality<T> inequality) =>
					inequality.Result;
				/// <summary>Adds a greater than operation to a running inequality.</summary>
				/// <param name="a">The current running inequality and left hand operand.</param>
				/// <param name="b">The value of the right hand operand of the greater than operation.</param>
				/// <returns>A running inequality with the additonal greater than operation.</returns>
				public static Inequality<T> operator >(Inequality<T> a, T b) =>
					new Inequality<T>(a.Result && Compare(a.A, b) == Greater, b);
				/// <summary>Adds a less than operation to a running inequality.</summary>
				/// <param name="a">The current running inequality and left hand operand.</param>
				/// <param name="b">The value of the right hand operand of the less than operation.</param>
				/// <returns>A running inequality with the additonal less than operation.</returns>
				public static Inequality<T> operator <(Inequality<T> a, T b) =>
					new Inequality<T>(a.Result && Compare(a.A, b) == Less, b);
				/// <summary>Adds a greater than or equal operation to a running inequality.</summary>
				/// <param name="a">The current running inequality and left hand operand.</param>
				/// <param name="b">The value of the right hand operand of the greater than or equal operation.</param>
				/// <returns>A running inequality with the additonal greater than or equal operation.</returns>
				public static Inequality<T> operator >=(Inequality<T> a, T b) =>
					new Inequality<T>(a.Result && Compare(a.A, b) != Less, b);
				/// <summary>Adds a less than or equal operation to a running inequality.</summary>
				/// <param name="a">The current running inequality and left hand operand.</param>
				/// <param name="b">The value of the right hand operand of the less than or equal operation.</param>
				/// <returns>A running inequality with the additonal less than or equal operation.</returns>
				public static Inequality<T> operator <=(Inequality<T> a, T b) =>
					new Inequality<T>(a.Result && Compare(a.A, b) != Greater, b);
				/// <summary>Adds an equal operation to a running inequality.</summary>
				/// <param name="a">The current running inequality and left hand operand.</param>
				/// <param name="b">The value of the right hand operand of the equal operation.</param>
				/// <returns>A running inequality with the additonal equal operation.</returns>
				public static Inequality<T> operator ==(Inequality<T> a, T b) =>
					new Inequality<T>(a.Result && Equate(a.A, b), b);
				/// <summary>Adds an inequal operation to a running inequality.</summary>
				/// <param name="a">The current running inequality and left hand operand.</param>
				/// <param name="b">The value of the right hand operand of the inequal operation.</param>
				/// <returns>A running inequality with the additonal inequal operation.</returns>
				public static Inequality<T> operator !=(Inequality<T> a, T b) =>
					new Inequality<T>(a.Result && Inequate(a.A, b), b);
				/// <summary>Converts the result of this inequality to a <see cref="string"/>.</summary>
				/// <returns>The result of this inequality converted to a <see cref="string"/>.</returns>
				public override string ToString() => Result.ToString();
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
				/// <summary>This member is not intended to be invoked.</summary>
				/// <param name="obj">This member is not intended to be invoked.</param>
				/// <returns>This member is not intended to be invoked.</returns>
				
				public override bool Equals(object? obj) => throw new InequalitySyntaxException();
				/// <summary>This member is not intended to be invoked.</summary>
				/// <returns>This member is not intended to be invoked.</returns>
				
				public override int GetHashCode() => throw new InequalitySyntaxException();
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
			}
		}

		#endregion

		#region UniversalQuantification


		#endregion
	}
}
