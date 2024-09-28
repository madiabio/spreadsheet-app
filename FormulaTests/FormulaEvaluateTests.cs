// <copyright file="FormulaEvaluateTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

/// <summary>
/// Author:    Madeline Abio
/// Partner:   N/A
/// Date:      22/09/2024
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
///    This file contains tests for the methods in <see cref="Formula"/>
///    that relate to <see cref="Formula.Evaluate()"/>.
/// </summary>
namespace FormulaTests;
using CS3500.Formula;

/// <summary>
/// Tests for the <see cref="Formula"/> class that relate to
/// its <see cref="Formula.Evaluate()"/> method.
/// </summary>
[TestClass]
public class FormulaEvaluateTests
{
    /// <summary>
    /// Tests if <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a formula
    /// when the formula contains multiple operators and parenthesis, without variables.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestComplexExpression1NoVariables_ExpectedBehaviour()
    {
        Formula f1 = new("(2+3)*5+2");
        Assert.AreEqual(27.0, f1.Evaluate(s => 0));
    }

    /// <summary>
    /// Tests if <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a formula
    /// when the formula contains multiple operators and parenthesis, with variables.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestComplexExpression1WithVariables_ExpectedBehaviour()
    {
        Formula f1 = new("(A1+B1)*(A1+B1)+A1");
        Assert.AreEqual(27.0, f1.Evaluate(MyVariables));
    }

    /// <summary>
    /// Tests if <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a formula
    /// when the formula contains multiple addition operators and parenthesis, with variables.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestMultipleAdditionWithVariables_ExpectedBehaviour()
    {
        Formula f1 = new("(A1+B1+A1+B1)");
        Assert.AreEqual(10.0, f1.Evaluate(MyVariables));
    }

    /// <summary>
    /// Tests if <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a formula
    /// when the formula contains multiple addition and parenthesis, without variables.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestMultipleAdditionNoVariables_ExpectedBehaviour()
    {
        Formula f1 = new("(2+3+2+3)");
        Assert.AreEqual(10.0, f1.Evaluate(s => 0));
    }

    /// <summary>
    /// Tests if <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a formula
    /// when the formula contains multiple subtraction and parenthesis, without variables.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestMultipleSubtractionNoVariables_ExpectedBehaviour()
    {
        Formula f1 = new("(2-3-2)");
        Assert.AreEqual(-3.0, f1.Evaluate(s => 0));
    }

    /// <summary>
    /// Tests if <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a formula
    /// when the formula contains multiple subtraction operators and parenthesis, with variables.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestMultipleSubtractionWithVariables_ExpectedBehaviour()
    {
        Formula f1 = new("(A1-B1-A1)");
        Assert.AreEqual(-3.0, f1.Evaluate(MyVariables));
    }

    /// <summary>
    /// Tests if <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a formula
    /// when the formula contains multiple multiplication and parenthesis, without variables.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestMultipleMultiplicationNoVariables_ExpectedBehaviour()
    {
        Formula f1 = new("(2*3*2)");
        Assert.AreEqual(12.0, f1.Evaluate(s => 0));
    }

    /// <summary>
    /// Tests if <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a formula
    /// when the formula contains multiple multiplication operators and parenthesis, with variables.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestMultipleMultiplicationWithVariables_ExpectedBehaviour()
    {
        Formula f1 = new("(A1*B1*A1)");
        Assert.AreEqual(12.0, f1.Evaluate(MyVariables));
    }

    /// <summary>
    /// Tests if <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a formula
    /// when the formula contains multiple division and parenthesis, without variables.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestMultipleDivisionNoVariables_ExpectedBehaviour()
    {
        Formula f1 = new("(100/10/10)");
        Assert.AreEqual(1.0, f1.Evaluate(s => 0));
    }

    /// <summary>
    /// Tests if <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a formula
    /// when the formula contains multiple division operators and parenthesis, with variables.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestMultipleDivisionWithVariables_ExpectedBehaviour()
    {
        double TempMyVars(string name) // made a new function so i could have 100 and 10 as A1 and B1 for division
        {
            if (name == "A1")
            {
                return 100;
            }
            else if (name == "B1")
            {
                return 10;
            }
            else
            {
                throw new ArgumentException("Unknown variable");
            }
        }

        Formula f1 = new("(A1/B1/B1)");
        Assert.AreEqual(1.0, f1.Evaluate(TempMyVars));
    }

    /// <summary>
    /// Tests if <see cref="Formula.Evaluate(Lookup)"/> correctly returns a <see cref="FormulaError"/>
    /// when the formula contains multiple division with a div by zero after a right parenthesis, without variables.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestMultipleDivisionDivByZeroErrorNoVariables_ExpectedBehaviour()
    {
        Formula f1 = new("(100/10)/0");
        Assert.IsInstanceOfType(f1.Evaluate(s => 0), typeof(FormulaError));
    }

    /// <summary>
    /// Tests if <see cref="Formula.Evaluate(Lookup)"/> correctly returns a <see cref="FormulaError"/>
    /// when the formula contains multiple division with a div by zero after a right parenthesis, without variables.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestMultipleDivisionDivByZeroErrorWithoVariables_ExpectedBehaviour()
    {
        Formula f1 = new("(A1/B1)/0");
        Assert.IsInstanceOfType(f1.Evaluate(s => 0), typeof(FormulaError));
    }

    /// <summary>
    /// Checks that <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a
    /// <see cref="Formula"/> containing addiition when no variables are used.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateAddNoVariables_ExpectedBehaviour()
    {
        Formula f = new("2+3");
        Assert.AreEqual(5.0, f.Evaluate(s => 0));
    }

    /// <summary>
    /// Tests <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a
    /// <see cref="Formula"/> containing addition when one known variable is used.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateAddKnownVariable_ExpectedBehaviour()
    {
        Formula f = new("A1 + 3");
        object ans = f.Evaluate(MyVariables);
        Assert.AreEqual(5.0, ans);
    }

    /// <summary>
    /// Tests <see cref="Formula.Evaluate(Lookup)"/> returns a correctly evaluated
    /// <see cref="Formula"/> containing addition when two known variables are used.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateAddKnownVariables_ExpectedBehaviour()
    {
        Formula f = new("A1 + B1");
        Assert.AreEqual(5.0, f.Evaluate(MyVariables));
    }

    /// <summary>
    /// Tests <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a
    /// <see cref="Formula"/> containing subtraction when no variables are used.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateSubNoVariables_ExpectedBehaviour()
    {
        Formula f = new("3-2");
        Assert.AreEqual(1.0, f.Evaluate(s => 0));
    }

    /// <summary>
    /// Tests <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a
    /// <see cref="Formula"/> containing subtraction when one known variable is used.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateSubKnownVariable_ExpectedBehaviour()
    {
        Formula f = new("B1-2");
        Assert.AreEqual(1.0, f.Evaluate(MyVariables));
    }

    /// <summary>
    /// Tests <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a
    /// <see cref="Formula"/> containing division when no variables are used.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateDivNoVariables_ExpectedBehaviour()
    {
        Formula f = new("2/2");
        Assert.AreEqual(1.0, f.Evaluate(s => 0));
    }

    /// <summary>
    /// Tests <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a
    /// <see cref="Formula"/> containing subtraction when one known variable is used.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateDivKnownVariable_ExpectedBehaviour()
    {
        Formula f = new("A1/2");
        Assert.AreEqual(1.0, f.Evaluate(MyVariables));
    }

    /// <summary>
    /// Tests <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a
    /// <see cref="Formula"/> containing multiplication when no variables are used.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateMultiplyNoVariables_ExpectedBehaviour()
    {
        Formula f = new("2*3");
        Assert.AreEqual(6.0, f.Evaluate(s => 0));
    }

    /// <summary>
    /// Tests <see cref="Formula.Evaluate(Lookup)"/> correctly evaluates a
    /// <see cref="Formula"/> containing multiplication when one known variable is used.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateMultiplyKnownVariable_ExpectedBehaviour()
    {
        Formula f = new("A1*B1");
        Assert.AreEqual(6.0, f.Evaluate(MyVariables));
    }

    /// <summary>
    /// <para>
    /// Checks that  a <see cref="FormulaError"/> is returned when two unknown variables
    /// are used in a <see cref="Formula"/> that is evaluated with <see cref="Formula.Evaluate(Lookup)"/>.
    /// </para>
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateTwoUnknownVariables_ExpectedBehaviour()
    {
        Formula f1 = new("A2 + B2");
        Assert.IsInstanceOfType(f1.Evaluate(MyVariables), typeof(FormulaError));
    }

    /// <summary>
    /// Checks that  a <see cref="FormulaError"/> is returned when an unknown variable
    /// is used with a known variable in a <see cref="Formula"/> that is evaluated with
    /// <see cref="Formula.Evaluate(Lookup)"/>.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateUnknownVariableWithKnownVariable_ExpectedBehaviour()
    {
        Formula f1 = new("A1 + B2");
        Assert.IsInstanceOfType(f1.Evaluate(MyVariables), typeof(FormulaError));
    }

    /// <summary>
    /// Checks that a <see cref="FormulaError"/> is returned when an unknown variable
    /// is used a number in a <see cref="Formula"/> that is evaluated with
    /// <see cref="Formula.Evaluate(Lookup)"/>.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateUnknownVariableWithNumber_ExpectedBehaviour()
    {
        Formula f1 = new("A2 + 3");
        object result = f1.Evaluate(MyVariables);
        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    /// <summary>
    /// <para>
    /// Tests <see cref="Formula.Evaluate(Lookup)"/> returns a
    /// <see cref="FormulaError"/> when an explicit division by zero occurs
    /// explicitly (no variables).
    /// </para>
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateDivByZeroExplicit_ExpectedBehaviour()
    {
        Formula f1 = new("1/0");
        Assert.IsInstanceOfType(f1.Evaluate(s => 0), typeof(FormulaError));
    }

    /// <summary>
    /// <para>
    /// Tests <see cref="Formula.Evaluate(Lookup)"/> returns a
    /// <see cref="FormulaError"/> when an explicit division by zero occurs
    /// explicitly (no variables), with parenthesis.
    /// </para>
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateDivByZeroExplicitWithParenthesis_ExpectedBehaviour()
    {
        Formula f1 = new("(1/(2-2))");
        Assert.IsInstanceOfType(f1.Evaluate(s => 0), typeof(FormulaError));
    }

    /// <summary>
    /// <para>
    /// Tests <see cref="Formula.Evaluate(Lookup)"/> returns a
    /// <see cref="FormulaError"/> when a division occurs on a subtraction
    /// operation with parenthesis.
    /// </para>
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateDivBySubtractionExpressionWithParenthesis_ExpectedBehaviour()
    {
        Formula f1 = new("(4/(6-2))");
        Assert.AreEqual(1.0, f1.Evaluate(s => 0));
    }

    /// <summary>
    /// <para>
    /// Tests <see cref="Formula.Evaluate(Lookup)"/> returns a
    /// <see cref="FormulaError"/> when an implicit division by zero occurs
    /// explicitly (has a variables).
    /// </para>
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEvaluateDivByZeroImplicit_ExpectedBehaviour()
    {
        Formula f1 = new("A1/0");
        Assert.IsInstanceOfType(f1.Evaluate(MyVariables), typeof(FormulaError));
    }

    /// <summary>
    /// <para>
    /// Checks that <see cref="Formula.Equals(object?)"/> works as expected
    /// by comparing two equivalent formulas with no variables.
    /// </para>
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEqualsNoVariables_ExpectedBehaviour()
    {
        Formula f1 = new("2 + 3");
        Formula f2 = new("2 + 3");
        Assert.IsTrue(f1.Equals(f2));

        Formula f3 = new("1+2");
        Assert.IsFalse(f1.Equals(f3));
    }

    /// <summary>
    /// Checks <see cref="Formula.Equals(object?)"/> works as expected
    /// when comparing two equivalent formulas with variables, when those
    /// formulas are equal.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEqualsWithVariables_ExpectedBehaviour()
    {
        Formula f1 = new("A1+A2");
        Formula f2 = new("A1+A2");

        Assert.IsTrue(f1.Equals(f2));

        Formula f3 = new("A1+3");
        Assert.IsFalse(f1.Equals(f3));
    }

    /// <summary>
    /// Checks that <see cref="bool Formula.operator ==(Formula f1, Formula f2)"/> works
    /// as expected when comparing two formulas with no varibles.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestOperatorEqualsNoVariables_ExpectedBehaviour()
    {
        Formula f1 = new("2 + 3");
        Formula f2 = new("2+3");
        Assert.IsTrue(f1 == f2);
    }

    /// <summary>
    /// Checks that <see cref="bool Formula.operator ==(Formula f1, Formula f2)"/> works
    /// as expected when comparing two formulas with varibles.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestOperatorEqualsWithVariables_ExpectedBehaviour()
    {
        Formula f1 = new("A1 + A2");
        Formula f2 = new("A1 + A2");
        Assert.IsTrue(f1 == f2);

        Formula f3 = new("1 + 2");
        Assert.IsFalse(f1 == f3);
    }

    /// <summary>
    /// Checks that <see cref="bool Formula.operator =!(Formula f1, Formula f2)"/> works
    /// as expected when comparing two formulas with no varibles.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestOperatorNEqualsNoVariables_ExpectedBehaviour()
    {
        Formula f1 = new("1 + 2");
        Formula f2 = new("2 + 3");
        Assert.IsTrue(f1 != f2);

        Formula f3 = new("1 + 2");
        Assert.IsFalse(f1 != f3);
    }

    /// <summary>
    /// Checks that <see cref="bool Formula.operator =!(Formula f1, Formula f2)"/> works
    /// as expected when comparing two formulas with varibles.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestOperatorNEqualsWithVariables_ExpectedBehaviour()
    {
        Formula f1 = new("A1 + A2");
        Formula f2 = new("A2 + A3");
        Assert.IsTrue(f1 != f2);

        Formula f3 = new("A1 + A2");
        Assert.IsFalse(f1 != f3);
    }

    /// <summary>
    /// Checks <see cref="Formula.Equals(object?)"/> works as
    /// expected when a null object is passed through.
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestEqualsNull_ExpectedBehaviour()
    {
        Formula f1 = new("1 + 2");
        Assert.IsFalse(f1.Equals(null));
    }

    /// <summary>
    /// Checks <see cref="Formula.GetHashCode()"/> override works
    /// when two formulas are equal (they should have the same
    /// hash code).
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestGetHashCodeEquivalent_ExpectedBehaviour()
    {
        Formula f1 = new("1+2");
        Formula f2 = new("1+2");
        Assert.IsTrue(f1 == f2);
        Assert.AreEqual(f1.GetHashCode(), f2.GetHashCode());
    }

    /// <summary>
    /// Checks <see cref="Formula.GetHashCode()"/> override works
    /// when two formulas are not equal (they should not have the same
    /// hash code).
    /// </summary>
    [TestMethod]
    public void FormulaEvaluate_TestGetHashCodeNotEquivalent_ExpectedBehaviour()
    {
        Formula f1 = new("1+2");
        Formula f2 = new("2+3");
        Assert.IsTrue(f1 != f2);
        Assert.AreNotEqual(f1.GetHashCode(), f2.GetHashCode());
    }

    // helper function used to manually define variables
    private double MyVariables(string name)
    {
        if (name == "A1")
        {
            return 2;
        }
        else if (name == "B1")
        {
            return 3;
        }
        else
        {
            throw new ArgumentException("Unknown variable");
        }
    }
}
