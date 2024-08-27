// <copyright file="FormulaSyntaxTests.cs" company="UofU-CS3500">
//   Copyright © 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> Madeline Abio </authors>
// <date> 08/21/2024 </date>

namespace CS3500.FormulaTests;

using CS3500.Formula1; // Change this using statement to use different formula implementations.

/// <summary>
///   <para>
///     The following class shows the basics of how to use the MSTest framework,
///     including:
///   </para>
///   <list type="number">
///     <item> How to catch exceptions. </item>
///     <item> How a test of valid code should look. </item>
///   </list>
/// </summary>
[TestClass]
public class FormulaSyntaxTests
{
    // --- Tests for One Token Rule ---

    /// <summary>
    ///   <para>
    ///     This test makes sure the right kind of exception is thrown
    ///     when trying to create a formula with no tokens.
    ///   </para>
    ///   <remarks>
    ///     <list type="bullet">
    ///       <item>
    ///         We use the _ (discard) notation because the formula object
    ///         is not used after that point in the method.  Note: you can also
    ///         use _ when a method must match an interface but does not use
    ///         some of the required arguments to that method.
    ///       </item>
    ///       <item>
    ///         string.Empty is often considered best practice (rather than using "") because it
    ///         is explicit in intent (e.g., perhaps the coder forgot to but something in "").
    ///       </item>
    ///       <item>
    ///         The name of a test method should follow the MS standard:
    ///         https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
    ///       </item>
    ///       <item>
    ///         All methods should be documented, but perhaps not to the same extent
    ///         as this one.  The remarks here are for your educational
    ///         purposes (i.e., a developer would assume another developer would know these
    ///         items) and would be superfluous in your code.
    ///       </item>
    ///       <item>
    ///         Notice the use of the attribute tag [ExpectedException] which tells the test
    ///         that the code should throw an exception, and if it doesn't an error has occurred;
    ///         i.e., the correct implementation of the constructor should result
    ///         in this exception being thrown based on the given poorly formed formula.
    ///       </item>
    ///     </list>
    ///   </remarks>
    ///   <example>
    ///     <code>
    ///        // here is how we call the formula constructor with a string representing the formula
    ///        _ = new Formula( "5+5" );
    ///     </code>
    ///   </example>
    /// </summary>
    [TestMethod]
    [ExpectedException( typeof( FormulaFormatException ) )]
    public void FormulaConstructor_TestNoTokens_Invalid( )
    {
        _ = new Formula( string.Empty );
    }

    // --- Tests for Valid Token Rule ---

    /// <summary>
    ///     <para>
    ///         This test checks that the correct kind of exception is thrown when
    ///         an invalid token is used. 
    ///     </para>
    ///     <remarks>
    ///         Only (, ), +, -, * /, variables, and numbers
    ///         are valid tokens. Therefore if an '@' sign is passed through, an
    ///         error should be returned.
    ///     </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestValidTokens_Invalid()
    {
        _ = new Formula("@"); // this tests rule 2 valid tokens to see if an error will be thrown for an invalid token alone
    }

    /// <summary>
    /// <para>
    ///     This test checks that scientific notation will be accepted as valid in a valid expression with a lowercase e.
    /// </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestValidTokenScientificNotationLowercase_Valid()
    {
        _ = new Formula("1e1+1");
    }


    /// <summary>
    /// <para>
    ///     This test checks that scientific notation will be accepted as valid in a valid expression with a capital E.
    /// </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestValidTokenScientificNotationUppercase_Valid()
    {
        _ = new Formula("1E1+1");
    }



    /// <summary>
    /// <para>
    ///     This test checks to see if the correct kind of exception is thrown for
    ///     an incorrectly defined variable, also has added benefit of checking that 
    ///     scientific notation isn't working because of incorrect variable definition 
    ///     (ie, 1e1 works, but 1a1 should not also work).
    /// </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestValidTokenVariable_Invalid()
    {
        _ = new Formula("1a1+1");
    }


    /// <summary>
    /// <para>
    ///     This test checks that if correctly defined variables are used, no exceptions are thrown
    /// </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestValidTokenVariable_Valid()
    {
        _ = new Formula("abc1+xyz1");
    }


    // --- Tests for Closing Parenthesis Rule

    /// <summary>
    ///     <para>
    ///         The number of closing parenthesis seen so far when read from left to right
    ///         should always be greater than or equal to the number of opening parenthesis 
    ///         seen so far.
    ///     </para>
    ///     <remarks>
    ///         This test checks that the correct kind of exception is thrown when reading
    ///         tokens from left to right, the number of closing parenthesis seen so far is
    ///         NOT less than than the number of opening parenthesis seen so far.
    ///     </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingParentheses_Invalid()
    {
        _ = new Formula("1+1)");
    }



    // --- Tests for Balanced Parentheses Rule

    /// <summary>
    ///     <para>
    ///         This test checks that no exceptions are given when the total number
    ///         of open parentheses is equal to the total number of closed parentheses.
    ///     </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestBalancedParentheses_Valid()
    {
        _ = new Formula("(1+1)");
    }

    /// <summary>
    ///     <para>
    ///         This checks if the correct type of exception is thrown when the
    ///         parenthesis are unbalanced (total num open parenthese > total num closed).
    ///     </para>
    ///     <remarks>
    ///         Only need to check (1+1, not ((1+1), because if (1+1) is valid as above, 
    ///         then ( (1+1) is redundant.
    ///     </remarks>
    /// </summary>
    [TestMethod]

    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestBalancedParentheses_Invalid()
    {
        _ = new Formula("(1+1");
    }



    // --- Tests for First Token Rule
    /// <summary>
    ///   <para>
    ///     Make sure a simple well formed formula is accepted by the constructor (the constructor
    ///     should not throw an exception).
    ///   </para>
    ///   <remarks>
    ///     This is an example of a test that is not expected to throw an exception, i.e., it succeeds.
    ///     In other words, the formula "1+1" is a valid formula which should not cause any errors.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenNumber_Valid()
    {
        _ = new Formula( "1+1" );
    }

    // --- Tests for  Last Token Rule ---
    /// <summary>
    ///     <para>
    ///         This test checks that the correct kind of exception is thrown when
    ///         the last token of an expression is not a number, variable, or closing parenthesis.
    ///     </para>
    ///     <remarks>
    ///          Ie, '1+' is invalid, because '+' is not a number, variable, or closing parenthesis.
    ///     </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLastTokenNumber_Invalid()
    {
        _ = new Formula("1+");
    }


    // --- Tests for Parentheses/Operator Following Rule ---
    /// <summary>
    ///     <para>
    ///         This test validates the rule that ONLY numbers, variables, or an open
    ///         parenthesis maybe immediately following an open parenthesis or operator.
    ///     </para>
    ///     <remarks>
    ///         It does so by checking that the correct kind of exception is thrown when
    ///         an invalid token follows an operator or open parenthesis. '1++1' is invalid
    ///         because '+' is an operator.
    ///     </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestParenthesisOperatorFollowing_Invalid()
    {
        _ = new Formula("1++1");
    }

    // --- Tests for Extra Following Rule ---
    /// <summary>
    ///     <para>
    ///         Tests that ONLY an operator or a closing parenthesis can immediately follow a
    ///         number, variable or closing parenthesis.
    ///     </para>
    ///     <remarks>
    ///         This test is performed by checking the expected exception is given when an open
    ///         parenthesis follows a number, variable or closing parenthesis.
    ///         
    ///         This rule also means that implicit multiplication is invalid, because (1)(1) isn't valid.
    ///     </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowing_Invalid()
    {
        _ = new Formula("1(1)");
    }


}