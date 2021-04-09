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

/* FileMarcEditor.cs -- ввод значений полей/подполей через обращение к внешнему файлу
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
    /// Ввод значений полей/подполей через обращение к внешнему файлу.
    /// </summary>
    public sealed class FileMarcEditor
        : IMarcEditor
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="provider">Провайдер сервисов.</param>
        public FileMarcEditor
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

    } // class FileMarcEditor

} // namespace ManagedIrbis.WinForms.Workspace
