// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* SearchManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.Collections;
using AM.IO;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    ///
    /// </summary>
    public sealed class SearchManager
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        public IrbisProvider Provider { get; }

        /// <summary>
        /// Search history.
        /// </summary>
        public NonNullCollection<SearchResult> SearchHistory { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchManager
            (
                IrbisProvider provider
            )
        {
            Provider = provider;
            SearchHistory = new NonNullCollection<SearchResult>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Load search scenarios.
        /// </summary>
        public SearchScenario[] LoadSearchScenarios
            (
                FileSpecification file
            )
        {
            Sure.NotNull(file, nameof(file));

            var text = Provider.ReadFile(file);
            if (string.IsNullOrEmpty(text))
            {
                return Array.Empty<SearchScenario>();
            }

            using StringReader reader = new StringReader(text);
            using IniFile iniFile = new IniFile();

            iniFile.Read(reader);
            var result = SearchScenario.ParseIniFile(iniFile);

            return result;
        } // method LoadSearchScenarios

        /// <summary>
        /// Search.
        /// </summary>
        public FoundLine[] Search
            (
                string database,
                string expression,
                string? prefix
            )
        {
            Provider.Database = database;
            int[] found = Provider.Search(expression);
            FoundLine[] result = new FoundLine[found.Length];
            for (int i = 0; i < found.Length; i++)
            {
                result[i] = new FoundLine
                {
                    Mfn = found[i]
                };
            }

            return result;

            //SearchParameters parameters = new SearchParameters
            //{
            //    Database = database,
            //    SearchExpression = expression,
            //    FormatSpecification = IrbisFormat.Brief
            //};

            //SearchCommand command
            //    = Provider.CommandFactory.GetSearchCommand();
            //command.ApplyParameters(parameters);

            //Provider.ExecuteCommand(command);

            //FoundLine[] result = command.Found
            //    .ThrowIfNull("command.Found")
            //    .Select
            //    (
            //        item => new FoundLine
            //        {
            //            Mfn = item.Mfn,
            //            Description = item.Text
            //        }
            //    )
            //    .ToArray();

            //if (!string.IsNullOrEmpty(prefix))
            //{
            //    int prefixLength = prefix.Length;

            //    foreach (FoundLine line in result)
            //    {
            //        string description = line.Description;
            //        if (string.IsNullOrEmpty(description))
            //        {
            //            continue;
            //        }
            //        if (description.StartsWith(prefix))
            //        {
            //            line.Description = description
            //                .Substring(prefixLength);
            //        }
            //    }
            //}

            //return result;
        }

        #endregion

    } // class SearchManager

} // namespace ManagedIrbis
