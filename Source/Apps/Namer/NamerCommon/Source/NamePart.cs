// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* NamePart.cs -- абстрактная часть имени
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Абстрактная часть имени.
/// </summary>
[PublicAPI]
public abstract class NamePart
{
    #region Properties

    /// <summary>
    /// Обозначение.
    /// </summary>
    public abstract string Designation { get; }
    
    /// <summary>
    /// Наименование части.
    /// </summary>
    public abstract string Title { get; }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор текста.
    /// </summary>
    public abstract NamePart Parse
        (
            string text
        );
    
    /// <summary>
    /// Рендеринг данной части.
    /// </summary>
    public abstract string Render
        (
            NamingContext context,
            FileInfo fileInfo
        );

    /// <summary>
    /// Сброс состояния (опциональный).
    /// </summary>
    public virtual void Reset()
    {
        // пустое тело метода
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => Designation;

    #endregion
}
