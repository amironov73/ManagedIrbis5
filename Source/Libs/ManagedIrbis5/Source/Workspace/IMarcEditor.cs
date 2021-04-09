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

/* IMarcEditor.cs -- интерфейс редактора поля/подполя.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Workspace
{
    /// <summary>
    /// Интерфейс редактора поля/подполя.
    /// </summary>
    public interface IMarcEditor
        : IServiceProvider
    {
        /// <summary>
        /// Осуществляет редактирование в указанном контексте.
        /// </summary>
        void PerformEdit
            (
                EditContext context
            );

    } // interface IMarcEditor

} // namespace ManagedIrbis.Workspace
