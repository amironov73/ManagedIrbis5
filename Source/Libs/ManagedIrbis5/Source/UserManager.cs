// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* UserManager.cs -- менеджер пользователей системы ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Менеджер пользователей системы ИРБИС64.
/// </summary>
public sealed class UserManager
{
    #region Properties

    /// <summary>
    /// Подключение к серверу.
    /// </summary>
    public ISyncConnection Connection { get; }

    /// <summary>
    /// Список уже зарегистрированных пользователей системы.
    /// </summary>
    public List<UserInfo> RegisteredUsers => GetUserListIfNotAlready();

    #endregion

    #region Private members

    private List<UserInfo>? _userList;

    private List<UserInfo> GetUserListIfNotAlready()
    {
        return _userList ??= Connection.ListUsers().ThrowIfNull().ToList();
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="connection">Активное подключение к серверу.
    /// </param>
    public UserManager
        (
            ISyncConnection connection
        )
    {
        Sure.NotNull (connection);
        connection.EnsureConnected();

        Connection = connection;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание INI-файла для указанного пользователя и АРМ.
    /// </summary>
    /// <param name="userName">Логин пользователя</param>
    /// <param name="workstation">Код АРМ.</param>
    /// <returns></returns>
    public string CreateIniFile
        (
            string userName,
            Workstation workstation
        )
    {
        Sure.NotNullNorEmpty (userName);

        var lowerWorkstation = char.ToLowerInvariant ((char) workstation);
        var upperWorkstation = char.ToUpperInvariant ((char) workstation);
        var fileName = $"ini\\{userName}{upperWorkstation}.ini";
        var content = $"""
        [@irbis{lowerWorkstation}]

        [MAIN]
        FIO={userName}
        OTVFACE={userName}

        [PRIVATE]
        FIO={userName}
        """;

        var specification = new FileSpecification
        {
            Path = IrbisPath.Data,
            FileName = fileName,
            Content = content
        };
        Connection.WriteTextFile (specification);

        return fileName;
    }

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    public void RegisterUser
        (
            UserInfo newUser
        )
    {
        Sure.VerifyNotNull (newUser);
        Connection.EnsureConnected();

        var userName = newUser.Name.ThrowIfNullOrWhiteSpace();
        if (UserExists (userName))
        {
            throw new IrbisException ($"User already exists: {userName}");
        }

        // TODO создавать файлы
        if (!string.IsNullOrEmpty (newUser.Cataloger))
        {
            newUser.Cataloger = CreateIniFile (userName, Workstation.Cataloger);
        }

        if (!string.IsNullOrEmpty (newUser.Circulation))
        {
            newUser.Circulation = CreateIniFile (userName, Workstation.Circulation);
        }

        // отправляем обновленный список на сервер
        RegisteredUsers.Add (newUser);
        Connection.UpdateUserList (RegisteredUsers);
    }

    /// <summary>
    /// Отмена регистрации указанного пользователя в ИРБИС64.
    /// </summary>
    /// <param name="userName">Логин, подлежащий удалению.</param>
    public void UnregisterUser
        (
            string userName
        )
    {
        Sure.NotNull (userName);
        Connection.EnsureConnected();

        var found = RegisteredUsers.FirstOrDefault
            (
                user => user.Name.SameString (userName)
            );
        if (found is not null)
        {
            // отправляем обновленный список на сервер
            RegisteredUsers.Remove (found);
            Connection.UpdateUserList (RegisteredUsers);
        }
    }

    /// <summary>
    /// Проверка, зарегистрирован ли пользователь с указанным логином.
    /// </summary>
    /// <param name="name">Логин для проверки.</param>
    /// <returns><c>true</c>, если пользователь уже зарегистрирован.</returns>
    public bool UserExists
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        return RegisteredUsers.Any (user => user.Name.SameString (name));
    }

    #endregion
}
