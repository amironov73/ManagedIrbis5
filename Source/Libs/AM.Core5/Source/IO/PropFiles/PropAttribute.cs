// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* PropAttribute.cs -- атрибут, задающий ключ в prop-файле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.IO.PropFiles;

/// <summary>
/// Атрибут, задающий ключ в prop-файле.
/// </summary>
[AttributeUsage (AttributeTargets.Field|AttributeTargets.Property)]
public sealed class PropAttribute
    : Attribute
{
    #region Properties

    /// <summary>
    /// Имя ключа.
    /// </summary>
    public string KeyName { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PropAttribute
        (
            string keyName
        )
    {
        Sure.NotNullNorEmpty (keyName);
        
        KeyName = keyName;
    }

    #endregion
}
