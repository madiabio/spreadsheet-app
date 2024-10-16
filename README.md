```
Author:	Madeline Abio
Partner: Sadie Bowen
Start Date: 16/10/2024
Course: CS 3500 Software Practice, University of Utah, School of Computing
GitHub ID: madiabio
Repo: https://github.com/uofu-cs3500-20-fall2024/spreadsheetpair-sadie-madi
Date: date time (when submission was completed)
Project: Spreadsheet
Copyright: CS 3500 and Madeline Abio and Sadie Bowen - This work may not be copied for use in Academic Coursework
```

# Comments to Evaluators:
At this stage, the Spreadsheet functionality has been implemented, though no GUI exists and also no methods for handling values of cells exist when the value relies on formulas.

# Assignment Specific Topics:

## Time Management Skill Paragraph:
DONT FORGET TO DO THIS FOR A6!!

## Time Estimation/Log:
Hours
Estimated/Worked,   Assignment,        Note

    3/4,            Assignment 2,      N/A
    3/5,            Assignment 3,      Spent about an hour extra debugging A2.
    3/6 ,           Assignment 4,      Was tired and a little defeated so took longer than usual
    4/9,            Assignment 5,      Spent 2 hours fixing an issue where the Formula project wouldn't load, and then SpreadsheetTests wasn't being recognised. Also was tired.

# Consulted Peers:
	1. Sadie Bowen

# References:
	1. C# Documentation - https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/
	2. ChatGPT - https://chatgpt.com/
	2. Dot Net Pearls - https://www.dotnetperls.com/

# NEW Examples of Good Software Practice (GSP) ** CHECK TO SEE IF THIS IS NECESSARY IF THE PARAGRAPH ALREADY EXISTS?

# Examples of Good Software Practice (GSP)
    1. DRY - Wrote a helper class inside of Spreadsheet.cs titled SpreadsheetUtils which contains a method that returns 
    true if a cell name is valid and false otherwise. This avoids repetition as a valid name check is needed
    many times in the Spreadsheet and Cell classes.
    
    2. Test methods - Wrote extensive testing for the name syntax test mentioned above in SpreadsheetTests. This ensure that
    InvalidNameExceptions are thrown when they should be.
    
    3. Encapsulation - Developers can interface with the software through well documented public methods without needing a 
    full understanding of what is occuring 'underneath the hood'.
  
# ChatGPT:

__Query 1:__
~~~
can you fix this regex pattern for me:
/// <summary>
/// Only alphanumeric characters or (, ), +, -, *, / are valid in a token.
/// </summary>
private const string ValidTokensPattern = @"^[a-zA-Z0-9()+-*/]+$";
~~~
_Respose:_
~~~
private const string ValidTokensPattern = @"^[a-zA-Z0-9\(\)\+\-\*/]+$";
~~~

__Query 2.__
~~~
how do you remove unnecessary .00s etc from a string that represents a number? (ie, "5.000" to "5") with regex
~~~

_Response:_
~~~
To remove unnecessary trailing .00 (or other trailing zeros) from a string representing a number, you can use a regular expression to match these patterns and replace them.

Here's a step-by-step approach:

Pattern: The regex pattern should identify:

Trailing zeroes after a decimal point.
Remove the decimal point itself if all digits after the decimal are zeros.
Regex Pattern:

Pattern: @"\.?0+$"
\.: Matches the decimal point (if it exists).
0+: Matches one or more zeros.
?: Makes the decimal point optional (in case there are only trailing zeros).
$: Ensures it matches only at the end of the string.
C# Example:
csharp
Copy code
using System;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main()
    {
        string[] numbers = { "5.000", "123.4500", "10.00", "3.14159", "0.00" };

        foreach (string number in numbers)
        {
            string cleanedNumber = RemoveTrailingZeros(number);
            Console.WriteLine(cleanedNumber);  // Output: 5, 123.45, 10, 3.14159, 0
        }
    }

    public static string RemoveTrailingZeros(string number)
    {
        return Regex.Replace(number, @"\.?0+$", "");
    }
}
~~~
__Query 3.__
~~~
how to match if a string is (, +, -, *, /
~~~
_Response_
~~~
To match a string that consists of one of the characters (, +, -, *, / in C#, you can use the following regular expression:

Regex Pattern:
^[()+\-*/]$
~~~