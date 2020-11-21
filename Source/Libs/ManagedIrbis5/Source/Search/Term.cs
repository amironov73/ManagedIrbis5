// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Term.cs -- термин в поисковом словаре
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Термин в поисковом словаре.
    /// </summary>
    public sealed class Term
    {
        #region Properties

        /// <summary>
        /// Количество ссылок.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Поисковый термин.
        /// </summary>
        public string? Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static Term[] Parse
            (
                Response response
            )
        {
            var result = new List<Term>();
            while (!response.EOT)
            {
                var line = response.ReadUtf();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var parts = line.Split('#', 2);
                var item = new Term
                {
                    Count = int.Parse(parts[0]),
                    Text = parts.Length == 2 ? parts[1] : string.Empty
                };
                result.Add(item);
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString"/>
        public override string ToString()
        {
            return $"{Count}#{Text}";
        }

        #endregion
    }
}
