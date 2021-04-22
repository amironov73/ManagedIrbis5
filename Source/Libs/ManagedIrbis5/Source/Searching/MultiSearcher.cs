// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* MultiSearcher.cs -- поиск по нескольким каталогам сразу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Searching
{
    /// <summary>
    /// Поиск по нескольким каталогам сразу.
    /// </summary>
    public sealed class MultiSearcher
    {
        #region Properties

        /// <summary>
        /// Провайдер.
        /// </summary>
        public ISyncIrbisProvider Provider { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="provider">Провайдер.</param>
        public MultiSearcher
            (
                ISyncIrbisProvider provider
            )
        {
            Provider = provider;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Поиск во всех перечисленных каталогах.
        /// </summary>
        public RecordBacket SearchAll
            (
                string expression,
                IEnumerable<string> databases
            )
        {
            var result = new RecordBacket();
            var savedDatabase = Provider.Database;

            try
            {
                foreach (var database in databases)
                {
                    Provider.Database = database;
                    var found = Provider.Search(expression);
                    foreach (var mfn in found)
                    {
                        var reference = new RecordReference
                        {
                            Database = database,
                            Mfn = mfn
                        };
                        result.Add(reference);
                    }
                }
            }
            finally
            {
                Provider.Database = savedDatabase;
            }

            return result;

        } // method SearchAll

        /// <summary>
        /// Поиск до первых найденных записей.
        /// </summary>
        public RecordBacket SearchAny
            (
                string expression,
                IEnumerable<string> databases
            )
        {
            var result = new RecordBacket();
            var savedDatabase = Provider.Database;

            try
            {
                foreach (var database in databases)
                {
                    Provider.Database = database;
                    var found = Provider.Search(expression);
                    foreach (var mfn in found)
                    {
                        var reference = new RecordReference
                        {
                            Database = database,
                            Mfn = mfn
                        };
                        result.Add(reference);
                    }

                    if (found.Length != 0)
                    {
                        break;
                    }
                }
            }
            finally
            {
                Provider.Database = savedDatabase;
            }

            return result;

        } // method SearchAny

        #endregion

    } // class MultiSearcher

} // namespace ManagedIrbis.Searching
