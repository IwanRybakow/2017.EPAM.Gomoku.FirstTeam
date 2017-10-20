using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestInfrastructure
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<int[]> list = new List<int[]>();
            list.Add(new int[] { 0, 0 });
            list.Add(new int[] { 0, 1 });
            list.Add(new int[] { 1, 1 });
            list.Add(new int[] { 0, 2 });
            list.Add(new int[] { 0, 3 });
            list.Add(new int[] { 1, 1 });
            list.Add(new int[] { 2, 4 });
            list.RemoveAll(x => (x[0] == 1 && x[1] == 1));
        }
    }
}
