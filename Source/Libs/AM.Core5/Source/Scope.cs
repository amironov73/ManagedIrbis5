// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Scope.cs -- область, по выходу из которой будет произведено определенное действие
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM;

/// <summary>
/// Область, по выходу из которой будет произведено определенное действие.
/// </summary>
public sealed class Scope
{
    #region Properties

    /// <summary>
    /// An empty scope, which does nothing when disposed.
    /// </summary>
    public static readonly Scope Empty = new Scope (null);

    #endregion

    #region Construciton

    private Scope (Action? dispose) => cleanAction = dispose;

    #endregion

    #region Private members

    private Action? cleanAction;

    #endregion

    #region Public methods

    /// <summary>
    /// Creates a <see cref="Scope" /> for the specified delegate.
    /// </summary>
    /// <param name="dispose">The delegate.</param>
    /// <returns>An instance of <see cref="Scope" /> that calls the delegate when disposed.</returns>
    /// <remarks>If dispose is null, the instance does nothing when disposed.</remarks>
    public static Scope Create (Action? dispose) => new (dispose);

    /// <summary>
    /// Creates a <see cref="Scope" /> that disposes the specified object.
    /// </summary>
    /// <param name="disposable">The object to dispose.</param>
    /// <returns>An instance of <see cref="Scope" /> that disposes the object when disposed.</returns>
    /// <remarks>If disposable is null, the instance does nothing when disposed.</remarks>
    public static Scope Create<T> (T? disposable)
        where T : IDisposable => disposable is null ? Empty : new Scope (disposable.Dispose);

    /// <summary>
    /// Cancel the call to the encapsulated delegate.
    /// </summary>
    /// <remarks>After calling this method, disposing this instance does nothing.</remarks>
    public void Cancel() => cleanAction = null;

    /// <summary>
    /// Returns a new Scope that will call the encapsulated delegate.
    /// </summary>
    /// <returns>A new Scope that will call the encapsulated delegate.</returns>
    /// <remarks>After calling this method, disposing this instance does nothing.</remarks>
    public Scope Transfer()
    {
        var scope = new Scope (cleanAction);
        cleanAction = null;
        return scope;
    }

    #endregion

    #region IDisposable members

    /// <summary>
    /// Calls the encapsulated delegate.
    /// </summary>
    public void Dispose()
    {
        if (cleanAction is not null)
        {
            cleanAction();
            cleanAction = null;
        }
    }

    #endregion
}
