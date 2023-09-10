// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* FoundPages.cs -- один результат полнотекстового поиска
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Один результат полнотекстового поиска.
    /// </summary>
    public sealed class FoundPages
    {
        #region Properties

        /// <summary>
        /// MFN найденной записи.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Номера страниц найденной записи.
        /// </summary>
        public int[]? Pages { get; set; }

        /// <summary>
        /// Результат расформатирования.
        /// </summary>
        public string? Formatted { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор текстовой строки.
        /// </summary>
        public void Decode
            (
                string line
            )
        {
            var parts = line.Split (Constants.NumberSign, 3);
            var pages = new List<int>();
            Mfn = int.Parse (parts[0]);
            if (parts.Length == 3)
            {
                Formatted = parts[2];
            }

            if (parts.Length > 1)
            {
                parts = parts[1].Split ('\x1F');
                foreach (var part in parts)
                {
                    if (!string.IsNullOrEmpty (part))
                    {
                        var page = int.Parse (part);
                        pages.Add (page);
                    }
                }
            }

            Pages = pages.ToArray();
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static FoundPages[] Decode
            (
                Response response
            )
        {
            // response.Debug(Console.Out);
            // response.DebugUtf(Console.Out);

            var number = response.ReadInteger(); // количество найденных записей
            var result = new List<FoundPages> (number);
            for (var i = 0; i < number; i++)
            {
                var line = response.ReadUtf();
                if (string.IsNullOrEmpty (line))
                {
                    break;
                }

                var one = new FoundPages();
                one.Decode (line);
                result.Add (one);
            }

            return result.ToArray();
        }

        #endregion
    }
}
