// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IrbisMorphologyProvider.cs -
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Morphology
{
    /// <summary>
    ///
    /// </summary>
    public sealed class IrbisMorphologyProvider
        : MorphologyProvider
    {
        #region Constants

        /// <summary>
        /// Default database name.
        /// </summary>
        public const string DefaultDatabase = "MORPH";

        /// <summary>
        /// Default prefix.
        /// </summary>
        public const string DefaultPrefix = "K=";

        #endregion

        #region Properties

        /// <summary>
        /// Client connection.
        /// </summary>
        public IrbisProvider? Provider { get; set; }

        /// <summary>
        /// Search prefix.
        /// </summary>
        public string? Prefix { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        public string? Database { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider()
        {
            Prefix = DefaultPrefix;
            Database = DefaultDatabase;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider
            (
                IrbisProvider provider
            )
            : this
                (
                    DefaultPrefix,
                    DefaultDatabase,
                    provider
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider
            (
                string prefix,
                string database,
                IrbisProvider provider
            )
        {
            Prefix = prefix;
            Database = database;
            Provider = provider;
        }

        #endregion

        #region MorphologyProvider members

        /// <inheritdoc cref="MorphologyProvider.FindWord" />
        public override MorphologyEntry[] FindWord
            (
                string word
            )
        {
            var connection = Provider.ThrowIfNull("Connection");
            var database = Database.ThrowIfNull("Database");

            var saveDatabase = connection.Database;
            try
            {
                connection.Database = database;
                var expression = string.Format
                    (
                        "\"{0}{1}\"",
                        Prefix,
                        word
                    );

                var found = connection.Search(expression);
                if (found.Length == 0)
                {
                    return Array.Empty<MorphologyEntry>();
                }

                var records = new List<Record>(found.Length);
                foreach (var mfn in found)
                {
                    var record = connection.ReadRecord(mfn);
                    if (!ReferenceEquals(record, null))
                    {
                        records.Add(record);
                    }
                }

                if (records.Count == 0)
                {
                    return Array.Empty<MorphologyEntry>();
                }

                var result = records
                    .Select(r => MorphologyEntry.Parse(r))
                    .ToArray();

                return result;
            }
            finally
            {
                connection.Database = saveDatabase;
            }
        }

        /// <inheritdoc cref="MorphologyProvider.RewriteQuery" />
        public override string RewriteQuery
            (
                string queryExpression
            )
        {
            var tokens
                = SearchQueryLexer.Tokenize(queryExpression);
            var parser = new SearchQueryParser(tokens);
            var program = parser.Parse();
            var terms = SearchQueryUtility.ExtractTerms(program);

            var prefix = Prefix.ThrowIfNull("Prefix");
            var prefixLength = prefix.Length;

            foreach (var oldTerm in terms)
            {
                var word = oldTerm.Term;
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }
                if (oldTerm.Tail == "$"
                    || !word.StartsWith(prefix))
                {
                    continue;
                }
                word = word.Substring(prefixLength);
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }
                var entries = FindWord(word);
                var flatten = Flatten(word, entries);
                if (flatten.Length < 2)
                {
                    continue;
                }

                var level7 = new SearchLevel7();
                var level6 = new SearchLevel6();
                level7.AddItem(level6);
                foreach (var s in flatten)
                {
                    var level5 = new SearchLevel5();
                    var level4 = new SearchLevel4();
                    var level3 = new SearchLevel3();
                    var level2 = new SearchLevel2();
                    var level1 = new SearchLevel1();
                    var level0 = new SearchLevel0();
                    level1.AddItem(level0);
                    level2.AddItem(level1);
                    level3.AddItem(level2);
                    level4.AddItem(level3);
                    level5.AddItem(level4);
                    level6.AddItem(level5);

                    var newTerm = new SearchTerm
                    {

                        Term = Prefix + s,
                        Tail = string.Empty,
                        Context = oldTerm.Context
                    };
                    level0.Term = newTerm;
                }

                var parent = (SearchLevel0) oldTerm.Parent
                    .ThrowIfNull("oldTerm.Parent");
                parent.Term = null;
                parent.Parenthesis = level7;
            }

            var result = program.ToString();

            return result;
        }

        #endregion
    }
}
