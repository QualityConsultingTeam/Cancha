using System;
using Access;
using Access.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.RepositoryTest
{
    [TestClass]
    public class FieldsTest
    {
        [TestMethod]
        public void TestFullSearch()
        {
            var context = AccessContext.Create();

            var repository = new FieldsRepository {Context = context};

            var fullSearch = repository.FullSearchAsync(new FilterOptionModel
            {
                lat = "13.720858499999999",
                lon = "-89.2304515",

            }).Result;

            Assert.IsNotNull(fullSearch);
        }
    }
}
