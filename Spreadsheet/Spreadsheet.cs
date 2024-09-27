// <copyright file="Spreadsheet.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

// Written by Joe Zachary for CS 3500, September 2013
// Update by Profs Kopta and de St. Germain
//     - Updated return types
//     - Updated documentation

/// <summary>
/// Author:    Madeline Abio
/// Partner:   N/A
/// Date:      20/09/2024
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Madeline Abio - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Madeline Abio, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///
///    This file contains the Spreadsheet class, which is used to store cells and their information.
/// </summary>
namespace CS3500.Spreadsheet;

using CS3500.Formula;
using CS3500.DependencyGraph;
using System.Text.RegularExpressions;

/// <summary>
///   <para>
///     Thrown to indicate that a change to a cell will cause a circular dependency.
///   </para>
/// </summary>
public class CircularException : Exception
{

}

/// <summary>
///   <para>
///     Thrown to indicate that a name parameter was invalid.
///   </para>
/// </summary>
public class InvalidNameException : Exception
{
    public InvalidNameException(string? message) : base(message)
    {
    }
}

/// <summary>
///   <para>
///     Thrown to indicate that the _contents attempting to be set are invalid.
///   </para>
/// </summary>
public class InvalidContentsException : Exception
{
    public InvalidContentsException(string message) // allow a message to be thrown with the exception
    : base(message)
    {
    }

}

/// <summary>
/// <para>
/// The <see cref="Cell"/> class is used in the <see cref="Spreadsheet"/> class to contain
/// cell name, cell contents, and cell value of each cell.
/// </para>
/// <para>
/// This class also handles errors regarding setting invalid cell names and values.
/// </para>
/// </summary>
public class Cell
{
    // Private fields to store the _contents and _value
    private string _name;
    private object _contents;
    private object _value;

    /// <summary>
    ///   All variables are letters followed by numbers.  This pattern
    ///   represents valid variable name strings.
    /// </summary>
    private const string VariableRegExPattern = @"^[a-zA-Z]+\d+$";


    // Public property for the Name of the cell
    public string Name
    {
        get { return _name; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidNameException("Cell name cannot be null or empty.");
            }
            else if (!Regex.IsMatch(value, VariableRegExPattern))
            {
                throw new InvalidNameException("Invalid name pattern.");
            }
            _name = value.ToUpper();


        }
    }

    // Public property for the Contents of the cell
    public object Contents
    {
        get { return _contents; }
        set // Only allow set for string, double or Formula data types. Otherwise, throw exception.
        {
            if (value is string str && string.IsNullOrWhiteSpace(str)) // handle if string  is empty
            {
                throw new InvalidContentsException("Contents may not be null or whitespace.");
            }
            else if ( value is string || value is double || value is Formula)
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

    public object Value
    {
        get { return _value; }
    }

    // Update the value based on the contents
    private void UpdateValue()
    {
        if (_contents is string str)
        {
            _value = str; // If contents is a string, the value is the same string
        }
        else if (_contents is double dbl)
        {
            _value = dbl; // If contents is a double, the value is the same number
        }
        else if (_contents is Formula formula)
        {
            // FIXME: for when Evaluate is implemented
            //_value = formula.Evaluate(); // Evaluate the formula and set the value
            _value = formula;
        }
    }
}



/// <summary>
///   <para>
///     An Spreadsheet object represents the state of a simple spreadsheet.  A
///     spreadsheet represents an infinite number of named cells.
///   </para>
/// <para>
///     Valid Cell Names: A string is a valid cell name if and only if it is one or
///     more letters followed by one or more numbers, e.g., A5, BC27.
/// </para>
/// <para>
///    Cell names are case insensitive, so "x1" and "X1" are the same cell name.
///    Your code should normalize (uppercased) any stored name but accept either.
/// </para>
/// <para>
///     A spreadsheet represents a cell corresponding to every possible cell name.  (This
///     means that a spreadsheet contains an infinite number of cells.)  In addition to
///     a name, each cell has a _contents and a _value.  The distinction is important.
/// </para>
/// <para>
///     The <b>_contents</b> of a cell can be (1) a string, (2) a double, or (3) a Formula.
///     If the _contents of a cell is set to the empty string, the cell is considered empty.
/// </para>
/// <para>
///     By analogy, the _contents of a cell in Excel is what is displayed on
///     the editing line when the cell is selected.
/// </para>
/// <para>
///     In a new spreadsheet, the _contents of every cell is the empty string. Note:
///     this is by definition (it is IMPLIED, not stored).
/// </para>
/// <para>
///     The <b>_value</b> of a cell can be (1) a string, (2) a double, or (3) a FormulaError.
///     (By analogy, the _value of an Excel cell is what is displayed in that cell's position
///     in the grid.)
/// </para>
/// <list type="number">
///   <item>If a cell's _contents is a string, its _value is that string.</item>
///   <item>If a cell's _contents is a double, its _value is that double.</item>
///   <item>
///     <para>
///       If a cell's _contents is a Formula, its _value is either a double or a FormulaError,
///       as reported by the Evaluate method of the Formula class.  For this assignment,
///       you are not dealing with values yet.
///     </para>
///   </item>
/// </list>
/// <para>
///     Spreadsheets are never allowed to contain a combination of Formulas that establish
///     a circular dependency.  A circular dependency exists when a cell depends on itself.
///     For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
///     A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
///     dependency.
/// </para>
/// </summary>

public class Spreadsheet
{

    private DependencyGraph dg = new DependencyGraph(); // Dependency graph containing all the dependencies of all of the cells in the spreadsheet
    private HashSet<string> cellNames = new HashSet<string>(); // HashSet containing all of the cell names in the spreadsheet

    /// <summary>
    ///   Provides a copy of the names of all of the cells in the spreadsheet
    ///   that contain information (i.e., not empty cells).
    /// </summary>
    /// <returns>
    ///   A set of the names of all the non-empty cells in the spreadsheet.
    /// </returns>
    public ISet<string> GetNamesOfAllNonemptyCells()
    {
        return cellNames;
    }

    /// <summary>
    ///   Returns the _contents (as opposed to the _value) of the named cell.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   Thrown if the name is invalid.
    /// </exception>
    ///
    /// <param name="name">The name of the spreadsheet cell to query. </param>
    /// <returns>
    ///   The _contents as either a string, a double, or a Formula.
    ///   See the class header summary.
    /// </returns>
    public object GetCellContents(string name)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///  Set the _contents of the named cell to the given number.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    ///
    /// <param name="name"> The name of the cell. </param>
    /// <param name="number"> The new content of the cell. </param>
    /// <returns>
    ///   <para>
    ///     This method returns an ordered list consisting of the passed in name
    ///     followed by the names of all other cells whose _value depends, directly
    ///     or indirectly, on the named cell.
    ///   </para>
    ///   <para>
    ///     The order must correspond to a valid dependency ordering for recomputing
    ///     all of the cells, i.e., if you re-evaluate each cell in the order of the list,
    ///     the overall spreadsheet will be correctly updated.
    ///   </para>
    ///   <para>
    ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    ///     list [A1, B1, C1] is returned, i.e., A1 was changed, so then A1 must be
    ///     evaluated, followed by B1 re-evaluated, followed by C1 re-evaluated.
    ///   </para>
    /// </returns>
    public IList<string> SetCellContents(string name, double number)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///   The _contents of the named cell becomes the given text.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="text"> The new content of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    public IList<string> SetCellContents(string name, string text)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///   Set the _contents of the named cell to the given formula.
    /// </summary>
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///   <para>
    ///     If changing the _contents of the named cell to be the formula would
    ///     cause a circular dependency, throw a CircularException.
    ///   </para>
    ///   <para>
    ///     No change is made to the spreadsheet.
    ///   </para>
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="formula"> The new content of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    public IList<string> SetCellContents(string name, Formula formula)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///   Returns an enumeration, without duplicates, of the names of all cells whose
    ///   values depend directly on the _value of the named cell.
    /// </summary>
    /// <param name="name"> This <b>MUST</b> be a valid name.  </param>
    /// <returns>
    ///   <para>
    ///     Returns an enumeration, without duplicates, of the names of all cells
    ///     that contain formulas containing name.
    ///   </para>
    ///   <para>For example, suppose that: </para>
    ///   <list type="bullet">
    ///      <item>A1 contains 3</item>
    ///      <item>B1 contains the formula A1 * A1</item>
    ///      <item>C1 contains the formula B1 + A1</item>
    ///      <item>D1 contains the formula B1 - C1</item>
    ///   </list>
    ///   <para> The direct dependents of A1 are B1 and C1. </para>
    /// </returns>
    private IEnumerable<string> GetDirectDependents(string name)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///   <para>
    ///     This method is implemented for you, but makes use of your GetDirectDependents.
    ///   </para>
    ///   <para>
    ///     Returns an enumeration of the names of all cells whose values must
    ///     be recalculated, assuming that the _contents of the cell referred
    ///     to by name has changed.  The cell names are enumerated in an order
    ///     in which the calculations should be done.
    ///   </para>
    ///   <exception cref="CircularException">
    ///     If the cell referred to by name is involved in a circular dependency,
    ///     throws a CircularException.
    ///   </exception>
    ///   <para>
    ///     For example, suppose that:
    ///   </para>
    ///   <list type="number">
    ///     <item>
    ///       A1 contains 5
    ///     </item>
    ///     <item>
    ///       B1 contains the formula A1 + 2.
    ///     </item>
    ///     <item>
    ///       C1 contains the formula A1 + B1.
    ///     </item>
    ///     <item>
    ///       D1 contains the formula A1 * 7.
    ///     </item>
    ///     <item>
    ///       E1 contains 15
    ///     </item>
    ///   </list>
    ///   <para>
    ///     If A1 has changed, then A1, B1, C1, and D1 must be recalculated,
    ///     and they must be recalculated in an order which has A1 first, and B1 before C1
    ///     (there are multiple such valid orders).
    ///     The method will produce one of those enumerations.
    ///   </para>
    ///   <para>
    ///      PLEASE NOTE THAT THIS METHOD DEPENDS ON THE METHOD GetDirectDependents.
    ///      IT WON'T WORK UNTIL GetDirectDependents IS IMPLEMENTED CORRECTLY.
    ///   </para>
    /// </summary>
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
    ///   A helper for the GetCellsToRecalculate method.
    ///   FIXME: You should fully comment what is going on below using XML tags as appropriate.
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

