// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

#nullable enable

namespace ManagedIrbis;

public sealed class SubField
{
    public char Code { get; set; }
    public string? Value { get; set; }

    public SubField()
    {
        // пустое тело конструктора
    }

    public SubField
        (
            char code,
            string? value = null
        )
    {
        Code = code;
        Value = value;
    }

    public override string ToString()
    {
        return $"^{Code}{Value}";
    }
}

