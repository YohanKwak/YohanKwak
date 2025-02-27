﻿//Formula class implemented by Yohan Kwak, based on the given
// Skeleton written by Profs Zachary, Kopta and Martin for CS 3500

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {

        private String expression;
        private Func<string, string> normalizeFunc;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        /// 
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            this.expression = formula;
            this.normalizeFunc = normalize;

            IEnumerable<String> tokens = Formula.GetTokens(expression);

            if (tokens.Count() == 0)
            {
                throw new FormulaFormatException("Empty Expression");
            }


            if (!isValidTokenOtherThanVariable(tokens.Last()))
            {
                if (!isValid(tokens.Last()))
                {
                    throw new FormulaFormatException("Illegal / Invalid variable format");
                }
            }
            else if (tokens.Last() == "+" || tokens.Last() == "-" || tokens.Last() == "*" || tokens.Last() == "(" || tokens.Last() == "/")
            {
                throw new FormulaFormatException("Last token must be a number, a variable, or an Closing paranthesis");
            }

            bool isFirstToken = true;
            int parenthesesNotClosed = 0;

            //Checks if following token needs to be number, variable or Opening Parenthesis
            bool nvopClicker = false;

            foreach (String token in tokens)
            {
                //Checking first Token
                if (isFirstToken)
                {
                    isFirstToken = false;
                    if (!isValidTokenOtherThanVariable(token))
                    {
                        if (!isValid(token))
                        {
                            throw new FormulaFormatException("Illegal / Invalid variable format");
                        }
                    }
                    else if (token == "+" || token == "-" || token == "*" || token == ")" || token == "/")
                    {
                        throw new FormulaFormatException("First token must be a number, a variable, or an opening paranthesis");
                    }
                    else if (token == "(")
                    {
                        nvopClicker = true;
                        parenthesesNotClosed++;
                    }
                }
                else
                {
                    if (nvopClicker)
                    {
                        if (!isValidTokenOtherThanVariable(token))
                        {
                            if (!isValid(token))
                            {
                                throw new FormulaFormatException("Illegal / Invalid variable format");
                            }
                            nvopClicker = false;
                        }
                        else if (token == "+" || token == "-" || token == "*" || token == ")" || token == "/")
                        {
                            throw new FormulaFormatException("Following Rule Error");
                        }
                        else if (token == "(")
                        {
                            parenthesesNotClosed++;
                        }
                        else
                        {
                            nvopClicker = false;
                        }
                    }
                    else
                    {
                        if (!isValidTokenOtherThanVariable(token))
                        {
                            throw new FormulaFormatException("Following Rule Error");
                        }
                        else if (token == "(")
                        {
                            throw new FormulaFormatException("Following Rule Error");
                        }
                        else if (double.TryParse(token, out double number) && Double.Parse(token).ToString() == number.ToString())
                        {
                            throw new FormulaFormatException("Following Rule Error");
                        }
                        else if (token == ")")
                        {
                            parenthesesNotClosed--;
                        }
                        else
                        {
                            nvopClicker = true;
                        }
                    }
                }
            }
            if (parenthesesNotClosed != 0)
            {
                throw new FormulaFormatException("Parentheses Error");
            }


            this.expression = this.ToString();
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> values = new Stack<double>();
            Stack<String> operators = new Stack<String>();

            double currentVal;
            String? CurrentOperation;

            IEnumerable<String> tokens = Formula.GetTokens(expression);


            foreach (string token in tokens)
            {
                //When token is number
                if (double.TryParse(token, out double number) && Double.Parse(token).ToString() == number.ToString())
                {
                    if (operators.TryPeek(out CurrentOperation) && (CurrentOperation.Equals("*") || CurrentOperation.Equals("/")))
                    {
                        operators.Pop();
                        if (CurrentOperation.Equals("*"))
                        {
                            currentVal = values.Pop() * number;
                        }
                        else
                        {
                            if (number == 0)
                            {
                                return new FormulaError("Divided by 0");
                            }
                            currentVal = values.Pop() / number;
                        }
                        values.Push(currentVal);
                    }
                    else
                    {
                        values.Push(number);
                    }
                }
                // when token is either "+" or "-"
                else if (token.Equals("+") || token.Equals("-"))
                {
                    if (operators.TryPeek(out CurrentOperation) && (CurrentOperation.Equals("+") || CurrentOperation.Equals("-")))
                    {
                        CurrentOperation = operators.Pop();

                        if (CurrentOperation.Equals("+"))
                        {
                            currentVal = values.Pop() + values.Pop();
                            values.Push(currentVal);
                        }
                        else
                        {
                            double secondNum = values.Pop();
                            currentVal = values.Pop() - secondNum;
                            values.Push(currentVal);
                        }
                    }
                    operators.Push(token);
                }
                // When token is either "*" or "/"
                else if (token.Equals("*") || token.Equals("/"))
                {
                    operators.Push(token);
                }
                // When token is "("
                else if (token.Equals("("))
                {
                    operators.Push(token);
                }
                // When token is ")"
                else if (token.Equals(")"))
                {
                    if (operators.TryPeek(out CurrentOperation) && (CurrentOperation.Equals("+") || CurrentOperation.Equals("-")))
                    {
                        CurrentOperation = operators.Pop();

                        if (CurrentOperation.Equals("+"))
                        {
                            currentVal = values.Pop() + values.Pop();
                            values.Push(currentVal);
                        }
                        else
                        {
                            double secondNum = values.Pop();
                            currentVal = values.Pop() - secondNum;
                            values.Push(currentVal);
                        }
                    }

                    operators.Pop();

                    if (operators.TryPeek(out CurrentOperation) && (CurrentOperation.Equals("*") || CurrentOperation.Equals("/")))
                    {
                        CurrentOperation = operators.Pop();

                        if (CurrentOperation.Equals("*"))
                        {
                            currentVal = values.Pop() * values.Pop();
                            values.Push(currentVal);
                        }
                        else
                        {
                            double secondNum = values.Pop();
                            if (secondNum == 0)
                            {
                                return new FormulaError("Divided by 0");
                            }
                            currentVal = values.Pop() / secondNum;
                            values.Push(currentVal);
                        }
                    }

                }
                // When token is something else(Variable)
                else
                {
                    try
                    {
                        number = lookup(this.normalizeFunc(token));
                    }
                    catch (ArgumentException)
                    {
                        return new FormulaError("Undefined Variable");
                    }

                    if (operators.TryPeek(out CurrentOperation) && (CurrentOperation.Equals("*") || CurrentOperation.Equals("/")))
                    {
                        operators.Pop();
                        if (CurrentOperation.Equals("*"))
                        {
                            currentVal = values.Pop() * number;
                        }
                        else
                        {
                            if (number == 0)
                            {
                                return new FormulaError("Divided by 0");
                            }
                            currentVal = values.Pop() / number;
                        }
                        values.Push(currentVal);
                    }
                    else
                    {
                        values.Push(number);
                    }
                }

            }

            //After going through all the token, compute the final procedure for evaluating(if needed) and returns the value.
            if (operators.Count != 0)
            {
                if (operators.Pop().Equals("+"))
                {
                    currentVal = values.Pop() + values.Pop();

                }
                else
                {
                    double secondNum = values.Pop();
                    currentVal = values.Pop() - secondNum;
                }
                values.Push(currentVal);

            }
            

            return values.Pop();
        }

        private bool isValidTokenOtherThanVariable(string token)
        {
            if (token.Equals("(") || token.Equals(")") || token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/"))
            {
                return true;
            }
            else if (double.TryParse(token, out double number) && Double.Parse(token).ToString() == number.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            IEnumerable<String?> tokens = Formula.GetTokens(expression);

            HashSet<String> variables = new HashSet<String>();

            foreach (string? token in tokens)
            {
                if (token != null) {
                    if (token.Equals("(") || token.Equals(")") || token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/"))
                    {
                        //Do nothing
                    }
                    else if (double.TryParse(token, out double number) && Double.Parse(token).ToString() == number.ToString())
                    {
                        //Do nothing
                    }
                    else
                    {
                        if (!variables.Contains(token))
                            variables.Add(token);
                    }
                }
            }
            return variables;
        }


        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string outString = "";

            if (expression != null)
            {
                IEnumerable<String?> tokens = Formula.GetTokens(expression);

                

                foreach (string? token in tokens)
                {
                    if (double.TryParse(token, out double number) && Double.Parse(token).ToString() == number.ToString())
                    {
                        outString += number.ToString();
                    }
                    else
                    {
                        if (token != null)
                        outString += this.normalizeFunc(token);
                    }
                }
            }

            return outString;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj != null && obj is Formula)
            {
                Formula formula2 = (Formula)obj;

                return this.ToString().Equals(formula2.ToString());
            }
            return false;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that f1 and f2 cannot be null, because their types are non-nullable
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that f1 and f2 cannot be null, because their types are non-nullable
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return String.GetHashCode(this.expression); ;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}