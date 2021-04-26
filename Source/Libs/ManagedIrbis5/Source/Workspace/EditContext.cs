// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* EditContext.cs -- контекст редактирования для поля/подполя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Workspace
{
    /// <summary>
    /// Контекст редактирования для поля/подполя.
    /// </summary>
    public sealed class EditContext
    {
        #region Properties

        /// <summary>
        /// Родительский (или корневой) контекст.
        /// </summary>
        public EditContext? ParentContext { get; set; }

        /// <summary>
        /// Провайдер.
        /// </summary>
        public ISyncProvider? Provider { get; set; }

        /// <summary>
        /// Код режима редактирования.
        /// </summary>
        public EditMode Mode { get; set; }

        /// <summary>
        /// Редактируемая запись.
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Редактируемые поля (как минимум, одно).
        /// </summary>
        public Field[]? Fields { get; set; }

        /// <summary>
        /// (Текущее) редактируемое поле.
        /// </summary>
        public Field? Field { get; set; }

        /// <summary>
        /// Редактируемое подполе.
        /// </summary>
        public SubField? Subfield { get; set; }

        /// <summary>
        /// Метка поля.
        /// </summary>
        public int Tag => Field?.Tag ?? 0;

        /// <summary>
        /// Код подполя (если есть).
        /// </summary>
        public char Code => Subfield?.Code ?? SubField.NoCode;

        /// <summary>
        /// Индекс повторения.
        /// -1 = все.
        /// -2 = неизвестен.
        /// </summary>
        public int Repeat { get; set; }

        /// <summary>
        /// Ссылка на элемент рабочего листа.
        /// </summary>
        public WorksheetItem? Item { get; set; }

        /// <summary>
        /// Пользователь подтвердил результат редактирования.
        /// </summary>
        public bool Accept { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        public object? UserData { get; set; }

        #endregion

    } // class EditContext

} // namespace ManagedIrbis.Workspace
