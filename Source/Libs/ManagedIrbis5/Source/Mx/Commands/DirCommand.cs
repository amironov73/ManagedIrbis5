// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DirCommand.cs -- получение списка файлов на сервере по маске
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Collections;

using ManagedIrbis.Infrastructure;

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

        var fileName = "*.*";
        if (arguments.Length != 0)
        {
            fileName = arguments[0].Text;
        }

        if (string.IsNullOrEmpty (fileName))
        {
            fileName = "*.*";
        }

        if (!FileSpecification.TryParse (fileName, out _))
        {
            fileName = "2." + executive.Provider.Database + "." + fileName;
        }

        var specification = FileSpecification.Parse (fileName);
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
