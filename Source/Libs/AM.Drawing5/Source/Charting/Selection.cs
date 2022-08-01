// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* Selection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// </summary>
/// <remarks>
/// </remarks>
public class Selection
    : CurveList
{
    // Revision: JCarpenter 10/06
    /// <summary>
    /// Subscribe to this event to receive notice
    /// that the list of selected CurveItems has changed
    /// </summary>
    public event EventHandler SelectionChangedEvent;

    #region static properties

    /// <summary>
    /// The <see cref="Border" /> type to be used for drawing "selected"
    /// <see cref="PieItem" />, <see cref="BarItem" />, <see cref="HiLowBarItem" />,
    /// <see cref="OHLCBarItem" />, and <see cref="JapaneseCandleStickItem" /> item types.
    /// </summary>
    public static Border Border = new Border (Color.Gray, 1.0f);

    /// <summary>
    /// The <see cref="Fill" /> type to be used for drawing "selected"
    /// <see cref="PieItem" />, <see cref="BarItem" />, <see cref="HiLowBarItem" />,
    /// and <see cref="JapaneseCandleStickItem" /> item types.
    /// </summary>
    public static Fill Fill = new Fill (Color.Gray);

    /// <summary>
    /// The <see cref="Line" /> type to be used for drawing "selected"
    /// <see cref="LineItem" /> and <see cref="StickItem" /> types
    /// </summary>
    public static Line Line = new Line (Color.Gray);

    //			public static ErrorBar ErrorBar = new ErrorBar( Color.Gray );
    /// <summary>
    /// The <see cref="Symbol" /> type to be used for drawing "selected"
    /// <see cref="LineItem" /> and <see cref="ErrorBarItem" /> types.
    /// </summary>
    public static Symbol Symbol = new Symbol (SymbolType.Circle, Color.Gray);

    //public static Color SelectedSymbolColor = Color.Gray;

    #endregion

    #region Methods

    /// <summary>
    /// Place a <see cref="CurveItem" /> in the selection list, removing all other
    /// items.
    /// </summary>
    /// <param name="master">The <see cref="MasterPane" /> that is the "owner"
    /// of the <see cref="CurveItem" />'s.</param>
    /// <param name="ci">The <see cref="CurveItem" /> to be added to the list.</param>
    public void Select (MasterPane master, CurveItem ci)
    {
        //Clear the selection, but don't send the event,
        //the event will be sent in "AddToSelection" by calling "UpdateSelection"
        ClearSelection (master, false);

        AddToSelection (master, ci);
    }

    /// <summary>
    /// Place a list of <see cref="CurveItem" />'s in the selection list, removing all other
    /// items.
    /// </summary>
    /// <param name="master">The <see cref="MasterPane" /> that is the "owner"
    /// of the <see cref="CurveItem" />'s.</param>
    /// <param name="ciList">The list of <see cref="CurveItem" /> to be added to the list.</param>
    public void Select (MasterPane master, CurveList ciList)
    {
        //Clear the selection, but don't send the event,
        //the event will be sent in "AddToSelection" by calling "UpdateSelection"
        ClearSelection (master, false);

        AddToSelection (master, ciList);
    }

    /// <summary>
    /// Add a <see cref="CurveItem" /> to the selection list.
    /// </summary>
    /// <param name="master">The <see cref="MasterPane" /> that is the "owner"
    /// of the <see cref="CurveItem" />'s.</param>
    /// <param name="ci">The <see cref="CurveItem" /> to be added to the list.</param>
    public void AddToSelection (MasterPane master, CurveItem ci)
    {
        if (Contains (ci) == false)
            Add (ci);

        UpdateSelection (master);
    }

    /// <summary>
    /// Add a list of <see cref="CurveItem" />'s to the selection list.
    /// </summary>
    /// <param name="master">The <see cref="MasterPane" /> that is the "owner"
    /// of the <see cref="CurveItem" />'s.</param>
    /// <param name="ciList">The list of <see cref="CurveItem" />'s to be added to the list.</param>
    public void AddToSelection (MasterPane master, CurveList ciList)
    {
        foreach (CurveItem ci in ciList)
        {
            if (Contains (ci) == false)
                Add (ci);
        }

        UpdateSelection (master);
    }

#if ( DOTNET1 )
		// Define a "Contains" method so that this class works with .Net 1.1 or 2.0
		internal bool Contains( CurveItem item )
		{
			foreach ( CurveItem ci in this )
				if ( item == ci )
					return true;

			return false;
		}
#endif

    /// <summary>
    /// Remove the specified <see cref="CurveItem" /> from the selection list.
    /// </summary>
    /// <param name="master">The <see cref="MasterPane" /> that is the "owner"
    /// of the <see cref="CurveItem" />'s.</param>
    /// <param name="ci">The <see cref="CurveItem" /> to be removed from the list.</param>
    public void RemoveFromSelection (MasterPane master, CurveItem ci)
    {
        if (Contains (ci))
            Remove (ci);

        UpdateSelection (master);
    }

    /// <summary>
    /// Clear the selection list and trigger a <see cref="SelectionChangedEvent" />.
    /// </summary>
    /// <param name="master">The <see cref="MasterPane" /> that "owns" the selection list.</param>
    public void ClearSelection (MasterPane master)
    {
        ClearSelection (master, true);
    }

    /// <summary>
    /// Clear the selection list and optionally trigger a <see cref="SelectionChangedEvent" />.
    /// </summary>
    /// <param name="master">The <see cref="MasterPane" /> that "owns" the selection list.</param>
    /// <param name="sendEvent">true to trigger a <see cref="SelectionChangedEvent" />,
    /// false otherwise.</param>
    public void ClearSelection (MasterPane master, bool sendEvent)
    {
        Clear();

        foreach (GraphPane pane in master.PaneList)
        {
            foreach (CurveItem ci in pane.CurveList)
            {
                ci.IsSelected = false;
            }
        }

        if (sendEvent)
        {
            if (SelectionChangedEvent != null)
                SelectionChangedEvent (this, new EventArgs());
        }
    }

    /// <summary>
    /// Mark the <see cref="CurveItem" />'s that are included in the selection list
    /// by setting the <see cref="CurveItem.IsSelected" /> property to true.
    /// </summary>
    /// <param name="master">The <see cref="MasterPane" /> that "owns" the selection list.</param>
    public void UpdateSelection (MasterPane master)
    {
        if (Count <= 0)
        {
            ClearSelection (master);
            return;
        }

        foreach (GraphPane pane in master.PaneList)
        {
            foreach (CurveItem ci in pane.CurveList)
            {
                //Make it Inactive
                ci.IsSelected = false;
            }
        }

        foreach (CurveItem ci in this)
        {
            //Make Active
            ci.IsSelected = true;

            //If it is a line / scatterplot, the selected Curve may be occluded by an unselected Curve
            //So, move it to the top of the ZOrder by removing it, and re-adding it.

            //Why only do this for Lines? ...Bar and Pie Curves are less likely to overlap,
            //and adding and removing Pie elements changes thier display order
            if (ci.IsLine)
            {
                //I don't know how to get a Pane, from a CurveItem, so I can only do it
                //if there is one and only one Pane, based on the assumption that the
                //Curve's Pane is MasterPane[0]

                //If there is only one Pane
                if (master.PaneList.Count == 1)
                {
                    GraphPane pane = master.PaneList[0];
                    pane.CurveList.Remove (ci);
                    pane.CurveList.Insert (0, ci);
                }
            }
        }

        //Send Selection Changed Event
        if (SelectionChangedEvent != null)
            SelectionChangedEvent (this, new EventArgs());
    }

    #endregion
}
