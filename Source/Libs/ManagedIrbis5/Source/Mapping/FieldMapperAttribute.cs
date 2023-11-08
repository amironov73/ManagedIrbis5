// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FieldMapperAttribute.cs -- активирует генератор маппинга для поля
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis.Mapping;

/// <summary>
/// Активирует генератор маппинга поля в строку/структуру и т. д.
/// </summary>
[AttributeUsage (AttributeTargets.Method)]
public class FieldMapperAttribute
    : Attribute
{
    // пустое тело класса
}
