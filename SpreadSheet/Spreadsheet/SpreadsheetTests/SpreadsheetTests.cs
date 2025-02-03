// SpreadsheetTests class that makes sure SpreadSheet class works appropriately.
// Written by Yohan Kwak, last fixed on 09/30/2022.
using SpreadsheetUtilities;
using System.Security.Cryptography;

namespace SS
{
    [TestClass]
    public class SpreadsheetTests
    {
        //Creating few variables to make writting testing easier
        //each cells value ends up in 1, 2, 3, 4, 5 respectively for a,b,c,d,e... so on.

        String a = "A1";
        string av = "1";

        String b = "B1";
        string bv = "=1+A1";

        String c = "C1";
        string cv = "=B1+A1";

        String d = "D1";
        string dv = "=1+2+1*1";

        String e = "E1";
        string ev = "=1+A1*C1+1";

        String f = "F1";
        string fv = "6";

        String g = "G1";
        string gv = "=B1+A1+C1+1";

        /// <summary>
        /// Testing calling methods with invalid variable name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void WrongNameTest1()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellValue("878cd231");
        }
        /// <summary>
        /// Testing calling methods with invalid variable name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void WrongNameTest2()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("8_");
        }
        /// <summary>
        /// Testing calling methods with invalid variable name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void WrongNameTest3()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("#fw");
        }
        /// <summary>
        /// Testing calling methods with invalid variable name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void WrongNameTest4()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(" ");
        }
        /// <summary>
        /// Testing calling methods with invalid variable name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void WrongNameTest5()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("ved***");
        }
        /// <summary>
        /// Testing calling methods with invalid variable name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void WrongNameTest6()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("win$#@cws98");
        }
        /// <summary>
        /// Testing calling methods with invalid variable name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void WrongNameTest7()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("");
        }
        /// <summary>
        /// Testing SetContent method.
        /// </summary>
        [TestMethod]
        public void TestGetSetContentVal1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(a, av);
            s.SetContentsOfCell(b, bv);
            s.SetContentsOfCell(c, cv);
            s.SetContentsOfCell(d, dv);
            s.SetContentsOfCell(e, ev);
            s.SetContentsOfCell(f, fv);
            s.SetContentsOfCell(g, gv);


            Assert.AreEqual((1.0).ToString(), s.GetCellValue(a).ToString());
            Assert.AreEqual((2.0).ToString(), s.GetCellValue(b).ToString());
            Assert.AreEqual((3.0).ToString(), s.GetCellValue(c).ToString());
            Assert.AreEqual((4.0).ToString(), s.GetCellValue(d).ToString());
            Assert.AreEqual((5.0).ToString(), s.GetCellValue(e).ToString());
            Assert.AreEqual((6.0).ToString(), s.GetCellValue(f).ToString());
            Assert.AreEqual((7.0).ToString(), s.GetCellValue(g).ToString());
            Assert.AreEqual("", s.GetCellValue("NO1").ToString());
            Assert.AreEqual("", s.GetCellContents("NO1").ToString());

        }

        /// <summary>
        /// Testing setContent method, when there are elements there already.
        /// </summary>
        [TestMethod]
        public void TestGetSetContent2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(a, av);
            s.SetContentsOfCell(b, bv);
            s.SetContentsOfCell(c, cv);
            s.SetContentsOfCell(d, dv);
            s.SetContentsOfCell(e, ev);
            s.SetContentsOfCell(f, fv);
            s.SetContentsOfCell(g, gv);

            s.SetContentsOfCell(a, "2");
            s.SetContentsOfCell(b, "3");
            s.SetContentsOfCell(c, "4");
            s.SetContentsOfCell(d, "=C1 + 1");
            s.SetContentsOfCell(e, "6");
            s.SetContentsOfCell(f, "7");
            s.SetContentsOfCell(g, "8");
            s.SetContentsOfCell("h3", "HI I AM STRING");


            Assert.AreEqual((2.0).ToString(), s.GetCellValue(a).ToString());
            Assert.AreEqual((3.0).ToString(), s.GetCellValue(b).ToString());
            Assert.AreEqual((4.0).ToString(), s.GetCellValue(c).ToString());
            Assert.AreEqual((5.0).ToString(), s.GetCellValue(d).ToString());
            Assert.AreEqual((6.0).ToString(), s.GetCellValue(e).ToString());
            Assert.AreEqual((7.0).ToString(), s.GetCellValue(f).ToString());
            Assert.AreEqual((8.0).ToString(), s.GetCellValue(g).ToString());
            Assert.AreEqual("HI I AM STRING", s.GetCellContents("h3").ToString());
            Assert.AreEqual("HI I AM STRING", s.GetCellValue("h3").ToString());

            s.SetContentsOfCell("h3", "HI I");

            Assert.AreEqual("HI I", s.GetCellContents("h3").ToString());

            s.SetContentsOfCell("h3", "");

            Assert.AreEqual("", s.GetCellValue("h3").ToString());
        }

        [TestMethod]
        public void TestGetSetContent3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(a, av);
            s.SetContentsOfCell(b, bv);
            s.SetContentsOfCell(c, cv);
            s.SetContentsOfCell(d, dv);
            s.SetContentsOfCell(e, ev);
            s.SetContentsOfCell(f, fv);
            s.SetContentsOfCell(g, gv);

            s.SetContentsOfCell(a, "2");
            s.SetContentsOfCell(b, "3");
            s.SetContentsOfCell(c, "4");
            s.SetContentsOfCell(d, "=C1 + 1");
            s.SetContentsOfCell(e, "6");
            s.SetContentsOfCell(f, "7");
            s.SetContentsOfCell(g, "8");
            s.SetContentsOfCell("h3", "HI I AM STRING");


            Assert.AreEqual((2.0).ToString(), s.GetCellContents(a).ToString());
            Assert.AreEqual((3.0).ToString(), s.GetCellContents(b).ToString());
            Assert.AreEqual((4.0).ToString(), s.GetCellContents(c).ToString());
            Assert.AreEqual(new Formula("C1 + 1"), s.GetCellContents(d));
            Assert.AreEqual((6.0).ToString(), s.GetCellContents(e).ToString());
            Assert.AreEqual((7.0).ToString(), s.GetCellContents(f).ToString());
            Assert.AreEqual((8.0).ToString(), s.GetCellContents(g).ToString());
            Assert.AreEqual("HI I AM STRING", s.GetCellContents("h3").ToString());
        }
            /// <summary>
            /// Testing setting and getting content using string input.
            /// </summary>
            [TestMethod]
        public void TestGetSetContentWithString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(a, "1");
            Assert.AreEqual("1", s.GetCellContents(a).ToString());
            s.SetContentsOfCell(a, "2");
            Assert.AreEqual("2", s.GetCellContents(a).ToString());
        }

        /// <summary>
        /// Testing wrong name handling of set methods
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void WrongNameForSetContent1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("w98b", "1.0");
        }
        /// <summary>
        /// Testing wrong name handling of set methods
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void WrongNameForSetContent2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("win$#@cws98", "fdfs");
        }
        /// <summary>
        /// Testing wrong name handling of set methods
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void WrongNameForSetContent3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("#@cws98", bv);
        }
        /// <summary>
        /// Testing GetNamesOfAllNonemptyCells method.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(a, av);
            s.SetContentsOfCell(b, bv);
            s.SetContentsOfCell(c, cv);
            s.SetContentsOfCell(d, dv);
            s.SetContentsOfCell(e, ev);
            s.SetContentsOfCell(f, fv);
            s.SetContentsOfCell(g, gv);

            Assert.IsTrue(s.GetNamesOfAllNonemptyCells().Count() == 7);
            Assert.IsTrue(s.GetNamesOfAllNonemptyCells().Contains(a));
            Assert.IsTrue(s.GetNamesOfAllNonemptyCells().Contains(b));
            Assert.IsTrue(s.GetNamesOfAllNonemptyCells().Contains(c));
            Assert.IsTrue(s.GetNamesOfAllNonemptyCells().Contains(d));
            Assert.IsTrue(s.GetNamesOfAllNonemptyCells().Contains(e));
            Assert.IsTrue(s.GetNamesOfAllNonemptyCells().Contains(f));
            Assert.IsTrue(s.GetNamesOfAllNonemptyCells().Contains(g));
        }

        /// <summary>
        /// Testing cells with undefined or inappropriate variable having FormulaError as its value.
        /// </summary>
        [TestMethod]
        public void TestUndefinedVariableandFormulaError()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(a, "=Vfe + eisj + __edc");
            s.SetContentsOfCell(b, "=Vfe + e + __edc");
            s.SetContentsOfCell(c, "=A1 + eisj + __edc");


            Assert.IsTrue(s.GetCellValue(a) is FormulaError);
            Assert.IsTrue(s.GetCellValue(b) is FormulaError);
            Assert.IsTrue(s.GetCellValue(c) is FormulaError);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CirculardependencyTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=a1+2");
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CirculardependencyTest2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=2+2");
            s.SetContentsOfCell("a1", "=a1+2");
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CirculardependencyTest3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=a3+2");
            s.SetContentsOfCell("a2", "=a1");
            s.SetContentsOfCell("a3", "=a2");
        }


    }
}