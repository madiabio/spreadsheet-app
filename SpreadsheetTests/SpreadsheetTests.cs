// <copyright file="SpreadsheetTests.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

// <summary>
// Author:    Madeline Abio
// Partner:   N/A
// Date:      20/09/2024
// Course:    CS 3500, University of Utah, School of Computing
// Copyright: CS 3500 and Madeline Abio - This work may not
//            be copied for use in Academic Coursework.
//
// I, Madeline Abio, certify that I wrote this code from scratch and
// did not copy it in part or whole from another source.  All
// references used in the completion of the assignments are cited
// in my README file.
//
// File Contents
//
//    This file contains the tests for the Spreadsheet, Cell and SpreadsheetUtils classes.
// </summary>
namespace CS3500.SpreadsheetTests;
using CS3500.Formula;
using CS3500.Spreadsheet;

/// <summary>
/// This test class contains the tests for the <see cref="Spreadsheet"/> class.
/// </summary>
[TestClass]
public class SpreadsheetTests
{
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

    /// <summary>
    /// Checks a <see cref="InvalidNameException"/> is thrown when an invalid cell name is passed
    /// into <see cref="Spreadsheet.SetCellContents(string, string)"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SpreadsheetConstructor_TestSetCellContentsString_InvalidName()
    {
        Spreadsheet ss = new();
        ss.SetCellContents("1A1A", "test");
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
        Formula f1 = new("B1+1");
        ss.SetCellContents("1A1A", f1);
    }

    /// <summary>
    /// Tests <see cref="Spreadsheet.SetCellContents(string, string)"/> when the cell being set had dependees before this
    /// method call.
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsStringWithDependees_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        ss.SetCellContents("A1", "B1*2"); // A is dependent on B, meaning A has a dependee (B)
        ss.SetCellContents("B1", "test");

        List<string> results = ss.SetCellContents("A1", "test").ToList(); // update A to no longer be dependent on B.
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
        ss.SetCellContents("A1", "B1*2"); // A is dependent on B, meaning A has a dependee (B)
        ss.SetCellContents("B1", "test");

        List<string> results = ss.SetCellContents("A1", 1.1).ToList(); // update A to no longer be dependent on B.
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
        List<string> results = ss.SetCellContents("A1", 1.1).ToList();

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
        List<string> results = ss.SetCellContents("A1", "test").ToList();

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
        ss.SetCellContents("A1", 1.1);
        Formula f1 = new("A1*2");
        Formula f2 = new("B1*2");
        ss.SetCellContents("B1", f1);
        ss.SetCellContents("C1", f2);

        List<string> results = ss.SetCellContents("A1", new Formula("D1*2")).ToList();

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

        ss.SetCellContents("A1", new Formula("B1*2"));
        ss.SetCellContents("B1", new Formula("C1*2"));
        ss.SetCellContents("C1", new Formula("A1*2"));
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

        ss.SetCellContents("A1", new Formula("A1*2"));
    }

    /// <summary>
    /// Checks that <see cref="Spreadsheet.GetNamesOfAllNonemptyCells()"/> works as expected
    /// when there ARE cells in the graph.
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestGetNamesOfAllNonemptyCellsCellsInGraph_ExpectedBehaviour()
    {
        Spreadsheet ss = new();
        double trueContents = 1.1;
        ss.SetCellContents("A1", trueContents);
        ss.SetCellContents("B1", trueContents);
        ss.SetCellContents("C1", trueContents);

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
        double trueContents = 1.1;
        ss.SetCellContents("A1", trueContents);
        ss.SetCellContents("B1", trueContents);
        ss.SetCellContents("C1", trueContents);

        ss.SetCellContents("A1", string.Empty);
        ss.SetCellContents("B1", string.Empty);
        ss.SetCellContents("C1", string.Empty);

        Assert.IsTrue(ss.GetNamesOfAllNonemptyCells().Count == 0);
    }

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

    /// <summary>
    /// This test checks  <see cref="Spreadsheet.SetCellContents(string, double)"/> will not work when an incorrect
    /// cell name is passed in.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SpreadsheetConstructor_TestSetCellContentsDouble_InvalidCellName()
    {
        Spreadsheet ss = new();
        double trueContents = 1.1;

        ss.SetCellContents("1A1A", trueContents);
    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, string)"/> method works as expected
    /// by setting the contents of a cell when the contents is a string, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsString_ExpectedBehvaiour()
    {
        Spreadsheet ss = new();
        string trueContents = "test";

        ss.SetCellContents("A1", trueContents);

        Assert.AreEqual(trueContents, ss.GetCellContents("A1"));
    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, double)"/> method works as expected
    /// by setting the contents of a cell with a double, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsDouble_ExpectedBehvaiour()
    {
        Spreadsheet ss = new();
        double trueContents = 1.1;

        ss.SetCellContents("A1", trueContents);

        Assert.AreEqual(trueContents, ss.GetCellContents("A1"));
    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, Formula)"/> method works as expected
    /// by setting the contents of a cell with a <see cref="Formula"/>, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsFormula_ExpectedBehvaiour()
    {
        Spreadsheet ss = new();
        Formula trueContents = new("1+1");

        ss.SetCellContents("A1", trueContents);

        Assert.AreEqual(trueContents, ss.GetCellContents("A1"));
    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, string)"/> method works as expected
    /// by setting the contents of a cell to an empty string, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsEmptyStringReturnsEmptyString_ExpectedBehvaiour()
    {
        Spreadsheet ss = new();
        string trueContents = string.Empty;

        ss.SetCellContents("A1", trueContents);

        Assert.AreEqual(trueContents, ss.GetCellContents("A1"));
    }

    /// <summary>
    /// <para>
    /// This test checks the <see cref="Spreadsheet.SetCellContents(string, string)"/> method works as expected
    /// by setting the contents of a cell to an empty string, then checking the contents of that cell.
    /// </para>
    /// </summary>
    [TestMethod]
    public void SpreadsheetConstructor_TestSetCellContentsEmptyStringNotInGraph_ExpectedBehvaiour()
    {
        Spreadsheet ss = new();
        string trueContents = string.Empty;

        ss.SetCellContents("A1", trueContents);

        bool condition = ss.GetNamesOfAllNonemptyCells().Contains("A1"); // A1 should not be in this set, making the bool false.
        Assert.IsFalse(condition);
    }
}

/// <summary>
/// This test class contains the tests for the <see cref="Cell"/> class.
/// </summary>
[TestClass]
public class CellTests
{
    /// <summary>
    /// Checks that the get function for <see cref="Cell.Name"/> returns the correct value.
    /// </summary>
    [TestMethod]
    public void CellConstructor_CellNameGet_ExpectedBehaviour()
    {
        Cell cell = new();

        // set the name and retrieve it using the get accessor
        cell.Name = "a1";  // set the name (it will be converted to uppercase in the set accessor)
        string actualName = cell.Name; // get the name

        // check that the name has been set and retrieved correctly in uppercase
        string expectedName = "A1"; // since the name is normalised to uppercase
        Assert.AreEqual(expectedName, actualName);
    }

    /// <summary>
    /// This checks that <see cref="Cell.Name"/> will not allow null or whitespace contents
    /// to be set by ensuring it throws a <see cref="InvalidContentsException"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void CellConstructor_CellNameWhitespace_Invalid()
    {
        Cell cell = new();
        cell.Name = " ";
    }

    /// <summary>
    /// This checks that <see cref="Cell.Contents"/> will not allow null or whitespace contents
    /// to be set by ensuring it throws a <see cref="InvalidContentsException"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidContentsException))]
    public void CellConstructor_CellContentsWhitespace_Invalid()
    {
        Cell cell = new();
        cell.Contents = " ";
    }

    /// <summary>
    /// This test checks get works correctly for the value portion of a cell.
    /// </summary>
    [TestMethod]
    public void CellValue_Get_ReturnsCorrectValue()
    {
        Cell cell = new();

        // set the cell contents, which should update the _value field
        cell.Contents = 1.1;

        // retrieve the value using the get accessor
        object trueValue = cell.Value;

        // check if the value is set correctly
        object expected = 1.1;  //  expected value should match the input to Contents
        Assert.AreEqual(expected, trueValue);
    }

    /// <summary>
    /// This test checks <see cref="Cell.Contents"/> will not allow invalid contents to be set
    /// by ensuring it throws a <see cref="InvalidContentsException"/> when a HashSet is attempted
    /// to be used as the contents.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidContentsException))]
    public void CellConstructor_TestContents_Invalid()
    {
        Cell cell = new();
        HashSet<string> trueContents = new() { "a", "b", "c" };

        cell.Contents = trueContents;
    }

    /// <summary>
    /// This test checks <see cref="Cell.Contents"/> will allow valid contents to be set
    /// when the contents are a string.
    /// </summary>
    [TestMethod]
    public void CellConstructor_TestContentsString_Valid()
    {
        Cell cell = new();
        string trueContents = "a";

        cell.Contents = trueContents;
    }

    /// <summary>
    /// This test checks <see cref="Cell"/> will allow an valid name to be set
    /// when the name uses lowercase characters.
    /// </summary>
    [TestMethod]
    public void CellConstructor_TestNameLowercase_Valid()
    {
        Cell cell = new();
        cell.Name = "abc1";
    }

    /// <summary>
    /// This test checks <see cref="Cell"/> will allow an valid name to be set
    /// when the name uses uppercase characters.
    /// </summary>
    [TestMethod]
    public void CellConstructor_TestNameUppercase_Valid()
    {
        Cell cell = new();
        cell.Name = "ABC1";
    }

    /// <summary>
    /// This test checks <see cref="Cell"/> will not allow an invalid name to be set
    /// by ensuring it throws a <see cref="InvalidNameException"/> when a number
    /// is the prefix of the name.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void CellConstructor_TestNameNumberFirst_Invalid()
    {
        Cell cell = new();
        cell.Name = "1ABC";
    }

    /// <summary>
    /// This test checks <see cref="Cell"/> will not allow an invalid name to be set
    /// by ensuring it throws a <see cref="InvalidNameException"/> when a number is
    /// in between some characters in the name.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void CellConstructor_TestNameNumberInMiddle_Invalid()
    {
        Cell cell = new();
        cell.Name = "A1BC";
    }
}

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
