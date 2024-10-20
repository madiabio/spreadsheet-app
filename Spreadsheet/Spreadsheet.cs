// <copyright file="Spreadsheet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// Written by Joe Zachary for CS 3500, September 2013
// Update by Profs Kopta and de St. Germain
// - Updated return types
// - Updated documentation

// <summary>
// <para>
//     Authors/Partnership:    Madeline Abio & Sadie Bowen
//     Date:      20/09/2024
//     Course:    CS 3500, University of Utah, School of Computing
//     Copyright: CS 3500 and Madeline Abio - This work may not
//            be copied for use in Academic Coursework.
// </para>
//
// <para>
//     We, Madeline Abio & Sadie Bowen, certify that I wrote this code from scratch and
//     did not copy it in part or whole from another source.  All
//     references used in the completion of the assignments are cited
//     in my README file.
//
//     The starter code for this file was written by profs Joe, Danny and Jim.
//     The code for this file was updated in partnership of Madi Abio and Sadie Bowen,
//         however, the original code was written by Madi Abio for Assignment 05.
// </para>
// </summary>
#pragma warning disable SA1200
using System.Globalization;
#pragma warning restore SA1200

// ReSharper disable once CheckNamespace
namespace CS3500.Spreadsheet;

// ReSharper disable RedundantNameQualifier
using CS3500.DependencyGraph;
using CS3500.Formula;
using System.Text.Json;
using System.Text.RegularExpressions;

/// <summary>
/// <para>
///     Thrown to indicate that a read or write attempt has failed with
///     an expected error message informing the user of what went wrong.
/// </para>
/// </summary>
public class SpreadsheetReadWriteException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SpreadsheetReadWriteException"/> class.
    ///  <para>
    ///     Creates the exception with a message defining what went wrong.
    ///  </para>
    /// </summary>
    ///
    /// <param name="msg"> An informative message to the user. </param>
    public SpreadsheetReadWriteException( string msg )
        : base( msg )
    {
    }
}

/// <summary>
///   <para>
///         Thrown to indicate that a change to a cell will cause a circular dependency.
///   </para>
/// </summary>
public class CircularException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CircularException"/> class.
    ///  <para>
    ///     Creates the exception with a message defining what went wrong.
    ///  </para>
    /// </summary>
    ///
    /// <param name="msg"> An informative message to the user. </param>
    public CircularException( string msg )
        : base( msg )
    {
    }
}

/// <summary>
///   <para>
///         Thrown to indicate that a name parameter was invalid.
///   </para>
/// </summary>
public class InvalidNameException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InvalidNameException"/> class.
    ///     Allows a message to be added to the exception.
    /// </summary>
    /// <param name="message"> is a message containing information about the warning. </param>
    public InvalidNameException(string? message)
        : base(message)
    {
    }
}

/// <summary>
///   <para>
///         Thrown to indicate that the _contents attempting to be set are invalid.
///   </para>
/// </summary>
public class InvalidContentsException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InvalidContentsException"/> class.
    ///     Allows a message to be added to the exception.
    /// </summary>
    ///
    /// <param name="message"> is the message to be thrown. </param>
    public InvalidContentsException(string message)
    : base(message)
    {
    }
}

/// <summary>
///  <para>
///     See file header for important ownership and certification of authenticity details.
///  </para>
///
///     Extension class of spreadsheet. This class is designed to support functionality of Spreadsheet.
///     Contains a function for validating cell references.
/// </summary>
internal static class SpreadsheetUtils
{
    /// <summary>
    ///     This method validates if a given cell name is valid.
    /// </summary>
    /// <returns>
    ///     Returns false if the name is invalid, true otherwise.
    /// </returns>
    /// <param name="name">The name of the cell to validate.</param>
    internal static bool IsValidCellName(string name)
    {
        // Regex for valid variable names (letters followed by numbers.
        const string variableRegExPattern = @"^[a-zA-Z]+\d+$";

        // Asser that cell name is not null or empty and it is valid syntax.
        return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, variableRegExPattern);
    }
}

/// <summary>
/// <para>
///     See file header for important ownership and certification of authenticity details.
/// </para>
///
/// <para>
///     The <see cref="Cell"/> class is used in the <see cref="Spreadsheet"/> class to represent cells
///     within a spreadsheet object. These cells have a name, contents and a cell.
/// </para>
/// <para>
///     This class determines whether a cell can be created based on valid cell names and values.
/// </para>
/// </summary>
internal class Cell
{
    // Private fields to store the _contents, _value and _name of a cell.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null cell when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private string _name;
    private object _contents;
    private object _value;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null cell when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    ///     Gets or sets public property for the Name of the cell. Validates that all names are letters followed
    ///     by numbers (proper variable syntax).
    /// </summary>
    internal string Name
    {
        get
        {
            return _name;
        }

        set
        {
            if (SpreadsheetUtils.IsValidCellName(value))
            {
                _name = value.ToUpper();
            }
            else
            {
                throw new InvalidNameException($"{_name} is not a valid cell name.");
            }
        }
    }

    /// <summary>
    /// Gets or sets public property for the Contents of the cell.
    /// </summary>
    internal object Contents
    {
        get
        {
            return _contents;
        }

        // Contents of a cell can only be set if it is a string, double or Formula. Otherwise, an exception is thrown.
        set
        {
            // handle if string is empty
            if (value is string str && string.IsNullOrWhiteSpace(str))
            {
                throw new InvalidContentsException("Contents may not be null or whitespace.");
            }

            if (value is string or double or Formula)
            {
                _contents = value;
            }
            else
            {
                throw new InvalidContentsException("Contents must be a string, double, or Formula.");
            }
        }
    }

    /// <summary>
    /// Gets or sets the <see cref="Value"/> of a cell. Since the cell class is internal to the spreadsheet,
    /// Value is accessible. It should only be managed by <see cref="Spreadsheet.SetContentsOfCell(string, string)"/>.
    /// </summary>
    internal object Value
    {
        get
        {
            return _value;
        }

        set
        {
            _value = value;
        }
    }

    /// <summary>
    ///  Gets the contents of the cell in string form.
    /// </summary>
    internal string StringForm
    {
        get
        {
            return _contents switch
            {
                string contents => contents,
                double d => d.ToString(CultureInfo.InvariantCulture),
                Formula => "=" + Contents,
                _ => string.Empty
            };
        }
    }
}

/// <summary>
/// <para>
///     See file header for important ownership and certification of authenticity information.
/// </para>
///
/// <para>
///     This file represents a the state of a Spreadsheet object. A spreadsheet represents an infinite number
///     of named cells. A cell can represent 3 different types: A string of text, a number (double), or a Formula.
/// </para>
/// </summary>
public class Spreadsheet
{
    /// <summary>
    ///     Name of spreadsheet created.
    /// </summary>
    private string _spreadsheetName; // Don't change because 'name' is used as a variable a lot in methods.

    private readonly DependencyGraph _dependencyGraph = new(); // Dependency graph containing all the dependencies of all of the cells in the spreadsheet
    private readonly Dictionary<string, Cell> _cells = []; // Dictionary containing all of the cell names pointing to their actual Cell object (cellName -> Cell object)

    /// <summary>
    ///     Initializes a new instance of the <see cref="Spreadsheet"/> class.
    ///     Construct a new spreadsheet that uses a name to store spreadsheet object.
    ///     If no name is passed through, then the name of the spreadsheet is set to "default".
    /// </summary>
    /// <param name="nameInput"> string name to be stored for spreadsheet. </param>
    public Spreadsheet(string nameInput)
    {
        _spreadsheetName = nameInput;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Spreadsheet"/> class.
    ///     Construct a new spreadsheet that uses a name to store spreadsheet object.
    ///     If no name is passed through, then the name of the spreadsheet is set to "default".
    /// </summary>
    public Spreadsheet()
    {
        _spreadsheetName = "default";
    }

    /// <summary>
    ///     Gets a value indicating whether the spreadsheet has been modified since the last save or initial load of a spreadsheet.
    /// </summary>
    #pragma warning disable SA1401
    public bool Changed { get; private set; } = true; // Flag to track if the spreadsheet has been modified
    #pragma warning restore SA1401

    /// <summary>
    /// <para>
    ///     Shortcut syntax to for getting the cell of the cell
    ///     using the [] operator.
    /// </para>
    /// <para>
    ///     See: <see cref="GetCellValue(string)"/>.
    /// </para>
    /// <para>
    ///     Example Usage:
    /// </para>
    /// <code>
    ///     sheet.SetContentsOfCell( "A1", "=5+5" );
    ///     sheet["A1"] == 10; vs. sheet.GetCellValue("A1") == 10;
    /// </code>
    /// </summary>
    /// <param name="cellName"> Any valid cell name. </param>
    /// <returns>
    ///     Returns the cell of a cell. Note: If the cell is a formula, the cell
    ///     should
    ///     already have been computed.
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///     If the name parameter is invalid, throw an InvalidNameException.
    /// </exception>
    public object this[string cellName]
    {
        get { return GetCellValue(cellName); }
    }

    /// <summary>
    /// <para>
    ///     Writes the contents of this spreadsheet to the named file using a JSON format.
    ///     If the file already exists, overwrite it.
    /// </para>
    /// <para>
    ///     The output JSON should look like the following.
    /// </para>
    /// <para>
    ///     For example, consider a spreadsheet that contains a cell "A1"
    ///     with contents being the double 5.0, and a cell "B3" with contents
    ///     being the Formula("A1+2"), and a cell "C4" with the contents "hello".
    /// </para>
    /// <para>
    ///     This method would produce the following JSON string:
    /// </para>
    /// <code>
    /// {
    ///  "Cells": {
    ///     "A1": {
    ///         "StringForm": "5"
    ///     },
    ///     "B3": {
    ///         "StringForm": "=A1+2"
    ///     },
    ///     "C4": {
    ///         "StringForm": "hello"
    ///     }
    ///  }
    /// }
    /// </code>
    /// <para>
    ///     You can achieve this by making sure your data structure is a dictionary
    ///     and that the contained objects (Cells) have property named "StringForm"
    ///     (if this name does not match your existing code, use the JsonPropertyName attribute).
    /// </para>
    /// <para>
    ///     There can be 0 cells in the dictionary, resulting in { "Cells" : {} }.
    /// </para>
    /// <para>
    ///     Further, when writing the cell of each cell...
    /// </para>
    /// <list type="bullet">
    /// <item>
    ///     If the contents is a string, the cell of StringForm is that string
    /// </item>
    /// <item>
    ///     If the contents is a double d, the cell of StringForm is d.ToString()
    /// </item>
    /// <item>
    ///     If the contents is a Formula f, the cell of StringForm is "=" + f.ToString()
    /// </item>
    /// </list>
    /// <para>
    ///     After saving the file, the spreadsheet is no longer "changed".
    /// </para>
    /// </summary>
    /// <param name="filename"> The name (with path) of the file to save to.</param>
    /// <exception cref="SpreadsheetReadWriteException">
    ///     If there are any problems opening, writing, or closing the file,
    ///     the method should throw a SpreadsheetReadWriteException with an
    ///     explanatory message.
    /// </exception>
    public void Save(string filename)
    {
        bool changedTemp = Changed; // save state in case of error
        try
        {
            // 1. Create a serializable structure for the cells
            var cellsData = new Dictionary<string, Dictionary<string, string>>();

            foreach (var cellEntry in _cells)
            { // Iterate thru each cell in the spreadsheet
                string cellName = cellEntry.Key;
                Cell cell = cellEntry.Value;

                // Create the cell's serializable content in the expected format
                var cellData = new Dictionary<string, string>
                {
                    { "StringForm", cell.StringForm },
                };

                cellsData[cellName] = cellData;
            }

            // 2. Prepare the final structure to be serialized
            var spreadsheetData = new Dictionary<string, object>
            {
                { "Cells", cellsData },
            };

            // 3. Serialize the data to JSON
            string json = JsonSerializer.Serialize(spreadsheetData, new JsonSerializerOptions { WriteIndented = true });

            // 4. Write the JSON to the specified file
            File.WriteAllText(filename, json);

            // 5. After saving, mark the spreadsheet as unchanged
            Changed = false;
        }
        catch (Exception exception)
        {
            // Handle any issues with opening/writing the file
            Changed = changedTemp; // revert changed back to old status
            throw new SpreadsheetReadWriteException($"Error saving the spreadsheet to file '{filename}': {exception.Message}");
        }
    }

    /// <summary>
    /// <para>
    ///     Read the data (JSON) from the file and instantiate the current
    ///     spreadsheet. See <see cref="Save(string)"/> for expected format.
    /// </para>
    /// <para>
    ///     Note: First deletes any current data in the spreadsheet.
    /// </para>
    /// <para>
    ///     Loading a spreadsheet should set changed to false. External
    ///     programs should alert the user before loading over a changed sheet.
    /// </para>
    /// </summary>
    /// <param name="filename"> The saved file name including the path. </param>
    /// <exception cref="SpreadsheetReadWriteException"> When the file cannot be
    ///     opened or the json is bad.</exception>
    public void Load( string filename )
    {
        bool changedTemp = Changed; // save state in case of error

        try
        {
            string jsonString = File.ReadAllText(filename);
            Spreadsheet loadSpreadsheet = JsonSerializer.Deserialize<Spreadsheet>(jsonString) ?? throw new InvalidOperationException(); // assignment Json to new spreadsheet.
            _cells.Clear();

            foreach (var cellEntry in loadSpreadsheet._cells)
            { // Iterate thru each cell in the spreadsheet
                Cell cell = cellEntry.Value;
                SetContentsOfCell(cellEntry.Key, cell.Contents.ToString() ?? throw new InvalidOperationException());
            }

            _spreadsheetName = filename;
            Changed = false;
        }
        catch (Exception exception)
        {
            // Handle any issues with opening/writing the file
            Changed = changedTemp; // revert changed back to old status
            throw new SpreadsheetReadWriteException($"Error loading the spreadsheet to file '{filename}': {exception.Message}");
        }
    }

    /// <summary>
    /// <para>
    ///     Return the cell of the named cell.
    /// </para>
    /// </summary>
    /// <param name="cellName"> The cell in to be evaluated. </param>
    /// <returns>
    ///     Returns the cell (as opposed to the contents) of the named cell. The return
    ///     cell's type should be either a string, a double, or a
    ///     CS3500.Formula.FormulaError.
    ///     If the cell contents are a formula, the cell should have already been computed at this point.
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///     If the provided name is invalid, throws an InvalidNameException.
    /// </exception>
    public object GetCellValue(string cellName)
    {
        if (!SpreadsheetUtils.IsValidCellName(cellName))
        {
            throw new InvalidNameException($"{cellName} is not a valid cell name.");
        }

        // Attempt to get a cell from the cell
        if (_cells.TryGetValue(cellName, out Cell? cell))
        {
            return cell.Value;
        }

        return string.Empty; // If no cell available, cell is empty.
    }

    /// <summary>
    /// <para>
    ///     Sets the contents of the named cell to the appropriate object
    ///     based on the string in <paramref name="content"/>.
    /// </para>
    /// <para>
    ///     First, if the <paramref name="content"/> parses as a double, the
    ///     contents of the named cell becomes that double.
    /// </para>
    /// <para>
    ///     Otherwise, if the <paramref name="content"/> begins with the
    ///     character '=', an attempt is made
    ///     to parse the remainder of content into a Formula.
    /// </para>
    /// <para>
    ///     There are then three possible outcomes when a formula is detected:
    /// </para>
    ///
    /// <list type="number">
    /// <item>
    ///     If the remainder of content cannot be parsed into a Formula, a
    ///     FormulaFormatException is thrown.
    /// </item>
    /// <item>
    ///     If changing the contents of the named cell to be f
    ///     would cause a circular dependency, a CircularException is thrown,
    ///     and no change is made to the spreadsheet.
    /// </item>
    /// <item>
    ///     Otherwise, the contents of the named cell becomes f.
    /// </item>
    /// </list>
    /// <para>
    ///     Finally, if the content is a string that is not a double and does not
    ///     begin with an "=" (equal sign), save the content as a string.
    /// </para>
    /// <para>
    ///     On successfully changing the contents of a cell, the spreadsheet will be <see cref="Changed"/>.
    /// </para>
    /// </summary>
    /// <param name="cellName"> The cell name that is being changed.</param>
    /// <param name="content"> The new content of the cell.</param>
    /// <returns>
    /// <para>
    ///     This method returns a list of all cells that have been updated
    ///     (of course including the cell that was just updated).
    /// </para>
    /// <para>
    ///     For example, if cellName is "A1", and B1 contains A1*2, and C1 contains
    ///     B1+A1, the
    ///     list containing [A1, B1, C1] is returned.
    /// </para>
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///     If the cellName parameter contains an invalid name, throw an
    ///     InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///     If changing the contents of the named cell to be the formula would
    ///     cause a circular dependency, throw a CircularException.
    ///     (NOTE: No change is made to the spreadsheet.)
    /// </exception>
    public IList<string> SetContentsOfCell(string cellName, string content)
    {
        cellName = cellName.ToUpper(); // Normalize the name to uppercase
        if (SpreadsheetUtils.IsValidCellName(cellName))
        {
            // Determine type of content
            IList<string> toRecalculate; // Cells to be recalculated
            if (double.TryParse(content, out double doubleContent))
            {
                // if contents are double:
                toRecalculate = SetCellContents(cellName, doubleContent);
            }
            else if (content.Length > 0 && content[0] == '=')
            {
                // if contents are formula:
                Formula fContent = new(content.Substring(1));
                toRecalculate = SetCellContents(cellName, fContent);
            }
            else
            {
                // if contents are string:
                toRecalculate = SetCellContents(cellName, content);
            }

            return toRecalculate;
        }

        throw new InvalidNameException($"{cellName} is not a valid cell name");
    }

    /// <summary>
    ///     Provides a copy of the names of all of the cells in the spreadsheet
    ///     that contain information (i.e., not empty cells).
    /// </summary>
    ///
    /// <returns>
    ///     A set of the names of all the non-empty cells in the spreadsheet.
    /// </returns>
    public ISet<string> GetNamesOfAllNonemptyCells()
    {
        return new HashSet<string>(_cells.Keys);
    }

    /// <summary>
    ///     Returns the _contents (as opposed to the _value) of the named cell.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///     Thrown if the name is invalid.
    /// </exception>
    ///
    /// <param name="name">The name of the spreadsheet cell to query. </param>
    ///
    /// <returns>
    ///     The _contents as either a string, a double, or a Formula.
    ///     See the class header summary.
    /// </returns>
    public object GetCellContents(string name)
    {
        // if a valid name is given, return either the contents of the cell,
        // or an empty string (if the cell has no contents)
        if (SpreadsheetUtils.IsValidCellName(name))
        {
            if (_cells.TryGetValue(name, out Cell? cell))
            {
                return cell.Contents;
            }

            return string.Empty;
        }

        throw new InvalidNameException($"{name} is not a valid cell name.");
    }

    /// <summary>
    ///     Set the contents of the named cell to the given number.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///     If the name is invalid, throw an InvalidNameException.
    /// </exception>
    ///
    /// <param name="name"> The name of the cell. </param>
    /// <param name="number"> The new content of the cell. </param>
    ///
    /// <returns>
    ///   <para>
    ///     This method returns an ordered list consisting of the passed in name
    ///     followed by the names of all other cells whose _value depends, directly
    ///     or indirectly, on the named cell.
    ///   </para>
    /// </returns>
    private IList<string> SetCellContents(string name, double number)
    {
        // If the cell has contents, check its dependencies and update.
        if (_cells.TryGetValue(name, out Cell? cell))
        {
            cell.Contents = number; // update the contents of the cell
            _dependencyGraph.ReplaceDependees(name, new HashSet<string>()); // remove all dependees of the cell because it now a constant.
        }

        // If empty cell, create new cell, add it to cells dictionary and update its contents.
        else
        {
            Cell newCell = new();
            newCell.Name = name;
            newCell.Contents = number;
            _cells.Add(name, newCell);
            _dependencyGraph.ReplaceDependees(name, new HashSet<string>()); // remove all dependees of the cell because it now a constant.
        }

        _cells[name].Value = number; // update cell of cell
        return GetCellsToRecalculate(name).ToList(); // return the list of cells that need to be recalculated
    }

    /// <summary>
    ///   The _contents of the named cell becomes the given text.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    ///
    /// <param name="name"> The name of the cell. </param>
    /// <param name="text"> The new content of the cell. </param>
    ///
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, string text)
    {
        // If the cell has contents, check its dependencies and update.
        if (_cells.ContainsKey(name))
        {
            if (string.IsNullOrEmpty(text))
            {
                _cells.Remove(name); // remove the cell if the text is empty (cell is now empty)
            }
            else
            {
                _cells[name].Contents = text; // update the contents of the cell
                _cells[name].Value = text; // update cell of cell
            }

            _dependencyGraph.ReplaceDependees(name, new HashSet<string>()); // remove all dependees of the cell because it now a constant.
        }

        // If empty cell, create new, add it to cells dictionary and update its contents.
        else
        {
            // only update if the string isn't empty.
            if ( !string.IsNullOrEmpty(text) )
            {
                Cell newCell = new();
                newCell.Name = name;
                newCell.Contents = text;
                _cells.Add(name, newCell);
                _cells[name].Value = text; // update cell of cell
            }
        }

        return GetCellsToRecalculate(name).ToList(); // return the list of cells that need to be recalculated
    }

    /// <summary>
    ///   Set the _contents of the named cell to the given formula.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    ///
    /// <exception cref="CircularException">
    ///   <para>
    ///     If changing the _contents of the named cell to be the formula would
    ///     cause a circular dependency, throw a CircularException.
    ///     No change will be made to the spreadsheet.
    ///   </para>
    /// </exception>
    ///
    /// <param name="name"> The name of the cell. </param>
    /// <param name="formula"> The new content of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, Formula formula)
    {
        ISet<string> variables = formula.GetVariables(); // get formula variables

        IEnumerable<string> cellsToRecalculate; // Stores cells to recalculate. May be un-needed

        IEnumerable<string> oldDependees = _dependencyGraph.GetDependees(name); // store og the dependees of the cell in case need to restore bc of circ except.
        IEnumerable<string> oldDependents = _dependencyGraph.GetDependents(name); // store og the dependents of the cell in case need to restore bc of circ except.

        IEnumerable<string> dependees = oldDependees as string[] ?? oldDependees.ToArray();
        IEnumerable<string> dependents = oldDependents as string[] ?? oldDependents.ToArray();

        if (variables.Any(var => (var == name) || dependents.Contains(var)))
        {
            throw new CircularException("A cell cannot reference itself.");
        }

        // If the cell has contents, check its dependencies and update.
        if (_cells.TryGetValue(name, out Cell? cell))
        {
            object oldContents = cell.Contents; // store old contents in case of  circular exception.
            object oldValue = cell.Value; // store old cell in case of circular exception

            _cells[name].Contents = formula; // update the contents of the cell
            _cells[name].Value = formula.Evaluate(s =>
            {
                var cellValue = GetCellValue(s);

                // Try to parse the cell cell as a double; if successful, return the cell, otherwise throw an exception
                if (double.TryParse(cellValue.ToString(), out double val))
                {
                    return val;
                }
                else
                {
                    throw new ArgumentException("Cell does not resolve to numeric cell");
                }
            }); // update the cell of the cell

            _dependencyGraph.ReplaceDependees(name, variables); // replace the dependees of the cell with the new variables. need to do this b4 getcells2recalc because it relies on this.

            try
            {
                cellsToRecalculate = GetCellsToRecalculate(name);
            }
            catch (CircularException)
            {
                _cells[name].Contents = oldContents; // change the contents back to the original and don't update.
                _cells[name].Value = oldValue;
                _dependencyGraph.ReplaceDependees(name, dependees); // Restore old dependees to cell in dg.
                throw;
            }
        }

        // If empty cell, create new, add it to cells dictionary and update its contents.
        else
        {
            Cell newCell = new()
            {
                Name = name,
                Contents = formula,
                Value = formula.Evaluate(s =>
                {
                    var cellValue = GetCellValue(s);

                    // Try to parse the cell cell as a double; if successful, return the cell, otherwise throw an exception
                    if (double.TryParse(cellValue.ToString(), out double val))
                    {
                        return val;
                    }
                    else
                    {
                        throw new ArgumentException("Cell does not resolve to numeric cell");
                    }
                }), // update the cell of the cell
            };

            _cells.Add(name, newCell); // add cell to dictionary
            _dependencyGraph.ReplaceDependees(name, variables); // replace the dependees of the cell with the new variables. need to do this b4 getcells2recalc because it relies on this.

            try
            {
                cellsToRecalculate = GetCellsToRecalculate(name);
            }
            catch (CircularException)
            {
                _cells.Remove(name); // remove cell from dictionary if adding it creates a circular dependency.
                _dependencyGraph.ReplaceDependees(name, dependees); // Restore old dependees to cell in dg.
                throw;
            }
        }

        return cellsToRecalculate.ToList();
    }

    /// <summary>
    ///     Returns an enumeration, without duplicates, of the names of all cells whose
    ///     values depend directly on the _value of the named cell.
    /// </summary>
    /// <param name="name"> This <b>MUST</b> be a valid name.  </param>
    /// <returns>
    ///   <para>
    ///     Returns an enumeration, without duplicates, of the names of all cells
    ///     that contain formulas containing name.
    ///   </para>
    /// </returns>
    private IEnumerable<string> GetDirectDependents(string name)
    {
        return _dependencyGraph.GetDependents(name);
    }

    /// <summary>
    ///   <para>
    ///     This method was included in starter code. See file header for ownership.
    ///   </para>
    ///
    ///   <para>
    ///     Returns an enumeration of the names of all cells whose values must
    ///     be recalculated, assuming that the _contents of the cell referred
    ///     to by name has changed.  The cell names are enumerated in an order
    ///     in which the calculations should be done.
    ///   </para>
    ///
    ///   <exception cref="CircularException">
    ///     If the cell referred to by name is involved in a circular dependency,
    ///     throws a CircularException.
    ///   </exception>
    /// </summary>
    ///
    /// <param name="name"> The name of the cell.  Requires that name be a valid cell name.</param>
    /// <returns>
    ///    Returns an enumeration of the names of all cells whose values must
    ///    be recalculated.
    /// </returns>
    private IEnumerable<string> GetCellsToRecalculate(string name)
    {
        LinkedList<string> changed = new();
        HashSet<string> visited = [];
        Visit(name, name, visited, changed);
        return changed;
    }

    /// <summary>
    ///     This function tracks the visited nodes by using an ordered list. It
    ///     iterates through each direct dependent of a node. If the dependent is equal to the
    ///     start node, a CircularException is thrown (a circular dependency was found).
    ///     Otherwise, the next unvisited dependent is checked.
    ///     If a node was changed, it will be placed at the beginning of the ordered list.
    /// </summary>
    private void Visit(string start, string name, ISet<string> visited, LinkedList<string> changed)
    {
        visited.Add(name);
        foreach (string dependent in GetDirectDependents(name))
        {
            if (dependent.Equals(start))
            {
                throw new CircularException("A cell cannot reference itself.");
            }
            else if (!visited.Contains(dependent))
            {
                Visit(start, dependent, visited, changed);
            }
        }

        changed.AddFirst(name);
    }
}
