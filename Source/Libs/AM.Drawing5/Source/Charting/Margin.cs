// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* Margin.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// Class that handles that stores the margin properties for the GraphPane
/// </summary>
[Serializable]
public class Margin
    : ICloneable, ISerializable
{
    #region Constructors

    /// <summary>
    /// Constructor to build a <see cref="Margin" /> from the default values.
    /// </summary>
    public Margin()
    {
        Left = Default.Left;
        Right = Default.Right;
        Top = Default.Top;
        Bottom = Default.Bottom;
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="rhs">the <see cref="Margin" /> instance to be copied.</param>
    public Margin (Margin rhs)
    {
        Left = rhs.Left;
        Right = rhs.Right;
        Top = rhs.Top;
        Bottom = rhs.Bottom;
    }

    /// <summary>
    /// Implement the <see cref="ICloneable" /> interface in a typesafe manner by just
    /// calling the typed version of <see cref="Clone" />
    /// </summary>
    /// <returns>A deep copy of this object</returns>
    object ICloneable.Clone()
    {
        return Clone();
    }

    /// <summary>
    /// Typesafe, deep-copy clone method.
    /// </summary>
    /// <returns>A new, independent copy of this class</returns>
    public Margin Clone()
    {
        return new Margin (this);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a float value that determines the margin area between the left edge of the
    /// <see cref="PaneBase.Rect"/> rectangle and the features of the graph.
    /// </summary>
    /// <value>This value is in units of points (1/72 inch), and is scaled
    /// linearly with the graph size.</value>
    /// <seealso cref="Default.Left"/>
    /// <seealso cref="PaneBase.IsFontsScaled"/>
    /// <seealso cref="Right"/>
    /// <seealso cref="Top"/>
    /// <seealso cref="Bottom"/>
    [field: CLSCompliant (false)]
    public float Left { get; set; }

    /// <summary>
    /// Gets or sets a float value that determines the margin area between the right edge of the
    /// <see cref="PaneBase.Rect"/> rectangle and the features of the graph.
    /// </summary>
    /// <value>This value is in units of points (1/72 inch), and is scaled
    /// linearly with the graph size.</value>
    /// <seealso cref="Default.Right"/>
    /// <seealso cref="PaneBase.IsFontsScaled"/>
    /// <seealso cref="Left"/>
    /// <seealso cref="Top"/>
    /// <seealso cref="Bottom"/>
    [field: CLSCompliant (false)]
    public float Right { get; set; }

    /// <summary>
    /// Gets or sets a float value that determines the margin area between the top edge of the
    /// <see cref="PaneBase.Rect"/> rectangle and the features of the graph.
    /// </summary>
    /// <value>This value is in units of points (1/72 inch), and is scaled
    /// linearly with the graph size.</value>
    /// <seealso cref="Default.Top"/>
    /// <seealso cref="PaneBase.IsFontsScaled"/>
    /// <seealso cref="Left"/>
    /// <seealso cref="Right"/>
    /// <seealso cref="Bottom"/>
    [field: CLSCompliant (false)]
    public float Top { get; set; }

    /// <summary>
    /// Gets or sets a float value that determines the margin area between the bottom edge of the
    /// <see cref="PaneBase.Rect"/> rectangle and the features of the graph.
    /// </summary>
    /// <value>This value is in units of points (1/72 inch), and is scaled
    /// linearly with the graph size.</value>
    /// <seealso cref="Default.Bottom"/>
    /// <seealso cref="PaneBase.IsFontsScaled"/>
    /// <seealso cref="Left"/>
    /// <seealso cref="Right"/>
    /// <seealso cref="Top"/>
    [field: CLSCompliant (false)]
    public float Bottom { get; set; }

    /// <summary>
    /// Concurrently sets all outer margin values to a single value.
    /// </summary>
    /// <value>This value is in units of points (1/72 inch), and is scaled
    /// linearly with the graph size.</value>
    /// <seealso cref="PaneBase.IsFontsScaled"/>
    /// <seealso cref="Bottom"/>
    /// <seealso cref="Left"/>
    /// <seealso cref="Right"/>
    /// <seealso cref="Top"/>
    public float All
    {
        set
        {
            Bottom = value;
            Top = value;
            Left = value;
            Right = value;
        }
    }

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
    protected Margin (SerializationInfo info, StreamingContext context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema");

        Left = info.GetSingle ("left");
        Right = info.GetSingle ("right");
        Top = info.GetSingle ("top");
        Bottom = info.GetSingle ("bottom");
    }

    /// <summary>
    /// Populates a <see cref="SerializationInfo"/> instance with the data needed to serialize the target object
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data</param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data</param>
    public virtual void GetObjectData
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        info.AddValue ("schema", schema);

        info.AddValue ("left", Left);
        info.AddValue ("right", Right);
        info.AddValue ("top", Top);
        info.AddValue ("bottom", Bottom);
    }

    #endregion

    #region Defaults

    /// <summary>
    /// A simple struct that defines the default property values for the <see cref="Margin"/> class.
    /// </summary>
    public class Default
    {
        /// <summary>
        /// The default value for the <see cref="Margin.Left"/> property, which is
        /// the size of the space on the left side of the <see cref="PaneBase.Rect"/>.
        /// </summary>
        /// <value>Units are points (1/72 inch)</value>
        public static float Left = 10.0F;

        /// <summary>
        /// The default value for the <see cref="Margin.Right"/> property, which is
        /// the size of the space on the right side of the <see cref="PaneBase.Rect"/>.
        /// </summary>
        /// <value>Units are points (1/72 inch)</value>
        public static float Right = 10.0F;

        /// <summary>
        /// The default value for the <see cref="Margin.Top"/> property, which is
        /// the size of the space on the top side of the <see cref="PaneBase.Rect"/>.
        /// </summary>
        /// <value>Units are points (1/72 inch)</value>
        public static float Top = 10.0F;

        /// <summary>
        /// The default value for the <see cref="Margin.Bottom"/> property, which is
        /// the size of the space on the bottom side of the <see cref="PaneBase.Rect"/>.
        /// </summary>
        /// <value>Units are points (1/72 inch)</value>
        public static float Bottom = 10.0F;
    }

    #endregion
}
