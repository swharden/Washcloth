using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Washcloth
{
    public static partial class Statics
    {

        /// <summary>Universal Quantification Operator.</summary>
        /// <typeparam name="T">The element type of the universal quantification to declare.</typeparam>
        /// <param name="values">The values of the universal quantification.</param>
        /// <returns>The declared universal quantification.</returns>
        public static UniversalQuantification<T> Quant<T>(params T[] values) => new UniversalQuantification<T>(values);

        /// <summary>Universal Quantification.</summary>
        /// <typeparam name="T">The element type of the universal quantification.</typeparam>
        public struct UniversalQuantification<T> :
            System.Collections.Generic.IEnumerable<T>,
            System.Collections.Generic.IList<T>
        {
            internal T[] Value;

            /// <summary>Constructs a new universal quantification from an array.</summary>
            /// <param name="array">The array value of the universal quantification.</param>
            internal UniversalQuantification(T[] array) => Value = array;

            #region Towel.Datastructures.IArray<T>
            /// <summary>The number of values in this universal quantification.</summary>

            public int Length => Value.Length;
            /// <summary>Iterates each value in this universal quantification and performs an action for each element.</summary>
            /// <param name="step">The action to perform on every step of the iteration.</param>

            public void Stepper(Action<T> step) => Value.Stepper(step);
            /// <summary>Iterates each value in this universal quantification and performs an action for each element.</summary>

            public StepStatus Stepper(Func<T, StepStatus> step) => Value.Stepper(step);
            #endregion

            #region System.Collections.Generic.IList<T>
            /// <summary>Index property for get/set operations.</summary>
            /// <param name="index">The index to get/set.</param>
            /// <returns>The value at the provided index.</returns>

            public T this[int index]
            {
                get => Value[index];
                set => Value[index] = value;
            }
            /// <summary>Gets the number of elements in this universal quantification.</summary>
            public int Count => Value.Length;
            /// <summary>Gets a value indicating whether the <see cref="System.Collections.Generic.ICollection{T}"/> is read-only.</summary>
            public bool IsReadOnly => false;
            /// <summary>Adds an item to this universal quantifier.</summary>
            /// <param name="item">The item to add to this universal quantifier.</param>

            public void Add(T item)
            {
                T[] newValue = new T[Value.Length + 1];
                Array.Copy(Value, newValue, Value.Length);
                newValue[Value.Length] = item;
                Value = newValue;
            }
            /// <summary>Not intended to be invoked directly.</summary>

            public void Clear() => Value = Array.Empty<T>();
            /// <summary>Not intended to be invoked directly.</summary>

            public bool Contains(T item) => Value.Contains(item);
            /// <summary>Not intended to be invoked directly.</summary>

            public void CopyTo(T[] array, int arrayIndex) =>
                Array.Copy(Value, 0, array, arrayIndex, Value.Length);
            /// <summary>Not intended to be invoked directly.</summary>

            public int IndexOf(T item) => Array.IndexOf(Value, item);
            /// <summary>Not intended to be invoked directly.</summary>

            public void Insert(int index, T item)
            {
                T[] newValue = new T[Value.Length + 1];
                for (int i = 0; i < newValue.Length; i++)
                {
                    newValue[i] = i == index
                        ? item
                        : i < index
                            ? Value[i]
                            : Value[i - 1];
                }
                Value = newValue;
            }
            /// <summary>Not intended to be invoked directly.</summary>

            public bool Remove(T item)
            {
                T[] newValue = new T[Value.Length - 1];
                bool found = false;
                for (int i = 0; i < Value.Length; i++)
                {
                    if (Equate(Value[i], item))
                    {
                        found = true;
                    }
                    else if (found)
                    {
                        newValue[i] = Value[i - 1];
                    }
                    else
                    {
                        newValue[i] = Value[i];
                    }
                }
                if (!found)
                {
                    return false;
                }
                Value = newValue;
                return true;
            }
            /// <summary>Not intended to be invoked directly.</summary>

            public void RemoveAt(int index)
            {
                T[] newValue = new T[Value.Length - 1];
                for (int i = 0; i < Value.Length; i++)
                {
                    if (i != index)
                    {
                        if (i < index)
                        {
                            newValue[i] = Value[i];
                        }
                        else
                        {
                            newValue[i] = Value[i - 1];
                        }
                    }
                }
                Value = newValue;
            }
            #endregion

            #region System.Collections.Generic.IEnumerable<T>
            /// <summary>Gets the <see cref="System.Collections.Generic.IEnumerator{T}"/> for this universal quantification.</summary>
            /// <returns>The <see cref="System.Collections.Generic.IEnumerator{T}"/> for this universal quantification.</returns>

            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => ((System.Collections.Generic.IEnumerable<T>)Value).GetEnumerator();
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => Value.GetEnumerator();
            #endregion

            #region Implicit Casting Operators

            /// <summary>Converts a universal quantification to an array.</summary>
            /// <param name="universalQuantification">The universal quantification to be converted.</param>
            public static implicit operator T[](UniversalQuantification<T> universalQuantification) => universalQuantification.Value;
            /// <summary>Converts a universal quantification to a <see cref="System.Collections.Generic.List{T}"/>.</summary>
            /// <param name="universalQuantification">The universal quantification to be converted.</param>
            public static implicit operator System.Collections.Generic.List<T>(UniversalQuantification<T> universalQuantification) => new System.Collections.Generic.List<T>(universalQuantification.Value);
            /// <summary>Converts a universal quantification to an <see cref="System.Collections.Generic.HashSet{T}"/>.</summary>
            /// <param name="universalQuantification">The universal quantification to be converted.</param>
            public static implicit operator System.Collections.Generic.HashSet<T>(UniversalQuantification<T> universalQuantification) => new System.Collections.Generic.HashSet<T>(universalQuantification.Value);
            /// <summary>Converts a universal quantification to a <see cref="System.Collections.Generic.LinkedList{T}"/>.</summary>
            /// <param name="universalQuantification">The universal quantification to be converted.</param>
            public static implicit operator System.Collections.Generic.LinkedList<T>(UniversalQuantification<T> universalQuantification) => new System.Collections.Generic.LinkedList<T>(universalQuantification.Value);
            /// <summary>Converts a universal quantification to an <see cref="System.Collections.Generic.Stack{T}"/>.</summary>
            /// <param name="universalQuantification">The universal quantification to be converted.</param>
            public static implicit operator System.Collections.Generic.Stack<T>(UniversalQuantification<T> universalQuantification) => new System.Collections.Generic.Stack<T>(universalQuantification.Value);
            /// <summary>Converts a universal quantification to an <see cref="System.Collections.Generic.Queue{T}"/>.</summary>
            /// <param name="universalQuantification">The universal quantification to be converted.</param>
            public static implicit operator System.Collections.Generic.Queue<T>(UniversalQuantification<T> universalQuantification) => new System.Collections.Generic.Queue<T>(universalQuantification.Value);
            /// <summary>Converts a universal quantification to a sorted <see cref="System.Collections.Generic.SortedSet{T}"/>.</summary>
            /// <param name="universalQuantification">The universal quantification to be converted.</param>
            public static implicit operator System.Collections.Generic.SortedSet<T>(UniversalQuantification<T> universalQuantification) => new System.Collections.Generic.SortedSet<T>(universalQuantification.Value);
            /// <summary>Converts a universal quantification to an Action&lt;Action&lt;T&gt;&gt;.</summary>
            /// <param name="universalQuantification">The universal quantification to be converted.</param>
            public static implicit operator Action<Action<T>>(UniversalQuantification<T> universalQuantification) => universalQuantification.Value.ToStepper();
            /// <summary>Converts a universal quantification to an <see cref="StepperRef{T}"/>.</summary>
            /// <param name="universalQuantification">The universal quantification to be converted.</param>
            public static implicit operator StepperRef<T>(UniversalQuantification<T> universalQuantification) => universalQuantification.Value.ToStepperRef();
            /// <summary>Converts a universal quantification to an Func&lt;Func&lt;T, StepStatus&gt;, StepStatus&gt;.</summary>
            /// <param name="universalQuantification">The universal quantification to be converted.</param>
            public static implicit operator Func<Func<T, StepStatus>, StepStatus>(UniversalQuantification<T> universalQuantification) => universalQuantification.Value.ToStepperBreak();
            /// <summary>Converts a universal quantification to an <see cref="StepperRefBreak{T}"/>.</summary>
            /// <param name="universalQuantification">The universal quantification to be converted.</param>
            public static implicit operator StepperRefBreak<T>(UniversalQuantification<T> universalQuantification) => universalQuantification.Value.ToStepperRefBreak();
            #endregion
        }
    }
}
