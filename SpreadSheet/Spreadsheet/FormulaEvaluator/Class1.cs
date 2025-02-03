using System.Text.RegularExpressions;


namespace FormulaEvaluator
{

    ///
    public static class Evaluator
    {
        /// <summary>
        /// Looks up the appropriate int value from the given String V.
        /// </summary>
        /// <param name="V"></param> the String lookup uses to return int.
        /// <returns></returns> integer representation of given string.
        public delegate int Lookup(String V);

        /// <summary>
        /// Checks the given token and returns if the given token is valid
        /// </summary>
        /// <param name="token"></param> token to be evaluated.
        /// <returns></returns>true or false regarding given token is valid
        private static bool checkToken(String token)
        {
            if (token.Equals("(") || token.Equals(")") || token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/"))
            {
                return true;
            }else if (int.TryParse(token, out int number) && token.Equals(number.ToString()))
            {
                return true;
            }
            else
            {
                bool isNum = false;
                char[] tokenchar = token.ToCharArray();
                if (!Char.IsLetter(tokenchar[0]))
                {
                    return false;
                }
                for (int i = 1; i < token.Length; i++)
                {
                    if (Char.IsDigit(tokenchar[i]))
                    {
                        isNum = true;
                    }
                    else if (isNum)
                    {
                        return false;

                    }
                }
            }
                return true;
        }

        /// <summary>
        /// Evaluates the the given expression, using specific variable evaluator given.
        /// </summary>
        /// <param name="exp"></param> The expression to be evaluated
        /// <param name="variableEvaluator"></param> Delegate to be used for evaluating variables.
        /// <returns></returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            Stack<int> values = new Stack<int>();
            Stack<String> operators = new Stack<String>();

            int currentVal;
            String CurrentOperation;

            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            foreach (string token in substrings)
            {

                currentVal = 0;
                CurrentOperation = "";

                //Skips the empty String""
                if (token.Equals(""))
                {

                }
                else if (!checkToken(token))
                {
                    throw new ArgumentException();
                }
                //When token is number
                else if (int.TryParse(token, out int number) && token.Equals(number.ToString()))
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
                            int secondNum = values.Pop();
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
                            int secondNum = values.Pop();
                            currentVal = values.Pop() - secondNum;
                            values.Push(currentVal);
                        }
                        operators.Pop();
                    }

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
                            int secondNum = values.Pop();
                            currentVal = values.Pop() / secondNum;
                            values.Push(currentVal);
                        }
                    }
                }
                // When token is something else(Variable)
                else
                {
                    number = variableEvaluator(token);

                    if (operators.TryPeek(out CurrentOperation) && (CurrentOperation.Equals("*") || CurrentOperation.Equals("/")))
                    {
                        operators.Pop();
                        if (CurrentOperation.Equals("*"))
                        {
                            currentVal = values.Pop() * number;
                        }
                        else
                        {
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
                    values.Push(currentVal);
                }
                else
                {
                    int secondNum = values.Pop();
                    currentVal = values.Pop() - secondNum;
                    values.Push(currentVal);
                }
            }


            return values.Pop();
        }




    }
}