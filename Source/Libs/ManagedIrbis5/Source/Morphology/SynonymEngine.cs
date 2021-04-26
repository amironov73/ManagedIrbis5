// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SynonymEngine.cs --
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
    ///
    /// </summary>
    public class SynonymEngine
    {
        #region Constants

        /// <summary>
        /// Default database name.
        /// </summary>
        public const string DefaultDatabase = "SYNON";

        /// <summary>
        /// Default prefix.
        /// </summary>
        public const string DefaultPrefix = "WORD=";

        #endregion

        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        public ISyncProvider Provider { get; private set; }

        /// <summary>
        /// Database name.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Prefix.
        /// </summary>
        public string Prefix { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SynonymEngine
            (
                ISyncProvider provider
            )
        {
            Provider = provider;
            Database = DefaultDatabase;
            Prefix = DefaultPrefix;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get synonyms for the word.
        /// </summary>
        public string[] GetSynonyms
            (
                string word
            )
        {
            var previousDatabase = Provider.Database.ThrowIfNull("Provider.Database");
            string[] result;
            try
            {
                Provider.Database = Database;
                var expression = "\"" + Prefix + word + "\"";
                var found = Provider.Search(expression);
                if (found.Length == 0)
                {
                    return Array.Empty<string>();
                }

                var records = new List<Record>(found.Length);
                foreach (var mfn in found)
                {
                    // TODO: считывать большими порциями
                    var record = Provider.ReadRecord(mfn);
                    if (!ReferenceEquals(record, null))
                    {
                        records.Add(record);
                    }
                }

                var entries = records
                    .Select(SynonymEntry.Parse)
                    .ToArray();

                result = entries
                    .NonNullItems()
                    .SelectMany(entry => entry.Synonyms!)
                    .Distinct()
                    .ToArray();
            }
            finally
            {
                Provider.Database = previousDatabase;
            }

            return result;
        }

        #endregion

    } // class SynonymEngine

} // namespace ManagedIrbis.Morphology
