// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TermParameters.cs -- параметры запроса терминов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Параметры запроса терминов.
    /// </summary>
    public sealed class TermParameters
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// Number of terms to return.
        /// </summary>
        public int NumberOfTerms { get; set; }

        /// <summary>
        /// Reverse order?
        /// </summary>
        public bool ReverseOrder { get; set; }

        /// <summary>
        /// Start term.
        /// </summary>
        public string? StartTerm { get; set; }

        /// <summary>
        /// Format.
        /// </summary>
        public string? Format { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование параметров постингов для клиентского запроса.
        /// </summary>
        /// <param name="connection">Ссылка на подключение к серверу.</param>
        /// <param name="query">Клиентский запрос.</param>
        public void Encode
            (
                Connection connection,
                Query query
            )
        {
            var database = (Database ?? connection.Database)
                .ThrowIfNull(nameof(Database));

            query.AddAnsi(database)
                .AddUtf(StartTerm)
                .Add(NumberOfTerms)
                .AddFormat(Format);
        }

        #endregion
    }
}
