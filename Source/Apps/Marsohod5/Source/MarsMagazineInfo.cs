// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MarsMagazineInfo.cs -- информация о журнале, получаемом из МАРС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Configuration;

using ManagedIrbis;

#endregion

#nullable enable

namespace Marsohod5
{
    /// <summary>
    /// Информация о журнале, получаемом из МАРС.
    /// </summary>
    public sealed class MarsMagazineInfo
    {
        #region Properties

        /// <summary>
        /// Заглавие журнала.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Шифр документа для сводной записи.
        /// </summary>
        public string? Index { get; set; }

        /// <summary>
        /// Код журнала в МАРС.
        /// </summary>
        public string? MarsCode { get; set; }

        /// <summary>
        /// Флаг, разрешающий/запрещающий обработку журнала.
        /// </summary>
        public string? Flag { get; set; }

        /// <summary>
        /// MFN сводной записи.
        /// </summary>
        public int Mfn { get; set; }

        #endregion

        #region Public methods


        public static MarsMagazineInfo FromRecord
            (
                Record record,
                MarsOptions options
            )
        {
            MarsMagazineInfo result = new MarsMagazineInfo
            {
                Title = record.FM(200, 'a'),
                Index = record.FM(903),
                MarsCode = record.FM(options.MarsCode),
                Flag = record.FM(options.MarsFlag),
                Mfn = record.Mfn
            };

            return result;

        } // method FromRecord

        #endregion

    } // class MarsMagazineInfo
}
