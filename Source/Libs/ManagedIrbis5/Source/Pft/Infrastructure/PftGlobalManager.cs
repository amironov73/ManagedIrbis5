// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftGlobalManager.cs -- global variable manager
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    ///
    /// </summary>

    public sealed class PftGlobalManager
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Dictionary holding all the global variables.
        /// </summary>
        public Dictionary<int, PftGlobal> Registry { get; private set; }

        /// <summary>
        /// Получение значения глобальной переменной по её индексу
        /// в строковом представлении. Если такой переменной нет,
        /// возвращается пустая строка.
        /// </summary>
        public string? this[int index]
        {
                get => Registry.TryGetValue(index, out var result)
                        ? result.ToString()
                        : string.Empty;
                set
            {
                if (ReferenceEquals(value, null))
                {
                    Registry.Remove(index);
                }
                else
                {
                    PftGlobal variable = new PftGlobal
                        (
                            index,
                            value
                        );
                    Registry[index] = variable;
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftGlobalManager()
        {
            Registry = new Dictionary<int, PftGlobal>();
        }

        #endregion

        #region Private members

        private PftGlobal _GetOrCreate
            (
                int index
            )
        {
            if (!Registry.TryGetValue(index, out var result))
            {
                result = new PftGlobal(index);
                Registry.Add(index, result);
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add the variable.
        /// </summary>
        public PftGlobalManager Add
            (
                int index,
                string? text
            )
        {
            this[index] = text;

            return this;
        }

        /// <summary>
        /// Append the variable.
        /// </summary>
        public PftGlobalManager Append
            (
                int index,
                string? text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                PftGlobal variable = _GetOrCreate(index);
                string[] lines = text.SplitLines()
                    .NonEmptyLines()
                    .ToArray();
                foreach (string line in lines)
                {
                    variable.ParseLine(line);
                }
            }

            return this;
        }

        /// <summary>
        /// Clear all the variables.
        /// </summary>
        public PftGlobalManager Clear()
        {
            Registry.Clear();

            return this;
        }

        /// <summary>
        /// Delete global variable with specified index.
        /// </summary>
        public PftGlobalManager Delete
            (
                int index
            )
        {
            Registry.Remove(index);

            return this;
        }

        /// <summary>
        /// Get fields for global variable with specified index.
        /// </summary>
        public Field[] Get
            (
                int index
            )
        {
            if (Registry.TryGetValue(index, out PftGlobal? variable))
            {
                return variable.Fields
                    .Select(f => f.Clone())
                    .ToArray();
            }

            return Array.Empty<Field>();
        }

        /// <summary>
        /// Get all variables.
        /// </summary>
        public PftGlobal[] GetAllVariables()
        {
            PftGlobal[] result = Registry.Values.ToArray();

            return result;
        }

        /// <summary>
        /// Have global variable with specified index?
        /// </summary>
        public bool HaveVariable
            (
                int index
            )
        {
            return Registry.ContainsKey(index);
        }

        /// <summary>
        /// Set the global variable.
        /// </summary>
        public void Set
            (
                int index,
                IEnumerable<Field>? fields
            )
        {
            if (ReferenceEquals(fields, null))
            {
                Delete(index);

                return;
            }

            var array = fields.ToArray();
            if (array.Length == 0)
            {
                Delete(index);

                return;
            }

            var variable = _GetOrCreate(index);
            variable.Fields.Clear();
            variable.Fields.AddRange(array);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        void IHandmadeSerializable.RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Clear();
            PftGlobal[] values = reader.ReadArray<PftGlobal>();
            foreach (PftGlobal value in values)
            {
                Registry.Add(value.Number, value);
            }
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            PftGlobal[] values = GetAllVariables();
            writer.WriteArray(values);
        }

        #endregion
    }
}

