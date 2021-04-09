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

/* MaskedMarcEditor.cs -- ввод значений полей/подполей с помощью маски (шаблона)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Workspace
{
    /// <summary>
    /// Ввод значений полей/подполей с помощью маски (шаблона).
    /// </summary>
    public sealed class MaskedMarcEditor
        : IMarcEditor
    {
        #region IMarcEditor members

        /// <inheritdoc cref="IMarcEditor.PerformEdit"/>
        public void PerformEdit
            (
                EditContext context
            )
        {
            throw new System.NotImplementedException();
        } // method PerformEdit

        #endregion

    } // class MaskedMarcEditor

} // namespace ManagedIrbis.WinForms.Workspace
