// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FormatCommand.cs -- форматирование найденных записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Форматирование найденных записей.
/// </summary>
public sealed class FormatCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public FormatCommand()
        : base ("format")
    {
        // пустое тело класса
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

        string? argument = null;

        if (arguments.Length != 0)
        {
            argument = arguments[0].Text;
        }

        if (string.IsNullOrEmpty (argument))
        {
            executive.WriteMessage (executive.DescriptionFormat);
            return true;
        }

        var needPrint = false;
        if (argument.SameString ("/print", "/show"))
        {
            needPrint = true;
            argument = executive.DescriptionFormat;
        }

        if (!string.IsNullOrEmpty (argument))
        {
            executive.DescriptionFormat = argument;

            if (executive.Provider.IsConnected
                && executive.Records.Count != 0)
            {
                var mfns = executive.Records.Select (r => r.Mfn)
                    .ToArray();
                var formatted = executive.Provider.FormatRecords
                    (
                        mfns,
                        executive.DescriptionFormat
                    );
                if (formatted is not null)
                {
                    for (var i = 0; i < mfns.Length; i++)
                    {
                        executive.Records[i].Description = formatted[i];
                    }
                }
            }
        }

        if (needPrint)
        {
            new PrintCommand().Execute (executive, Array.Empty<MxArgument>());
        }

        OnAfterExecute();

        return true;
    }

    #endregion
}
