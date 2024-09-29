// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* OpacUtility.cs -- утилитные методы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;

using AM.Collections;

using ManagedIrbis;
using ManagedIrbis.Providers;
using ManagedIrbis.Readers;

#endregion

namespace Opac2025;

/// <summary>
/// Утилитные методы
/// </summary>
internal static class OpacUtility
{
    public static string? GetString
        (
            string keyName,
            string? defaultValue = null
        )
    {
        var configuration = Program.Application.Configuration;
        var result = configuration[keyName];
        if (string.IsNullOrEmpty (result))
        {
            result = defaultValue;
        }

        return result;
    }

    public static string GetRequiredString
        (
            string keyName
        )
    {
        var result = GetString (keyName);
        if (string.IsNullOrEmpty (result))
        {
            throw new ApplicationException ($"{keyName} not configured");
        }

        return result;
    }

    public static bool GetBoolean
        (
            string keyName,
            bool defaultValue = false
        )
    {
        var result = GetString (keyName);
        return string.IsNullOrEmpty (result)
            ? defaultValue
            : bool.Parse (result);
    }

    public static int GetInt32
        (
            string keyName,
            int defaultValue = -1
        )
    {
        var result = GetString (keyName);
        return string.IsNullOrEmpty (result)
            ? defaultValue
            : int.Parse (result, NumberStyles.Any, CultureInfo.InvariantCulture);
    }

    public static int GetRequiredInt32
        (
            string keyName,
            int defaultValue = -1
        )
    {
        var result = GetInt32 (keyName);
        if (result == defaultValue)
        {
            throw new ApplicationException ($"{keyName} not configured");
        }

        return result;
    }

    /// <summary>
    /// Подключение к серверу ИРБИС64.
    /// </summary>
    public static ISyncProvider? ConnectToIrbis
        (
            bool gracefully = false
        )
    {
        var connectionString = GetRequiredString ("irbis-connection");
        if (string.IsNullOrWhiteSpace (connectionString))
        {
            Program.Logger.LogError ("IRBIS connection string is empty");
            return null;
        }

        var timeout = GetInt32 ("timeout", 100);
        SyncConnection result;
        if (gracefully)
        {
            var socket = new GracefulSocket
            {
                Timeout = timeout
            };
            var serviceProvider = Program.Application.Services;
            result = new SyncConnection (socket, serviceProvider);
        }
        else
        {
            result = new SyncConnection();
        }

        result.ParseConnectionString (connectionString);
        if (!result.Connect() || !result.IsConnected)
        {
            result.Dispose();
            return null;
        }

        return result;
    }

    /// <summary>
    /// Проверка подключения к серверу ИРБИС64.
    /// </summary>
    public static bool TestIrbisConnection()
    {
        try
        {
            var connection = ConnectToIrbis();

            if (connection is null)
            {
                return false;
            }

            connection.Dispose();
        }
        catch (Exception exception)
        {
            Program.Logger.LogError (exception, $"Error during {nameof (TestIrbisConnection)}");
            return false;
        }

        return true;
    }
}
