// <copyright file="SpreadsheetGUI.razor.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace SpreadsheetNS;
using CS3500.Spreadsheet;
using CS3500.Formula;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;

/// <summary>
///     This partial class represents an aspect of the Spreadsheet GUI. It essentially represents the View aspect of the
///     MVC architecture of the Spreadsheet. It contains all important aspects for updating the GUI / interacting with the user.
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
    ///     Gets or sets the javascript object for this web page that allows
    ///     you to interact with any javascript in the associated file.
    /// </summary>
    private IJSObjectReference? JSModule { get; set; }

   /// <summary>
    ///     Gets or Sets the name of the spreadsheet file to be saved to.
    /// </summary>
    private string FileSaveName { get; set; } = "Spreadsheet.sprd";

    /// <summary>
    ///   <para> Gets or sets the data for the Tool Bar Cell Contents text area, e.g., =57+A2. </para>
    ///   <remarks>Backing Store for HTML</remarks>
    /// </summary>
    private string ToolBarCellContents { get; set; } = string.Empty;

    /// <summary>
    ///   <para> Gets or sets the data for all of the cells in the spreadsheet GUI. </para>
    ///   <remarks>Backing Store for HTML</remarks>
    /// </summary>
    private string[,] CellsBackingStore { get; set; } = new string[ _rowSize, _columnSize ];

    /// <summary>
    ///   <para> Gets or sets the html class string for all of the cells in the spreadsheet GUI. </para>
    ///   <remarks>Backing Store for HTML CLASS strings</remarks>
    /// </summary>
    private string[,] CellsClassBackingStore { get; set; } = new string[ _rowSize, _columnSize ];

    /// <summary>
    ///     Gets or sets a value indicating whether we are showing the save "popup" or not.
    /// </summary>
    private bool SaveGUIView { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether we are showing the Beta view of the spreadsheet.
    /// </summary>
    private bool BetaCellEditor { get; set; }

    /// <summary>
    ///     Query the spreadsheet to see if it has been changed.
    /// </summary>
    /// <returns>
    ///     returns true if the spreadsheet is changed.
    /// </returns>
    [JSInvokable]
    public bool HasSpreadSheetChanged(  )
    {
        return _spreadsheet.Changed;
    }

    /// <summary>
    ///     Update the contents bar of the toolbar displayed to user when a new cell is clicked.
    /// </summary>
    /// <param name="cell"> cell to be updated. </param>
    /// <returns> return the string of contents to be updated. </returns>
    public string UpdateToolbarContents(string cell)
    {
        object contents = _spreadsheet.GetCellContents(cell);
        if (contents is Formula)
        {
            return "=" + contents;
        }

        return contents.ToString() ?? throw new InvalidOperationException();
    }

    /// <summary>
    ///     Update the value shown to the user in the tool bar when a new cell is clicked.
    /// </summary>
    /// <param name="cell"> cell to be updated. </param>
    /// <returns> return. </returns>
    public string UpdateToolbarValue(string cell)
    {
        if (_spreadsheet.GetCellValue(cell) is FormulaError)
        {
            return "ERROR";
        }

        return _spreadsheet.GetCellValue(_currentCell).ToString() ?? throw new InvalidOperationException();
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
    ///     Set up initial state and event handlers. Display A1 as the first "focused" cell / automatically selected
    ///     cell to user.
    /// </summary>
    protected override void OnInitialized( )
    {
        HighlightCell(0, 0);
    }

    /// <summary>
    ///     Called anytime in the lifetime of the web page were the page is re-rendered.
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
    ///     Cells are stored as cell names. This function converts the cell names to represent the matrix of cells
    ///     (the backing store). The matrix is 0 based but the first row in the spreadsheet is displayed as 1.
    ///  based but the first row in the spreadsheet is 1.
    /// </summary>
    /// <param name="cellName"> The name of the cell. </param>
    /// <param name="row"> The returned conversion between row and zero based index. </param>
    /// <param name="col"> The returned conversion between column letter and zero based matrix index. </param>
    private static void ConvertCellNameToRowCol( string cellName, out int row, out int col )
    {
        // Separate cellName into numbers and letters.
        col = 0;
        row = 0;
        string letters = new (cellName.TakeWhile(char.IsLetter).ToArray());
        string numbers = new (cellName.SkipWhile(char.IsLetter).ToArray());

        int.TryParse(numbers, out row);

        // Convert letters in cell name to representative number.
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
            _inputWidgetBackingStore = $"{row},{col}";
            string cellName = CellNameFromRowCol(row, col);
            IList<string> list = _spreadsheet.SetContentsOfCell(cellName, newContents);

            // Update dependencies of the cell that was updated/ input.
            foreach (string item in list)
            {
                ConvertCellNameToRowCol(item, out int updateRow, out int updateCol);
                CellsBackingStore[updateRow, updateCol] = _spreadsheet.GetCellValue(item).ToString() ?? throw new InvalidOperationException();
            }

            _cellValue = _spreadsheet.GetCellValue(cellName).ToString();
            CellsBackingStore[row, col] = _cellValue ?? throw new InvalidOperationException();
        }
        catch
        {
            // Alert the user that their input was invalid.
            await JS.InvokeVoidAsync( "alert", "Your input is invalid." );
        }
    }

    /// <summary>
    ///     Update the cell values when cell contents are changed.
    /// </summary>
    /// <param name="e"> Change Event argument representing the user's input. </param>
    private void UpdateCellValues(ChangeEventArgs e)
    {
        _cellValue = _spreadsheet.GetCellValue(_currentCell).ToString();
    }

    /// <summary>
    ///     Updates CellsBackingStore and CellsClassBackingStore with data from _spreadsheet after loading.
    /// </summary>
    private void UpdateBackingStoresFromSpreadsheet()
    {
        // Clear existing backing stores
        CellsBackingStore = new string[_rowSize, _columnSize];
        CellsClassBackingStore = new string[_rowSize, _columnSize];

        // Iterate through non-empty cells in the loaded spreadsheet
        foreach (string cell in _spreadsheet.GetNamesOfAllNonemptyCells())
        {
            ConvertCellNameToRowCol(cell, out int row, out int col);

            // Get cell value from the spreadsheet
            var value = _spreadsheet.GetCellValue(cell).ToString();

            // Update CellsBackingStore and CellsClassBackingStore
            CellsBackingStore[row, col] = value ?? string.Empty;

            CellsClassBackingStore[row, col] = "cell-populated";
        }

        ConvertCellNameToRowCol("A1", out int row2, out int col2);
        FocusMainInput(row2, col2);

        StateHasChanged();
    }

    /// <summary>
    ///   <para>
    ///     Using a Web Input ask the user for a file and then process the
    ///     data in the file.
    ///   </para>
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

            string fileContent = string.Empty; // initialize fileContent to nothing so it can't be null

            InputFileChangeEventArgs eventArgs = args as InputFileChangeEventArgs ?? throw new Exception("that didn't work");
            if (eventArgs.FileCount != 1)
            {
                return;
            }

            var file = eventArgs.File;

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
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
        catch ( Exception )
        {
            await JS.InvokeVoidAsync( "alert", "The load attempt was unsuccessful. Your file may be invalid." );
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
    ///     Call the JavaScript necessary to download the data via the Browser's Download
    ///     Folder.
    /// </summary>
    /// <param name="e"> Ignored. </param>
    private async void HandleSaveFile(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        // <remarks> this null check is done because Visual Studio doesn't understand
        // the Blazor life cycle and cannot assure of non-null. </remarks>
        if ( JSModule is not null )
        {
            string sprdDataJSON = _spreadsheet.GetJSON(); // get the JSON string.
            var success = await JSModule.InvokeAsync<bool>("saveToFile", _saveFileName, sprdDataJSON);
            if (success)
            {
                ShowHideSaveGUI(false);
                StateHasChanged();
            }
        }
    }

    /// <summary>
    ///     Clear the spreadsheet if not modified.
    /// </summary>
    /// <param name="e"> Ignored. </param>
    private async void HandleClear(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        if (JSModule is null)
        {
            return;
        }

        // If the spreadsheet has been modified since last save, display warning dialogue
        if (HasSpreadSheetChanged())
        {
            bool proceed = await ConfirmUnsavedDataLoss();
            if (!proceed)
            {
                return; // user decided to cancel action
            }
        }

        _spreadsheet = new(); // Initialize a new spreadsheet
        ResetBackingStores(); // Reset all back end data
    }

    /// <summary>
    /// Resets all backend spreadsheet data then refreshes the UI.
    /// </summary>
    private IList<object> ResetBackingStores()
    {
        string[,] oldBackingStore = CellsBackingStore;
        string[,] oldCellsClassBackingStore = CellsClassBackingStore;

        string oldCurrentCell = _currentCell;
        string? oldCellValue = _cellValue ?? string.Empty;

        string oldToolbarCellContents = ToolBarCellContents;

        IList<object> oldValues = [oldBackingStore, oldCellsClassBackingStore, oldCurrentCell, oldCellValue, oldToolbarCellContents];

        // Reset backing stores to reflect the cleared state
        CellsBackingStore = new string[_rowSize, _columnSize];
        CellsClassBackingStore = new string[_rowSize, _columnSize];

        // Reset other state variables
        _currentCell = "A1";
        _cellValue = string.Empty;

        ConvertCellNameToRowCol("A1", out int row, out int col);
        FocusMainInput(row, col);

        StateHasChanged();

        return oldValues;
    }

    /// <summary>
    ///     Displays a warning dialog if an action will result in the loss of unsaved data.
    /// </summary>
    /// <returns> A Task that returns true if the user confirms; otherwise, false.</returns>
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
