// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MstDictionaryEntry.cs -- элемент справочника MST-файла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Элемент справочника MST-файла,
    /// описывающий поле переменной длины.
    /// </summary>
    public struct MstDictionaryEntry64
    {
        #region Constants

        /// <summary>
        /// Длина элемента справочника MST-файла.
        /// </summary>
        public const int EntrySize = 12;

        #endregion

        #region Properties

        /// <summary>
        /// Field tag.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Data offset.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Data length.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Raw data.
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// Decoded data.
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString ()
        {
            return $"Tag: {Tag}, Position: {Position}, Length: {Length}, Text: {Text}";
        }

        #endregion
    }
}
