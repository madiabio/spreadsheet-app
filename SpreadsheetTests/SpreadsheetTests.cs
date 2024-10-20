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
///     Authors/Partnership:    Madeline Abio and Sadie Bowen
///     Date:      20/09/2024
///     Course:    CS 3500, University of Utah, School of Computing
///     Copyright: CS 3500 and Madeline Abio - This work may not
///            be copied for use in Academic Coursework.
/// </para>
///
/// <para>
///     We, Madeline Abio and Sadie Bowen, certify that I wrote this code from scratch and
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
    // ----------- TEST Spreadsheet Empty Constructor-----------

    /// <summary>
    ///     Checks that when a spreadsheet is initialized with empty constructor,
    ///     the object is saved under the name "default".
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefaultConstructor_TestNameIsDefault_Valid()
    {
        // TODO need to update this test to actually use a filename.
        Spreadsheet spreadsheet = new();
        spreadsheet.Save("default");
        spreadsheet.Load("default");
    }

    // ----------- TEST Spreadsheet named Constructor-----------

    /// <summary>
    ///     Checks that when a spreadsheet is initialized with the named constructor,
    ///     the object is saved under the name given to the constructor.
    /// </summary>
    [TestMethod]
    public void SpreadsheetNamedConstructor_TestSpreadsheetHasName_Valid()
    {
        // TODO need to update this test to actually use a filename.
        Spreadsheet spreadsheet = new("namedSheet");
        spreadsheet.Save("namedSheet");
    }

    // ----------- TEST this[string cellName] accessor -----------

    /// <summary>
    ///     Ensures the updated accessor [] can properly return the value of a cell with a formula.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestAccessorFormulaValue_AreEqual()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet.SetContentsOfCell( "A1", "=5+5" );
        Assert.AreEqual(10.0, spreadsheet["A1"]);
    }

    /// <summary>
    /// Ensures the updated accessor [] can properly return the value of a cell with a formula.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestAccessorFormulaValue_AreNotEqual()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet.SetContentsOfCell( "A1", "=5+5" );
        Assert.AreNotEqual(11, spreadsheet["A1"]);
    }

    /// <summary>
    ///     Ensures the updated accessor [] can properly return the value of a cell with a double.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestAccessorDoubleValue_AreEqual()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet.SetContentsOfCell( "A1", "10.0" );
        Assert.AreEqual(10.0, spreadsheet["A1"]);
    }

    /// <summary>
    ///     Ensures the updated accessor [] can properly return the value of a cell with a double.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestAccessorDoubleValue_AreNotEqual()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet.SetContentsOfCell( "A1", "4.2" );
        Assert.AreNotEqual(6.2, spreadsheet["A1"]);
    }

    /// <summary>
    ///     Ensures the updated accessor [] can properly return the value of a cell with a string.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestAccessorTextValue_AreEqual()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet.SetContentsOfCell( "A1", "5+5" );
        Assert.AreEqual("5+5", spreadsheet["A1"]);
    }

    /// <summary>
    ///     Ensures the updated accessor [] can properly return the value of a cell with a string.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestAccessorTextValue_AreNotEqual()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet.SetContentsOfCell( "A1", "5+5" );
        Assert.AreNotEqual("10", spreadsheet["A1"]);
    }

    // ----------- TEST Save function -----------
    // #FIXME need to determine more tests for save function.

    /// <summary>
    ///     Ensures a spreadsheet can save to a file name that is valid.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestSaveFunction_FilenameIsValid()
    {
        Spreadsheet spreadsheet = new();
        const string filename = "file.txt";
        spreadsheet.Save(filename);
    }

    /// <summary>
    ///     Ensures a <see cref="SpreadsheetReadWriteException"/> is thrown when an
    ///     spreadsheet is saved to an invalid file location.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void SpreadsheetDefault_TestSaveFunction_FilenameIsInvalid()
    {
        Spreadsheet spreadsheet = new();
        const string filename = "This file doesn't exist";
        spreadsheet.Save(filename);
    }


    /// <summary>
    ///     Ensures a <see cref="SpreadsheetReadWriteException"/> is thrown when an
    ///     spreadsheet is saved to an invalid file location.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void SpreadsheetDefault_TestSaveFunction_InvalidPath1()
    {
        Spreadsheet spreadsheet = new();
        const string filename = "C:\\Users\\Madi\\Desktop\\file.txt";
        spreadsheet.Save(filename);
    }


    /// <summary>
    ///     Ensures a <see cref="SpreadsheetReadWriteException"/> is thrown when an
    ///     spreadsheet is saved to an invalid file location.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void SpreadsheetDefault_TestSaveFunction_SpreadsheetHasContents_InvalidPath1()
    {
        Spreadsheet spreadsheet = new();
        spreadsheet.SetContentsOfCell("A1", "=1+1");
        spreadsheet.SetContentsOfCell("B1", "=A1+1");
        spreadsheet.SetContentsOfCell("C1", "1");
        const string filename = "C:\\Users\\Madi\\Desktop\\file.txt";
        spreadsheet.Save(filename);
    }

    /// <summary>
    ///     Ensures a <see cref="SpreadsheetReadWriteException"/> is thrown when an
    ///     spreadsheet is saved to an invalid file location.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void SpreadsheetDefault_TestSaveFunction_InvalidPath2()
    {
        Spreadsheet spreadsheet = new();
        const string filename = "some\\invalid\\path\\file.txt";
        spreadsheet.Save(filename);
    }

    /// <summary>
    ///     Ensures a <see cref="SpreadsheetReadWriteException"/> is thrown when an
    ///     spreadsheet is saved to an invalid file location.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void SpreadsheetDefault_TestSaveFunction_InvalidPath3()
    {
        Spreadsheet spreadsheet = new();
        const string filename = "some/invalid/path/file.txt";
        spreadsheet.Save(filename);
    }

    /// <summary>
    ///     Ensures a <see cref="SpreadsheetReadWriteException"/> is thrown when an
    ///     spreadsheet is saved to an invalid file location.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void SpreadsheetDefault_TestSaveFunction_InvalidPath4()
    {
        Spreadsheet spreadsheet = new();
        const string filename = ".";
        spreadsheet.Save(filename);
    }


    // TODO add the following tests:
    // If any of the cell names contained in the saved spreadsheet are invalid
    // If any invalid formulas or circular dependencies are encountered
    // If there are any problems opening, reading, or closing the file
    // There are no doubt other things that can go wrong
    // if anything goes wrong, check to make sure original spreadsheet is unchanged.

    // ----------- TEST Load function -----------

    // #FIXME need to determine more tests for load function.

    /// <summary>
    ///     Ensures a spreadsheet can save to a file name that is valid.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestLoadFunction_FileNameIsValid()
    {
        Spreadsheet spreadsheet = new();
        const string filename = "file.txt";
        spreadsheet.Load(filename);
    }

    /// <summary>
    ///     Ensures a <see cref="SpreadsheetReadWriteException"/> is thrown when an
    ///     spreadsheet is saved to an invalid file location.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void SpreadsheetDefault_TestLoadFunction_FileNameIsInValid()
    {
        Spreadsheet spreadsheet = new();
        const string filename = "This file doesn't exist";
        spreadsheet.Load(filename);
    }

    // TODO add the following tests:
    // If any of the cell names contained in the saved spreadsheet are invalid
    // If any invalid formulas or circular dependencies are encountered
    // If there are any problems opening, reading, or closing the file
    // There are no doubt other things that can go wrong
    // if anything goes wrong, check to make sure original spreadsheet is unchanged.

    // ----------- TEST GetCellValue -----------

    /// <summary>
    ///     Ensures GetCellValue returns a double value for a cell that contains a formula.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestGetCellValueFormula_Expected()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("A1", "=1+1");
        Assert.AreEqual(2.0, ss.GetCellValue("A1"));
    }

    /// <summary>
    ///     Ensures GetCellValue returns a double value for a cell that contains a double.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestGetCellValueDouble_Expected()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("A1", "5.4");
        Assert.AreEqual(5.4, ss.GetCellValue("A1"));
    }

    /// <summary>
    ///     Ensures GetCellValue returns a string value for a cell that contains a string.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestGetCellValueString_Expected()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("A1", "text");
        Assert.AreEqual("text", ss.GetCellValue("A1"));
    }

    /// <summary>
    ///     Ensures a <see cref="InvalidNameException"/> is thrown when an invalid cell name is
    ///     passed to GetCellValue().
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SpreadsheetDefault_TestGetCellValueInvalidName_InvalidNameException()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("A1", "text");
        ss.GetCellValue("A1B");
    }

    // ----------- TEST SetContentsOfCell function -----------

    /// <summary>
    ///     Checks a <see cref="InvalidNameException"/> is thrown when an invalid cell name is passed
    ///     into <see cref="Spreadsheet.SetContentsOfCell(string, string)"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SpreadsheetDefault_TestSetContentsOfCell_InvalidName()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("1A1A", "test");
    }

    /// <summary>
    ///     Checks a <see cref="InvalidNameException"/> is thrown when an invalid cell name is passed
    ///     into <see cref="Spreadsheet.SetContentsOfCell(string, string)"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void SpreadsheetDefault_TestSetContentsOfCellFormula_InvalidFormat()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("A1", "=M1B + 4");
    }

    /// <summary>
    /// <para>
    ///     This test checks the <see cref="Spreadsheet.SetContentsOfCell"/> method works as expected
    ///     by setting the contents of a cell when the contents is a string, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_ChangeIsTrackedInChangeProperty_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("A1", "string");

        Assert.IsTrue(ss.Changed);
    }

    /// <summary>
    /// <para>
    ///     This test checks the <see cref="Spreadsheet.SetContentsOfCell"/> method works as expected
    ///     by setting the contents of a cell when the contents is a string, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestSetContentsOfCellString_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("A1", "string");

        Assert.AreEqual("string", ss.GetCellContents("A1"));
    }

    /// <summary>
    /// <para>
    ///     This test checks the <see cref="Spreadsheet.SetContentsOfCell"/> method works as expected
    ///     by setting the contents of a cell with a double, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestSetContentsOfCellDouble_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("A1", "1.1");

        Assert.AreEqual(1.1, ss.GetCellContents("A1"));
    }

    /// <summary>
    /// <para>
    ///     This test checks the <see cref="Spreadsheet.SetContentsOfCell"/> method works as expected
    ///     by setting the contents of a cell with a <see cref="Formula"/>, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestSetContentsOfCellFormula_ExpectedBehaviour()
    {
        Spreadsheet ss = new();

        ss.SetContentsOfCell("A1", "=1+1");
        Formula formula = new("1+1");

        Assert.AreEqual(formula, ss.GetCellContents("A1"));
    }

    /// <summary>
    ///     Ensure <see cref="Spreadsheet.SetContentsOfCell"/> returns the correct list of cells that
    ///     were updated when the cell was set.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestContentsOfCellReturnedList_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("B1", "=A1*2"); // A is dependent on B, meaning A has a dependee (B)
        ss.SetContentsOfCell("C1", "=B1+A1");

        List<string> results = ss.SetContentsOfCell("A1", "=4+2").ToList(); // update A to no longer be dependent on B.
        List<string> expected = new() { "A1", "B1", "C1" };

        CollectionAssert.AreEqual(expected, results);
    }

    /// <summary>
    ///     Tests <see cref="Spreadsheet.SetContentsOfCell"/> when the cell being set had dependees before this
    ///     method call.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestSetContentsOfCellDoubleWithDependees_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell("A1", "=B1*2"); // A is dependent on B, meaning A has a dependee (B)
        ss.SetContentsOfCell("B1", "test");

        List<string> results = ss.SetContentsOfCell("B1", "1.1").ToList(); // update A to no longer be dependent on B.
        List<string> expected = new() { "B1", "A1" };

        CollectionAssert.AreEqual(expected, results);
    }

    /// <summary>
    ///     Checks a <see cref="CircularException"/> is thrown when an indirect circular dependency is attempted
    ///     to be put into the graph.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SpreadsheetDefault_TestCircularDependencyIndirect_Invalid()
    {
        Spreadsheet ss = new();

        ss.SetContentsOfCell("A1", "=B1*2");
        ss.SetContentsOfCell("B1", "=C1*2");
        ss.SetContentsOfCell("C1", "=A1*2");
    }

    /// <summary>
    ///     Checks a <see cref="CircularException"/> is thrown when a direct circular dependency is attempted
    ///     to be put into the graph.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SpreadsheetDefault_TestCircularDependencyDirect_Invalid()
    {
        Spreadsheet ss = new();

        ss.SetContentsOfCell("A1", "=A1*2");
    }

    /// <summary>
    /// <para>
    ///     This test checks the <see cref="Spreadsheet.SetContentsOfCell"/> method works as expected
    ///     by setting the contents of a cell to an empty string, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestSetSetContentsOfCellEmptyStringReturnsEmptyString_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        string trueContents = string.Empty;

        ss.SetContentsOfCell("A1", trueContents);

        Assert.AreEqual(trueContents, ss.GetCellContents("A1"));
    }

    /// <summary>
    /// <para>
    ///     This test checks the <see cref="Spreadsheet.SetContentsOfCell"/> method works as expected
    ///     by setting the contents of a cell to an empty string, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestSetSetContentsOfCellEmptyStringNotInGraph_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        string trueContents = string.Empty;

        ss.SetContentsOfCell("A1", trueContents);

        bool condition = ss.GetNamesOfAllNonemptyCells().Contains("A1"); // A1 should not be in this set, making the bool false.
        Assert.IsFalse(condition);
    }

    // ----------- TEST GetNamesOfAllNonEmptyCells -----------

    /// <summary>
    ///     Checks that <see cref="Spreadsheet.GetNamesOfAllNonemptyCells()"/> works as expected
    ///     when there ARE cells in the graph.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestGetNamesOfAllNonemptyCellsCellsInGraph_ExpectedBehaviour()
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
    ///     Checks that <see cref="Spreadsheet.GetNamesOfAllNonemptyCells()"/> works as expected
    ///     when cells are added and then removed from the graph.
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestGetNamesOfAllNonemptyCellsAddedAndRemoved_ExpectedBehaviour()
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

    // ----------- TEST GetCellContents function-----------

    /// <summary>
    ///     Checks a <see cref="InvalidNameException"/> is thrown when an invalid cell name is passed
    ///     into <see cref="Spreadsheet.GetCellContents(string)"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SpreadsheetDefault_TestGetCellContents_InvalidName()
    {
        Spreadsheet ss = new();
        ss.GetCellContents("1A1A");
    }

    /// <summary>
    ///     This test checks <see cref="Spreadsheet.GetCellContents(string)"/> will
    ///     return an empty string if the cell is not in the graph (the cell is an empty cell).
    /// </summary>
    [TestMethod]
    public void SpreadsheetDefault_TestGetCellContentsNoContents_Valid()
    {
        Spreadsheet ss = new();
        Assert.AreEqual(string.Empty, ss.GetCellContents("A1"));
    }
}

// #FIXME I am pretty sure the utils and cell class should be internal? And tested through the public methods... not sure though.
// I removed the tests for those classes but if you think differently, we can just add them back in by copying them from a previous commit.