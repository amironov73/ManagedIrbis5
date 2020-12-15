// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* LiteSubField.cs -- облегченное подполе библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Облегченное подполе библиографической записи.
    /// </summary>
    public sealed class LiteSubField
        : IVerifiable
    {
        #region Constants

        /// <summary>
        /// Нет кода подполя, т. е. код пока не задан.
        /// </summary>
        public const byte NoCode = 0;

        /// <summary>
        /// Subfield delimiter.
        /// </summary>
        public const byte Delimiter = (byte)'^';

        #endregion
        #region Properties

        /// <summary>
        /// Код подполя.
        /// </summary>
        public byte Code { get; set; }

        /// <summary>
        /// Значение подполя.
        /// </summary>
        public ReadOnlyMemory<byte> Value { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование подполя.
        /// </summary>
        public LiteSubField Clone()
        {
            return (LiteSubField)MemberwiseClone();
        }

        /// <summary>
        /// Декодирование строки.
        /// </summary>
        public void Decode
            (
                ReadOnlyMemory<byte> text
            )
        {
            if (!text.IsEmpty)
            {
                Code = text.Span[0];
                Value = text.Slice(1);
            }
        } // method Decode

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<LiteSubField>(this, throwOnError);

            verifier.Assert(Code > (byte)' ', "Wrong Code");
            verifier.Assert(!Value.Span.Contains(Delimiter));

            return verifier.Result;
        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var value = Value.IsEmpty
                ? string.Empty
                : IrbisEncoding.Utf8.GetString(Value.Span);

            return Code == 0
                ? value
                : "^" + (char)Code + value;
        } // method ToString

        #endregion
    }
}
