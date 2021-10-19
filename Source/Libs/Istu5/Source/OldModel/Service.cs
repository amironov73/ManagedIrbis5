// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* Service.cs -- услуга, оказанная библиотекой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace Istu.OldModel
{
    /// <summary>
    /// Услуга, оказанная библиотекой.
    /// </summary>
    public sealed class Service
    {
        #region Properties

        /// <summary>
        /// Наименование услуги.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Цена за единицу.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Единица измерения.
        /// </summary>
        public string? Unit { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Чтение текстового файла с информацией об услугах.
        /// </summary>
        public static Service[] ReadFile
            (
                string fileName
            )
        {
            var result = new List<Service>();

            using (var reader = new StreamReader (fileName))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty (line))
                    {
                        continue;
                    }

                    var parts = line.Split (';');
                    if (parts.Length != 3)
                    {
                        continue;
                    }

                    var service = new Service
                    {
                        Title = parts[0],
                        Price = int.Parse (parts[1]),
                        Unit = parts[2]
                    };
                    result.Add (service);
                }
            }

            return result.ToArray();

        } // method ReadFile

        #endregion

    } // class Service

} // namespace Istu.OldModel
