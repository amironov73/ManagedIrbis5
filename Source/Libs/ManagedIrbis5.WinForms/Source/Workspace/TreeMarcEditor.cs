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

/* TreeMarcEditor.cs -- ввод значений полей/подполей через иерархический справочник
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Workspace;

/// <summary>
/// Ввод значений полей/подполей через иерархический справочник (дерево).
/// </summary>
public sealed class TreeMarcEditor
    : IMarcEditor
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    public TreeMarcEditor
        (
            IServiceProvider provider
        )
    {
        Sure.NotNull (provider);

        _provider = provider;
    }

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
        Sure.NotNull (context);

        throw new NotImplementedException();
    } // method PerformEdit

    #endregion

    #region IServiveProvider members

    /// <inheritdoc cref="IServiceProvider.GetService"/>
    public object? GetService (Type serviceType) => _provider.GetService (serviceType);

    #endregion
}
