// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FoundLine.cs -- одна строка в ответе на поисковый запрос
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Одна строка в ответе сервера на поисковый запрос.
    /// </summary>
    [DebuggerDisplay("{Mfn} {Text}")]
    public sealed class FoundItem
    {
        #region Properties

        /// <summary>
        /// Text.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static FoundItem[] Parse
            (
                Response response
            )
        {
            var expected = response.ReadInteger();
            var result = new List<FoundItem>(expected);
            while (!response.EOT)
            {
                var line = response.ReadAnsi();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var parts = line.Split('#', 2);
                var item = new FoundItem
                {
                    Mfn = int.Parse(parts[0]),
                    Text = parts.Length == 2 ? parts[1] : string.Empty
                };
                result.Add(item);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static int[] ParseMfn
            (
                Response response
            )
        {
            var expected = response.ReadInteger();
            var result = new List<int>(expected);
            while (!response.EOT)
            {
                var line = response.ReadAnsi();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var parts = line.Split('#', 2);
                var mfn = int.Parse(parts[0]);
                result.Add(mfn);
            }

            return result.ToArray();
        }

        #endregion
    }
}
