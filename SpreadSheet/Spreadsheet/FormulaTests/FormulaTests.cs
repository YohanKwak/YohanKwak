using SpreadsheetUtilities;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {

        /// <summary>
        /// Testing empty token on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest1()
        {
            new Formula("   ");
        }

        /// <summary>
        /// Testing empty token on constructor 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest2()
        {
            new Formula("");

        }

        /// <summary>
        /// Testing wrong parenthesis on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest3()
        {
            new Formula("1+ 2+ 3) + 5");
        }

        /// <summary>
        /// Testing wrong parenthesis on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest4()
        {
            new Formula("1+ ((2+ 3) + 5");

        }

        /// <summary>
        /// Testing wrong first token on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest5()
        {
            new Formula("+ 2+ 3 + 5");
        }

        /// <summary>
        /// Testing wrong first token on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest6()
        {
            new Formula(") 2+ 3 + 5");
        }

        /// <summary>
        /// Testing wrong first token on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest7()
        {
            new Formula("/ 2+ 3 + 5");
        }

        /// <summary>
        /// Testing wrong first token on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest8()
        {
            new Formula("* 2+ 3 + 5");
        }

        /// <summary>
        /// Testing wrong first token on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest9()
        {
            new Formula("- 2+ 3 + 5");
        }

        /// <summary>
        /// Testing wrong last token on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest10()
        {
            new Formula("1+ 2+ 3 + 5 +");
        }

        /// <summary>
        /// Testing wrong last token on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest11()
        {
            new Formula("1+ 2+ 3 + 5-");
        }

        /// <summary>
        /// Testing wrong last token on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest12()
        {
            new Formula("1+ 2+ 3 + 5(");
        }

        /// <summary>
        /// Testing wrong last token on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest13()
        {
            new Formula("1+ 2+ 3 + 5*");
        }

        /// <summary>
        /// Testing wrong last token on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest14()
        {
            new Formula("1+ 2+ 3 + 5/");
        }

        /// <summary>
        /// Testing wrong format on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest15()
        {
            new Formula("1+ 2+ 3 +* 5");
        }

        /// <summary>
        /// Testing wrong format on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest16()
        {
            new Formula("1+ 2+ 3 +/ 5");

        }

        /// <summary>
        /// Testing wrong format on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest17()
        {
            new Formula("1+ 2+ 3 ++ 5");

        }

        /// <summary>
        /// Testing wrong format on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest18()
        {
            new Formula("1+ 2+ 3 +- 5");
        }

        /// <summary>
        /// Testing wrong format on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest19()
        {
            new Formula("1+ (-2+ 3) + 5");

        }

        /// <summary>
        /// Testing wrong format on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest20()
        {
            new Formula("1+ (2 2 + 3) + 5");

        }

        /// <summary>
        /// Testing wrong format on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest21()
        {
            new Formula("1+ (A2 A2 + 3) + 5");

        }

        /// <summary>
        /// Testing wrong format on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest22()
        {
            new Formula("1+ (-2+ 3)6 + 5");

        }

        /// <summary>
        /// Testing wrong format on Constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest23()
        {
            new Formula("1+ 2( + 36 + 5");

        }

        /// <summary>
        /// Testing wrong format on Constructor with illegal variable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest24()
        {
            new Formula("1+ 2( + 36 + B", allUpperCase, correctVariable);

        }

        /// <summary>
        /// Testing wrong format on Constructor with illegal variable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest25()
        {
            new Formula("B+ 2( + 36 + 1", allUpperCase, correctVariable);

        }

        /// <summary>
        /// Testing wrong format on Constructor with illegal variable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void WrongFormatTest26()
        {
            new Formula("1+ B( + 36 + 1", allUpperCase, correctVariable);

        }


        /// <summary>
        /// Testing divided by 0 on Evaluate
        /// </summary>
        [TestMethod]
        public void EvaluateFailTest1()
        {
            Formula f = new Formula("1+ 2 / (0 + 1 - 1)");
            Assert.IsTrue(f.Evaluate(LookLook) is FormulaError);
        }

        /// <summary>
        /// Testing undefined variable on Evaluate
        /// </summary>
        [TestMethod]
        public void EvaluateFailTest2()
        {
            Formula f = new Formula("1+ 2 / (0 + V23)");
            Assert.IsTrue(f.Evaluate(LookLook) is FormulaError);
        }

        /// <summary>
        /// Testing divided by 0 on Evaluate
        /// </summary>
        [TestMethod]
        public void EvaluateFailTest3()
        {
            Formula f = new Formula("1+ 2 / 0");
            Assert.IsTrue(f.Evaluate(LookLook) is FormulaError);
        }

        /// <summary>
        /// Testing divided by 0 on Evaluate
        /// </summary>
        [TestMethod]
        public void EvaluateFailTest4()
        {
            Formula f = new Formula("1+ 2 / Z1");
            Assert.IsTrue(f.Evaluate(LookLook) is FormulaError);
        }

        /// <summary>
        /// Testing ToString with working formulas (basic test for constructors are tested above)
        /// </summary>
        [TestMethod]
        public void TestConstructorAndToString()
        {
            Formula f = new Formula("1+ 2+ 3 + 5");
            Assert.AreEqual(f.ToString(), "1+2+3+5");
            f = new Formula(" 1+ 2+ 3 + 5");
            Assert.AreEqual(f.ToString(), "1+2+3+5");
            f = new Formula("1 + 2+ 3 + 5");
            Assert.AreEqual(f.ToString(), "1+2+3+5");
            f = new Formula("     1     + 2+ 3 + 5    ");
            Assert.AreEqual(f.ToString(), "1+2+3+5");
            f = new Formula("1     + 2+ 3 + 5");
            Assert.AreEqual(f.ToString(), "1+2+3+5");
            f = new Formula("1  + (   2    + 3    ) + 5");
            Assert.AreEqual(f.ToString(), "1+(2+3)+5");
            f = new Formula("(1+ 2)+ 3 + 5");

        }

        /// <summary>
        /// Test GetVariavles function
        /// </summary>
        [TestMethod]
        public void TestGetVariables()
        {
            Formula f = new Formula("1+ 2 + a2 * vdse2 / sdfwe2 * (fdfs23 + 3) + 5 + a2");
            Assert.AreEqual(f.GetVariables().Count(), 4);
            Assert.IsTrue(f.GetVariables().Contains("a2"));
            Assert.IsTrue(f.GetVariables().Contains("vdse2"));
            Assert.IsTrue(f.GetVariables().Contains("sdfwe2"));
            Assert.IsTrue(f.GetVariables().Contains("fdfs23"));

            f = new Formula("1+ 2 + a2 * vdse2 / sdfwe2 * (fdfs23 + 3) + 5 + a2 + A2");
            Assert.AreEqual(f.GetVariables().Count(), 5);
            Assert.IsTrue(f.GetVariables().Contains("a2"));
            Assert.IsTrue(f.GetVariables().Contains("A2"));
            Assert.IsTrue(f.GetVariables().Contains("vdse2"));
            Assert.IsTrue(f.GetVariables().Contains("sdfwe2"));
            Assert.IsTrue(f.GetVariables().Contains("fdfs23"));
        }

        /// <summary>
        /// Test Equals functions
        /// </summary>
        [TestMethod]
        public void TestEquals()
        {
            Formula f = new Formula("1+ 2 + a2 * vdse2 / sdfwe2 * (fdfs23 + 3) + 5 + a2");
            Formula f2 = new Formula("1+              2 + a2               * vdse2 / sdfwe2 * (fdfs23    + 3) + 5 + a2");
            Assert.AreEqual(f, f2);
            Assert.IsFalse(new Formula("x1+y2").Equals(new Formula("X1+Y2")));
            Assert.IsFalse(new Formula("x1+y2").Equals(new Formula("y2+x1")));
            Assert.IsFalse(new Formula("x1+y2").Equals("x1+y2"));
            Assert.IsTrue(new Formula("x1+y2", allUpperCase, s => true).Equals(new Formula("X1  +  Y2")));
            Assert.AreEqual(new Formula("2.0 + x7"), (new Formula("2.000 + x7")));
        }

        /// <summary>
        /// test == and != operator
        /// </summary>
        [TestMethod]
        public void TestOperators()
        {
            Formula f = new Formula("1+ 2 + a2 * vdse2 / sdfwe2 * (fdfs23 + 3) + 5 + a2");
            Formula f2 = new Formula("1+              2 + a2               * vdse2 / sdfwe2 * (fdfs23    + 3) + 5 + a2");
            Assert.IsTrue(f == f2);
            Assert.IsFalse(f != f2);
        }

        /// <summary>
        ///  test GetHashCode function
        /// </summary>
        [TestMethod]
        public void TestHashCode()
        {
            Formula f = new Formula("1+ 2 + a2 * vdse2 / sdfwe2 * (fdfs23 + 3) + 5 + a2");
            Formula f2 = new Formula("1+              2 + a2               * vdse2 / sdfwe2 * (fdfs23    + 3) + 5 + a2");
            Formula f3 = new Formula("1+              2 + a6               * vdse2 / sdfwe2 * (fdfs23    + 3) + 5 + a3");
            Assert.AreEqual(f.GetHashCode(), f2.GetHashCode());
            Assert.AreNotEqual(f.GetHashCode(), f3.GetHashCode());
        }

        /// <summary>
        /// test Evaluate function
        /// </summary>
        [TestMethod]
        public void TestEvaluate1()
        {
            Formula f = new Formula("1+ 2 + 3 / 1 +(( 3 + 2) / 5 ) -2");
            Assert.IsTrue(5.0 == (double)f.Evaluate(LookLook));
        }

        /// <summary>
        /// testing evaluate function 2
        /// </summary>
        [TestMethod]
        public void TestEvaluate2()
        {
            Formula f = new Formula("a1 + b1 + c1 + d1+ e1", allUpperCase, s => true);
            Assert.IsTrue(15 == (double)f.Evaluate(LookLook));
        }

        /// <summary>
        /// testing evaluate function 3
        /// </summary>
        [TestMethod]
        public void TestEvaluate3()
        {
            Formula f = new Formula("1 - 2 + 3");
            Assert.IsTrue(2.0 == (double)f.Evaluate(LookLook));
        }

        /// <summary>
        /// testing evaluate function 4
        /// </summary>
        [TestMethod]
        public void TestEvaluate4()
        {
            Formula f = new Formula("1+ 2 + (2+ 3 * 4)");
            Assert.IsTrue(17.0 == (double)f.Evaluate(LookLook));
        }

        /// <summary>
        /// testing evaluate function 5
        /// </summary>
        [TestMethod]
        public void TestEvaluate5()
        {
            Formula f = new Formula("1+ 2 * (2 - 1)");
            Assert.IsTrue(3.0 == (double)f.Evaluate(LookLook));
        }

        /// <summary>
        /// testing evaluate function 6
        /// </summary>
        [TestMethod]
        public void TestEvaluate6()
        {
            Formula f = new Formula("1+ 2 / (0 + 1)");
            Assert.IsTrue(3.0 == (double)f.Evaluate(LookLook));
        }

        /// <summary>
        /// testing evaluate function 7
        /// </summary>
        [TestMethod]
        public void TestEvaluate7()
        {
            Formula f = new Formula("1+ 2 / (0.0 + 1.0 / 2.0)");
            Assert.IsTrue(5.0 == (double)f.Evaluate(LookLook));
        }

        /// <summary>
        /// testing evaluate function 8
        /// </summary>
        [TestMethod]
        public void TestEvaluate8()
        {
            Formula f = new Formula("1+ 2 / (1 / 2) + 3");
            Assert.IsTrue(8.0 == (double)f.Evaluate(LookLook));
        }

        /// <summary>
        /// testing evaluate function 9
        /// </summary>
        [TestMethod]
        public void TestEvaluate9()
        {
            Formula f = new Formula("1+ 2 / (1 / B1) + 3");
            Assert.IsTrue(8.0 == (double)f.Evaluate(LookLook));
        }

        /// <summary>
        /// testing evaluate function 10
        /// </summary>
        [TestMethod]
        public void TestEvaluate10()
        {
            Formula f = new Formula("1+ 2 / (1 * B1) + 3");
            Assert.IsTrue(5.0 == (double)f.Evaluate(LookLook));
        }



        /// <summary>
        /// Makes all the alphabet in string s to uppercase
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public String allUpperCase(String s)
        {
            String newString = "";

            foreach (char c in s)
            {
                if (char.IsLetter(c))
                {
                    newString += char.ToUpper(c);
                }
                else
                {
                    newString += c;
                }
            }

            return newString;
        }

        /// <summary>
        /// variable looking up function for testing
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public double LookLook(string token)
        {
            if (token == "A1")
            {
                return 1;
            }
            if (token == "B1")
            {
                return 2;
            }
            if (token == "C1")
            {
                return 3;
            }
            if (token == "D1")
            {
                return 4;
            }
            if (token == "E1")
            {
                return 5;
            }

            if (token == "Z1")
            {
                return 0;
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Test validator for testing returns false if s is not "A"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool correctVariable(String s)
        {
            return s.Equals("A");
        }
    }


}