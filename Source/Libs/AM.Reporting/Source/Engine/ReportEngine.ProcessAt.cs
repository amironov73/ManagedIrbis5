// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Utils;

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Reporting.Engine
{
    internal enum EngineState
    {
        ReportStarted,
        ReportFinished,
        ReportPageStarted,
        ReportPageFinished,
        PageStarted,
        PageFinished,
        ColumnStarted,
        ColumnFinished,
        BlockStarted,
        BlockFinished,
        GroupStarted,
        GroupFinished
    }

    internal class EngineStateChangedEventArgs
    {
        #region Fields

        #endregion Fields

        #region Properties

        public ReportEngine Engine { get; }

        public EngineState State { get; }

        #endregion Properties

        #region Constructors

        internal EngineStateChangedEventArgs (ReportEngine engine, EngineState state)
        {
            this.Engine = engine;
            this.State = state;
        }

        #endregion Constructors
    }

    internal delegate void EngineStateChangedEventHandler (object sender, EngineStateChangedEventArgs e);

    internal class ProcessInfo
    {
        #region Fields

        private TextObject textObject;
        private XmlItem xmlItem;

        #endregion Fields

        #region Properties

        public TextObjectBase TextObject => textObject;

        #endregion Properties

        #region Constructors

        public ProcessInfo (TextObject obj, XmlItem item)
        {
            textObject = obj;
            xmlItem = item;
        }

        #endregion Constructors

        #region Public Methods

        public void Process()
        {
            textObject.SaveState();
            try
            {
                textObject.GetData();
                var fill_clr = textObject.FillColor.IsNamedColor
                    ? textObject.FillColor.Name
                    : "#" + textObject.FillColor.Name;
                var txt_clr = textObject.TextColor.IsNamedColor
                    ? textObject.TextColor.Name
                    : "#" + textObject.TextColor.Name;

                xmlItem.SetProp ("x", textObject.Text);
                xmlItem.SetProp ("Fill.Color", fill_clr);
                xmlItem.SetProp ("TextFill.Color", txt_clr);
                xmlItem.SetProp ("Font.Name", textObject.Font.Name);
            }
            finally
            {
                textObject.RestoreState();
            }
        }

        public bool Process (object sender, EngineState state)
        {
            var processAt = textObject.ProcessAt;
            var canProcess = false;

            if ((processAt == ProcessAt.DataFinished && state == EngineState.BlockFinished) ||
                (processAt == ProcessAt.GroupFinished && state == EngineState.GroupFinished))
            {
                // check which data is finished
                var topParentBand = textObject.Band;
                if (topParentBand is ChildBand band)
                {
                    topParentBand = band.GetTopParentBand;
                }

                if (processAt == ProcessAt.DataFinished && state == EngineState.BlockFinished)
                {
                    // total can be printed on the same data header, or on its parent data band
                    var senderBand = sender as DataBand;
                    canProcess = true;
                    if (topParentBand is DataHeaderBand && (topParentBand.Parent != sender))
                    {
                        canProcess = false;
                    }

                    if (topParentBand is DataBand && senderBand.Parent != topParentBand)
                    {
                        canProcess = false;
                    }
                }
                else
                {
                    // total can be printed on the same group header
                    canProcess = sender == topParentBand;
                }
            }
            else
            {
                canProcess = (processAt == ProcessAt.ReportFinished && state == EngineState.ReportFinished) ||
                             (processAt == ProcessAt.ReportPageFinished && state == EngineState.ReportPageFinished) ||
                             (processAt == ProcessAt.PageFinished && state == EngineState.PageFinished) ||
                             (processAt == ProcessAt.ColumnFinished && state == EngineState.ColumnFinished);
            }

            if (canProcess)
            {
                Process();
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Public Methods
    }

    public partial class ReportEngine
    {
        #region Fields

        private List<ProcessInfo> objectsToProcess;

        #endregion Fields

        #region Events

        internal event EngineStateChangedEventHandler StateChanged;

        #endregion Events

        #region Private Methods

        private void ProcessObjects (object sender, EngineState state)
        {
            for (var i = 0; i < objectsToProcess.Count; i++)
            {
                var info = objectsToProcess[i];
                if (info.Process (sender, state))
                {
                    objectsToProcess.RemoveAt (i);
                    i--;
                }
            }
        }

        private void OnStateChanged (object sender, EngineState state)
        {
            ProcessObjects (sender, state);
            if (StateChanged != null)
            {
                StateChanged (sender, new EngineStateChangedEventArgs (this, state));
            }
        }

        #endregion Private Methods

        #region Internal Methods

        internal void AddObjectToProcess (Base obj, XmlItem item)
        {
            if (obj is not TextObject textObj || textObj.ProcessAt == ProcessAt.Default)
            {
                return;
            }

            objectsToProcess.Add (new ProcessInfo (textObj, item));
        }

        #endregion Internal Methods

        #region Public Methods

        /// <summary>
        /// Processes the specified text object which <b>ProcessAt</b> property is set to <b>Custom</b>.
        /// </summary>
        /// <param name="obj">The text object to process.</param>
        public void ProcessObject (TextObjectBase obj)
        {
            for (var i = 0; i < objectsToProcess.Count; i++)
            {
                var info = objectsToProcess[i];
                if (info.TextObject == obj)
                {
                    info.Process();
                    objectsToProcess.RemoveAt (i);
                    break;
                }
            }
        }

        #endregion Public Methods
    }
}
