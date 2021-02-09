﻿using System;
using static Washcloth.Statics;

namespace Washcloth.DataStructures
{
	/// <summary>An unsorted structure of unique items.</summary>
	/// <typeparam name="T">The type of values to store in the set.</typeparam>
	public interface ISet<T> : IDataStructure<T>,
		// Structure Properties
		DataStructure.IAuditable<T>,
		DataStructure.IAddable<T>,
		DataStructure.IRemovable<T>,
		DataStructure.ICountable,
		DataStructure.IClearable,
		DataStructure.IEquating<T>
	{
	}

	/// <summary>An unsorted structure of unique items implemented as a hashed table of linked lists.</summary>
	/// <typeparam name="T">The type of values to store in the set.</typeparam>
	/// <typeparam name="Equate">The function for equality comparing values.</typeparam>
	/// <typeparam name="Hash">The function for computing hash codes.</typeparam>
	public class SetHashLinked<T, Equate, Hash> : ISet<T>,
		// Structure Properties
		DataStructure.IHashing<T>
		where Equate : struct, IFunc<T, T, bool>
		where Hash : struct, IFunc<T, int>
	{
		internal const float _maxLoadFactor = .7f;
		internal const float _minLoadFactor = .3f;

		internal Equate _equate;
		internal Hash _hash;
		internal Node?[] _table;
		internal int _count;

		#region Node

		internal class Node
		{
			internal T Value;
			internal Node? Next;

			internal Node(T value, Node? next = null)
			{
				Value = value;
				Next = next;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a hashed set.
		/// <para>Runtime: O(1)</para>
		/// </summary>
		/// <param name="equate">The equate delegate.</param>
		/// <param name="hash">The hashing function.</param>
		/// <param name="expectedCount">The expected count of the set.</param>
		public SetHashLinked(
			Equate equate = default,
			Hash hash = default,
			int? expectedCount = null)
		{
			if (expectedCount.HasValue && expectedCount.Value > 0)
			{
				int tableSize = (int)(expectedCount.Value * (1 / _maxLoadFactor));
				while (!IsPrime(tableSize))
				{
					tableSize++;
				}
				_table = new Node[tableSize];
			}
			else
			{
				_table = new Node[2];
			}
			_equate = equate;
			_hash = hash;
			_count = 0;
		}

		/// <summary>
		/// This constructor is for cloning purposes.
		/// <para>Runtime: O(n)</para>
		/// </summary>
		/// <param name="set">The set to clone.</param>
		internal SetHashLinked(SetHashLinked<T, Equate, Hash> set)
		{
			_equate = set._equate;
			_hash = set._hash;
			_table = (Node[])set._table.Clone();
			_count = set._count;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The current size of the hashed table.
		/// <para>Runtime: O(1)</para>
		/// </summary>
		public int TableSize => _table.Length;

		/// <summary>
		/// The current number of values in the set.
		/// <para>Runtime: O(1)</para>
		/// </summary>
		public int Count => _count;

		/// <summary>
		/// The delegate for computing hash codes.
		/// <para>Runtime: O(1)</para>
		/// </summary>
		Func<T, int> DataStructure.IHashing<T>.Hash =>
			_hash is FuncRuntime<T, int> hash
				? hash._delegate
				: _hash.Do;

		/// <summary>
		/// The delegate for equality checking.
		/// <para>Runtime: O(1)</para>
		/// </summary>
		Func<T, T, bool> DataStructure.IEquating<T>.Equate =>
			_equate is FuncRuntime<T, T, bool> func
			? func._delegate
			: _equate.Do;

		#endregion

		#region Methods

		internal int GetLocation(T value) =>
			(_hash.Do(value) & int.MaxValue) % _table.Length;

		/// <summary>
		/// Adds a value to the set.
		/// <para>Runtime: O(n), Ω(1), ε(1)</para>
		/// </summary>
		/// <param name="value">The value to add to the set.</param>
		/// <param name="exception">The exception that occurred if the add failed.</param>
		public bool TryAdd(T value, out Exception? exception)
		{
			int location = GetLocation(value);
			for (Node? node = _table[location]; node is not null; node = node.Next)
			{
				if (_equate.Do(node.Value, value))
				{
					exception = new ArgumentException("Attempting to add a duplicate value to a set.", nameof(value));
					return false;
				}
			}
			_table[location] = new Node(value: value, next: _table[location]);
			if (++_count > _table.Length * _maxLoadFactor)
			{
				float tableSizeFloat = (_count * 2) * (1 / _maxLoadFactor);
				if (tableSizeFloat <= int.MaxValue)
				{
					int tableSize = (int)tableSizeFloat;
					while (!IsPrime(tableSize))
					{
						tableSize++;
					}
					Resize(tableSize);
				}
			}

			exception = null;
			return true;
		}

		/// <summary>Tries to remove a value from the set.</summary>
		/// <param name="value">The value to remove.</param>
		/// <param name="exception">The exception that occurred if the remove failed.</param>
		/// <returns>True if the remove was successful or false if not.</returns>
		public bool TryRemove(T value, out Exception? exception)
		{
			if (TryRemoveWithoutTrim(value, out exception))
			{
				if (_table.Length > 2 && _count < _table.Length * _minLoadFactor)
				{
					int tableSize = (int)(_count * (1 / _maxLoadFactor));
					while (!IsPrime(tableSize))
					{
						tableSize++;
					}
					Resize(tableSize);
				}
				return true;
			}
			return false;
		}

		/// <summary>Tries to remove a value from the set without shrinking the hash table.</summary>
		/// <param name="value">The value to remove.</param>
		/// <param name="exception">The exception that occurred if the remove failed.</param>
		/// <returns>True if the remove was successful or false if not.</returns>
		public bool TryRemoveWithoutTrim(T value, out Exception? exception)
		{
			int location = GetLocation(value);
			for (Node? node = _table[location], previous = null; node is not null; previous = node, node = node.Next)
			{
				if (_equate.Do(node.Value, value))
				{
					if (previous is null)
					{
						_table[location] = node.Next;
					}
					else
					{
						previous.Next = node.Next;
					}
					_count--;
					exception = null;
					return true;
				}
			}
			exception = new ArgumentException("Attempting to remove a value that is no in a set.", nameof(value));
			return false;
		}

		/// <summary>Resizes the table.</summary>
		/// <param name="tableSize">The desired size of the table.</param>
		internal void Resize(int tableSize)
		{
			if (tableSize == _table.Length)
			{
				return;
			}
			Node?[] temp = _table;
			_table = new Node[tableSize];
			for (int i = 0; i < temp.Length; i++)
			{
				for (Node? node = temp[i]; node is not null; node = temp[i])
				{
					temp[i] = node.Next;
					int location = GetLocation(node.Value);
					node.Next = _table[location];
					_table[location] = node;
				}
			}
		}

		/// <summary>
		/// Trims the table to an appropriate size based on the current count.
		/// <para>Runtime: O(n), Ω(1)</para>
		/// </summary>
		public void Trim()
		{
			int tableSize = _count;
			while (!IsPrime(tableSize))
			{
				tableSize++;
			}
			Resize(tableSize);
		}

		/// <summary>
		/// Creates a shallow clone of this set.
		/// <para>Runtime: Θ(n)</para>
		/// </summary>
		/// <returns>A shallow clone of this set.</returns>
		public SetHashLinked<T, Equate, Hash> Clone() => new SetHashLinked<T, Equate, Hash>(this);

		/// <summary>
		/// Determines if a value has been added to a set.
		/// <para>Runtime: O(n), Ω(1), ε(1)</para>
		/// </summary>
		/// <param name="value">The value to look for in the set.</param>
		/// <returns>True if the value has been added to the set or false if not.</returns>
		public bool Contains(T value)
		{
			int location = GetLocation(value);
			for (Node? node = _table[location]; node is not null; node = node.Next)
			{
				if (_equate.Do(node.Value, value))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Removes all the values in the set.
		/// <para>Runtime: O(1)</para>
		/// </summary>
		public void Clear()
		{
			_table = new Node[2];
			_count = 0;
		}

		/// <inheritdoc cref="DataStructure.Stepper_O_n_step_XML"/>
		public void Stepper(Action<T> step)
		{
			for (int i = 0; i < _table.Length; i++)
			{
				for (Node? node = _table[i]; node is not null; node = node.Next)
				{
					step(node.Value);
				}
			}
		}

		/// <inheritdoc cref="DataStructure.Stepper_O_n_step_XML"/>
		public StepStatus Stepper(Func<T, StepStatus> step)
		{
			for (int i = 0; i < _table.Length; i++)
			{
				for (Node? node = _table[i]; node is not null; node = node.Next)
				{
					if (step(node.Value) is Break)
					{
						return Break;
					}
				}
			}
			return Continue;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

		/// <summary>Gets the enumerator for the set.</summary>
		/// <returns>The enumerator for the set.</returns>
		public System.Collections.Generic.IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < _table.Length; i++)
			{
				for (Node? node = _table[i]; node is not null; node = node.Next)
				{
					yield return node.Value;
				}
			}
		}

		/// <summary>
		/// Puts all the values in this set into an array.
		/// <para>Runtime: Θ(<see cref="Count"/> + <see cref="TableSize"/>)</para>
		/// </summary>
		/// <returns>An array with all the values in the set.</returns>
		public T[] ToArray()
		{
			T[] array = new T[_count];
			for (int i = 0, index = 0; i < _table.Length; i++)
			{
				for (Node? node = _table[i]; node is not null; node = node.Next, index++)
				{
					array[index] = node.Value;
				}
			}
			return array;
		}

		#endregion
	}

	/// <summary>An unsorted structure of unique items implemented as a hashed table of linked lists.</summary>
	/// <typeparam name="T">The type of values to store in the set.</typeparam>
	public class SetHashLinked<T> : SetHashLinked<T, FuncRuntime<T, T, bool>, FuncRuntime<T, int>>
	{
		#region Constructors

		/// <summary>
		/// Constructs a hashed set.
		/// <para>Runtime: O(1)</para>
		/// </summary>
		/// <param name="equate">The equate delegate.</param>
		/// <param name="hash">The hashing function.</param>
		/// <param name="expectedCount">The expected count of the set.</param>
		public SetHashLinked(
			Func<T, T, bool>? equate = null,
			Func<T, int>? hash = null,
			int? expectedCount = null) : base(equate ?? Statics.Equate, hash ?? DefaultHash, expectedCount) { }

		/// <summary>
		/// This constructor is for cloning purposes.
		/// <para>Runtime: O(n)</para>
		/// </summary>
		/// <param name="set">The set to clone.</param>
		internal SetHashLinked(SetHashLinked<T> set)
		{
			_equate = set._equate;
			_hash = set._hash;
			_table = (Node[])set._table.Clone();
			_count = set._count;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The delegate for computing hash codes.
		/// <para>Runtime: O(1)</para>
		/// </summary>
		public Func<T, int> Hash => _hash._delegate;

		/// <summary>
		/// The delegate for equality checking.
		/// <para>Runtime: O(1)</para>
		/// </summary>
		public Func<T, T, bool> Equate => _equate._delegate;

		#endregion

		#region Clone

		/// <summary>
		/// Creates a shallow clone of this set.
		/// <para>Runtime: Θ(n)</para>
		/// </summary>
		/// <returns>A shallow clone of this set.</returns>
		public new SetHashLinked<T> Clone() => new SetHashLinked<T>(this);

		#endregion
	}
}
