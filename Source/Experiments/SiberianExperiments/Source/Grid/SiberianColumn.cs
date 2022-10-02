// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

// Событие никогда не используется
#pragma warning disable CS0067

/* SiberianColumn.cs -- дефолтная реализация колонки грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;

using AM;
using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid;

/// <summary>
/// Дефолтная реализация колонки грида.
/// </summary>
public class SiberianColumn
    : ISiberianColumn
{
    #region ISiberianColumn members

    /// <inheritdoc cref="ISiberianColumn.Click"/>
    public event EventHandler<SiberianClickEventArgs>? Click;

    /// <inheritdoc cref="ISiberianColumn.Grid"/>
    public ISiberianGrid Grid { get; }

    /// <inheritdoc cref="ISiberianColumn.Index"/>
    public int Index { get; set; }

    /// <inheritdoc cref="ISiberianColumn.Member"/>
    public string? Member { get; set; }

    /// <inheritdoc cref="ISiberianColumn.ReadOnly"/>
    public bool ReadOnly { get; set; }

    /// <inheritdoc cref="ISiberianColumn.Title"/>
    public string? Title { get; set; }

    /// <inheritdoc cref="ISiberianColumn.Width"/>
    public int Width { get; set; }

    /// <inheritdoc cref="ISiberianColumn.CreateCell"/>
    public virtual ISiberianCell CreateCell (ISiberianRow row)
    {
        return new SiberianCell
            (
                Grid,
                column: this,
                row,
                GetMemberData (row.Data)
            );
    }

    /// <inheritdoc cref="ISiberianColumn.GetMemberData"/>
    public virtual object? GetMemberData
        (
            object? obj
        )
    {
        if (obj is null || string.IsNullOrEmpty (Member))
        {
            return null;
        }

        if (_getter is null)
        {
            var type = obj.GetType();
            var propertyInfo = type.GetProperty (Member, Flags);
            if (propertyInfo is null)
            {
                return null;
            }

            _getter = ReflectionUtility.CreateUntypedGetter (propertyInfo);
        }

        return _getter (obj);
    }

    /// <inheritdoc cref="ISiberianColumn.PutMemberData"/>
    public virtual void PutMemberData
        (
            object? obj,
            object? value
        )
    {
        if (obj is null || string.IsNullOrEmpty (Member))
        {
            return;
        }

        if (_setter is null)
        {
            var type = obj.GetType();
            var propertyInfo = type.GetProperty (Member, Flags);
            if (propertyInfo is null)
            {
                return;
            }

            _setter = ReflectionUtility.CreateUntypedSetter (propertyInfo);
        }

        _setter (obj, value);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SiberianColumn
        (
            ISiberianGrid grid,
            int index
        )
    {
        Sure.NotNull (grid);

        Grid = grid;
        Index = index;
    }

    #endregion

    #region Private members

    private const BindingFlags Flags = BindingFlags.Instance
        | BindingFlags.Public
        | BindingFlags.NonPublic;

    private Func<object, object?>? _getter;
    private Action<object, object?>? _setter;

    #endregion
}
