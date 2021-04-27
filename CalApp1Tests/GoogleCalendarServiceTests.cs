using System;
using System.Collections.Generic;
using CalApp1.Services;
using NUnit.Framework;

namespace CalApp1Tests
{

    public class GoogleCalendarServiceTests
    {
        private GoogleCalendarService _googleCalendarService;
        private static DateTime _DateStartExample;
        private static DateTime _DateEndExample;
        
        [SetUp]
        public void Setup()
        {
            _googleCalendarService = new GoogleCalendarService();
            _DateStartExample = new DateTime(2021, 01, 13, 15, 30, 0);
            _DateEndExample = new DateTime(2021, 01, 14, 15, 30, 0);

        }

        public static IEnumerable<TestCaseData> InsertArgumentsProvider()
        {
            var DateStart = new DateTime(2021, 01, 13, 15, 30, 0);
            var DateEnd = new DateTime(2021, 01, 14, 15, 30, 0);

            yield return new TestCaseData(null, null, null, null);
            yield return new TestCaseData("Lala", null, DateStart, DateEnd);
            yield return new TestCaseData(null, "Lala", DateStart, DateEnd);
            

        }

        public static IEnumerable<TestCaseData> InsertReccuringArgumentsProvider()
        {
            var DateStart = new DateTime(2021, 01, 13, 15, 30, 0);
            var DateEnd = new DateTime(2021, 01, 14, 15, 30, 0);

            yield return new TestCaseData(null, null, null, null, null, null);
            yield return new TestCaseData("Lala", null, DateStart, DateEnd, "DAILY", 2);
            yield return new TestCaseData(null, "Lala", DateStart, DateEnd, "DAILY", 2);
            yield return new TestCaseData("Lala", "Lala", DateStart, DateEnd, "DAILY", null);
            yield return new TestCaseData("Lala", "Lala", DateStart, DateEnd, null, 2);
            
        }

        [Test, TestCaseSource("InsertArgumentsProvider")]
        public void InsertEventNullError_test(string Name, string Description, DateTime DateStart, DateTime DateEnd)
        {
            Assert.AreEqual(false, _googleCalendarService.InsertEvent(Name, Description, DateStart, DateEnd));
        }

        [TestCase("Lala", "Lala") ]
        public void InsertEvent_test(string Name, string Description)
        {
            var DateStart = _DateStartExample;
            var DateEnd = _DateEndExample;
            Assert.AreEqual(true, _googleCalendarService.InsertEvent(Name, Description, DateStart, DateEnd));
        }

        [TestCase("Lala", "Lala")]
        public void InsertEvent_DateComparisonTest(string Name, string Description)
        {
            var DateStart = _DateEndExample;
            var DateEnd = _DateStartExample;
            Assert.AreEqual(false, _googleCalendarService.InsertEvent(Name, Description, DateStart, DateEnd));
        }

        [TestCase("Lala", "Lala")]
        public void InsertEvent_DatesAreEqual(string Name, string Description)
        {
            var DateStart = _DateStartExample;
            var DateEnd = _DateStartExample;
            Assert.AreEqual(false, _googleCalendarService.InsertEvent(Name, Description, DateStart, DateEnd));
        }

        [Test, TestCaseSource("InsertReccuringArgumentsProvider")]
        public void InsertReccuringEventNullError_test(string Name, string Description, DateTime DateStart,
            DateTime DateEnd, string HowOften, int HowMany)
        {
            Assert.AreEqual(false, _googleCalendarService.InsertReccuringEvent(Name, Description, DateStart,
                DateEnd, HowOften, HowMany ));
        }

        public static IEnumerable<TestCaseData> InsertReccuringHowOftenCorrectArgumentsProvider()
        {

            yield return new TestCaseData("Lala", "Lala",  "WEEKLY", 2);
            yield return new TestCaseData("Lala", "Lala", "DAILY", 2);
            yield return new TestCaseData("Lala", "Lala",  "YEARLY", 2);
            yield return new TestCaseData("Lala", "Lala",  "MONTHLY", 2);

        }
        [Test, TestCaseSource("InsertReccuringHowOftenCorrectArgumentsProvider")]
        public void InsertRecurringEvent_HowOftenCorrecttest(string Name,  string Description,
            string HowOften, int HowMany)
        {
            var DateStart = _DateStartExample;
            var DateEnd = _DateEndExample;

            Assert.AreEqual(true, _googleCalendarService.InsertReccuringEvent(Name, Description, DateStart,
                DateEnd, HowOften, HowMany));
        }

        [TestCase("Lala", "Lala", "Wrongstring", 2)]
        public void InsertRecurringEvent_HowOftenWrongtest(string Name, string Description,  string HowOften,
            int HowMany)
        {
            var DateStart = _DateStartExample;
            var DateEnd = _DateEndExample;
            Assert.AreEqual(false, _googleCalendarService.InsertReccuringEvent(Name, Description, DateStart,
                DateEnd, HowOften, HowMany));
        }

        [TestCase("Lala", "Lala", "DAILY", 2)]
        public void InsertRecurringEvent_HowManyCorrecttest(string Name, string Description, string HowOften,
            int HowMany)
        {
            var DateStart = _DateStartExample;
            var DateEnd = _DateEndExample;
            Assert.AreEqual(true, _googleCalendarService.InsertReccuringEvent(Name, Description, DateStart,
                DateEnd, HowOften, HowMany));
        }


        [TestCase("Lala", "Lala", "DAILY", 0)]
        [TestCase("Lala", "Lala", "DAILY", -5)]
        public void InsertRecurringEvent_HowManyWrongtest(string Name, string Description, string HowOften,
            int HowMany)
        {
            var DateStart = _DateStartExample;
            var DateEnd = _DateEndExample;
            Assert.AreEqual(false, _googleCalendarService.InsertReccuringEvent(Name, Description, DateStart,
                DateEnd, HowOften, HowMany));
        }

        [TestCase("Lala", "Lala", "DAILY", 2)]
        public void InsertRecurringEvent_DateComparisonTest(string Name, string Description, string HowOften,
            int HowMany)
        {
            var DateStart = _DateEndExample;
            var DateEnd = _DateStartExample;
            Assert.AreEqual(false, _googleCalendarService.InsertReccuringEvent(Name, Description, DateStart,
                DateEnd, HowOften, HowMany));
        }

        [TestCase("Lala", "Lala", "DAILY", 2)]
        public void InsertRecurringEvent_DatesAreEqual(string Name, string Description, string HowOften,
            int HowMany)
        {
            var DateStart = _DateStartExample;
            var DateEnd = _DateStartExample;
            Assert.AreEqual(false, _googleCalendarService.InsertReccuringEvent(Name, Description, DateStart,
                DateEnd, HowOften, HowMany));
        }
        
    }
}
