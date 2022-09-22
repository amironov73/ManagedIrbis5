// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

#region Using directives

using System;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using System.Xml.Serialization;

using AM;
using AM.Json;

#endregion

#nullable enable

namespace SiberianTests;

public sealed class SiberianTest
    : ISiberianTest
{
    #region Properties

    /// <summary>
    /// Имя класса с тестом.
    /// </summary>
    [XmlAttribute ("class")]
    [JsonPropertyName ("class")]
    public string? ClassName { get; set; }

    /// <summary>
    /// Заголовок.
    /// </summary>
    [XmlAttribute ("title")]
    [JsonPropertyName ("title")]
    public string? Title { get; set; }

    #endregion

    #region Public methods

    public static SiberianTest[] LoadFromFile (string fileName) =>
        JsonUtility.ReadObjectFromFile<SiberianTest[]> (fileName);

    #endregion

    #region IFormsTest members

    /// <summary>
    /// Run the test.
    /// </summary>
    public void RunTest
        (
            IWin32Window? ownerWindow
        )
    {
        var type = Type.GetType
                (
                    ClassName.ThrowIfNull(),
                    true
                )
            .ThrowIfNull ("Type.GetType");

        var testObject = (ISiberianTest)Activator.CreateInstance (type)
            .ThrowIfNull ("Activator.CreateInstance");

        testObject.RunTest (ownerWindow);

        // ReSharper disable once SuspiciousTypeConversion.Global
        (testObject as IDisposable)?.Dispose();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => Title.ToVisibleString();

    #endregion
}
