// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DirCommand.cs -- получение списка файлов на сервере по маске
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Получение списка файлов на сервере по маске.
/// </summary>
public sealed class DirCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DirCommand()
        : base ("dir")
    {
        // пустое тело конструктора
    }

    #endregion

    #region MxCommand members

    /// <inheritdoc cref="MxCommand.Execute" />
    public override bool Execute
        (
            MxExecutive executive,
            MxArgument[] arguments
        )
    {
        OnBeforeExecute();

        if (!executive.Provider.IsConnected)
        {
            executive.WriteError ("Not connected");
            return false;
        }

        // TODO обрабатывать несколько спецификаций

        var specification = MxUtility.ParseFileSpecification (executive, arguments);
        if (specification is null)
        {
            return false;
        }

        var found = executive.Provider.ListFiles (specification);
        if (found.IsNullOrEmpty())
        {
            executive.WriteError ("No files found");
        }
        else
        {
            Array.Sort (found);
            foreach (var one in found)
            {
                executive.WriteOutput (one);
            }
        }

        OnAfterExecute();

        return true;
    }

    #endregion
}
