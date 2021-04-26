// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MorphologyEngine.cs -- morphology engine
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Morphology
{
    /// <summary>
    /// Morphology engine.
    /// </summary>
    public sealed class MorphologyEngine
    {
        #region Properties

        /// <summary>
        /// Client connection.
        /// </summary>
        public ISyncProvider Connection { get; private set; }

        /// <summary>
        /// Morphology provider.
        /// </summary>
        public MorphologyProvider Morphology { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MorphologyEngine
            (
                ISyncProvider provider
            )
        {
            Connection = provider;
            Morphology = new IrbisMorphologyProvider(provider);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MorphologyEngine
            (
                ISyncProvider provider,
                MorphologyProvider morphology
            )
        {
            Connection = provider;
            Morphology = morphology;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Rewrite the query.
        /// </summary>
        public string RewriteQuery
            (
                string queryText
            )
        {
            var provider = Morphology.ThrowIfNull("Provider");

            return provider.RewriteQuery(queryText);
        }

        /// <summary>
        /// Search with query rewritting.
        /// </summary>
        public int[] Search
            (
                string format,
                params object[] args
            )
        {
            var original = string.Format(format, args);
            var rewritten = RewriteQuery(original);
            var result = Connection.Search(rewritten);

            return result;
        }

        /// <summary>
        /// Search and read records with query rewritting.
        /// </summary>
        public Record[] SearchRead
            (
                string format,
                params object[] args
            )
        {
            var original = string.Format(format, args);
            var rewritten = RewriteQuery(original);
            var found = Connection.Search(rewritten);
            if (found.Length == 0)
            {
                return Array.Empty<Record>();
            }

            var result = new List<Record>(found.Length);
            foreach (var mfn in found)
            {
                var record = Connection.ReadRecord(mfn);
                if (!ReferenceEquals(record, null))
                {
                    result.Add(record);
                }
            }
            if (result.Count == 0)
            {
                return Array.Empty<Record>();
            }

            return result.ToArray();
        }

        /// <summary>
        /// Search and read first found record using query rewritting.
        /// </summary>
        public Record? SearchReadOneRecord
            (
                string format,
                params object[] args
            )
        {
            var original = string.Format(format, args);
            var rewritten = RewriteQuery(original);
            var found = Connection.Search(rewritten);
            if (found.Length == 0)
            {
                return null;
            }

            var result = Connection.ReadRecord(found[0]);

            return result;
        }

        /// <summary>
        /// Search and format found records using query rewritting.
        /// </summary>
        public FoundItem[] SearchFormat
            (
                string expression,
                string format
            )
        {
            var rewritten = RewriteQuery(expression);
            var found = Connection.Search(rewritten);
            if (found.Length == 0)
            {
                return Array.Empty<FoundItem>();
            }

            var result = new List<FoundItem>(found.Length);
            foreach (var mfn in found)
            {
                var record = Connection.ReadRecord(mfn);
                if (!ReferenceEquals(record, null))
                {
                    var text = Connection.FormatRecord(format, record);
                    if (!string.IsNullOrEmpty(text))
                    {
                        var item = new FoundItem
                        {
                            Mfn = mfn,
                            Text = text
                        };
                        result.Add(item);
                    }
                }
            }

            return result.ToArray();
        }

        #endregion
    }
}
