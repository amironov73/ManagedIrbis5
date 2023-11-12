// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

#pragma warning disable 67

/* Command.cs -- некая команда (действие) приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

using JetBrains.Annotations;

#endregion

namespace AM.Commands;

/// <summary>
/// Некая команда (действие) приложения.
/// </summary>
[PublicAPI]
public class Command
    : ICommand
{
    #region Events

    /// <inheritdoc cref="ICommand.Execute"/>
    public event EventHandler? Execute;

    /// <inheritdoc cref="ICommand.Update"/>
    public event EventHandler? Update;

    /// <inheritdoc cref="ICommand.Changed"/>
    public event EventHandler? Changed;

    /// <inheritdoc cref="ICommand.Disposed"/>
    public event EventHandler? Disposed;

    #endregion

    #region Properties

    /// <inheritdoc cref="ICommand.Enabled"/>
    [XmlAttribute ("enabled")]
    [JsonPropertyName ("enabled")]
    [Description ("Разрешена")]
    public bool Enabled
    {
        get => _enabled;
        set
        {
            if (_enabled != value)
            {
                _enabled = value;
                PerformChange();
            }
        }
    }

    /// <summary>
    /// Заглавие команды (произвольное).
    /// </summary>
    [XmlAttribute ("title")]
    [JsonPropertyName ("title")]
    [Description ("Заглавие")]
    public string? Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                PerformChange();
            }
        }
    }

    /// <summary>
    /// Описание команды в произвольной форме.
    /// </summary>
    [XmlAttribute ("description")]
    [JsonPropertyName ("description")]
    [Description ("Описание команды")]
    public string? Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                _description = value;
                PerformChange();
            }
        }
    }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public object? UserData
    {
        get => _userData;
        set
        {
            _userData = value;
            PerformChange();
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Command()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="title">Название команды (произвольное).
    /// </param>
    /// <param name="description">Описание команды в произвольной
    /// форме.</param>
    /// <param name="enabled">Команда разрешена к выполнению?</param>
    public Command
        (
            string? title,
            string? description = default,
            bool enabled = true
        )
    {
        _enabled = enabled;
        _title = title;
        _description = description;
    }

    #endregion

    #region Private members

    private bool _enabled;
    private string? _title, _description;
    private object? _userData;

    #endregion

    #region Public methods

    /// <inheritdoc cref="ICommand.PerformExecute()"/>
    public virtual void PerformExecute()
    {
        if (Enabled)
        {
            Execute?.Invoke (this, EventArgs.Empty);
        }
    }

    /// <inheritdoc cref="ICommand.PerformExecute(System.EventArgs)"/>
    public virtual void PerformExecute
        (
            EventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        if (Enabled)
        {
            Execute?.Invoke (this, eventArgs);
        }
    }

    /// <inheritdoc cref="ICommand.PerformExecuteAsync()"/>
    public virtual Task PerformExecuteAsync() => Enabled
            ? Execute.RaiseAsync (this)
            : Task.CompletedTask;

    /// <inheritdoc cref="ICommand.PerformExecuteAsync(System.EventArgs)"/>
    public virtual Task PerformExecuteAsync
        (
            EventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        return Enabled
            ? Execute.RaiseAsync (this, eventArgs)
            : Task.CompletedTask;
    }

    /// <inheritdoc cref="ICommand.PerformUpdate()"/>
    public virtual void PerformUpdate()
    {
        Update?.Invoke (this, EventArgs.Empty);
    }

    /// <inheritdoc cref="ICommand.PerformUpdate(System.EventArgs)"/>
    public virtual void PerformUpdate
        (
            EventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        Update?.Invoke (this, eventArgs);
    }

    /// <inheritdoc cref="ICommand.PerformUpdateAsync()"/>
    public virtual Task PerformUpdateAsync() =>
        Update.RaiseAsync (this);

    /// <inheritdoc cref="ICommand.PerformUpdateAsync(System.EventArgs)"/>
    public virtual Task PerformUpdateAsync
        (
            EventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        return Update.RaiseAsync (this, eventArgs);
    }

    /// <inheritdoc cref="ICommand.PerformChange()"/>
    public virtual void PerformChange() =>
        Changed?.Invoke (this, EventArgs.Empty);

    /// <inheritdoc cref="ICommand.PerformChange(System.EventArgs)"/>
    public virtual void PerformChange
        (
            EventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        Changed?.Invoke (this, eventArgs);
    }

    /// <inheritdoc cref="ICommand.PerformChangeAsync()"/>
    public virtual Task PerformChangeAsync() => Changed.RaiseAsync (this);

    /// <inheritdoc cref="ICommand.PerformChangeAsync(System.EventArgs)"/>
    public virtual Task PerformChangeAsync
        (
            EventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        return Changed.RaiseAsync (this, eventArgs);
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public virtual void Dispose() =>
        Disposed?.Invoke (this, EventArgs.Empty);

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Title}: {Description} [{Enabled}]";

    #endregion
}
