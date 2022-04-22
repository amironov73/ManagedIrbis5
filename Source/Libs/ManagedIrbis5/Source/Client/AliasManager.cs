// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AliasManager.cs -- менеджер псевдонимов для баз данных/серверов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;
using AM.IO;
using AM.Linq;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
/// Менеджер псевдонимов для баз данных/серверов.
/// </summary>
public sealed class AliasManager
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public AliasManager()
    {
        _aliases = new List<ConnectionAlias>();
    }

    #endregion

    #region Private members

    private readonly List<ConnectionAlias> _aliases;

    private ConnectionAlias? _GetAlias
        (
            string name
        )
    {
        foreach (var theAlias in _aliases)
        {
            if (theAlias.Name.SameString (name))
            {
                return theAlias;
            }
        }

        return null;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Clear the table.
    /// </summary>
    public AliasManager Clear()
    {
        _aliases.Clear();

        return this;
    }

    /// <summary>
    /// Создание менеджера с одновременным чтением
    /// всех псевдонимов из текстового файла.
    /// </summary>
    public static AliasManager FromPlainTextFile
        (
            string fileName
        )
    {
        using var reader = TextReaderUtility.OpenRead (fileName, IrbisEncoding.Ansi);
        var result = new AliasManager();

        while (true)
        {
            var line1 = reader.ReadLine();
            var line2 = reader.ReadLine();

            if (string.IsNullOrEmpty (line1)
                || string.IsNullOrEmpty (line2))
            {
                break;
            }

            var theAlias = new ConnectionAlias
            {
                Name = line1,
                Value = line2
            };
            result._aliases.Add (theAlias);
        }

        return result;
    }

    /// <summary>
    /// Get alias value if exists.
    /// </summary>
    public string? GetAliasValue
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        return _GetAlias (name)?.Value;
    }

    /// <summary>
    /// Перечень всех алиасов.
    /// </summary>
    public string[] ListAliases()
    {
        var result = _aliases
            .Select (alias => alias.Name)
            .NonNullItems()
            .ToArray();

        return result;
    }

    /// <summary>
    /// Сохранение псевдонимов в файл.
    /// </summary>
    public void SaveToPlainTextFile
        (
            string fileName
        )
    {
        using var writer = TextWriterUtility.Create (fileName, IrbisEncoding.Ansi);
        foreach (var alias in _aliases)
        {
            writer.WriteLine (alias.Name);
            writer.WriteLine (alias.Value);
        }
    }

    /// <summary>
    /// Добавление нового или модификация существующего псевдонима.
    /// </summary>
    public AliasManager SetAlias
        (
            string name,
            string? value
        )
    {
        var alias = _GetAlias (name);
        if (alias is null)
        {
            if (!string.IsNullOrEmpty (value))
            {
                alias = new ConnectionAlias
                {
                    Name = name,
                    Value = value
                };
                _aliases.Add (alias);
            }
        }
        else
        {
            if (string.IsNullOrEmpty (value))
            {
                _aliases.Remove (alias);
            }
            else
            {
                alias.Value = value;
            }
        }

        return this;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return _aliases.Count.ToInvariantString();
    }

    #endregion
}
