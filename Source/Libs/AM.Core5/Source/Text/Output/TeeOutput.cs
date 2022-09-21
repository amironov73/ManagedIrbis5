// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TeeOutput.cs -- расщепление (повтор) потока вывода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Text.Output;

/// <summary>
/// Расщепление (повтор) потока вывода.
/// </summary>
public sealed class TeeOutput
    : AbstractOutput
{
    #region Properties

    /// <summary>
    /// Подчинённые потоки.
    /// </summary>
    public List<AbstractOutput> Output { get; } = new ();

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TeeOutput()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Создание объекта с заранее установленным
    /// списком.
    /// </summary>
    public TeeOutput
        (
            params AbstractOutput[] children
        )
    {
        Output.AddRange (children);
    }

    #endregion

    #region AbstractOutput members

    /// <inheritdoc cref="AbstractOutput.HaveError"/>
    public override bool HaveError { get; set; }

    /// <inheritdoc cref="AbstractOutput.Clear"/>
    public override AbstractOutput Clear()
    {
        HaveError = false;
        foreach (AbstractOutput output in Output)
        {
            output.Clear();
        }

        return this;
    }

    /// <inheritdoc cref="AbstractOutput.Configure"/>
    public override AbstractOutput Configure
        (
            string configuration
        )
    {
        foreach (AbstractOutput output in Output)
        {
            output.Configure (configuration);
        }

        return this;
    }

    /// <inheritdoc cref="AbstractOutput.Write(string)"/>
    public override AbstractOutput Write
        (
            string text
        )
    {
        foreach (AbstractOutput output in Output)
        {
            output.Write (text);
        }

        return this;
    }

    /// <inheritdoc cref="AbstractOutput.WriteError(string)"/>
    public override AbstractOutput WriteError
        (
            string text
        )
    {
        HaveError = true;
        foreach (AbstractOutput output in Output)
        {
            output.WriteError (text);
        }

        return this;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public override void Dispose()
    {
        foreach (AbstractOutput output in Output)
        {
            output.Dispose();
        }

        base.Dispose();
    }

    #endregion
}
