// <copyright file="Spreadsheet.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

// Note: the above exception is NOT nested in the Spreadsheet class!

public class Spreadsheet
{
    // Add these methods (one being an indexer) to the class:

    /// <summary>
    ///   <para>
    ///     Shortcut syntax to for getting the value of the cell
    ///     using the [] operator.
    ///   </para>
    ///   <para>
    ///     See: <see cref="GetCellValue(string)"/>.
    ///   </para>
    ///   <para>
    ///     Example Usage:
    ///   </para>
    ///   <code>
    ///      sheet.SetContentsOfCell( "A1", "=5+5" );
    ///
    ///      sheet["A1"] == 10;
    ///      // vs.
    ///      sheet.GetCellValue("A1") == 10;
    ///   </code>
    /// </summary>
    /// <param name="cellName"> Any valid cell name. </param>
    /// <returns>
    ///   Returns the value of a cell.  Note: If the cell is a formula, the value should
    ///   already have been computed.
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///     If the name parameter is invalid, throw an InvalidNameException.
    /// </exception>
    public object this[ string cellName ];

    /// <summary>
    ///   <para>
    ///     Writes the contents of this spreadsheet to the named file using a JSON format.
    ///     If the file already exists, overwrite it.
    ///   </para>
    ///   <para>
    ///     The output JSON should look like the following.
    ///   </para>
    ///   <para>
    ///     For example, consider a spreadsheet that contains a cell "A1" 
    ///     with contents being the double 5.0, and a cell "B3" with contents 
    ///     being the Formula("A1+2"), and a cell "C4" with the contents "hello".
    ///   </para>
    ///   <para>
    ///      This method would produce the following JSON string:
    ///   </para>
    ///   <code>
    ///   {
    ///     "Cells": {
    ///       "A1": {
    ///         "StringForm": "5"
    ///       },
    ///       "B3": {
    ///         "StringForm": "=A1+2"
    ///       },
    ///       "C4": {
    ///         "StringForm": "hello"
    ///       }
    ///     }
    ///   }
    ///   </code>
    ///   <para>
    ///     You can achieve this by making sure your data structure is a dictionary 
    ///     and that the contained objects (Cells) have property named "StringForm"
    ///     (if this name does not match your existing code, use the JsonPropertyName 
    ///     attribute).
    ///   </para>
    ///   <para>
    ///     There can be 0 cells in the dictionary, resulting in { "Cells" : {} } 
    ///   </para>
    ///   <para>
    ///     Further, when writing the value of each cell...
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///       If the contents is a string, the value of StringForm is that string
    ///     </item>
    ///     <item>
    ///       If the contents is a double d, the value of StringForm is d.ToString()
    ///     </item>
    ///     <item>
    ///       If the contents is a Formula f, the value of StringForm is "=" + f.ToString()
    ///     </item>
    ///   </list>
    ///   <para>
    ///     After saving the file, the spreadsheet is no longer "changed".
    ///   </para>
    /// </summary>
    /// <param name="filename"> The name (with path) of the file to save to.</param>
    /// <exception cref="SpreadsheetReadWriteException">
    ///   If there are any problems opening, writing, or closing the file, 
    ///   the method should throw a SpreadsheetReadWriteException with an
    ///   explanatory message.
    /// </exception>
    public void Save( string filename );

    /// <summary>
    ///   <para>
    ///     Read the data (JSON) from the file and instantiate the current
    ///     spreadsheet.  See <see cref="Save(string)"/> for expected format.
    ///   </para>
    ///   <para>
    ///     Note: First deletes any current data in the spreadsheet.
    ///   </para>
    ///   <para>
    ///     Loading a spreadsheet should set changed to false.  External
    ///     programs should alert the user before loading over a changed sheet.
    ///   </para>
    /// </summary>
    /// <param name="filename"> The saved file name including the path. </param>
    /// <exception cref="SpreadsheetReadWriteException"> When the file cannot be opened or the json is bad.</exception>
    public void Load( string filename );

    /// <summary>
    ///   <para>
    ///     Return the value of the named cell.
    ///   </para>
    /// </summary>
    /// <param name="cellName"> The cell in question. </param>
    /// <returns>
    ///   Returns the value (as opposed to the contents) of the named cell.  The return
    ///   value's type should be either a string, a double, or a CS3500.Formula.FormulaError.
    ///   If the cell contents are a formula, the value should have already been computed
    ///   at this point.  
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the provided name is invalid, throws an InvalidNameException.
    /// </exception>
    public object GetCellValue( string cellName );

    /// <summary>
    ///   <para>
    ///       Sets the contents of the named cell to the appropriate object
    ///       based on the string in <paramref name="content"/>.
    ///   </para>
    ///   <para>
    ///       First, if the <paramref name="content"/> parses as a double, the contents of the named
    ///       cell becomes that double.
    ///   </para>
    ///   <para>
    ///       Otherwise, if the <paramref name="content"/> begins with the character '=', an attempt is made
    ///       to parse the remainder of content into a Formula.
    ///   </para>
    ///   <para>
    ///       There are then three possible outcomes when a formula is detected:
    ///   </para>
    ///
    ///   <list type="number">
    ///     <item>
    ///       If the remainder of content cannot be parsed into a Formula, a
    ///       FormulaFormatException is thrown.
    ///     </item>
    ///     <item>
    ///       If changing the contents of the named cell to be f
    ///       would cause a circular dependency, a CircularException is thrown,
    ///       and no change is made to the spreadsheet.
    ///     </item>
    ///     <item>
    ///       Otherwise, the contents of the named cell becomes f.
    ///     </item>
    ///   </list>
    ///   <para>
    ///     Finally, if the content is a string that is not a double and does not
    ///     begin with an "=" (equal sign), save the content as a string.
    ///   </para>
    ///   <para>
    ///     On successfully changing the contents of a cell, the spreadsheet will be <see cref="Changed"/>.
    ///   </para>
    /// </summary>
    /// <param name="cellName"> The cell name that is being changed.</param>
    /// <param name="content"> The new content of the cell.</param>
    /// <returns>
    ///   <para>
    ///     This method returns a list of all cells that have been updated
    ///     (of course including the cell that was just updated).
    ///   </para>
    ///   <para>
    ///     For example, if cellName is "A1", and B1 contains A1*2, and C1 contains B1+A1, the
    ///     list containing [A1, B1, C1] is returned.
    ///   </para>
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the cellName parameter contains an invalid name, throw an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///   If changing the contents of the named cell to be the formula would
    ///   cause a circular dependency, throw a CircularException.
    ///   (NOTE: No change is made to the spreadsheet.)
    /// </exception>
    public IList<string> SetContentsOfCell( string cellName, string content );
}
