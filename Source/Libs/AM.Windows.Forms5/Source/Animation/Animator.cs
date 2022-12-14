// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Animator.cs --
 * Ars Magna project, http://arsmagna.ru
 */

//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE.
//
//  License: GNU Lesser General Public License (LGPLv3)
//
//  Email: pavel_torgashov@mail.ru.
//
//  Copyright (C) Pavel Torgashov, 2013.

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Animation;

/// <summary>
/// Animation manager
/// </summary>
[ProvideProperty ("Decoration", typeof (Control))]
public class Animator : Component, IExtenderProvider
{
    private IContainer? _components = null;
    protected List<QueueItem> queue = new ();
    private Thread? _thread;

    #region Events

    /// <summary>
    /// Occurs when animation of the control is completed
    /// </summary>
    public event EventHandler<AnimationCompletedEventArg>? AnimationCompleted;

    /// <summary>
    /// Ocuurs when all animations are completed
    /// </summary>
    public event EventHandler? AllAnimationsCompleted;

    /// <summary>
    /// Occurs when needed transform matrix
    /// </summary>
    public event EventHandler<TransfromNeededEventArg>? TransfromNeeded;

    /// <summary>
    /// Occurs when needed non-linear transformation
    /// </summary>
    public event EventHandler<NonLinearTransfromNeededEventArg>? NonLinearTransfromNeeded;

    /// <summary>
    /// Occurs when user click on the animated control
    /// </summary>
    public event EventHandler<MouseEventArgs>? MouseDown;

    /// <summary>
    /// Occurs when frame of animation is painting
    /// </summary>
    public event EventHandler<PaintEventArgs>? FramePainted;

    #endregion

    /// <summary>
    /// Max time of animation (ms)
    /// </summary>
    [DefaultValue (1500)]
    public int MaxAnimationTime { get; set; }

    /// <summary>
    /// Default animation
    /// </summary>
    [TypeConverter (typeof (ExpandableObjectConverter))]
    public Animation? DefaultAnimation { get; set; }

    /// <summary>
    /// Cursor of animated control
    /// </summary>
    [DefaultValue (typeof (Cursor), "Default")]
    public Cursor Cursor { get; set; }

    /// <summary>
    /// Are all animations completed?
    /// </summary>
    public bool IsCompleted
    {
        get
        {
            lock (queue) return queue.Count == 0;
        }
    }

    /// <summary>
    /// Interval between frames (ms)
    /// </summary>
    [DefaultValue (10)]
    public int Interval { get; set; }

    private AnimationType _animationType;

    /// <summary>
    /// Type of built-in animation
    /// </summary>
    public AnimationType AnimationType
    {
        get => _animationType;
        set
        {
            _animationType = value;
            InitDefaultAnimation (_animationType);
        }
    }

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Animator()
    {
        Init();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="container"></param>
    public Animator
        (
            IContainer container
        )
    {
        Sure.NotNull (container);

        container.Add (this);
        Init();
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    protected virtual void Init()
    {
        DefaultAnimation = new Animation();
        MaxAnimationTime = 1500;
        TimeStep = 0.02f;
        Interval = 10;

        Disposed += Animator_Disposed;

        //main working thread
        _thread = new Thread (Work);
        _thread.IsBackground = true;
        _thread.Start();
    }

    private void Animator_Disposed
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        ClearQueue();
        _thread.Abort();
    }

    private void Work()
    {
        while (true)
        {
            Thread.Sleep (Interval);
            try
            {
                var count = 0;
                var completed = new List<QueueItem>();
                var actived = new List<QueueItem>();

                //find completed
                lock (queue)
                {
                    count = queue.Count;
                    var wasActive = false;

                    foreach (var item in queue)
                    {
                        if (item.IsActive)
                        {
                            wasActive = true;
                        }

                        if (item.controller != null && item.controller.IsCompleted)
                        {
                            completed.Add (item);
                        }
                        else
                        {
                            if (item.IsActive)
                            {
                                if ((DateTime.Now - item.ActivateTime).TotalMilliseconds > MaxAnimationTime)
                                {
                                    completed.Add (item);
                                }
                                else
                                {
                                    actived.Add (item);
                                }
                            }
                        }
                    }

                    //start next animation
                    if (!wasActive)
                    {
                        foreach (var item in queue)
                        {
                            if (!item.IsActive)
                            {
                                actived.Add (item);
                                item.IsActive = true;
                                break;
                            }
                        }
                    }
                }

                //completed
                foreach (var item in completed)
                {
                    OnCompleted (item);
                }

                //build next frame of DoubleBitmap
                foreach (var item in actived)
                {
                    try
                    {
                        //build next frame of DoubleBitmap
                        item.control.BeginInvoke (new MethodInvoker (() => DoAnimation (item)));
                    }
                    catch
                    {
                        //we can not start animation, remove from queue
                        OnCompleted (item);
                    }
                }

                if (count == 0)
                {
                    if (completed.Count > 0)
                    {
                        OnAllAnimationsCompleted();
                    }

                    CheckRequests();
                }
            }
            catch
            {
                //form was closed
            }
        }
    }

    /// <summary>
    /// Check result state of controls
    /// </summary>
    private void CheckRequests()
    {
        var toRemove = new List<QueueItem>();

        lock (_requests)
        {
            var dict = new Dictionary<Control, QueueItem>();
            foreach (var item in _requests)
            {
                if (item.control != null)
                {
                    if (dict.ContainsKey (item.control))
                    {
                        toRemove.Add (dict[item.control]);
                    }

                    dict[item.control] = item;
                }
                else
                {
                    toRemove.Add (item);
                }
            }

            foreach (var item in dict.Values)
            {
                if (item.control != null && !IsStateOK (item.control, item.mode))
                {
                    RepairState (item.control, item.mode);
                }
                else
                {
                    toRemove.Add (item);
                }
            }

            foreach (var item in toRemove)
            {
                _requests.Remove (item);
            }
        }
    }

    private bool IsStateOK (Control control, AnimateMode mode)
    {
        switch (mode)
        {
            case AnimateMode.Hide: return !control.Visible;
            case AnimateMode.Show: return control.Visible;
        }

        return true;
    }

    private void RepairState (Control control, AnimateMode mode)
    {
        control.BeginInvoke (new MethodInvoker (() =>
        {
            switch (mode)
            {
                case AnimateMode.Hide:
                    control.Visible = false;
                    break;
                case AnimateMode.Show:
                    control.Visible = true;
                    break;
            }
        }));
    }

    private void DoAnimation (QueueItem item)
    {
        if (Monitor.TryEnter (item))
        {
            try
            {
                if (item.controller == null)
                {
                    item.controller = CreateDoubleBitmap (item.control, item.mode, item.animation,
                        item.clipRectangle);
                }

                if (item.controller.IsCompleted)
                {
                    return;
                }

                item.controller.BuildNextFrame();
            }
            catch
            {
                OnCompleted (item);
            }
        }
    }

    private void InitDefaultAnimation (AnimationType animationType)
    {
        switch (animationType)
        {
            case AnimationType.Custom:
                break;

            case AnimationType.Rotate:
                DefaultAnimation = Animation.Rotate;
                break;

            case AnimationType.HorizontalSlide:
                DefaultAnimation = Animation.HorizontalSlide;
                break;

            case AnimationType.VerticalSlide:
                DefaultAnimation = Animation.VerticalSlide;
                break;

            case AnimationType.Scale:
                DefaultAnimation = Animation.Scale;
                break;

            case AnimationType.ScaleAndRotate:
                DefaultAnimation = Animation.ScaleAndRotate;
                break;

            case AnimationType.HorizontalSlideAndRotate:
                DefaultAnimation = Animation.HorizontalSlideAndRotate;
                break;

            case AnimationType.ScaleAndHorizontalSlide:
                DefaultAnimation = Animation.ScaleAndHorizontalSlide;
                break;

            case AnimationType.Transparent:
                DefaultAnimation = Animation.Transparent;
                break;

            case AnimationType.Leaf:
                DefaultAnimation = Animation.Leaf;
                break;

            case AnimationType.Mosaic:
                DefaultAnimation = Animation.Mosaic;
                break;

            case AnimationType.Particles:
                DefaultAnimation = Animation.Particles;
                break;

            case AnimationType.VerticalBlind:
                DefaultAnimation = Animation.VerticalBlind;
                break;

            case AnimationType.HorizontalBlind:
                DefaultAnimation = Animation.HorizBlind;
                break;
        }
    }

    /// <summary>
    /// Time step
    /// </summary>
    [DefaultValue (0.02f)]
    public float TimeStep { get; set; }

    /// <summary>
    /// Shows the control. As result the control will be shown with animation.
    /// </summary>
    /// <param name="control">Target control</param>
    /// <param name="parallel">Allows to animate it same time as other animations</param>
    /// <param name="animation">Personal animation</param>
    public void Show
        (
            Control control,
            bool parallel = false,
            Animation? animation = null
        )
    {
        AddToQueue (control, AnimateMode.Show, parallel, animation);
    }

    /// <summary>
    /// Shows the control and waits while animation will be completed. As result the control will be shown with animation.
    /// </summary>
    /// <param name="control">Target control</param>
    /// <param name="parallel">Allows to animate it same time as other animations</param>
    /// <param name="animation">Personal animation</param>
    public void ShowSync
        (
            Control control,
            bool parallel = false,
            Animation? animation = null
        )
    {
        Show (control, parallel, animation);
        WaitAnimation (control);
    }

    /// <summary>
    /// Hides the control. As result the control will be hidden with animation.
    /// </summary>
    /// <param name="control">Target control</param>
    /// <param name="parallel">Allows to animate it same time as other animations</param>
    /// <param name="animation">Personal animation</param>
    public void Hide
        (
            Control control,
            bool parallel = false,
            Animation? animation = null
        )
    {
        AddToQueue (control, AnimateMode.Hide, parallel, animation);
    }

    /// <summary>
    /// Hides the control and waits while animation will be completed. As result the control will be hidden with animation.
    /// </summary>
    /// <param name="control">Target control</param>
    /// <param name="parallel">Allows to animate it same time as other animations</param>
    /// <param name="animation">Personal animation</param>
    public void HideSync
        (
            Control control,
            bool parallel = false,
            Animation? animation = null
        )
    {
        Hide (control, parallel, animation);
        WaitAnimation (control);
    }

    /// <summary>
    /// It makes snapshot of the control before updating. It requires EndUpdate calling.
    /// </summary>
    /// <param name="control">Target control</param>
    /// <param name="parallel">Allows to animate it same time as other animations</param>
    /// <param name="animation">Personal animation</param>
    /// <param name="clipRectangle">Clip rectangle for animation</param>
    public void BeginUpdateSync
        (
            Control control,
            bool parallel = false,
            Animation? animation = null,
            Rectangle clipRectangle = default
        )
    {
        AddToQueue (control, AnimateMode.BeginUpdate, parallel, animation, clipRectangle);

        var wait = false;
        do
        {
            wait = false;
            lock (queue)
                foreach (var item in queue)
                {
                    if (item.control == control && item.mode == AnimateMode.BeginUpdate)
                    {
                        if (item.controller == null)
                        {
                            wait = true;
                        }
                    }
                }

            if (wait)
            {
                Application.DoEvents();
            }
        } while (wait);
    }

    /// <summary>
    /// Upadates control view with animation. It requires to call BeginUpdate before.
    /// </summary>
    /// <param name="control">Target control</param>
    public void EndUpdate (Control control)
    {
        lock (queue)
        {
            foreach (var item in queue)
            {
                if (item.control == control && item.mode == AnimateMode.BeginUpdate)
                {
                    item.controller.EndUpdate();
                    item.mode = AnimateMode.Update;
                }
            }
        }
    }

    /// <summary>
    /// Upadates control view with animation and waits while animation will be completed. It requires to call BeginUpdate before.
    /// </summary>
    /// <param name="control">Target control</param>
    public void EndUpdateSync
        (
            Control control
        )
    {
        EndUpdate (control);
        WaitAnimation (control);
    }

    /// <summary>
    /// Waits while all animations will completed.
    /// </summary>
    public void WaitAllAnimations()
    {
        while (!IsCompleted)
        {
            Application.DoEvents();
        }
    }

    /// <summary>
    /// Waits while animation of the control will completed.
    /// </summary>
    /// <param name="animatedControl"></param>
    public void WaitAnimation (Control animatedControl)
    {
        while (true)
        {
            var flag = false;
            lock (queue)
                foreach (var item in queue)
                {
                    if (item.control == animatedControl)
                    {
                        flag = true;
                        break;
                    }
                }

            if (!flag)
            {
                return;
            }

            Application.DoEvents();
        }
    }

    private readonly List<QueueItem> _requests = new ();

    private void OnCompleted
        (
            QueueItem item
        )
    {
        if (item.controller != null)
        {
            item.controller.Dispose();
        }

        lock (queue)
            queue.Remove (item);

        OnAnimationCompleted (new AnimationCompletedEventArg
            { Animation = item.animation, Control = item.control, Mode = item.mode });
    }

    /// <summary>
    /// Adds the contol to animation queue.
    /// </summary>
    /// <param name="control">Target control</param>
    /// <param name="mode">Animation mode</param>
    /// <param name="parallel">Allows to animate it same time as other animations</param>
    /// <param name="animation">Personal animation</param>
    /// <param name="clipRectangle"></param>
    public void AddToQueue
        (
            Control control,
            AnimateMode mode,
            bool parallel = true,
            Animation? animation = null,
            Rectangle clipRectangle = default
        )
    {
        if (animation == null)
        {
            animation = DefaultAnimation;
        }

        if (control is IFakeControl)
        {
            control.Visible = false;
            return;
        }

        var item = new QueueItem()
        {
            animation = animation, control = control, IsActive = parallel, mode = mode,
            clipRectangle = clipRectangle
        };

        //check visible state
        switch (mode)
        {
            case AnimateMode.Show:
                if (control.Visible) //already showed
                {
                    OnCompleted (new QueueItem { control = control, mode = mode });
                    return;
                }

                break;
            case AnimateMode.Hide:
                if (!control.Visible) //already hidden
                {
                    OnCompleted (new QueueItem { control = control, mode = mode });
                    return;
                }

                break;
        }

        //add to queue
        lock (queue)
            queue.Add (item);
        lock (_requests)
            _requests.Add (item);
    }

    private AnimationController CreateDoubleBitmap (Control control, AnimateMode mode, Animation animation,
        Rectangle clipRect)
    {
        var controller = new AnimationController (control, mode, animation, TimeStep, clipRect);
        controller.TransformNeeded += OnTransformNeeded;
        controller.NonLinearTransformNeeded += OnNonLinearTransformNeeded;
        controller.MouseDown += OnMouseDown;
        controller.DoubleBitmap.Cursor = Cursor;
        controller.FramePainted += OnFramePainted;
        return controller;
    }

    private void OnFramePainted (object sender, PaintEventArgs e)
    {
        if (FramePainted != null)
        {
            FramePainted (sender, e);
        }
    }

    protected virtual void OnMouseDown (object sender, MouseEventArgs e)
    {
        try
        {
            //transform point to animated control's coordinates
            var db = (AnimationController)sender;
            var l = e.Location;
            l.Offset (db.DoubleBitmap.Left - db.AnimatedControl.Left, db.DoubleBitmap.Top - db.AnimatedControl.Top);

            //
            if (MouseDown != null)
            {
                MouseDown (sender, new MouseEventArgs (e.Button, e.Clicks, l.X, l.Y, e.Delta));
            }
        }
        catch
        {
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArg"></param>
    protected virtual void OnNonLinearTransformNeeded
        (
            object? sender,
            NonLinearTransfromNeededEventArg eventArg
        )
    {
        if (NonLinearTransfromNeeded != null)
        {
            NonLinearTransfromNeeded (this, eventArg);
        }
        else
        {
            eventArg.UseDefaultTransform = true;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArg"></param>
    protected virtual void OnTransformNeeded
        (
            object? sender,
            TransfromNeededEventArg eventArg
        )
    {
        if (TransfromNeeded != null)
        {
            TransfromNeeded (this, eventArg);
        }
        else
        {
            eventArg.UseDefaultMatrix = true;
        }
    }

    /// <summary>
    /// Clears queue.
    /// </summary>
    public void ClearQueue()
    {
        List<QueueItem> items = null;
        lock (queue)
        {
            items = new List<QueueItem> (queue);
            queue.Clear();
        }


        foreach (var item in items)
        {
            if (item.control != null)
            {
                item.control.BeginInvoke (new MethodInvoker (() =>
                {
                    switch (item.mode)
                    {
                        case AnimateMode.Hide:
                            item.control.Visible = false;
                            break;

                        case AnimateMode.Show:
                            item.control.Visible = true;
                            break;
                    }
                }));
            }

            OnAnimationCompleted (new AnimationCompletedEventArg
                { Animation = item.animation, Control = item.control, Mode = item.mode });
        }

        if (items.Count > 0)
        {
            OnAllAnimationsCompleted();
        }
    }

    protected virtual void OnAnimationCompleted
        (
            AnimationCompletedEventArg e
        )
    {
        AnimationCompleted?.Invoke (this, e);
    }

    protected virtual void OnAllAnimationsCompleted()
    {
        AllAnimationsCompleted?.Invoke (this, EventArgs.Empty);
    }

    #region Nested type: QueueItem

    /// <summary>
    ///
    /// </summary>
    protected class QueueItem
    {
        /// <summary>
        ///
        /// </summary>
        public Animation? animation;

        /// <summary>
        ///
        /// </summary>
        public AnimationController? controller;

        /// <summary>
        ///
        /// </summary>
        public Control? control;

        /// <summary>
        ///
        /// </summary>
        public DateTime ActivateTime { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public AnimateMode mode;

        /// <summary>
        ///
        /// </summary>
        public Rectangle clipRectangle;

        /// <summary>
        ///
        /// </summary>
        public bool isActive;

        /// <summary>
        ///
        /// </summary>
        public bool IsActive
        {
            get => isActive;
            set
            {
                if (isActive == value)
                {
                    return;
                }

                isActive = value;
                if (value)
                {
                    ActivateTime = DateTime.Now;
                }
            }
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (control != null!)
            {
                sb.Append (control.GetType().Name + " ");
            }

            sb.Append (mode);
            return sb.ToString();
        }
    }

    #endregion

    #region IExtenderProvider

    /// <summary>
    ///
    /// </summary>
    /// <param name="control"></param>
    /// <returns></returns>
    public DecorationType GetDecoration (Control control)
    {
        if (_decorationByControls.ContainsKey (control))
        {
            return _decorationByControls[control].DecorationType;
        }
        else
        {
            return DecorationType.None;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="control"></param>
    /// <param name="decoration"></param>
    public void SetDecoration
        (
            Control control,
            DecorationType decoration
        )
    {
        var wrapper = _decorationByControls.ContainsKey (control)
            ? _decorationByControls[control]
            : null;
        if (decoration == DecorationType.None)
        {
            if (wrapper != null)
            {
                wrapper.Dispose();
            }

            _decorationByControls.Remove (control);
        }
        else
        {
            if (wrapper == null)
            {
                wrapper = new DecorationControl (decoration, control);
            }

            wrapper.DecorationType = decoration;
            _decorationByControls[control] = wrapper;
        }
    }

    private readonly Dictionary<Control, DecorationControl> _decorationByControls = new ();

    /// <summary>
    ///
    /// </summary>
    /// <param name="extendee"></param>
    /// <returns></returns>
    public bool CanExtend (object extendee)
    {
        return extendee is Control;
    }

    #endregion
}

/// <summary>
///
/// </summary>
public enum DecorationType
{
    /// <summary>
    ///
    /// </summary>
    None,

    /// <summary>
    ///
    /// </summary>
    BottomMirror,

    /// <summary>
    ///
    /// </summary>
    Custom
}

/// <summary>
///
/// </summary>
public class AnimationCompletedEventArg
    : EventArgs
{
    /// <summary>
    ///
    /// </summary>
    public Animation? Animation { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Control? Control { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public AnimateMode Mode { get; internal set; }
}

/// <summary>
///
/// </summary>
public class TransfromNeededEventArg
    : EventArgs
{
    /// <summary>
    ///
    /// </summary>
    public TransfromNeededEventArg()
    {
        Matrix = new Matrix (1, 0, 0, 1, 0, 0);
    }

    /// <summary>
    ///
    /// </summary>
    public Matrix Matrix { get; set; }

    /// <summary>
    ///
    /// </summary>
    public float CurrentTime { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public Rectangle ClientRectangle { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public Rectangle ClipRectangle { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public Animation Animation { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Control Control { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public AnimateMode Mode { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public bool UseDefaultMatrix { get; set; }
}

/// <summary>
///
/// </summary>
public class NonLinearTransfromNeededEventArg
    : EventArgs
{
    /// <summary>
    ///
    /// </summary>
    public float CurrentTime { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public Rectangle ClientRectangle { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public byte[]? Pixels { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public int Stride { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public Rectangle SourceClientRectangle { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public byte[]? SourcePixels { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public int SourceStride { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Animation? Animation { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Control? Control { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public AnimateMode Mode { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public bool UseDefaultTransform { get; set; }
}

/// <summary>
///
/// </summary>
public enum AnimateMode
{
    /// <summary>
    ///
    /// </summary>
    Show,

    /// <summary>
    ///
    /// </summary>
    Hide,

    /// <summary>
    ///
    /// </summary>
    Update,

    /// <summary>
    ///
    /// </summary>
    BeginUpdate
}
