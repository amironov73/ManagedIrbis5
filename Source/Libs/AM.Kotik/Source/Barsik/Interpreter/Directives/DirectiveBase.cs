// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DirectiveBase.cs -- абстрактная директива интерпретатора
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System.Text.Json.Serialization;

using AM.Json;

namespace AM.Kotik.Barsik.Directives;

/// <summary>
/// Абстрактная директива интерпретатора.
/// </summary>
[JsonConverter (typeof (AnyTypeConverter<DirectiveBase>))]
public abstract class DirectiveBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected DirectiveBase
        (
            string command
        )
    {
        Sure.NotNullNorEmpty (command);

        _command = command;
    }

    #endregion

    #region Private members

    private readonly string _command;

    #endregion

    #region Public methods

    /// <summary>
    /// Исполнение директивы.
    /// </summary>
    public abstract void Execute
        (
            Context context,
            string? argument
        );

    /// <summary>
    /// Распознание директивы.
    /// </summary>
    public bool Recognize
        (
            string command,
            string? argument
        )
    {
        Sure.NotNullNorEmpty (command);
        argument.NotUsed();

        return string.CompareOrdinal (command, _command) == 0;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => _command;

    #endregion
}
