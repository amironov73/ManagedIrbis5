// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ConnectCommand.cs -- подключение к серверу или локальной базе данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Подключение к серверу ИРБИС64 либо к локальной базе данных.
/// </summary>
public sealed class ConnectCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ConnectCommand()
        : base ("connect")
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private members

    #endregion

    #region Public methods

    /// <summary>
    /// Initialize the provider.
    /// </summary>
    public static ISyncProvider InitializeProvider
        (
            string argument
        )
    {
        var result = ProviderManager.GetAndConfigureProvider (argument);

        return result;
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

        var provider = executive.Provider;
        if (arguments.Length != 0)
        {
            var argument = arguments[0].Text;
            provider.Dispose();
            executive.Provider = string.IsNullOrEmpty (argument)
                ? ProviderManager.GetPreconfiguredProvider()
                : InitializeProvider (argument);
        }
        else
        {
            const string defaultAlias = "default";
            var aliases = executive.Aliases;
            string? argument = null;
            if (aliases.ContainsKey (defaultAlias))
            {
                argument = aliases[defaultAlias];
            }

            provider.Dispose();
            executive.Provider = string.IsNullOrEmpty (argument)
                ? ProviderManager.GetPreconfiguredProvider()
                : InitializeProvider (argument);
        }

        executive.Context.SetProvider (provider);
        provider = executive.Provider;
        provider.Connect();
        if (!provider.IsConnected)
        {
            executive.WriteError ("Can't connect");
            var errorDescription = IrbisException.GetErrorDescription (provider.LastError);
            executive.WriteError (errorDescription);

            return false;
        }

        var maxMfn = provider.GetMaxMfn();
        executive.WriteMessage ($"Connected, database {provider.Database}, max MFN {maxMfn}");

        OnAfterExecute();

        return true;
    }

    /// <inheritdoc cref="MxCommand.GetShortHelp"/>
    public override string? GetShortHelp()
    {
        return "Connect to the server";
    }

    #endregion
}
