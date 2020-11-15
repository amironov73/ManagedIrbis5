// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SubField.cs -- подполе библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Подполе библиографической записи.
    /// </summary>
    public class SubField
    {
        #region Properties

        /// <summary>
        /// Код подполя.
        /// </summary>
        public char Code { get; set; } = '\0';

        /// <summary>
        /// Значение подполя.
        /// </summary>
        public string? Value { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование подполя.
        /// </summary>
        public SubField Clone()
        {
            return (SubField) MemberwiseClone();
        }

        /// <summary>
        /// Декодирование строки.
        /// </summary>
        public void Decode
            (
                ReadOnlySpan<char> text
            )
        {
            Code = text[0];
            Value = new string(text.Slice(1));
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Code == '\0'
                ? Value ?? string.Empty
                : "^" + Code + Value;
        }

        #endregion
    }
}