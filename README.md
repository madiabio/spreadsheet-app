# Spreadsheet Solution README
### Ownership
- Authors/ Partnership -> Madeline Abio, Sadie Bowen
- Course -> CS 3500 Software Practice, University of Utah, School of Computing
- Copyright -> CS 3500 and Madeline Abio and Sadie Bowen _This work may not be copied for use in Academic Coursework_.

### Versioning
- GitHub IDs -> madiabio & sadie-bowen
- [GitHub Repository](https://github.com/uofu-cs3500-20-fall2024/spreadsheetpair-sadie-madi)

### Important Dates
- Solution Start Date -> 01-Sep-2024 
- Partnership Start Date ->  17-Oct-2024
- Commit Date -> 20-Oct-2024

# Comments to Evaluators:
``
At this stage, the Spreadsheet functionality has been implemented, though no GUI exists and also no methods for handling values of cells exist when the value relies on formulas.
There was an issue with submission and we had to submit the branch. Submitting main was timing out.
``

# Assignment Specific Topics:

### Parnerhsip Contributions
``
All work was done via pair programming
``

### Branching
``
Detail any branches that were made so that each partner could wokr on seperate functionality. Describe merge process and any challenges. Mkae sure to list commit numbers.``

### Time Management Skill Paragraph (PS6):
``
We discussed how our time management has improved. We have learned that it is quite hard to predict how long something will take you. It can be difficult to not be too optimistic with time estimates 
which would not serve us well at a job. We have both found the most useful way to predict how long somethign will take is easier done when you 
break down the different aspects of what you are being asked to do and then predict how long those components will take.
``
### Time Management Skill Table (PS7):
``
Time estimates are getting somewhat worse as the task complexity inceeased over the past two assignments. Debugging has been considered more in the time estimations, but learning 
new material was not completely considered for A7 as neither partners were familiar with Blazor before this project. This says that there is still much to leran about time
estimations and how to make them more accurate, especially when new material is incorporated into an assignment.
``

### Partnership evaluation
```
This partnership proved highly effective, particularly in our collaborative approach to debugging and the open exchange of assistance throughout the assignment. 
When encountering complex bugs, working together provided invaluable insights that one of us alone might not have spotted. For example, debugging as a 
team brought fresh perspectives and ideas, often leading to faster and more effective problem-solving. Additionally, although we didn’t divide tasks explicitly, 
we maintained a fluid dynamic, where each of us readily provided support when the other needed it. This unstructured but collaborative approach allowed us to 
maximize our individual strengths and work through challenges as a unit, leading to a more refined final product.

However, an area for improvement in our teamwork is in managing our time and scheduling collaborative work sessions. Coordinating times to work together was 
challenging, and this sometimes led to delays or interruptions in our workflow. Improving our time management skills and proactively setting dedicated times for 
joint work could make our process smoother and more efficient in future collaborations. This would help us avoid scheduling conflicts and ensure that both partners 
are consistently on the same page, ultimately allowing us to complete tasks more seamlessly and with less stress.
```

### Time Expenditures:
| Assignment | Predicted Hours | Actual Hours |                                                                   Notes                                                                   |         Ownership         |
|:----------:|:---------------:|:------------:|:-----------------------------------------------------------------------------------------------------------------------------------------:|:-------------------------:|
|     A2     |        3        |      4       |                                                                    N/A                                                                    |         Madi Abio         |
|     A3     |        3        |      5       |                                                  Spent about an hour extra debugging A2.                                                  |         Madi Abio         |
|     A4     |        3        |      6       |                                         Was tired and a little defeated so took longer than usual                                         |         Madi Abio         |
|     A5     |        4        |      9       | Spent 2 hours fixing an issue where the Formula project wouldn't load, and then SpreadsheetTests wasn't being recognised. Also was tired. |         Madi Abio         |
|     A6     |        8        |      9       |                                          Spent some extra time for whiteboards and debugging A5.                                          | Madi Abio and Sadie Bowen |
|     A7     |        8        |      12      |                                                         Spent extra time learning Blazor                                                  | Madi Abio and Sadie Bowen |

#### PS7 time tracking (needs to be updated):
```
** maybe remove this tbh: **
- 2Hrs debugging A6
- 30 min adding stuff to readme based off design specs and doing all the branching/merging/etc stuff
- 2Hrs trying to learn Blazor stuff
- 3Hrs implementing save, load, clear
- 2Hrs bugfixing
```

# References:
- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-8.0)
- [C# Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/)
- [ChatGPT](https://chatgpt.com/)
- [Dot Net Pearls](https://www.dotnetperls.com/)

# Our Software Practices
## Examples of Good Software Practice (GSP)
    1. DRY - Wrote a helper class inside of Spreadsheet.cs titled SpreadsheetUtils which contains a method that returns 
    true if a cell name is valid and false otherwise. This avoids repetition as a valid name check is needed
    many times in the Spreadsheet and Cell classes.
    
    2. Test methods - Wrote extensive testing for the name syntax test mentioned above in SpreadsheetTests. This ensure that
    InvalidNameExceptions are thrown when they should be.
    
    3. Encapsulation - Developers can interface with the software through well documented public methods without needing a 
    full understanding of what is occuring 'underneath the hood'.
  
# Resources & Peers

### Chat GPT
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

## Consulted Sources
- Professor de St. Germain's lecture slides were consulted.
- Consulted Piazza to help learn from other students' questions.
- [ChatGPT](chatgpt.com)

## Consulted Peers
- No peers outside of this partnership were consulted.