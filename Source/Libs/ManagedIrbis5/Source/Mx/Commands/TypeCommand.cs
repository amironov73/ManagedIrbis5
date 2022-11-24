// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TypeCommand.cs -- вывод на экран содержимого файла
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Вывод на экран содержимого файла.
/// </summary>
public sealed class TypeCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TypeCommand()
        : base ("type")
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

        var specification = MxUtility.ParseFileSpecification (executive, arguments);
        if (specification is null)
        {
            return false;
        }

        var result = executive.Provider.ReadTextFile (specification)
                     ?? "<EMPTY>";
        executive.WriteOutput (result);

        OnAfterExecute();

        return true;
    }

    #endregion
}
