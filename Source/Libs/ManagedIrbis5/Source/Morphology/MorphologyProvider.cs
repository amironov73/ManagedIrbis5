// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MorphologyProvider.cs -- base morphology provider
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Morphology
{
    /// <summary>
    /// Base morphology provider.
    /// </summary>
    public class MorphologyProvider
    {
        #region Public methods

        /// <summary>
        /// Flatten the query.
        /// </summary>
        public string[] Flatten
            (
                string word,
                MorphologyEntry[] entries
            )
        {
            var result = new List<string>
            {
                word.ToUpper()
            };

            foreach (var entry in entries)
            {
                string entryMainTerm = entry.MainTerm
                    .ThrowIfNull("entry.MainTerm");
                string[] entryForms = entry.Forms
                    .ThrowIfNull("entry.Forms");

                result.Add(entryMainTerm.ToUpper());
                result.AddRange(entryForms.Select(w => w.ToUpper()));
            }

            return result
                .Distinct()
                .ToArray();
        }

        /// <summary>
        /// Find the word in the morphology database.
        /// </summary>
        public virtual MorphologyEntry[] FindWord
            (
                string word
            )
        {
            return Array.Empty<MorphologyEntry>();
        }

        /// <summary>
        /// Rewrite the query using morphology.
        /// </summary>
        public virtual string RewriteQuery
            (
                string queryExpression
            )
        {
            return queryExpression;
        }

        #endregion
    }
}
