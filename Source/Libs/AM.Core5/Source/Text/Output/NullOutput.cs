// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* NullOutput.cs -- пустой объект вывода
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Text.Output;

/// <summary>
/// Пустой (нулевой) объект вывода.
/// </summary>
public sealed class NullOutput
    : AbstractOutput
{
    #region AbstractOutput members

    /// <inheritdoc cref="AbstractOutput.HaveError" />
    public override bool HaveError { get; set; }

    /// <inheritdoc cref="AbstractOutput.Clear" />
    public override AbstractOutput Clear()
    {
        HaveError = false;

        return this;
    }

    /// <inheritdoc cref="AbstractOutput.Configure" />
    public override AbstractOutput Configure
        (
            string configuration
        )
    {
        // Ничего делать не надо

        return this;
    }

    /// <inheritdoc cref="AbstractOutput.Write(string)" />
    public override AbstractOutput Write
        (
            string text
        )
    {
        // Ничего не нужно делать

        return this;
    }

    /// <inheritdoc cref="AbstractOutput.WriteError(string)" />
    public override AbstractOutput WriteError
        (
            string text
        )
    {
        // Больше ничего не нужно делать
        HaveError = true;

        return this;
    }

    #endregion
}
