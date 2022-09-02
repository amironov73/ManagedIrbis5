// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* Label.cs -- текстовый заголовок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// Класс, содержащий данные, связанные с текстовым заголовком
/// и связанными с ним свойствами шрифта.
/// </summary>
[Serializable]
public class Label
    : ICloneable, ISerializable
{
    #region Constructors

    /// <summary>
    /// Constructor to build an <see cref="AxisLabel" /> from the text and the
    /// associated font properties.
    /// </summary>
    /// <param name="text">The <see cref="string" /> representing the text to be
    /// displayed</param>
    /// <param name="fontFamily">The <see cref="String" /> font family name</param>
    /// <param name="fontSize">The size of the font in points and scaled according
    /// to the <see cref="PaneBase.CalcScaleFactor" /> logic.</param>
    /// <param name="color">The <see cref="Color" /> instance representing the color
    /// of the font</param>
    /// <param name="isBold">true for a bold font face</param>
    /// <param name="isItalic">true for an italic font face</param>
    /// <param name="isUnderline">true for an underline font face</param>
    public Label
        (
            string? text,
            string fontFamily,
            float fontSize,
            Color color,
            bool isBold,
            bool isItalic,
            bool isUnderline
        )
    {
        Text = text ?? string.Empty;

        FontSpec = new FontSpec (fontFamily, fontSize, color, isBold, isItalic, isUnderline);
        IsVisible = true;
    }

    /// <summary>
    /// Constructor that builds a <see cref="Label" /> from a text <see cref="string" />
    /// and a <see cref="FontSpec" /> instance.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="fontSpec"></param>
    public Label
        (
            string? text,
            FontSpec? fontSpec
        )
    {
        Text = text ?? string.Empty;

        FontSpec = fontSpec;
        IsVisible = true;
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="rhs">the <see cref="Label" /> instance to be copied.</param>
    public Label (Label rhs)
    {
        if (rhs.Text != null)
        {
            Text = (string)rhs.Text.Clone();
        }
        else
        {
            Text = string.Empty;
        }

        IsVisible = rhs.IsVisible;
        FontSpec = rhs.FontSpec?.Clone();
    }

    /// <inheritdoc cref="ICloneable.Clone"/>
    object ICloneable.Clone()
    {
        return Clone();
    }

    /// <summary>
    /// Typesafe, deep-copy clone method.
    /// </summary>
    /// <returns>A new, independent copy of this class</returns>
    public Label Clone()
    {
        return new Label (this);
    }

    #endregion

    #region Properties

    /// <summary>
    /// The <see cref="String" /> text to be displayed
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// A <see cref="Charting.FontSpec" /> instance representing the font properties
    /// for the displayed text.
    /// </summary>
    public FontSpec? FontSpec { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that determines whether or not this label will be displayed.
    /// </summary>
    public bool IsVisible { get; set; }

    #endregion

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>
    public const int schema = 10;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected Label
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema").NotUsed();

        Text = info.GetString ("text");
        IsVisible = info.GetBoolean ("isVisible");
        FontSpec = (FontSpec?) info.GetValue ("fontSpec", typeof (FontSpec));
    }

    /// <inheritdoc cref="ISerializable.GetObjectData"/>
    public virtual void GetObjectData
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        info.AddValue ("schema", schema);
        info.AddValue ("text", Text);
        info.AddValue ("isVisible", IsVisible);
        info.AddValue ("fontSpec", FontSpec);
    }

    #endregion
}
