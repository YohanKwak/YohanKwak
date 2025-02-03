using SpreadsheetUtilities;

namespace DependencyGraphTests
{
    [TestClass]
    public class DependencyGraphTests
    {
        /// <summary>
        /// Checks the DependencyGraph Constructor
        /// </summary>
        [TestMethod]
        public void TestEmptyConstructor()
        {
            DependencyGraph g = new DependencyGraph();
            Assert.IsNotNull(g);
            Assert.AreEqual(0, g.Size);

        }

        /// <summary>
        /// checks adding dependency on empty DependencyGraph
        /// </summary>
        [TestMethod]
        public void TestAddDependencyOnEmptyGraph()
        {
            DependencyGraph g = new DependencyGraph();
            Assert.AreEqual(0, g.Size);
            g.AddDependency("1", "2");
            Assert.AreEqual(1, g.Size);
        }

        /// <summary>
        /// Testing this with any String on empty DependencyGraph
        /// </summary>
        [TestMethod]
        public void TestThisOnEmptyGraph()
        {
            DependencyGraph g = new DependencyGraph();
            Assert.AreEqual(0, g[""]);
        }

        /// <summary>
        /// Testing removing dependency on empty DependencyGraph (not making difference)
        /// </summary>
        [TestMethod]
        public void TestRemoveDepencencyOnEmptyGraph()
        {
            DependencyGraph g = new DependencyGraph();
            Assert.AreEqual(0, g.Size);
            g.RemoveDependency("1", "2");
            Assert.AreEqual(0, g.Size);
        }

        /// <summary>
        /// Testing HasDependents and HasHependees on empty DependencyGraph
        /// </summary>
        [TestMethod]
        public void TestHasOnEmpty()
        {
            DependencyGraph g = new DependencyGraph();
            Assert.IsFalse(g.HasDependents("1"));
            Assert.IsFalse(g.HasDependees("1"));

        }

        /// <summary>
        /// Testing GetDependees and GetDependents on empty DependencyGraph (returning empty IEnumerable)
        /// </summary>
        [TestMethod]
        public void TestGetOnEmpty()
        {
            DependencyGraph g = new DependencyGraph();
            Assert.IsTrue(g.GetDependees("1").ToList().Count == 0);
            Assert.IsTrue(g.GetDependents("1").ToList().Count == 0);
        }


        // Test on Regular Graph

        /// <summary>
        /// Testing AddDependency on small DependencyGraph
        /// </summary>
        [TestMethod]
        public void TestAddDepencencyOnGraph()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a", "b");
            g.AddDependency("c", "b");
            g.AddDependency("b", "d");
            g.AddDependency("b", "e");
            g.AddDependency("d", "f");
            g.AddDependency("e", "f");
            g.AddDependency("f", "g");
            g.AddDependency("f", "h");
            g.AddDependency("a", "d");
            g.AddDependency("d", "h");
            Assert.AreEqual(10, g.Size);
            //Checking no changes for the duplicates
            g.AddDependency("a", "b");
            g.AddDependency("c", "b");
            g.AddDependency("b", "d");
            g.AddDependency("b", "e");
            g.AddDependency("d", "f");
            g.AddDependency("e", "f");
            g.AddDependency("f", "g");
            g.AddDependency("f", "h");
            g.AddDependency("a", "d");
            g.AddDependency("d", "h");
            Assert.AreEqual(10, g.Size);
        }

        /// <summary>
        /// Testing This[] on small DependencyGraph
        /// </summary>
        [TestMethod]
        public void TestThisOnSmallGraph()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a", "b");
            g.AddDependency("c", "b");
            g.AddDependency("b", "d");
            g.AddDependency("b", "e");
            g.AddDependency("d", "f");
            g.AddDependency("e", "f");
            g.AddDependency("f", "g");
            g.AddDependency("f", "h");
            g.AddDependency("a", "d");
            g.AddDependency("d", "h");
            Assert.AreEqual(0, g["a"]);
            Assert.AreEqual(2, g["b"]);
            Assert.AreEqual(0, g["c"]);
            Assert.AreEqual(2, g["d"]);
            Assert.AreEqual(1, g["e"]);
            Assert.AreEqual(2, g["f"]);
            Assert.AreEqual(1, g["g"]);
            Assert.AreEqual(2, g["h"]);
        }

        /// <summary>
        /// Testing RemoveDependency on small DependencyGraph
        /// </summary>
        [TestMethod]
        public void TestRemoveDepencencyOnSmallGraph()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a", "b");
            g.AddDependency("c", "b");
            g.AddDependency("b", "d");
            g.AddDependency("b", "e");
            g.AddDependency("d", "f");
            g.AddDependency("e", "f");
            g.AddDependency("f", "g");
            g.AddDependency("f", "h");
            g.AddDependency("a", "d");
            g.AddDependency("d", "h");
            Assert.AreEqual(10, g.Size);
            g.RemoveDependency("1", "2");
            Assert.AreEqual(10, g.Size);
            g.RemoveDependency("a", "b");
            Assert.AreEqual(9, g.Size);
            g.RemoveDependency("a", "b");
            Assert.AreEqual(9, g.Size);
            g.RemoveDependency("c", "b");
            Assert.AreEqual(8, g.Size);
            g.RemoveDependency("b", "d");
            Assert.AreEqual(7, g.Size);
            g.RemoveDependency("b", "e");
            Assert.AreEqual(6, g.Size);
            g.RemoveDependency("d", "f");
            Assert.AreEqual(5, g.Size);
            g.RemoveDependency("e", "f");
            Assert.AreEqual(4, g.Size);
            g.RemoveDependency("d", "nh");
            Assert.AreEqual(4, g.Size);
            g.RemoveDependency("f", "g");
            Assert.AreEqual(3, g.Size);
            g.RemoveDependency("f", "h");
            Assert.AreEqual(2, g.Size);
            g.RemoveDependency("a", "d");
            Assert.AreEqual(1, g.Size);
            g.RemoveDependency("d", "h");
            Assert.AreEqual(0, g.Size);
        }

        /// <summary>
        /// Testing HasDependents and HasDependees on small DependencyGraph
        /// </summary>
        [TestMethod]
        public void TestHasOnSmallGraph()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a", "b");
            g.AddDependency("c", "b");
            g.AddDependency("b", "d");
            g.AddDependency("b", "e");
            g.AddDependency("d", "f");
            g.AddDependency("e", "f");
            g.AddDependency("f", "g");
            g.AddDependency("f", "h");
            g.AddDependency("a", "d");
            g.AddDependency("d", "h");
            Assert.IsTrue(g.HasDependents("a"));
            Assert.IsFalse(g.HasDependees("a"));
            Assert.IsTrue(g.HasDependents("b"));
            Assert.IsTrue(g.HasDependees("b"));
            Assert.IsFalse(g.HasDependents("h"));
            Assert.IsTrue(g.HasDependees("h"));

        }

        /// <summary>
        ///  Testing GetDependents and GetDependees on small DependencyGraph
        /// </summary>
        [TestMethod]
        public void TestGetOnSmallGraph()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a", "b");
            g.AddDependency("c", "b");
            g.AddDependency("b", "d");
            g.AddDependency("b", "e");
            g.AddDependency("d", "f");
            g.AddDependency("e", "f");
            g.AddDependency("f", "g");
            g.AddDependency("f", "h");
            g.AddDependency("a", "d");
            g.AddDependency("d", "h");
            List<String> list = (List<String>)g.GetDependents("a");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("b"));
            Assert.IsTrue(list.Contains("d"));
            list = (List<String>)g.GetDependents("b");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("d"));
            Assert.IsTrue(list.Contains("e"));
            list = (List<String>)g.GetDependents("c");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("b"));
            list = (List<String>)g.GetDependents("d");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("f"));
            Assert.IsTrue(list.Contains("h"));
            list = (List<String>)g.GetDependents("e");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("f"));
            list = (List<String>)g.GetDependents("f");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("g"));
            Assert.IsTrue(list.Contains("h"));
            list = (List<String>)g.GetDependents("g");
            Assert.AreEqual(0, list.Count);
            list = (List<String>)g.GetDependents("h");
            Assert.AreEqual(0, list.Count);


            list = (List<String>)g.GetDependees("a");
            Assert.AreEqual(0, list.Count);
            list = (List<String>)g.GetDependees("b");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("a"));
            Assert.IsTrue(list.Contains("c"));
            list = (List<String>)g.GetDependees("c");
            Assert.AreEqual(0, list.Count);
            list = (List<String>)g.GetDependees("d");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("a"));
            Assert.IsTrue(list.Contains("b"));
            list = (List<String>)g.GetDependees("e");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("b"));
            list = (List<String>)g.GetDependees("f");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("d"));
            Assert.IsTrue(list.Contains("e"));
            list = (List<String>)g.GetDependees("g");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("f"));
            list = (List<String>)g.GetDependees("h");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("d"));
            Assert.IsTrue(list.Contains("f"));
        }

        /// <summary>
        ///  Testing ReplaceDependency on small DependencyGraph
        /// </summary>
        [TestMethod]
        public void TestReplaceOnGraph()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a", "b");
            g.AddDependency("c", "b");
            g.AddDependency("b", "d");
            g.AddDependency("b", "e");
            g.AddDependency("d", "f");
            g.AddDependency("e", "f");
            g.AddDependency("f", "g");
            g.AddDependency("f", "h");
            g.AddDependency("a", "d");
            g.AddDependency("d", "h");

            List<String> list = new List<string>();
            g.ReplaceDependents("fsdlkjflskdjsdlk", list);
            Assert.AreEqual(10, g.Size);

            list.Add("b");
            list.Add("c");
            list.Add("d");
            list.Add("e");
            list.Add("f");
            g.ReplaceDependents("a", list);
            Assert.AreEqual(13, g.Size);

            g.ReplaceDependees("a", list);
            Assert.AreEqual(18, g.Size);

            g.ReplaceDependents("fsdlkjflskdjsdlk", list);
            g.ReplaceDependees("fsdlkjflskdjsdlk", list);
            Assert.AreEqual(28, g.Size);


        }


    }
}