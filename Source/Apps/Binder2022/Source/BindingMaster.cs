// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* BindingMaster.cs -- умеет работать с журналами и подшивками
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AM;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.CommandLine;
using ManagedIrbis.Magazines;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#endregion

#pragma warning disable CS8618 // non-nullable members not initialized in constructor

#nullable enable

namespace Binder2022;

/// <summary>
/// Умеет работать с журналами и подшивками.
/// </summary>
internal sealed class BindingMaster
{
    #region Private members

    /// <summary>
    /// Менеджер журналов.
    /// </summary>
    public MagazineManager Manager { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BindingMaster
        (
            Program program
        )
    {
        Manager = new MagazineManager (program.Connection.ThrowIfNull());
    }

    #endregion

    #region Public methods



    #endregion
}
