// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DictionaryEntry.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    ///
    /// </summary>
    public sealed class DictionaryEntry
    {
        #region Properties

        /// <summary>
        /// Title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// List of references.
        /// </summary>
        public List<int> References { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DictionaryEntry()
        {
            References = new List<int>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append(Title);
            result.Append(' ');
            int[] refs = References.ToArray();
            Array.Sort(refs);
            bool first = true;
            foreach (int reference in refs)
            {
                if (!first)
                {
                    result.Append(", ");
                }
                result.Append(reference);
                first = false;
            }

            return result.ToString();
        }

        #endregion
    }
}
