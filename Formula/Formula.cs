// <copyright file="Formula.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <summary>
//   <para>
//     This code is provides to start your assignment.  It was written
//     by Profs Joe, Danny, and Jim.  You should keep this attribution
//     at the top of your code where you have your header comment, along
//     with the other required information.
//   </para>
//   <para>
//     You should remove/add/adjust comments in your file as appropriate
//     to represent your work and any changes you make.
//   </para>
// </summary>

/// <summary>
/// Author:    Madeline Abio
/// Partner:   N/A
/// Date:      07/09/2024
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
///    This file contains the Formula class which is used to construct formulas.
/// </summary>

namespace CS3500.Formula;

using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text.RegularExpressions;

/// <summary>
///   <para>
///     This class represents formulas written in standard infix notation using standard precedence
///     rules.  The allowed symbols are non-negative numbers written using double-precision
///     floating-point syntax; variables that consist of one ore more letters followed by
///     one or more numbers; parentheses; and the four operator symbols +, -, *, and /.
///   </para>
///   <para>
///     Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
///     a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;
///     and "x 23" consists of a variable "x" and a number "23".  Otherwise, spaces are to be removed.
///   </para>
///   <para>
///     For Assignment Two, you are to implement the following functionality:
///   </para>
///   <list type="bullet">
///     <item>
///        Formula Constructor which checks the syntax of a formula.
///     </item>
///     <item>
///        Get Variables
///     </item>
///     <item>
///        ToString
///     </item>
///   </list>
/// </summary>
public class Formula
{
    /// <summary>
    /// Only alphanumeric characters or (, ), +, -, *, / are valid in a token.
    /// This pattern includes only valid characters.
    /// </summary>
    private const string ValidCharactersPattern = @"^[a-zA-Z0-9\(\)\+\-\*/\.]+$";

    /// <summary>
    ///   All variables are letters followed by numbers.  This pattern
    ///   represents valid variable name strings.
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    // Class variables
    private readonly string formulaString;
    private readonly List<string> tokens;

    /// <summary>
    ///   Initializes a new instance of the <see cref="Formula"/> class.
    ///   <para>
    ///     Creates a Formula from a string that consists of an infix expression written as
    ///     described in the class comment.  If the expression is syntactically incorrect,
    ///     throws a FormulaFormatException with an explanatory Message.  See the assignment
    ///     specifications for the syntax rules you are to implement.
    ///   </para>
    ///   <para>
    ///     Non Exhaustive Example Errors:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///        Invalid variable name, e.g., x, x1x  (Note: x1 is valid, but would be normalized to X1)
    ///     </item>
    ///     <item>
    ///        Empty formula, e.g., string.Empty
    ///     </item>
    ///     <item>
    ///        Mismatched Parentheses, e.g., "(("
    ///     </item>
    ///     <item>
    ///        Invalid Following Rule, e.g., "2x+5"
    ///     </item>
    ///   </list>
    /// </summary>
    /// <param name="formula"> The string representation of the formula to be created.</param>
    public Formula(string formula)
    {
        if (string.IsNullOrWhiteSpace(formula))
        {
            throw new FormulaFormatException("Formula cannot be empty.");
        }

        List<string> tokens = GetTokens(formula);
        if (tokens.Count == 0)
        {
            throw new FormulaFormatException("Formula cannot be empty.");
        }

        ValidateSyntax(tokens);

        this.tokens = tokens;

        formula = string.Join(string.Empty, tokens); // join formatted tokens to create properly formatted formula

        formulaString = formula;
    }

    /// <summary>
    ///   <para>
    ///     Reports whether f1 == f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are the same.</returns>
    public static bool operator ==(Formula f1, Formula f2)
    {
        if (f1.ToString() == f2.ToString())
        {
            return true;
        }

        return false;
    }

    /// <summary>
    ///   <para>
    ///     Reports whether f1 != f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are not equal to each other.</returns>
    public static bool operator !=(Formula f1, Formula f2)
    {
        if (f1.ToString() != f2.ToString())
        {
            return true;
        }

        return false;
    }

    /// <summary>
    ///   <para>
    ///     Determines if two formula objects represent the same formula.
    ///   </para>
    ///   <para>
    ///     By definition, if the parameter is null or does not reference
    ///     a Formula Object then return false.
    ///   </para>
    ///   <para>
    ///     Two Formulas are considered equal if their canonical string representations
    ///     (as defined by ToString) are equal.
    ///   </para>
    /// </summary>
    /// <param name="obj"> The other object.</param>
    /// <returns>
    ///   True if the two objects represent the same formula.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is Formula f)
        {
            return this.ToString() == f.ToString();
        }

        return false;
    }

    /// <summary>
    ///   <para>
    ///     Evaluates this Formula, using the lookup delegate to determine the values of
    ///     variables.
    ///   </para>
    ///   <remarks>
    ///     When the lookup method is called, it will always be passed a Normalized (capitalized)
    ///     variable name.  The lookup method will throw an ArgumentException if there is
    ///     not a definition for that variable token.
    ///   </remarks>
    ///   <para>
    ///     If no undefined variables or divisions by zero are encountered when evaluating
    ///     this Formula, the numeric value of the formula is returned.  Otherwise, a
    ///     FormulaError is returned (with a meaningful explanation as the Reason property).
    ///   </para>
    ///   <para>
    ///     This method should never throw an exception.
    ///   </para>
    /// </summary>
    /// <param name="lookup">
    ///   <para>
    ///     Given a variable symbol as its parameter, lookup returns the variable's (double) value
    ///     (if it has one) or throws an ArgumentException (otherwise).  This method should expect
    ///     variable names to be capitalized.
    ///   </para>
    /// </param>
    /// <returns> Either a double or a formula error, based on evaluating the formula.</returns>
    public object Evaluate(Lookup lookup)
    {
        Stack<string> vstack = new(); // value stack
        Stack<string> ostack = new(); // operator stack
        foreach (string t in tokens)
        {
            if (IsNumber(t))
            {
                if (ostack.Count > 0 && (ostack.Peek() == "*" || ostack.Peek() == "/"))
                {
                    double v1 = double.Parse(vstack.Pop()); // pop the value stack and make it a double
                    string op = ostack.Pop(); // pop the operator stack
                    double v2 = double.Parse(t); // make t a double
                    if (op == "*") // do multiplication on the two values (and change it back to a string), then push it on the value stack.
                    {
                        vstack.Push((v1 * v2).ToString());
                    }
                    else if (op == "/") // do division on the two values, checking for div by zero. then push result on value stack (if result is possible)
                    {
                        if (v2 == 0)
                        {
                            return new FormulaError("Division by zero");
                        }

                        vstack.Push((v1 / v2).ToString());
                    }
                }
                else // if not * or /, push t on the value stack.
                {
                    vstack.Push(t);
                }
            }
            else if (IsVar(t))
            {
                try
                {
                    string t_val = lookup(t).ToString(); // lookup the value of the variable and make it a string
                    if (ostack.Count > 0 && (ostack.Peek() == "*" || ostack.Peek() == "/"))
                    {
                        double v1 = double.Parse(vstack.Pop()); // pop the value stack and make it a double
                        string op = ostack.Pop(); // pop the operator stack
                        double v2 = double.Parse(t_val); // make t_val a double
                        if (op == "*") // do multiplication on the two values (and change it back to a string), then push it on the value stack.
                        {
                            vstack.Push((v1 * v2).ToString());
                        }
                        else if (op == "/") // do division on the two values, checking for div by zero. then push result on value stack (if result is possible)
                        {
                            if (v2 == 0)
                            {
                                return new FormulaError("Division by zero");
                            }

                            vstack.Push((v1 / v2).ToString());
                        }
                    }
                    else // if not * or /, push t_val on the value stack.
                    {
                        vstack.Push(t_val);
                    }
                }
                catch (ArgumentException) // if the variable is not found, return a FormulaError
                {
                    return new FormulaError($"Unknown variable: {t}");
                }
            }
            else if ((t == "+") || (t == "-"))
            {
                if (ostack.Count > 0 && (ostack.Peek() == "+" || ostack.Peek() == "-")) // if + or - is at top of operator stack,
                {
                    double v1 = double.Parse(vstack.Pop()); // pop the value stack twice (& make them doubles)
                    double v2 = double.Parse(vstack.Pop());
                    string op = ostack.Pop(); // pop the operator stack once

                    // then apply operator to the vals, and push the result (as a string) on the value stack.
                    if (op == "+")
                    {
                        vstack.Push((v1 + v2).ToString());
                    }
                    else
                    {
                        vstack.Push((v2 - v1).ToString());
                    }
                }

                ostack.Push(t); // push t on to the operator stack
            }
            else if ((t == "*") || (t == "/")) // if t is * or /, push it on the operator stack.
            {
                ostack.Push(t);
            }
            else if (t == "(") // if t is left parenthesis, push it on the operator stack.
            {
                ostack.Push(t);
            }
            else if (t == ")")
            {
                if (ostack.Count > 0 && ((ostack.Peek() == "+") || (ostack.Peek() == "-")))// if + or - is at top of ostack,
                {
                    // pop val stack twice and op stack once.
                    double v1 = double.Parse(vstack.Pop());
                    double v2 = double.Parse(vstack.Pop());
                    string op = ostack.Pop();

                    // apply operator to the vals, and push the result (as a string) on the value stack.
                    if (op == "+")
                    {
                        vstack.Push((v1 + v2).ToString());
                    }
                    else
                    {
                        vstack.Push((v2 - v1).ToString());
                    }

                    ostack.Pop(); // pop the operator stack. top should be "(".
                }
                else // otherwise, the top of the stack is "("
                {
                    ostack.Pop();
                }

                if (ostack.Count > 0 && ((ostack.Peek() == "*") || (ostack.Peek() == "/")))
                {
                    // pop value stack twice and operator stack once
                    double v1 = double.Parse(vstack.Pop());
                    double v2 = double.Parse(vstack.Pop());
                    string op2 = ostack.Pop();

                    // apply operator to the vals, and push the results (as a double) onto value stack
                    if (op2 == "*")
                    {
                        vstack.Push((v1 * v2).ToString());
                    }
                    else
                    {
                        if (v1 == 0)
                        {
                            return new FormulaError("Division by zero");
                        }

                        vstack.Push((v2 / v1).ToString());
                    }
                }
            }
        }

        // when the last token has been processed, two conditions:
        if (ostack.Count == 0) // if operator stack is empty, then the value stack should have only one value, which is the result.
        {
            return double.Parse(vstack.Pop());
        }

        { // the operator will always either be + or -.
            double v1 = double.Parse(vstack.Pop());
            double v2 = double.Parse(vstack.Pop());
            string op = ostack.Pop();

            if (op == "+")
            {
                return v1 + v2;
            }
            else if (op == "-")
            {
                return v2 - v1;
            }
        }

        throw new Exception("Unknown Issue");
    }

    /// <summary>
    ///   <para>
    ///     Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
    ///     case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two
    ///     randomly-generated unequal Formulas have the same hash code should be extremely small.
    ///   </para>
    /// </summary>
    /// <returns> The hashcode for the object. </returns>
    public override int GetHashCode()
    {
        return formulaString.GetHashCode();
    }

    /// <summary>
    ///   <para>
    ///     Returns a set of all the variables in the formula.
    ///   </para>
    ///   <remarks>
    ///     Important: no variable may appear more than once in the returned set, even
    ///     if it is used more than once in the Formula.
    ///   </remarks>
    ///   <para>
    ///     For example, if N is a method that converts all the letters in a string to upper case:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>new("x1+y1*z1").GetVariables() should enumerate "X1", "Y1", and "Z1".</item>
    ///     <item>new("x1+X1"   ).GetVariables() should enumerate "X1".</item>
    ///   </list>
    /// </summary>
    /// <returns> the set of variables (string names) representing the variables referenced by the formula. </returns>
    public ISet<string> GetVariables()
    {
        ISet<string> vars = new HashSet<string>();

        /// <summary>
        /// Uses isvar function to check if a token is a variable, if so, adds it to the set of variables.
        /// </summary>
        foreach (string token in tokens)
        {
            if (IsVar(token))
            {
                // string token_upper = token.ToUpper(); // normalise token, but also this shoudl already be done by fomrmula constructor.
                vars.Add(token);
            }
        }

        return vars;
    }

    /// <returns>
    ///  A canonical version (string) of the formula. All "equal" formulas
    ///   should have the same value here.
    /// </returns><summary>
    ///   <para>
    ///     Returns a string representation of a canonical form of the formula.
    ///   </para>
    ///   <para>
    ///     The string will contain no spaces.
    ///   </para>
    ///   <para>
    ///     If the string is passed to the Formula constructor, the new Formula f
    ///     will be such that this.ToString() == f.ToString().
    ///   </para>
    ///   <para>
    ///     All of the variables in the string will be normalized.  This
    ///     means capital letters.
    ///   </para>
    ///   <para>
    ///       For example:
    ///   </para>
    ///   <code>
    ///       new("x1 + y1").ToString() should return "X1+Y1"
    ///       new("X1 + 5.0000").ToString() should return "X1+5".
    ///   </code>
    ///   <para>
    ///     This code should execute in O(1) time.
    ///   <para>
    /// </summary>
    public override string ToString()
    {
        return formulaString;
    }

    /// <summary>
    /// Validates the syntax of all tokens in a formula, also performs normalization.
    /// </summary>
    /// <param name="tokens">list of tokens in formula.</param>
    private static void ValidateSyntax(List<string> tokens)
    {
        Regex openParenthesisOrOperatorPattern = new(@"^[(+\-*/]$");
        Regex closingParenthesisOrOperatorPattern = new(@"^[)+\-*/]$");
        Regex scientificNotationPattern = new(@"^([+-]?\d+(\.\d+)?)[eE]([+-]?\d+)$"); // Matches explicitely defined scientific notation (ie, when there is a +/-)

        // open & closed parenthesis counters
        int openCount = 0;
        int closeCount = 0;
        for (int i = 0; i < tokens.Count; i++)
        {
            // FORMATTING:
            tokens[i] = tokens[i].ToUpper(); // Normalise token

            if (scientificNotationPattern.IsMatch(tokens[i]) && tokens.Count == 1) // Normalise implicitely defined scientfic notation
            {
                Match scientficMatch = scientificNotationPattern.Match(tokens[i]);
                string basePart = scientficMatch.Groups[1].Value; // Capture base (eg 1.1)
                string exponentPart = scientficMatch.Groups[3].Value; // Capture exponent (eg +10)

                if (!exponentPart.StartsWith("-") && !exponentPart.StartsWith("+")) // Add the missing + sign
                {
                    exponentPart = "+" + exponentPart; // append + sign to exponent part
                    tokens[i] = basePart + "E" + exponentPart; // re-assign tokens[i] with fixed part.
                }
            }
            else if (scientificNotationPattern.IsMatch(tokens[i]) && tokens.Count > 1) // Convert scientific notation to double form if other tokens in expression
            {
                tokens[i] = $"{double.Parse(tokens[i])}";
            }
            else if (IsNumber(tokens[i])) // Remove trailing zeros from token if its a number
            {
                // tokens[i] = TrailingZerosPattern.Replace(tokens[i], string.Empty);
                tokens[i] = double.Parse(tokens[i]).ToString("G");
            }

            // PARENTHESIS COUNT
            if (tokens[i] == "(")
            {
                openCount += 1;
            }
            else if (tokens[i] == ")")
            {
                closeCount += 1;
            }

            // SYNTAX CHECKING:
            if (closeCount > openCount) // CLOSED PARENTHESIS RULE: when reading tokens left to right, there should never be more closed parentehsis than open parenthesis.
            {
                throw new FormulaFormatException($"Number of closed parenthesis seen so far exceeds number of open parenthesis seen so far.");
            }

            if (!Regex.IsMatch(tokens[i], ValidCharactersPattern)) // VALID TOKENS: if there are invalid characters in the token, raise error
            {
                throw new FormulaFormatException($"Invalid character in token {tokens[i]}.");
            }

            if (i == 0 && !(IsVar(tokens[i]) || tokens[i] == "(" || IsNumber(tokens[i]))) // FIRST TOKEN RULE: the token must be variable, open parenthesis, or number.
            {
                throw new FormulaFormatException($"First token in formula must be either a variable, number, or (. Token was {tokens[i]}");
            }
            else if (i == tokens.Count - 1 && !(IsVar(tokens[i]) || tokens[i] == ")" || IsNumber(tokens[i]))) // LAST TOKEN RULE: Last token of expression must be a number, variable, or closing parenthesis
            {
                throw new FormulaFormatException($"Last token in formula must be either a variable, number, or ). Token was {tokens[i]}");
            }

            if (i < tokens.Count - 1 && openParenthesisOrOperatorPattern.IsMatch(tokens[i]) && !(IsVar(tokens[i + 1]) || tokens[i + 1] == "(" || IsNumber(tokens[i + 1]))) // PARENTHESIS/OPERATOR FOLLOWING RULE: Only a number, variable, or open parenthesis may immediately follow an open parentehsis or operator
            {
                throw new FormulaFormatException($"Only a variable, number or open parenthesis may immediately follow an open parenthesis or operator. Token = {tokens[i]}, following token = {tokens[i + 1]}.");
            }

            if (i < tokens.Count - 1 && (IsVar(tokens[i]) || tokens[i] == ")" || IsNumber(tokens[i])) && !closingParenthesisOrOperatorPattern.IsMatch(tokens[i + 1])) // EXTRA FOLLOWING RULE: Only an opeartor or closing parenthesis can immediately  follow a number, variable, or closing parenthesis
            {
                throw new FormulaFormatException($"Only an operator or closing parenthesis may immediately follow a number, variable, or closing parenthesis. Token = {tokens[i]}, following token = {tokens[i + 1]}.");
            }
        }

        if (openCount != closeCount) // BALANCED PARENTEHSIS RULE: Total num opening parenthesis must equal total num closing parenthesis.
        {
            throw new FormulaFormatException($"Parenthesis are not balanced. Num open = {openCount}, num close = {closeCount}");
        }
    }

    /// <summary>
    /// Checks if a token is a number (could be float, int, or exponential).
    /// </summary>
    /// <param name="token">a token in the formula.</param>
    /// <returns>Returns True if it is a number, False if else. </returns>
    private static bool IsNumber(string token)
    {
        return double.TryParse(token, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double result);
    }

    /// <summary>
    ///   Reports whether "token" is a variable.  It must be one or more letters
    ///   followed by one or more numbers.
    /// </summary>
    /// <param name="token"> A token that may be a variable. </param>
    /// <returns> true if the string matches the requirements, e.g., A1 or a1. </returns>
    private static bool IsVar(string token)
    {
        // notice the use of ^ and $ to denote that the entire string being matched is just the variable
        string standaloneVarPattern = $"^{VariableRegExPattern}$";
        return Regex.IsMatch(token, standaloneVarPattern);
    }

    /// <summary>
    ///   <para>
    ///     Given an expression, enumerates the tokens that compose it.
    ///   </para>
    ///   <para>
    ///     Tokens returned are:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>left paren</item>
    ///     <item>right paren</item>
    ///     <item>one of the four operator symbols</item>
    ///     <item>a string consisting of one or more letters followed by one or more numbers</item>
    ///     <item>a double literal</item>
    ///     <item>and anything that doesn't match one of the above patterns</item>
    ///   </list>
    ///   <para>
    ///     There are no empty tokens; white space is ignored (except to separate other tokens).
    ///   </para>
    /// </summary>
    /// <param name="formula"> A string representing an infix formula such as 1*B1/3.0. </param>
    /// <returns> The ordered list of tokens in the formula. </returns>
    private static List<string> GetTokens(string formula)
    {
        List<string> results = [];

        string lpPattern = @"\(";
        string rpPattern = @"\)";
        string opPattern = @"[\+\-*/]";
        string doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        string spacePattern = @"\s+";

        // Overall pattern
        string pattern = string.Format(
                                        "({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                        lpPattern,
                                        rpPattern,
                                        opPattern,
                                        VariableRegExPattern,
                                        doublePattern,
                                        spacePattern);

        // Enumerate matching tokens that don't consist solely of white space.
        foreach (string s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
        {
            if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
            {
                results.Add(s);
            }
        }

        return results;
    }
}

/// <summary>
///   Used to report syntax errors in the argument to the Formula constructor.
/// </summary>
public class FormulaFormatException : Exception
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaFormatException"/> class.
    ///   <para>
    ///      Constructs a FormulaFormatException containing the explanatory message.
    ///   </para>
    /// </summary>
    /// <param name="message"> A developer defined message describing why the exception occured.</param>
    public FormulaFormatException(string message)
        : base(message)
    {
        // All this does is call the base constructor. No extra code needed.
    }
}

/// <summary>
/// Used as a possible return value of the Formula.Evaluate method.
/// </summary>
public class FormulaError
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaError"/> class.
    ///   <para>
    ///     Constructs a FormulaError containing the explanatory reason.
    ///   </para>
    /// </summary>
    /// <param name="message"> Contains a message for why the error occurred.</param>
    public FormulaError(string message)
    {
        Reason = message;
    }

    /// <summary>
    ///  Gets the reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
}

/// <summary>
///   Any method meeting this type signature can be used for
///   looking up the value of a variable.  In general the expected behavior is that
///   the Lookup method will "know" about all variables in a formula
///   and return their appropriate value.
/// </summary>
/// <exception cref="ArgumentException">
///   If a variable name is provided that is not recognized by the implementing method,
///   then the method should throw an ArgumentException.
/// </exception>
/// <param name="variableName">
///   The name of the variable (e.g., "A1") to lookup.
/// </param>
/// <returns> The value of the given variable (if one exists). </returns>
public delegate double Lookup(string variableName);