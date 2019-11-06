using Microsoft.VisualStudio.TestTools.UnitTesting;
using NezarkaBookstore;
using System.IO;



namespace NezarkaTests
{
    [TestClass]
    public class InputCheckTests
    {
        [TestMethod]
        public void InputCorrect()
        {
            StreamReader s = new StreamReader(@"D:\MFF\ZS_2019\c#_repos\Nezarka\NezarkaTests\bin\Debug\netcoreapp3.0\Ins\Databaze.txt");
            ModelStore store = ModelStore.LoadFrom(s);
            Assert.IsNotNull(store);
           
        }
        [TestMethod]
        public void FlawedBegin()
        {
            StreamReader s = new StreamReader(@"D:\MFF\ZS_2019\c#_repos\Nezarka\NezarkaTests\bin\Debug\netcoreapp3.0\Ins\FlawedBegin.txt");
            ModelStore store = ModelStore.LoadFrom(s);
            Assert.IsNull(store);
        }
        [TestMethod]
        public void FlawedEnd()
        {
            StreamReader s = new StreamReader(@"D:\MFF\ZS_2019\c#_repos\Nezarka\NezarkaTests\bin\Debug\netcoreapp3.0\Ins\FlawedEnd.txt");
            ModelStore store = ModelStore.LoadFrom(s);
            Assert.IsNull(store);
            
        }
        [TestMethod]
        public void FlawedNumber()
        {
            StreamReader s = new StreamReader(@"D:\MFF\ZS_2019\c#_repos\Nezarka\NezarkaTests\bin\Debug\netcoreapp3.0\Ins\FlawedNumber.txt");
            ModelStore store = ModelStore.LoadFrom(s);
            Assert.IsNull(store);
        }
        [TestMethod]
        public void FlawedID()
        {
            StreamReader s = new StreamReader(@"D:\MFF\ZS_2019\c#_repos\Nezarka\NezarkaTests\bin\Debug\netcoreapp3.0\Ins\FlawedID.txt");
            ModelStore store = ModelStore.LoadFrom(s);
            Assert.IsNull(store);
        }
        [TestMethod]
        public void FlawedBookID()
        {
            StreamReader s = new StreamReader(@"D:\MFF\ZS_2019\c#_repos\Nezarka\NezarkaTests\bin\Debug\netcoreapp3.0\Ins\FlawedBookID.txt");
            ModelStore store = ModelStore.LoadFrom(s);
            Assert.IsNull(store);
        }
        [TestMethod]
        public void FlawedLenghtOfLIne()
        {
            StreamReader s = new StreamReader(@"D:\MFF\ZS_2019\c#_repos\Nezarka\NezarkaTests\bin\Debug\netcoreapp3.0\Ins\FlawedLenghtOfLine.txt");
            ModelStore store = ModelStore.LoadFrom(s);
            Assert.IsNull(store);
        }
    }

}

