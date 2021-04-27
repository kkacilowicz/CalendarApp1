using System;
using CalApp1.Services;
using NUnit.Framework;

namespace CalApp1Tests
{

    public class GoogleCalendarServiceTests
    {
        private GoogleCalendarService _googleCalendarService;
        [SetUp]
        public void Setup()
        {
            _googleCalendarService = new GoogleCalendarService();
            //var DateStart = new DateTime(2021, 01, 13, 15, 30, 0);
            //var DateEnd = new DateTime(2021, 01, 14, 15, 30, 0);
        }

        [Test]
        public void GetCalendarId_test()
        {
            Assert.AreEqual(@"calapp553@gmail.com", _googleCalendarService.GetCalendarId());
        }
    }
}
