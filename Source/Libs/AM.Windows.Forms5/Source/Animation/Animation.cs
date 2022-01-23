// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Animation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Animation;

/// <summary>
/// Settings of animation
/// </summary>
public class Animation
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Visible),
     EditorBrowsable (EditorBrowsableState.Advanced), TypeConverter (typeof (PointFConverter))]
    public PointF SlideCoefficient { get; set; }

    /// <summary>
    ///
    /// </summary>
    public float RotateCoefficient { get; set; }

    /// <summary>
    ///
    /// </summary>
    public float RotateLimit { get; set; }

    /// <summary>
    ///
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Visible),
     EditorBrowsable (EditorBrowsableState.Advanced), TypeConverter (typeof (PointFConverter))]
    public PointF ScaleCoefficient { get; set; }

    /// <summary>
    ///
    /// </summary>
    public float TransparencyCoefficient { get; set; }

    /// <summary>
    ///
    /// </summary>
    public float LeafCoefficient { get; set; }

    /// <summary>
    ///
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Visible),
     EditorBrowsable (EditorBrowsableState.Advanced), TypeConverter (typeof (PointFConverter))]
    public PointF MosaicShift { get; set; }

    /// <summary>
    ///
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Visible),
     EditorBrowsable (EditorBrowsableState.Advanced), TypeConverter (typeof (PointFConverter))]
    public PointF MosaicCoefficient { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int MosaicSize { get; set; }

    /// <summary>
    ///
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Visible),
     EditorBrowsable (EditorBrowsableState.Advanced), TypeConverter (typeof (PointFConverter))]
    public PointF BlindCoefficient { get; set; }

    /// <summary>
    ///
    /// </summary>
    public float TimeCoefficient { get; set; }

    /// <summary>
    ///
    /// </summary>
    public float MinTime { get; set; }

    /// <summary>
    ///
    /// </summary>
    public float MaxTime { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Padding Padding { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool AnimateOnlyDifferences { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Animation()
    {
        MinTime = 0.0f;
        MaxTime = 1.0f;
        AnimateOnlyDifferences = true;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Клонирование.
    /// </summary>
    public Animation Clone()
    {
        return (Animation) MemberwiseClone();
    }

    /// <summary>
    /// Вращение.
    /// </summary>
    public static Animation Rotate => new()
    {
        RotateCoefficient = 1.0f,
        TransparencyCoefficient = 1.0f,
        Padding = new Padding (50, 50, 50, 50)
    };

    /// <summary>
    /// Горизонтальный сдвиг.
    /// </summary>
    public static Animation HorizontalSlide => new()
    {
        SlideCoefficient = new PointF (1, 0)
    };

    /// <summary>
    /// Вертикальный сдвиг.
    /// </summary>
    public static Animation VerticalSlide => new()
    {
        SlideCoefficient = new PointF (0, 1)
    };

    /// <summary>
    ///  Масштабирование.
    /// </summary>
    public static Animation Scale => new()
    {
        ScaleCoefficient = new PointF (1, 1)
    };

    /// <summary>
    /// Масштабирование и вращение
    /// </summary>
    public static Animation ScaleAndRotate => new()
    {
        ScaleCoefficient = new PointF (1, 1),
        RotateCoefficient = 0.5f,
        RotateLimit = 0.2f,
        Padding = new Padding (30, 30, 30, 30)
    };

    /// <summary>
    /// Горизонтальный сдвиг и вращение.
    /// </summary>
    public static Animation HorizontalSlideAndRotate => new()
    {
        SlideCoefficient = new PointF (1, 0),
        RotateCoefficient = 0.3f,
        RotateLimit = 0.2f,
        Padding = new Padding (50, 50, 50, 50)
    };

    /// <summary>
    /// Масштабирование и горизонтальный сдвиг.
    /// </summary>
    public static Animation ScaleAndHorizontalSlide => new()
    {
        ScaleCoefficient = new PointF (1, 1),
        SlideCoefficient = new PointF (1, 0),
        Padding = new Padding (30, 0, 0, 0)
    };

    /// <summary>
    /// Прозрачность.
    /// </summary>
    public static Animation Transparent => new()
    {
        TransparencyCoefficient = 1
    };

    /// <summary>
    /// Створки.
    /// </summary>
    public static Animation Leaf => new()
    {
        LeafCoefficient = 1
    };

    /// <summary>
    /// Мозаика.
    /// </summary>
    public static Animation Mosaic => new()
    {
        MosaicCoefficient = new PointF (100f, 100f),
        MosaicSize = 20,
        Padding = new Padding (30, 30, 30, 30)
    };

    /// <summary>
    /// Частицы.
    /// </summary>
    public static Animation Particles => new()
    {
        MosaicCoefficient = new PointF (200, 200),
        MosaicSize = 1,
        MosaicShift = new PointF (0, 0.5f),
        Padding = new Padding (100, 50, 100, 150),
        TimeCoefficient = 2
    };

    /// <summary>
    /// Вертикальные жалюзи.
    /// </summary>
    public static Animation VerticalBlind => new()
    {
        BlindCoefficient = new PointF (0f, 1f)
    };

    /// <summary>
    /// Горизонтальные жалюзи.
    /// </summary>
    public static Animation HorizBlind => new()
    {
        BlindCoefficient = new PointF (1f, 0f)
    };


    /// <summary>
    /// Добавление анимации.
    /// </summary>
    public void Add
        (
            Animation animation
        )
    {
        Sure.NotNull (animation);

        SlideCoefficient = new PointF (SlideCoefficient.X + animation.SlideCoefficient.X, SlideCoefficient.Y + animation.SlideCoefficient.Y);
        RotateCoefficient += animation.RotateCoefficient;
        RotateLimit += animation.RotateLimit;
        ScaleCoefficient = new PointF (ScaleCoefficient.X + animation.ScaleCoefficient.X, ScaleCoefficient.Y + animation.ScaleCoefficient.Y);
        TransparencyCoefficient += animation.TransparencyCoefficient;
        LeafCoefficient += animation.LeafCoefficient;
        MosaicShift = new PointF (MosaicShift.X + animation.MosaicShift.X, MosaicShift.Y + animation.MosaicShift.Y);
        MosaicCoefficient = new PointF (MosaicCoefficient.X + animation.MosaicCoefficient.X, MosaicCoefficient.Y + animation.MosaicCoefficient.Y);
        MosaicSize += animation.MosaicSize;
        BlindCoefficient = new PointF (BlindCoefficient.X + animation.BlindCoefficient.X, BlindCoefficient.Y + animation.BlindCoefficient.Y);
        TimeCoefficient += animation.TimeCoefficient;
        Padding += animation.Padding;
    }

    #endregion
}
