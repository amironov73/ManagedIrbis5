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

/* SubSheetMarcEditor.cs -- ввод значений полей/подполей через вложенный рабочий лист
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Workspace
{
    /// <summary>
    /// Ввод значений полей/подполей через вложенный рабочий лист.
    /// </summary>
    public class SubSheetMarcEditor
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

    } // class SubSheetMarcEditor

} // namespace ManagedIrbis.WinForms.Workspace
