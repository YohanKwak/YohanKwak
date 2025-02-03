// Dependency Graph that keeps track of dependency relationship between given elements.
// Written by Yohan Kwak, Last fixed on 09/23/2022


using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<String, List<String>> dependents;
        private Dictionary<String, List<String>> dependees;
        private int size;



        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<String, List<String>>();
            dependees = new Dictionary<String, List<String>>();
            size = 0;
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {

            get
            {
                dependees.TryGetValue(s, out List<String>? currentdependees);
                if (currentdependees != null)
                {
                    return currentdependees.Count;
                }
                else return 0;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (dependents.TryGetValue(s, out List<String>? l)) {
                return l.Count != 0;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (dependees.TryGetValue(s, out List<String>? l)) {
                return l.Count != 0;
            }
            else {
                 return false;
            }
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {

            dependents.TryGetValue(s, out List<string>? currentdependents);

            if (currentdependents == null)
            {
                currentdependents = new List<string>();
            }


            return currentdependents;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {

            dependees.TryGetValue(s, out List<string>? currentdependees);

            if (currentdependees == null)
            {
                currentdependees = new List<string>();
            }

            return currentdependees;
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {

            if (dependents.TryGetValue(s, out List<String>? currentdependees))
            {
                if (currentdependees.Contains(t))
                {
                    return;
                }
                
            }
            else
            {
                currentdependees = new List<string>();
                dependents.Add(s, currentdependees);
            }
            currentdependees.Add(t);
            if (dependees.TryGetValue(t, out List<String>? currentdependents))
            {
                currentdependents.Add(s);
            }
            else
            {
                currentdependents = new List<string>();
                currentdependents.Add(s);
                dependees.Add(t, currentdependents);
            }
            size++;

        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            if (dependents.TryGetValue(s, out List<String>? currentdependees))
            {
                if (currentdependees.Remove(t))
                {
                    size--;
                }
            }

            if (dependees.TryGetValue(t, out List<String>? currentdependents))
            {
                currentdependents.Remove(s);
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {

            List<String> currentDependents = GetDependents(s).ToList();
            foreach (string t in currentDependents)
            {
                RemoveDependency(s, t);
            }


            foreach (string t in newDependents)
            {
                AddDependency(s, t);
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            List<String> currentDependees = GetDependees(s).ToList();

            foreach (string t in currentDependees)
            {
                RemoveDependency(t, s);
            }

            foreach (string t in newDependees)
            {
                AddDependency(t, s);
            }
        }

    }

}
