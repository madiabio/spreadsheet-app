// <copyright file="SpreadsheetGUI.razor.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// Ignore Spelling: Spreadsheeeeeeeeee

namespace SpreadsheetNS;
using CS3500.Spreadsheet;
using CS3500.Formula;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

/// <summary>
///  FIXME.
///  <remarks>
///    <para>
///      This is a partial class because class SpreadsheetGUI is also automatically
///      generated from the SpreadsheetGUI.razor file.  Any code in that file, and variable in
///      that file can be referenced here, and vice versa.
///    </para>
///    <para>
///      It is usually better to put the code in a separate CS isolation file so that Visual Studio
///      can use intellisense better.
///    </para>
///    <para>
///      Note: only GUI related information should go in the sheet. All (Model) spreadsheet
///      operations should happen through the Spreadsheet class API.
///    </para>
///    <para>
///      The "backing stores" are strings that are used to affect the content of the GUI
///      display.  When you update the Spreadsheet, you will then have to copy that information
///      into the backing store variable(s).
///    </para>
///  </remarks>
/// </summary>
public partial class SpreadsheetGUI
{
    /// <summary>
    ///     Instantiate a new instance of a Spreadsheet object.
    /// </summary>
    private Spreadsheet _spreadsheet = new();

    /// <summary>
    ///    Gets the alphabet for ease of creating columns.
    /// </summary>
    private static char[ ] Alphabet { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    /// <summary>
    ///   Gets or sets the javascript object for this web page that allows
    ///   you to interact with any javascript in the associated file.
    /// </summary>
    private IJSObjectReference? JSModule { get; set; }

   /// <summary>
    ///   Gets or sets FIXME.
    /// </summary>
    private string FileSaveName { get; set; } = "Spreadsheet.sprd";

    /// <summary>
    ///   <para> Gets or sets the data for the Tool Bar Cell Contents text area, e.g., =57+A2. </para>
    ///   <remarks>Backing Store for HTML</remarks>
    /// </summary>
    // FIXME: issues w/ toolbar not updating cell contents Scould be with how this is defined
    private string ToolBarCellContents { get; set; } = string.Empty;

    /// <summary>
    ///   <para> Gets or sets the data for all of the cells in the spreadsheet GUI. </para>
    ///   <remarks>Backing Store for HTML</remarks>
    /// </summary>
    private string[,] CellsBackingStore { get; set; } = new string[ rowSize, columnSize ];

    /// <summary>
    ///   <para> Gets or sets the html class string for all of the cells in the spreadsheet GUI. </para>
    ///   <remarks>Backing Store for HTML CLASS strings</remarks>
    /// </summary>
    private string[,] CellsClassBackingStore { get; set; } = new string[ rowSize, columnSize ];

    /// <summary>
    ///   Gets or sets a value indicating whether we are showing the save "popup" or not.
    /// </summary>
    private bool SaveGUIView { get; set; }

    private bool BetaCellEditor { get; set; }

    /// <summary>
    ///   Query the spreadsheet to see if it has been changed.
    ///   <remarks>
    ///     Any method called from JavaScript must be public
    ///     and JSInvokable!
    ///   </remarks>
    /// </summary>
    /// <returns>
    ///   true if the spreadsheet is changed.
    /// </returns>
    [JSInvokable]
    public bool HasSpreadSheetChanged(  )
    {
        // FIXME: potentially remove this debug line??
        Debug.WriteLine( $"{"HasSpreadSheetChanged",-30}: {Navigator.Uri}. Remove Me." );
        return _spreadsheet.Changed;
    }

    /// <summary>
    ///     updates the current contents of the toolbar.
    ///     FIXME docs.
    /// </summary>
    /// <param name="currentCell"> cell. </param>
    /// <returns> return. </returns>
    public string UpdateToolbarContents(string currentCell)
    {
        object contents = _spreadsheet.GetCellContents(currentCell);
        if (contents is Formula)
        {
            return "=" + contents;
        }

        return contents.ToString() ?? throw new InvalidOperationException();
    }

    /// <summary>
    ///     updates the current contents of the toolbar.
    ///     FIXME docs.
    /// </summary>
    /// <param name="currentCell"> cell. </param>
    /// <returns> return. </returns>
    public string UpdateToolbarValue(string currentCell)
    {
        if (_spreadsheet.GetCellValue(currentCell) is FormulaError)
        {
            return "ERROR";
        }

        return _spreadsheet.GetCellValue(currentCell).ToString() ?? throw new InvalidOperationException();
    }

    /// <summary>
    ///   Example of how JavaScript can talk "back" to the C# side.
    /// </summary>
    /// <param name="message"> string from javascript side. </param>
    [JSInvokable]
    public void TestBlazorInterop( string message )
    {
        Debug.WriteLine( $"JavaScript has send me a message: {message}" );
    }

    /// <summary>
    ///   Set up initial state and event handlers.
    ///   <remarks>
    ///     This is somewhat like a constructor for a Blazor Web Page (object).
    ///     You probably don't need to do anything here.
    ///   </remarks>
    /// </summary>
    protected override void OnInitialized( )
    {
        HighlightCell(0, 0);
    }

    /// <summary>
    ///   Called anytime in the lifetime of the web page were the page is re-rendered.
    ///   <remarks>
    ///     You probably don't need to do anything in here beyond what is provided.
    ///   </remarks>
    /// </summary>
    /// <param name="firstRender"> true the very first time the page is rendered.</param>
    protected async override void OnAfterRender( bool firstRender )
    {
        base.OnAfterRender( firstRender );

        if ( firstRender )
        {
            /////////////////////////////////////////////////
            //
            // The following three lines setup and test the
            // ability for Blazor to talk to javascript and vice versa.
            JSModule = await JS.InvokeAsync<IJSObjectReference>( "import", "./Pages/SpreadsheetGUI.razor.js" ); // create/read the javascript
            await JSModule.InvokeVoidAsync( "SetDotNetInterfaceObject", DotNetObjectReference.Create( this ) ); // tell the javascript about us (dot net)
            await JSModule.InvokeVoidAsync( "TestJavaScriptInterop", "Hello JavaScript!" ); // test that it is working.  You could remove this.
            await FormulaContentEditableInput.FocusAsync(); // when we start up, put the focus on the input. you will want to do this anytime a cell is clicked.
        }
    }

    /// <summary>
    ///  cells should be of the form "A5" or "B1".  The matrix of cells (the backing store) is zero
    ///  based but the first row in the spreadsheet is 1.
    /// </summary>
    /// <param name="cellName"> The name of the cell. </param>
    /// <param name="row"> The returned conversion between row and zero based index. </param>
    /// <param name="col"> The returned conversion between column letter and zero based matrix index. </param>
    private static void ConvertCellNameToRowCol( string cellName, out int row, out int col )
    {
        col = 0;
        row = 0;
        string letters = new (cellName.TakeWhile(char.IsLetter).ToArray());
        string numbers = new (cellName.SkipWhile(char.IsLetter).ToArray());

        int.TryParse(numbers, out row);

        foreach (char t in letters)
        {
            col *= 26;
            col += char.ToUpper(t) - 'A' + 1;
        }

        col--;
        row--;
    }

    /// <summary>
    ///   Given a row,col such as "(0,0)" turn this into the appropriate
    ///   cell name, such as: "A1".
    /// </summary>
    /// <param name="row"> The row number (0-A, 1-B, ...).</param>
    /// <param name="col"> The column number (0 based).</param>
    /// <returns>A string defining the cell name, where the col is A-Z and row is not zero based.</returns>
    private static string CellNameFromRowCol( int row, int col )
    {
        return $"{Alphabet[col]}{row + 1}";
    }

    /// <summary>
    ///   Called when the input widget (representing the data in a particular cell) is modified.
    ///  </summary>
    /// <param name="newContents"> The new contents to put at row/col. </param>
    /// <param name="row"> The matrix row identifier. </param>
    /// <param name="col"> The matrix column identifier. </param>
    private async void HandleUpdateCellInSpreadsheet( string newContents, int row, int col )
    {
        try
        {
            InputWidgetBackingStore = $"{row},{col}";
            string cellName = CellNameFromRowCol(row, col);
            IList<string> list = _spreadsheet.SetContentsOfCell(cellName, newContents);

            foreach (string item in list)
            {
                ConvertCellNameToRowCol(item, out int updateRow, out int updateCol);
                if (_spreadsheet.GetCellValue(currentCell) is FormulaError)
                {
                    CellsBackingStore[updateRow, updateCol] = "ERROR";
                }
                else
                {
                    CellsBackingStore[updateRow, updateCol] = _spreadsheet.GetCellValue(item).ToString() ?? throw new InvalidOperationException();
                }
            }

            // FIXME this might be not needed.
            if (_spreadsheet.GetCellValue(currentCell) is FormulaError)
            {
                cellValue = "ERROR";
                CellsBackingStore[row, col] = "ERROR";
            }
            else
            {
                cellValue = _spreadsheet.GetCellValue(cellName).ToString();
                CellsBackingStore[row, col] = cellValue ?? throw new InvalidOperationException();
            }
        }
        catch
        {
            // a way to communicate to the user that something went wrong.
            await JS.InvokeVoidAsync( "alert", "Your input is invalid." );
        }
    }

    // FIXME: make a summary for this?
    private void UpdateCellValues(ChangeEventArgs e)
    {
        cellValue = _spreadsheet.GetCellValue(currentCell).ToString();
    }

    /// <summary>
    /// Updates CellsBackingStore and CellsClassBackingStore with data from _spreadsheet after loading.
    /// </summary>
    private void UpdateBackingStoresFromSpreadsheet()
    {
        // Clear existing backing stores
        CellsBackingStore = new string[rowSize, columnSize];
        CellsClassBackingStore = new string[rowSize, columnSize];

        // Iterate through non-empty cells in the loaded spreadsheet
        foreach (var cellName in _spreadsheet.GetNamesOfAllNonemptyCells())
        {
            ConvertCellNameToRowCol(cellName, out int row, out int col);

            // Get cell content and value from the spreadsheet
            var cellContent = _spreadsheet.GetCellContents(cellName).ToString();
            var cellValue = _spreadsheet.GetCellValue(cellName).ToString();

            // Update CellsBackingStore and CellsClassBackingStore
            CellsBackingStore[row, col] = cellValue ?? string.Empty;

            // FIXME: check the following line. Not sure what HTML class is lowkey
            CellsClassBackingStore[row, col] = "cell-populated"; // Optional: Apply a class for non-empty cells
        }

        ConvertCellNameToRowCol("A1", out int row2, out int col2);
        FocusMainInput(row2, col2);

        // Notify the component to refresh the UI
        StateHasChanged();
    }

    /// <summary>
    ///   <para>
    ///     Using a Web Input ask the user for a file and then process the
    ///     data in the file.
    ///   </para>
    ///   <remarks>
    ///     Unfortunately, this happens after the file is chosen, but we will live with that.
    ///   </remarks>
    /// </summary>
    /// <param name="args"> Information about the file that has been selected. </param>
    private async void HandleLoadFile( EventArgs args )
    {
        try
        {
            // Check if spreadsheet has been changed. If so, give warning.
            if (HasSpreadSheetChanged())
            {
                bool success = await ConfirmUnsavedDataLoss();
                if (!success)
                {
                    return;    // user canceled the action.
                }
            }

            string fileContent = string.Empty; // init to nothing so it can't be null

            InputFileChangeEventArgs eventArgs = args as InputFileChangeEventArgs ?? throw new Exception("that didn't work");
            if ( eventArgs.FileCount == 1 )
            {
                var file = eventArgs.File;
                if ( file is null )
                {
                    return;
                }

                using var stream = file.OpenReadStream();
                using var reader = new System.IO.StreamReader(stream);
                fileContent = await reader.ReadToEndAsync();

                try
                {
                    _spreadsheet.InstantiateFromJSON(fileContent); // Load from JSON string
                    ResetBackingStores();
                    UpdateBackingStoresFromSpreadsheet(); // Update backing stores with new data
                    FileSaveName = _spreadsheet.SpreadsheetName;
                }
                catch
                {
                    if (JSModule is null)
                    {
                        JSModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/SpreadsheetGUI.razor.js");
                    }

                    await JS.InvokeAsync<bool>(
            "alert", "The load attempt was unsuccessful.");
                }

                StateHasChanged();
            }
        }
        catch ( Exception e )
        {
            Debug.WriteLine( "something went wrong with loading the file..." + e );
        }
    }

    /// <summary>
    ///   Switch between the file save view or main view.
    /// </summary>
    /// <param name="show"> if true, show the file save view. </param>
    private void ShowHideSaveGUI(bool show)
    {
        SaveGUIView = show;
        StateHasChanged();
    }

    /// <summary>
    ///   Switch between the beta spreadsheet view and the main spreadsheet view..
    /// </summary>
    /// <param name="show"> if true, show the beta spreadsheet view. </param>
    private void ShowBetaCellEditor(bool show)
    {
        BetaCellEditor = show;
        StateHasChanged();
    }

    /// <summary>
    ///   Call the JavaScript necessary to download the data via the Browser's Download
    ///   Folder.
    /// </summary>
    /// <param name="e"> Ignored. </param>
    private async void HandleSaveFile(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        // <remarks> this null check is done because Visual Studio doesn't understand
        // the Blazor life cycle and cannot assure of non-null. </remarks>
        if ( JSModule is not null )
        {
            string sprdDataJSON = _spreadsheet.GetJSON(); // get the JSON string.
            var success = await JSModule.InvokeAsync<bool>("saveToFile", SaveFileName, sprdDataJSON);
            if (success)
            {
                ShowHideSaveGUI(false);
                StateHasChanged();
            }
        }
    }

    /// <summary>
    ///   Clear the spreadsheet if not modified.
    /// </summary>
    /// <param name="e"> Ignored. </param>
    private async void HandleClear(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        if ( JSModule is not null )
        {
            bool success = await JS.InvokeAsync<bool>( "confirm", "Clear the sheet?" );

            // If the spreadsheet has been modified since last save, display warning dialogue
            if (HasSpreadSheetChanged())
            {
                bool proceed = await ConfirmUnsavedDataLoss();
                if (!proceed)
                {
                    return; // user decided to cancel action
                }
            }

            _spreadsheet = new(); // Init new spreadsheet
            ResetBackingStores(); // Reset all back end data
        }
    }

    /// <summary>
    /// Resets all backend spreadsheet data then refreshes the UI.
    /// </summary>
    private IList<object> ResetBackingStores()
    {
        string[,] oldBackingStore = CellsBackingStore;
        string[,] oldCellsClassBackingStore = CellsClassBackingStore;

        string oldCurrentCell = currentCell;
        string? oldCellValue = cellValue ?? string.Empty;

        string oldToolbarCellContents = ToolBarCellContents;

        IList<object> oldVals = [oldBackingStore, oldCellsClassBackingStore, oldCurrentCell, oldCellValue, oldToolbarCellContents];

        // Reset backing stores to reflect the cleared state
        CellsBackingStore = new string[rowSize, columnSize];
        CellsClassBackingStore = new string[rowSize, columnSize];

        // Reset other state variables
        currentCell = "A1";
        cellValue = string.Empty;

        ConvertCellNameToRowCol("A1", out int row, out int col);
        FocusMainInput(row, col);

        StateHasChanged(); // Refresh UI

        return oldVals;
    }

    /// <summary>
    /// Restores data in case of failed load operation. For use with old vals returned by <see cref="ResetBackingStores"/>.
    /// </summary>
    private void RestoreBackingStores(IList<object> oldVals)
    {
        // FIXME: not sure if we need this function actually.
        CellsBackingStore = (string[,])oldVals[0];
        CellsClassBackingStore = (string[,])oldVals[1];
        currentCell = (string)oldVals[2];
        cellValue = (string)oldVals[3];
        ToolBarCellContents = (string)oldVals[4];
    }

    /// <summary>
    /// Displays a warning dialog if an action will result in the loss of unsaved data.
    /// </summary>
    /// <returns>A Task that returns true if the user confirms; otherwise, false.</returns>
    private async Task<bool> ConfirmUnsavedDataLoss()
    {
        // Ensure JSModule is initialized (for example, in OnAfterRender when firstRender is true)
        if (JSModule is null)
        {
            JSModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/SpreadsheetGUI.razor.js");
        }

        // Use JavaScript to show a confirmation dialog
        bool userConfirmed = await JS.InvokeAsync<bool>(
            "confirm", "You have unsaved changes. Would you like to proceed and lose them?");

        return userConfirmed;
    }
}
