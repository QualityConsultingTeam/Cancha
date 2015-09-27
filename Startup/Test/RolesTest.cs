﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Admin.Models;
using Access;
using Admin;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Access.Extensions;
using Identity.Models;
using Identity.Context;
using Identity;
using Identity.Config;

namespace Test
{
    /// <summary>
    /// Summary description for RolesTest
    /// </summary>
    [TestClass]
    public class RolesTest
    {
        public RolesTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
            var context = AccessContext.Create();
            var identityContext = ApplicationDbContext.Create();
            var service = new IdentityManagerService(identityContext);

            var users = service.GetUsersAsync(new Access.Models.FilterOptionModel(), context).Result;

            var center = context.Centers.FirstOrDefault();

            var user = users[1];

            user.CenterId = center.Id;

            var userStore = new UserStore<ApplicationUser>(identityContext);
            var manager = new ApplicationUserManager(userStore);

            var model = new IdentityUserViewModel();
            model.Assign(user);
            
             var result = service.InsertOrUpdate(model,manager).Result;

            Assert.IsNotNull(users);
            
        }

        [TestMethod]
        public void TestUserClaims()
        {
            var service = new IdentityManagerService(ApplicationDbContext.Create());

            
        }
            
    }
}
