// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BiblioDictionary.cs -- словарь, в котором хранятся ссылки для указателя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Text;
using AM.Text.Output;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Словарь, в котором хранятся ссылки для указателя
/// (например, указателя заглавий).
/// </summary>
[PublicAPI]
public sealed class BiblioDictionary
    : Dictionary<string, DictionaryEntry>
{
    #region Public methods

    /// <summary>
    /// Добавление заголовка и связанной с ним ссылки
    /// на библиографическую запись (порядковый номер записи).
    /// </summary>
    /// <remarks>
    /// Повторно ссылки с одинаковыми заголовком и номером
    /// не добавляются.
    /// </remarks>
    public void Add
        (
            string title,
            int reference
        )
    {
        Sure.NotNull (title);
        Sure.NonNegative (reference);
        
        if (!TryGetValue (title, out var entry))
        {
            entry = new DictionaryEntry
            {
                Title = title
            };
            Add (title, entry);
        }

        if (!entry.References.Contains (reference))
        {
            entry.References.Add (reference);
        }
    }

    /// <summary>
    /// Дамп словаря, например, для отладки.
    /// </summary>
    public void Dump
        (
            AbstractOutput output
        )
    {
        Sure.NotNull (output);
        
        var keys = NumberText.Sort (Keys).ToArray();
        foreach (var key in keys)
        {
            var entry = this[key];
            output.WriteLine (entry.ToString());
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => Count.ToInvariantString();

    #endregion
}
