using System;
using System.Collections.Generic;
using System.Linq;
using cryptopals.Lib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cryptopals.Test.Extensions
{
    [TestClass]
    public class ArrayExtensionsTest
    {
        [TestMethod]
        public void TestChunkOneItem()
        {
            var array = new [] { 1 };
            var actual = array.Chunks(1);
            Assert.AreEqual(1, actual.Count);
            CollectionAssert.AreEqual(array, actual.First());
        }

        [TestMethod]
        public void TestSingleChunkSmallerThanChunkSize()
        {
            var array = new[] { 1 };
            var actual = array.Chunks(2);
            Assert.AreEqual(1, actual.Count);
            CollectionAssert.AreEqual(array, actual.First());


            array = new[] { 1, 2, 3, 4 };
            actual = array.Chunks(5);
            Assert.AreEqual(1, actual.Count);
            CollectionAssert.AreEqual(array, actual.First());
        }

        [TestMethod]
        public void TestChunkSizeOne()
        {
            var array = new[] { 1, 2, 3 };
            var expected = new List<int[]>
            {
                new[] { 1 },
                new[] { 2 },
                new[] { 3 }
            };
            var actual = array.Chunks(1);
            Assert.AreEqual(3, actual.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                CollectionAssert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void TestChunksEven()
        {
            var array = new[] { 1, 2, 3, 4, 5, 6 };
            var expected = new List<int[]>
            {
                new[] { 1, 2 },
                new[] { 3, 4 },
                new[] { 5, 6 }
            };
            var actual = array.Chunks(2);
            Assert.AreEqual(3, actual.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                CollectionAssert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void TestChunks()
        {
            var array = new[] { 1, 2, 3, 4, 5, 6, 7 };
            var expected = new List<int[]>
            {
                new[] { 1, 2, 3 },
                new[] { 4, 5, 6 },
                new[] { 7 }
            };
            var actual = array.Chunks(3);
            Assert.AreEqual(3, actual.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                CollectionAssert.AreEqual(expected[i], actual[i]);
            }

            expected = new List<int[]>
            {
                new[] { 1, 2, 3, 4 },
                new[] { 5, 6, 7}
            };
            actual = array.Chunks(4);
            Assert.AreEqual(2, actual.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                CollectionAssert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void TestChunksEmpty()
        {
            var array = new int[] { };
            var actual = array.Chunks(1);
            Assert.AreEqual(0, actual.Count);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestChunkSizeZero()
        {
            var array = new[] { 1 };
            array.Chunks(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestChunkSizeNegative()
        {
            var array = new[] { 1 };
            array.Chunks(-1);
        }
    }
}
