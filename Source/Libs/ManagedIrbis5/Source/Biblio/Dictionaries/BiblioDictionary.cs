// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BiblioDictionary.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Text;
using AM.Text.Output;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    ///
    /// </summary>
    public sealed class BiblioDictionary
        : Dictionary<string, DictionaryEntry>
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Add title and reference.
        /// </summary>
        public void Add
            (
                string title,
                int reference
            )
        {
            if (!TryGetValue(title, out var entry))
            {
                entry = new DictionaryEntry
                {
                    Title = title
                };
                Add(title, entry);
            }

            if (!entry.References.Contains(reference))
            {
                entry.References.Add(reference);
            }
        }

        /// <summary>
        /// Dump the dictionary.
        /// </summary>
        public void Dump
            (
                AbstractOutput output
            )
        {
            string[] keys = NumberText.Sort(Keys).ToArray();
            foreach (string key in keys)
            {
                DictionaryEntry entry = this[key];
                output.WriteLine(entry.ToString());
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Count.ToInvariantString();
        }

        #endregion
    }
}
