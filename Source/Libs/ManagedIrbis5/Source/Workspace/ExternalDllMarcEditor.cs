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

/* ExternalDllMarcEditor.cs -- использует внешнюю динамическую библиотеку для редактирования
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Workspace
{
    /// <summary>
    /// Использует внешнюю динамическую библиотеку
    /// для редактирования поля/подполя.
    /// </summary>
    public sealed class ExternalDllMarcEditor
        : IMarcEditor
    {
        #region IMarcEditor members

        /// <inheritdoc cref="IMarcEditor.PerformEdit"/>
        public void PerformEdit
            (
                EditContext context
            )
        {
            throw new NotImplementedException();
        } // method PerformEdit

        #endregion

    } // class ExternalDllMarcEditor

} // namespace ManagedIrbis.Workspace
