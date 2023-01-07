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

        private readonly List<ProcessInfo> _objectsToProcess;

        #endregion Fields

        #region Events

        internal event EngineStateChangedEventHandler? StateChanged;

        #endregion Events

        #region Private methods

        private void ProcessObjects (object sender, EngineState state)
        {
            for (var i = 0; i < _objectsToProcess.Count; i++)
            {
                var info = _objectsToProcess[i];
                if (info.Process (sender, state))
                {
                    _objectsToProcess.RemoveAt (i);
                    i--;
                }
            }
        }

        private void OnStateChanged (object sender, EngineState state)
        {
            ProcessObjects (sender, state);
            StateChanged?.Invoke (sender, new EngineStateChangedEventArgs (this, state));
        }

        #endregion

        #region Internal methods

        internal void AddObjectToProcess (Base obj, XmlItem item)
        {
            if (obj is not TextObject textObj || textObj.ProcessAt == ProcessAt.Default)
            {
                return;
            }

            _objectsToProcess.Add (new ProcessInfo (textObj, item));
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Processes the specified text object which <b>ProcessAt</b> property is set to <b>Custom</b>.
        /// </summary>
        /// <param name="obj">The text object to process.</param>
        public void ProcessObject (TextObjectBase obj)
        {
            for (var i = 0; i < _objectsToProcess.Count; i++)
            {
                var info = _objectsToProcess[i];
                if (info.TextObject == obj)
                {
                    info.Process();
                    _objectsToProcess.RemoveAt (i);
                    break;
                }
            }
        }

        #endregion
    }
}
