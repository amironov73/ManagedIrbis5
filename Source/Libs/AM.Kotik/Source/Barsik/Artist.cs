// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DummyClass.cs -- тестовый класс для опытов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Тестовый класс для опытов.
/// </summary>
public sealed class Artist
{
    #region Properties

    /// <summary>
    /// Имя.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Возраст.
    /// </summary>
    public int Year { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Artist()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Artist
        (
            string? name, 
            int year
        )
    {
        Name = name;
        Year = year;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"Name: {Name}, year {Year}";

    #endregion
}
