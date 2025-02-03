namespace FormulaChecker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            String testString7 = "4*2+3-3*4+9*(42-31)+3/3-4";

            int answer = 4 * 2 + 3 - 3 * 4 + 9 * (42 - 31) + 3 / 3 - 4;

            Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate(testString7, int.Parse));
            Console.WriteLine("Correct Answer is: " + answer);


            Console.Read();
        }
    }
}