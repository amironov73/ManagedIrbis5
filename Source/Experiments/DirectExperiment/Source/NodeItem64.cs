// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UseStringInterpolation

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;

#endregion

#nullable enable

namespace DirectExperiment
{
    /// <summary>
    /// Справочник в N01/L01 является таблицей, определяющей
    /// поисковый термин. Каждый ключ переменной длины, который
    /// есть в записи, представлен в справочнике одним входом,
    /// формат которого описывает следующая структура
    /// </summary>
    [DebuggerDisplay("Length={Length}, KeyOffset={KeyOffset}, Text={Text}")]
    public sealed class NodeItem64
    {
        #region Properties

        /// <summary>
        /// Длина ключа.
        /// </summary>
        public short Length { get; set; }

        /// <summary>
        /// Смещение ключа от начала записи
        /// </summary>
        public short KeyOffset { get; set; }

        /// <summary>
        /// Младшее слово смещения.
        /// </summary>
        public int LowOffset { get; set; }

        /// <summary>
        /// Старшее слово смещения.
        /// </summary>
        public int HighOffset { get; set; }

        /// <summary>
        /// Полное смещение.
        /// </summary>
        public long FullOffset => unchecked(((long)HighOffset << 32) + LowOffset);

        /// <summary>
        /// Ссылается на лист?
        /// </summary>
        public bool RefersToLeaf => LowOffset < 0;

        /// <summary>
        /// Текстовое значение ключа.
        /// </summary>
        public string? Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void Dump
            (
                TextWriter writer
            )
        {
            writer.WriteLine("LEN : {0}", Length);
            writer.WriteLine("KEY : {0}", KeyOffset);
            writer.WriteLine("HIGH: {0}", HighOffset);
            writer.WriteLine("LOW : {0}", LowOffset);
            writer.WriteLine("TEXT: {0}", Text);

        } // method Dump

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Length: {0}, KeyOffset: {1}, "
                    + "LowOffset: {2}, HighOffset: {3}, "
                    + "FullOffset: {4}, RefersToLeaf: {5}, "
                    + "Text: {6}",
                    Length,
                    KeyOffset,
                    LowOffset,
                    HighOffset,
                    FullOffset,
                    RefersToLeaf,
                    Text
                );
        } // method ToString

        #endregion

    } // class NodeItem
}
