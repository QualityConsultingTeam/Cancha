using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core;
using System.Data.Entity.Spatial;
using System.Data.Entity.Validation;
using System.Linq;
using Access;
using Access.Models;
using Access.Repositories;
using Admin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SeedRoles()
        {
            //var result = ApplicationUserManager.SeedRoles().Result;

            //Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetLocationTest()
        {
            double lat =  13.684752;
            double lon = -89.231373;
            var pointString = string.Format("POINT({0} {1})", lon, lat);

            var point = DbGeography.FromText(pointString);

            var context = AccessContext.Create();

            var cancha = context.Centers.FirstOrDefault(t=>t.Id == 10);

            if (cancha != null)
            {
                cancha.Coordinates = point;
            }

            //context.SaveChanges();
        }

        [TestMethod]
        public void TestFixBookingsEndDates()
        {
            var context = AccessContext.Create();

            var bookings = context.Bookings.ToList();
            var start = new DateTime(2015, 08, 09, 15, 00, 00, DateTimeKind.Utc);
            var end = start.AddHours(1);
            
            foreach(var booking in bookings)
            {
                booking.Start = start;
                booking.End = end;
            }

            context.SaveChanges();
        }

        [TestMethod]
        public void TestSearchEngine()
        {
            var repo = new FieldsRepository() {Context = AccessContext.Create()};
            var fields = repo.FullSearchAsync(new FilterOptionModel()
            {
                date = new DateTime(2015, 7, 10, 11, 0, 0),
                end = new DateTime(2015, 7, 11, 11, 0, 0),
                keywords = "",

            }).Result;

            Assert.IsNotNull(fields);
        }

        [TestMethod]
        public void InsertTestprices()
        {

            var Context = AccessContext.Create();

            var canchas = Context.Fields.ToList();

            foreach (var cancha in canchas)
            {
                cancha.Cost = new Cost()
                {
                    Price = RandomPrice(),
                    UserSign = cancha.UserSign,
                    IdCancha = cancha.Id,
                };
            }

            
              
            try
            {

                    Context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                
            }

        }

        private int RandomPrice()
        {
            return new Random().Next(20, 30);
        }

        [TestMethod]
        public void TimezoneTest()
        {

            var date = DateTime.Now;

            var universal = date.ToUniversalTime();
            var utc = universal.ToLocalTime();

            
        }


    }
}
