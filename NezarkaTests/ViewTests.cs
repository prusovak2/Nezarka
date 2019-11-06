using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NezarkaBookstore;
using System.IO;

namespace NezarkaTests
{
    [TestClass]
    public class ViewTests
    {
        [TestMethod]
        public void GenFirstHeadTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\FirtsHeader.html");
            View.GenFirstHead(sw);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\FirtsHeader.html", @"TestFiles\FirstHeader.txt");
            Assert.IsTrue(b);           
        }
        [TestMethod]
        public void GenInvalidRequestTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\InvalidRequest.html");
            View.GenInvalidRequest(sw);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\InvalidRequest.html", @"TestFiles\InvalidRequest.txt");
            Assert.IsTrue(b);
        }
        [TestMethod]
        public void GenStyleTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\Style.html");
            View.GenStyle(sw);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\Style.html", @"TestFiles\Style.txt");
            Assert.IsTrue(b);
        }
        [TestMethod]
        public void GenCommonHeaderTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\CommonHeader.html");
            View.GenCommonHeader(sw, "Kachna", 42);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\CommonHeader.html", @"TestFiles\CommonHeader.txt");
            Assert.IsTrue(b);
        }
        [TestMethod]
        public void GenBookInfoBodyTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\BookInfoBody.html");
            View.GenBookInfoBody(sw, "HP", "JKR", (decimal)4.2);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\BookInfoBody.html", @"TestFiles\BookInfoBody.txt");
            Assert.IsTrue(b);
        }
        [TestMethod]
        public void DecimalConvert()
        {
            decimal d = (decimal)2.1;
            string A = d.ToString(View.StdCulture);
            string B = "2.1";
            Assert.IsTrue(Utils.Diff(A, B));
        }
        [TestMethod]
        public void GenBookInfoTests()
        {
            StreamWriter sw = new StreamWriter(@"TestFiles\Outs\BookInfo.html");
            View.GenBookDetail(sw, "Kachna", 42, "HP", "JKR", (decimal)4.2);
            sw.Close();
            bool b = Utils.FileDiff(@"TestFiles\Outs\BookInfo.html", @"TestFiles\BookInfo.txt");
            Assert.IsTrue(b);
        }
    }
}
