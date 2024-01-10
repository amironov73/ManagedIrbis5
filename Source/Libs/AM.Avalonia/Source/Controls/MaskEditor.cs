// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MaskEditor.cs -- редактирование маски изображения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;

using JetBrains.Annotations;

#endregion

namespace AM.Avalonia.Controls;

/// <summary>
/// Редактирование маски изображения.
/// </summary>
[PublicAPI]
public sealed class MaskEditor
    : Control
{
    #region Private members

    private IImage? _image;
    private IImage? _mask;
    private bool _circleVisible;
    private Point _circlePoint;
    private double _circleSize = 10;
    private bool _drawing;

    private Rect GetImageRect()
    {
        var image = _image.ThrowIfNull();
        var imageBounds = new Rect (image.Size);
        return AvaloniaUtility.Fit (Bounds, imageBounds);
    }

    private Rect GetMaskRect()
    {
        var mask = _mask.ThrowIfNull();
        var imaskBounds = new Rect (mask.Size);
        return AvaloniaUtility.Fit (Bounds, imaskBounds);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Установка изображения, для которого редактируется маска.
    /// </summary>
    public void SetImage
        (
            IImage image
        )
    {
        Sure.NotNull (image);

        _image = image;
        InvalidateVisual();
    }

    /// <summary>
    /// Установка изображения, для которого редактируется маска.
    /// </summary>
    public void SetMask
        (
            IImage image
        )
    {
        Sure.NotNull (image);

        _mask = image;
        InvalidateVisual();
    }

    private void _Draw()
    {
        if (_image is null
            || _mask is null
            || !_drawing)
        {
            return;
        }

        var fittedImage = GetImageRect();
        Debug.WriteLine ($"Fitted image: {fittedImage}");
        var fittedMask = GetMaskRect();
        Debug.WriteLine ($"Fitted mask: {fittedMask}");
        // using var drawingContext = _mask.CreateDrawingContext();
        // drawingContext.FillRectangle
        //     (
        //         Brushes.Aqua,
        //         new Rect (fittedMask.Size)
        //     );
        // var circleCenter = new Point
        //     (
        //         fittedMask.X + _circlePoint.X,
        //         fittedMask.Y + _circlePoint.Y
        //     );
        // Debug.WriteLine ($"Circle center: {circleCenter}");
        // drawingContext.DrawEllipse
        //     (
        //         Brushes.Red,
        //         null,
        //         circleCenter,
        //         _circleSize,
        //         _circleSize
        //     );
        //
        // _mask.Save ("mask.png", 100);
    }

    #endregion

    #region Control members

    /// <inheritdoc cref="OnPointerPressed"/>
    protected override void OnPointerPressed
        (
            PointerPressedEventArgs eventArgs
        )
    {
        base.OnPointerPressed (eventArgs);
        _drawing = true;
        _Draw();
        InvalidateVisual();
    }

    /// <inheritdoc cref="Control.OnPointerReleased"/>
    protected override void OnPointerReleased
        (
            PointerReleasedEventArgs eventArgs
        )
    {
        base.OnPointerReleased (eventArgs);
        _drawing = false;
    }

    /// <inheritdoc cref="InputElement.OnPointerMoved"/>
    protected override void OnPointerMoved
        (
            PointerEventArgs eventArgs
        )
    {
        base.OnPointerMoved (eventArgs);

        if (_image is null)
        {
            return;
        }

        var cursorPosition = eventArgs.GetPosition (this);
        var imageRect = GetImageRect();
        cursorPosition = new Point
            (
                cursorPosition.X - imageRect.X,
                cursorPosition.Y - imageRect.Y
            );
        Debug.WriteLine ($"Cursor position: {cursorPosition}");
        _circleVisible = cursorPosition.X >= 0 && cursorPosition.X < imageRect.Width
            && cursorPosition.Y > 0 && cursorPosition.Y < imageRect.Height;
        if (_circleVisible)
        {
            _circlePoint = cursorPosition;
            // TODO скрыть курсор

            if (_drawing)
            {
                _Draw();
            }
        }
        else
        {
            // TODO показать курсор
        }

        InvalidateVisual();
    }

    /// <inheritdoc cref="Visual.Render"/>
    public override void Render
        (
            DrawingContext context
        )
    {
        Sure.NotNull (context);

        context.FillRectangle
            (
                Brushes.Coral,
                Bounds
            );

        if (_image is not null)
        {
            var imageBounds = new Rect (_image.Size);
            var fittedImage = GetImageRect();
            context.DrawImage (_image, imageBounds, fittedImage);
            if (_mask is not null)
            {
                var maskBounds = new Rect (_mask.Size);
                var fittedMask = GetMaskRect();
                context.DrawImage (_mask, maskBounds, fittedMask);

                // var visual = new Visual();
                // _mask.Render (visual);
            }

            if (_circleVisible)
            {
                var circleCenter = new Point
                    (
                        fittedImage.X + _circlePoint.X,
                        fittedImage.Y + _circlePoint.Y
                    );
                context.DrawEllipse
                    (
                        Brushes.White,
                        new Pen (Brushes.Black),
                        circleCenter,
                        _circleSize,
                        _circleSize
                    );
            }
        }
    }

    #endregion
}
