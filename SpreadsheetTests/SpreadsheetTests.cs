// <copyright file="SpreadsheetTests.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

// ReSharper disable once CheckNamespace
namespace CS3500.SpreadsheetTests;

// ReSharper disable RedundantNameQualifier
using CS3500.Formula;
using CS3500.Spreadsheet;

/// <summary>
/// <para>
///     Authors/Partnership:    Madeline Abio & Sadie Bowen
///     Date:      20/09/2024
///     Course:    CS 3500, University of Utah, School of Computing
///     Copyright: CS 3500 and Madeline Abio - This work may not
///            be copied for use in Academic Coursework.
/// </para>
///
/// <para>
///     We, Madeline Abio & Sadie Bowen, certify that I wrote this code from scratch and
///     did not copy it in part or whole from another source.  All
///     references used in the completion of the assignments are cited
///     in my README file.
///
///     The starter code for this file was written by profs Joe, Danny and Jim.
///     The code for this file was updated in partnership of Madi Abio and Sadie Bowen,
///         however, the original code was written by Madi Abio for Assignment 05.
/// </para>
///
/// <para>
///     This class represents unit tests used to determine the accuracy of the Spreadsheet project implementation.
///     These tests cannot prove whether the project is correct, however it seeks to expose errors in the project.
///     This class represents complete testing coverage of the Spreadsheet project.
/// </para>
/// </summary>
[TestClass]
public class SpreadsheetTests
{
    // ----------- TEST GetCellContents function-----------

    /// <summary>
    /// Checks a <see cref="InvalidNameException"/> is thrown when an invalid cell name is passed
    /// into <see cref="Spreadsheet.GetCellContents(string)"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SpreadsheetConstructor_TestGetCellContents_InvalidName()
    {
        Spreadsheet ss = new();
        ss.GetCellContents("1A1A");
    }

    // ----------- TEST this[string cellName] accessor -----------
    // ----------- TEST Save function -----------
    // ----------- TEST Load function -----------
    // ----------- TEST GetCellValue function -----------

    // ----------- TEST SetContentsOfCell -----------

    /// <summary>
    /// Checks a <see cref="InvalidNameException"/> is thrown when an invalid cell name is passed
    /// into <see cref="Spreadsheet.SetCellContents(string, string)"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SpreadsheetConstructor_TestSetCellContentsString_InvalidName()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("1A1A", "test");
    }

    /// <summary>
    /// Checks a <see cref="InvalidNameException"/> is thrown when an invalid cell name is passed
    /// into <see cref="Spreadsheet.SetCellContents(string, Formula)"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SpreadsheetConstructor_TestSetCellContentsFormula_InvalidName()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("1A1A", "B1+1");
    }

    /// <summary>
    /// Tests <see cref="Spreadsheet.SetCellContents(string, string)"/> when the cell being set had dependees before this
    /// method call.
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsStringWithDependees_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("A1", "B1*2"); // A is dependent on B, meaning A has a dependee (B)
        ss.SetContentsOfCell("B1", "test");

        List<string> results = ss.SetContentsOfCell("A1", "test").ToList(); // update A to no longer be dependent on B.
        List<string> expected = new() { "A1" };

        CollectionAssert.AreEqual(expected, results);
    }

    /// <summary>
    /// Tests <see cref="Spreadsheet.SetCellContents(string, double)"/> when the cell being set had dependees before this
    /// method call.
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsDoubleWithDependees_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("A1", "B1*2"); // A is dependent on B, meaning A has a dependee (B)
        ss.SetContentsOfCell("B1", "test");

        List<string> results = ss.SetContentsOfCell("A1", "1.1").ToList(); // update A to no longer be dependent on B.
        List<string> expected = new() { "A1" };

        CollectionAssert.AreEqual(expected, results);
    }

    /// <summary>
    /// Checks the ordering of dependencies for the return value of <see cref="Spreadsheet.SetCellContents(string, Formula)"/>.
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsDoubleReturn_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        List<string> results = ss.SetContentsOfCell("A1", "1.1").ToList();

        List<string> expected = new() { "A1" };
        CollectionAssert.AreEqual(expected, results);
    }

    /// <summary>
    /// Checks the ordering of dependencies for the return value of <see cref="Spreadsheet.SetCellContents(string, Formula)"/>.
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsStringReturn_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        List<string> results = ss.SetContentsOfCell("A1", "test").ToList();

        List<string> expected = new() { "A1" };
        CollectionAssert.AreEqual(expected, results);
    }

    /// <summary>
    /// Checks the ordering of dependencies for the return value of <see cref="Spreadsheet.SetCellContents(string, Formula)"/>.
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsFormulaReturn_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("A1", "1.1");
        ss.SetContentsOfCell("B1", "f1");
        ss.SetContentsOfCell("C1", "f2");

        List<string> results = ss.SetContentsOfCell("A1", "D1*2").ToList();

        List<string> expected = new() { "A1", "B1", "C1" };
        CollectionAssert.AreEqual(expected, results);
    }

    /// <summary>
    /// Checks a <see cref="CircularException"/> is thrown when an indirect circular dependency is attempted
    /// to be put into the graph.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SpreadsheetConstructor_TestCircularDependencyIndirect_Invalid()
    {
        Spreadsheet ss = new();

        ss.SetContentsOfCell("A1", "B1*2");
        ss.SetContentsOfCell("B1", "C1*2");
        ss.SetContentsOfCell("C1", "A1*2");
    }

    /// <summary>
    /// Checks a <see cref="CircularException"/> is thrown when a direct circular dependency is attempted
    /// to be put into the graph.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SpreadsheetConstructor_TestCircularDependencyDirect_Invalid()
    {
        Spreadsheet ss = new();

        ss.SetContentsOfCell("A1", "A1*2");
    }

    /// <summary>
    /// This test checks  <see cref="Spreadsheet.SetCellContents(string, double)"/> will not work when an incorrect
    /// cell name is passed in.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SpreadsheetConstructor_TestSetCellContentsDouble_InvalidCellName()
    {
        Spreadsheet ss = new();
        string trueContents = "1.1";

        ss.SetContentsOfCell("1A1A", trueContents);
    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, string)"/> method works as expected
    /// by setting the contents of a cell when the contents is a string, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsString_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        string trueContents = "test";

        ss.SetContentsOfCell("A1", trueContents);

        Assert.AreEqual(trueContents, ss.GetCellContents("A1"));
    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, double)"/> method works as expected
    /// by setting the contents of a cell with a double, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsDouble_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        string trueContents = "1.1";

        ss.SetContentsOfCell("A1", trueContents);

        Assert.AreEqual(trueContents, ss.GetCellContents("A1"));
    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, Formula)"/> method works as expected
    /// by setting the contents of a cell with a <see cref="Formula"/>, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsFormula_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        string trueContents = "1+1";

        ss.SetContentsOfCell("A1", trueContents);

        Assert.AreEqual(trueContents, ss.GetCellContents("A1"));
    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, string)"/> method works as expected
    /// by setting the contents of a cell to an empty string, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsEmptyStringReturnsEmptyString_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        string trueContents = string.Empty;

        ss.SetContentsOfCell("A1", trueContents);

        Assert.AreEqual(trueContents, ss.GetCellContents("A1"));
    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, string)"/> method works as expected
    /// by setting the contents of a cell to an empty string, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsEmptyStringNotInGraph_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        string trueContents = string.Empty;

        ss.SetContentsOfCell("A1", trueContents);

        bool condition = ss.GetNamesOfAllNonemptyCells().Contains("A1"); // A1 should not be in this set, making the bool false.
        Assert.IsFalse(condition);
    }

    // ----------- TEST GetNamesOfAllNonEmptyCells -----------

    /// <summary>
    /// Checks that <see cref="Spreadsheet.GetNamesOfAllNonemptyCells()"/> works as expected
    /// when there ARE cells in the graph.
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestGetNamesOfAllNonemptyCellsCellsInGraph_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        string trueContents = "1.1";
        ss.SetContentsOfCell("A1", trueContents);
        ss.SetContentsOfCell("B1", trueContents);
        ss.SetContentsOfCell("C1", trueContents);

        HashSet<string> results = ss.GetNamesOfAllNonemptyCells().ToHashSet();
        HashSet<string> expected = new() { "A1", "B1", "C1" };
        Assert.IsTrue(expected.SetEquals(results));
    }

    /// <summary>
    /// Checks that <see cref="Spreadsheet.GetNamesOfAllNonemptyCells()"/> works as expected
    /// when cells are added and then removed from the graph.
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestGetNamesOfAllNonemptyCellsAddedAndRemoved_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        string trueContents = "1.1";
        ss.SetContentsOfCell("A1", trueContents);
        ss.SetContentsOfCell("B1", trueContents);
        ss.SetContentsOfCell("C1", trueContents);

        ss.SetContentsOfCell("A1", string.Empty);
        ss.SetContentsOfCell("B1", string.Empty);
        ss.SetContentsOfCell("C1", string.Empty);

        Assert.IsTrue(ss.GetNamesOfAllNonemptyCells().Count == 0);
    }

    // ----------- TEST GetCellContents function -----------

    /// <summary>
    /// This test checks <see cref="Spreadsheet.GetCellContents(string)"/> will
    /// return an empty string if the cell is not in the graph (the cell is an empty cell).
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestGetCellContentsNoContents_Valid()
    {
        Spreadsheet ss = new();
        Assert.AreEqual(string.Empty, ss.GetCellContents("A1"));
    }

    // ----------- TEST GetCellValue function -----------
}

// #FIXME I am pretty sure the utils class should be internal? And tested through the public methods... not sure though.

/// <summary>
/// This test class contains the tests for the <see cref="SpreadsheetUtils"/>
/// which is a helper class for <see cref="Spreadsheet"/> and <see cref="Cell"/>.
/// </summary>
[TestClass]
public class SpreadsheetUtilsTests
{
    /// <summary>
    /// Checks <see cref="SpreadsheetUtils.IsValidCellName(string)"/> is true when
    /// a valid cell name with mixed case is passed through.
    /// </summary>
    [TestMethod]
    public void SpreadsheetUtils_TestIsValidCellNameMixedCase_Valid()
    {
        Assert.IsTrue(SpreadsheetUtils.IsValidCellName("AaA1"));
    }

    /// <summary>
    /// Checks <see cref="SpreadsheetUtils.IsValidCellName(string)"/> is false when
    /// an invalid cell name that starts with a number is passed through.
    /// </summary>
    [TestMethod]
    public void SpreadsheetUtils_TestIsValidCellNameNumberFirst_Invalid()
    {
        Assert.IsFalse(SpreadsheetUtils.IsValidCellName("1A"));
    }

    /// <summary>
    /// Checks <see cref="SpreadsheetUtils.IsValidCellName(string)"/> is false when
    /// an invalid cell name that contains numbers and letters in a mixed order is
    /// passed through.
    /// </summary>
    [TestMethod]
    public void SpreadsheetUtils_TestIsValidCellNameNumbersLettersMixed_Invalid()
    {
        Assert.IsFalse(SpreadsheetUtils.IsValidCellName("A1A1"));
    }

    /// <summary>
    /// Checks <see cref="SpreadsheetUtils.IsValidCellName(string)"/> is false when
    /// an invalid cell name that contains an invalid character is passed through.
    /// </summary>
    [TestMethod]
    public void SpreadsheetUtils_TestIsValidCellNameInvalidCharacter_Invalid()
    {
        Assert.IsFalse(SpreadsheetUtils.IsValidCellName("A!"));
    }

    /// <summary>
    /// Checks <see cref="SpreadsheetUtils.IsValidCellName(string)"/> is false when
    /// an invalid cell name that is an empty string is passed through.
    /// </summary>
    [TestMethod]
    public void SpreadsheetUtils_TestIsValidCellNameEmptyString_Invalid()
    {
        Assert.IsFalse(SpreadsheetUtils.IsValidCellName(string.Empty));
    }

    /// <summary>
    /// Checks <see cref="SpreadsheetUtils.IsValidCellName(string)"/> is false when
    /// an invalid cell name that is whitespace is passed through.
    /// </summary>
    [TestMethod]
    public void SpreadsheetUtils_TestIsValidCellNameWhitespace_Invalid()
    {
        Assert.IsFalse(SpreadsheetUtils.IsValidCellName(" "));
    }

    /// <summary>
    /// Checks <see cref="SpreadsheetUtils.IsValidCellName(string)"/> is false when
    /// an invalid cell name that is an otherwise valid cell name containing whitespace
    /// in the middle is passed through.
    /// </summary>
    [TestMethod]
    public void SpreadsheetUtils_TestIsValidCellNameWhitespaceInMiddle_Invalid()
    {
        Assert.IsFalse(SpreadsheetUtils.IsValidCellName("A 1"));
    }

    /// <summary>
    /// Checks <see cref="SpreadsheetUtils.IsValidCellName(string)"/> is false when
    /// an invalid cell name that is an otherwise valid cell name containing whitespace
    /// at the front is passed through.
    /// </summary>
    [TestMethod]
    public void SpreadsheetUtils_TestIsValidCellNameWhitespaceAtStart_Invalid()
    {
        Assert.IsFalse(SpreadsheetUtils.IsValidCellName(" A1"));
    }

    /// <summary>
    /// Checks <see cref="SpreadsheetUtils.IsValidCellName(string)"/> is false when
    /// an invalid cell name that is an otherwise valid cell name containing whitespace
    /// at the end is passed through.
    /// </summary>
    [TestMethod]
    public void SpreadsheetUtils_TestIsValidCellNameWhitespaceAtEnd_Invalid()
    {
        Assert.IsFalse(SpreadsheetUtils.IsValidCellName("A1 "));
    }
}
