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

using System;

using AM;

using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Workspace;

/// <summary>
/// Ввод значений полей/подполей через вложенный рабочий лист.
/// </summary>
public class SubSheetMarcEditor
    : IMarcEditor
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="provider">Провайдер сервисов.</param>
    public SubSheetMarcEditor
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
    }

    #endregion

    #region IServiveProvider members

    /// <inheritdoc cref="IServiceProvider.GetService"/>
    public object? GetService (Type serviceType) => _provider.GetService (serviceType);

    #endregion
}
