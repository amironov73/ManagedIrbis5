// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Абстрактная полезная нагрузка.
/// </summary>
public abstract class Payload
{
    #region Properties

    /// <summary>
    /// Версия.
    /// </summary>
    public virtual int Version => -1;

    /// <summary>
    /// Уровень коррекции ошибок.
    /// </summary>
    public virtual QRCodeGenerator.ECCLevel EccLevel => QRCodeGenerator.ECCLevel.M;

    /// <summary>
    /// Extended Channel Interpretation.
    /// </summary>
    public virtual QRCodeGenerator.EciMode EciMode => QRCodeGenerator.EciMode.Default;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public abstract override string ToString();

    #endregion
}
