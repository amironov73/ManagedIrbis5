// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* NopCommand.cs -- пустая команда
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using AM;

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Пустая команда (может использоваться для поддержания
/// сессии с сервером ИРБИС64).
/// </summary>
public sealed class NopCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public NopCommand()
        : base("Nop")
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
        arguments.NotUsed ();

        OnBeforeExecute();

        var provider = executive.Provider;
        if (!provider.IsConnected)
        {
            executive.WriteError ("Not connected");
            return false;
        }

        executive.WriteLine ("NOP");
        provider.NoOperation();

        OnAfterExecute();

        return true;
    }

    #endregion
}
