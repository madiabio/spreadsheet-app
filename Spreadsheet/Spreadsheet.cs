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


namespace CS3500.Spreadsheet;

using CS3500.DependencyGraph;
using CS3500.Formula;
using System.Text.RegularExpressions;

/// <summary>
///   <para>
///         Thrown to indicate that a change to a cell will cause a circular dependency.
///   </para>
/// </summary>
public class CircularException : Exception
{
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
public static class SpreadsheetUtils
{
    /// <summary>
    ///     This method validates if a given cell name is valid.
    /// </summary>
    /// <returns>
    ///     Returns false if the name is invalid, true otherwise.
    /// </returns>
    /// <param name="name">The name of the cell to validate.</param>
    public static bool IsValidCellName(string name)
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
///     within a spreadsheet object. These cells have a name, contents and a value.
/// </para>
/// <para>
///     This class determines whether a cell can be created based on valid cell names and values.
/// </para>
/// </summary>
public class Cell
{
    // Private fields to store the _contents, _value and _name of a cell.
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private string _name;
    private object _contents;
    private object _value;
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    ///     Gets or sets public property for the Name of the cell. Validates that all names are letters followed
    ///     by numbers (proper variable syntax).
    /// </summary>
    public string Name
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
    public object Contents
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
                UpdateValue();
            }
            else
            {
                throw new InvalidContentsException("Contents must be a string, double, or Formula.");
            }
        }
    }

    /// <summary>
    /// Gets the <see cref="Value"/> of a cell. The value of a cell may not be set.
    /// </summary>
    public object Value
    {
        get { return _value; }
    }

    /// <summary>
    /// Update the value of a cell based on the contents.
    /// </summary>
    private void UpdateValue()
    {
        // Value is dependent on object type of cell.
        switch (_contents)
        {
            case string str:
                _value = str; // If contents is a string, the value is the same string
                break;

            case double dbl:
                _value = dbl; // If contents is a double, the value is the same number
                break;

            case Formula formula:
                // TODO: for when value handling is implemented
                // _value = formula.Evaluate(); // Evaluate the formula and set the value
                _value = formula;
                break;
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
    private readonly DependencyGraph _dependencyGraph = new(); // Dependency graph containing all the dependencies of all of the cells in the spreadsheet
    private readonly Dictionary<string, Cell> _cells = []; // Dictionary containing all of the cell names pointing to their actual Cell object (cellName -> Cell object)

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
            if (_cells.TryGetValue(name, out Cell? value))
            {
                return value.Contents;
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
    public IList<string> SetCellContents(string name, double number)
    {
        // If valid name, check if cell exists in cells dictionary.
        if (SpreadsheetUtils.IsValidCellName(name))
        {
            name = name.ToUpper(); // Normalize the name to uppercase

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

            return GetCellsToRecalculate(name).ToList(); // return the list of cells that need to be recalculated
        }

        // if Invalid name, throw exception.
        throw new InvalidNameException($"{name} is not a valid cell name.");
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
    public IList<string> SetCellContents(string name, string text)
    {
        // If valid name, check if cell exists in cells dictionary.
        if (SpreadsheetUtils.IsValidCellName(name))
        {
            name = name.ToUpper(); // Normalize the name to uppercase

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
                }
            }

            return GetCellsToRecalculate(name).ToList(); // return the list of cells that need to be recalculated
        }

        // if Invalid name, throw exception.
        throw new InvalidNameException($"{name} is not a valid cell name.");
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
    public IList<string> SetCellContents(string name, Formula formula)
    {
        if (!SpreadsheetUtils.IsValidCellName(name))
        {
            throw new InvalidNameException($"{name} is not a valid cell name.");
        }

        name = name.ToUpper(); // normalize name
        ISet<string> variables = formula.GetVariables(); // get formula variables

        IEnumerable<string> toRecalc = []; // Stores cells to recalculate. May be un-needed

        IEnumerable<string> oldDependees = _dependencyGraph.GetDependees(name); // store og the dependees of the cell in case need to restore bc of circ except.
        IEnumerable<string> oldDependents = _dependencyGraph.GetDependents(name); // store og the dependents of the cell in case need to restore bc of circ except.

        IEnumerable<string> dependees = oldDependees as string[] ?? oldDependees.ToArray();
        IEnumerable<string> dependents = oldDependents as string[] ?? oldDependents.ToArray();

        if (variables.Any(var => (var == name) || dependents.Contains(var)))
        {
            throw new CircularException();
        }

        // If the cell has contents, check its dependencies and update.
        if (_cells.TryGetValue(name, out Cell? value))
        {
            object oldContents = value.Contents; // store old contents in case of  circular exception.
            value.Contents = formula; // update the contents of the cell

            _dependencyGraph.ReplaceDependees(name, variables); // replace the dependees of the cell with the new variables. need to do this b4 getcells2recalc because it relies on this.

            try
            {
                toRecalc = GetCellsToRecalculate(name);
            }
            catch (CircularException)
            {
                _cells[name].Contents = oldContents; // change the contents back to the original and don't update.

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
            };

            _cells.Add(name, newCell); // add cell to dictionary
            _dependencyGraph.ReplaceDependees(name, variables); // replace the dependees of the cell with the new variables. need to do this b4 getcells2recalc because it relies on this.

            try
            {
                toRecalc = GetCellsToRecalculate(name);
            }
            catch (CircularException)
            {
                _cells.Remove(name); // remove cell from dictionary if adding it creates a circular dependency.
                _dependencyGraph.ReplaceDependees(name, dependees); // Restore old dependees to cell in dg.
                throw;
            }
        }

        return toRecalc.ToList();
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
                throw new CircularException();
            }
            else if (!visited.Contains(dependent))
            {
                Visit(start, dependent, visited, changed);
            }
        }

        changed.AddFirst(name);
    }
}
