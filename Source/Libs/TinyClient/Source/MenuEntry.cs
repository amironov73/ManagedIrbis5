// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MenuEntry.cs -- пара строк в MNU
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Пара строк в MNU-файле: код и соответствующее значение
    /// (либо комментарий).
    /// </summary>
    [DebuggerDisplay ("{" + nameof (Code) + "} = {" + nameof (Comment) + "}")]
    public sealed class MenuEntry
    {
        #region Properties

        /// <summary>
        /// Первая строка - код.
        /// Коды могут повторяться в рамках одного MNU-файла.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Вторая строка - значение либо комментарий.
        /// Часто бывает пустой.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Ссылка на другую пару строк, применяется при построении дерева
        /// (TRE-файла).
        /// </summary>
        public MenuEntry? OtherEntry { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => string.IsNullOrEmpty (Comment)
            ? Code.ToVisibleString()
            : $"{Code} - {Comment}";

        #endregion
    }
}
