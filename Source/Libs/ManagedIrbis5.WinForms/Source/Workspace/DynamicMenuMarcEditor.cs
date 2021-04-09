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

/* DynamicMenuMarcEditor.cs -- ввод значений полей/подполей с помощью динамического справочника
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Workspace
{
    /// <summary>
    /// Ввод значений полей/подполей с помощью динамического справочника.
    /// </summary>
    public sealed class DynamicMenuMarcEditor
        : IMarcEditor
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="provider">Провайдер сервисов.</param>
        public DynamicMenuMarcEditor
            (
                IServiceProvider provider
            )
        {
            _provider = provider;
        } // constructor

        #endregion

        #region Private members

        private readonly IServiceProvider _provider;

        #endregion

        #region IMarcEditor

        /// <inheritdoc cref="IMarcEditor.PerformEdit"/>
        public void PerformEdit
            (
                EditContext context
            )
        {
            throw new NotImplementedException();
        } // method PerformEdit

        #endregion

        #region IServiveProvider members

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public object? GetService(Type serviceType) => _provider.GetService(serviceType);

        #endregion

    } // class DynamicMenuMarcEditor

} // namespace ManagedIrbis.WinForms.Workspace
