// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MxCommand.cs -- абстрактная MX-команда
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mx;

/// <summary>
/// Абстрактная MX-команда.
/// </summary>
public abstract class MxCommand
    : IDisposable
{
    #region Events

    /// <summary>
    /// Fired before <see cref="Execute"/>.
    /// </summary>
    public event EventHandler? BeforeExecute;

    /// <summary>
    /// Fired after <see cref="Execute"/>.
    /// </summary>
    public event EventHandler? AfterExecute;

    #endregion

    #region Properties

    /// <summary>
    /// Main name of the command.
    /// </summary>
    public string Name { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected MxCommand
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        Name = name;
    }

    #endregion

    #region Private members

    /// <summary>
    /// Raises <see cref="BeforeExecute"/> event.
    /// </summary>
    protected virtual void OnBeforeExecute() =>
        BeforeExecute?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Raises <see cref="AfterExecute"/> event.
    /// </summary>
    protected virtual void OnAfterExecute() =>
        AfterExecute?.Invoke(this, EventArgs.Empty);

    #endregion

    #region Public methods

    /// <summary>
    /// Отложенная инициализация команды.
    /// </summary>
    public virtual void Initialize
        (
            MxExecutive executive
        )
    {
        // переопределить в потомке
    }

    /// <summary>
    /// Выполнение команды.
    /// </summary>
    /// <returns><c>true</c>, если можно продолжать выполнение скрипта,
    /// иначе <c>false</c>.</returns>
    public virtual bool Execute
        (
            MxExecutive executive,
            MxArgument[] arguments
        )
    {
        OnBeforeExecute();

        OnAfterExecute();

        return true;
    }

    /// <summary>
    /// Краткое описание команды.
    /// </summary>
    public virtual string? GetShortHelp()
    {
        return null;
    }

    /// <summary>
    /// Длинное описание команды.
    /// </summary>
    public virtual string? GetLongHelp()
    {
        return null;
    }

    /// <summary>
    ///
    /// </summary>
    public virtual bool RecognizeLine
        (
            string line
        )
    {
        return false;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public virtual void Dispose()
    {
        // переопределить в потомке
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Name.ToVisibleString();
    }

    #endregion
}
