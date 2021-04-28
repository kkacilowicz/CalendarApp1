using System;
using System.Collections.Generic;
using CalApp1.Services;
using NUnit.Framework;

namespace CalApp1Tests
{
    /// <summary>
    /// Class which Tests GoogleCalendarServiceClass
    /// </summary>
    public class GoogleCalendarServiceTests
    {
        /// <summary>
        /// It contains GoogleCalendarService object
        /// </summary>
        private GoogleCalendarService _googleCalendarService;
        /// <summary>
        ///  Date Start sample for tests
        /// </summary>
        private static DateTime _DateStartExample;
        /// <summary>
        ///  Date End sample for tests
        /// </summary>
        private static DateTime _DateEndExample;

        /// <summary>
        ///  Method to Initialize parameters for tests
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _googleCalendarService = new GoogleCalendarService();
            _DateStartExample = new DateTime(2021, 01, 13, 15, 30, 0);
            _DateEndExample = new DateTime(2021, 01, 14, 15, 30, 0);

        }

        /// <summary>
        ///  TestCaseData Collection to check function with null arguments
        /// </summary>
        public static IEnumerable<TestCaseData> InsertArgumentsProvider()
        {
            var DateStart = new DateTime(2021, 01, 13, 15, 30, 0);
            var DateEnd = new DateTime(2021, 01, 14, 15, 30, 0);

            yield return new TestCaseData(null, null, null, null);
            yield return new TestCaseData("Lala", null, DateStart, DateEnd);
            yield return new TestCaseData(null, "Lala", DateStart, DateEnd);
            

        }

        /// <summary>
        ///  TestCaseData Collection to check function with null arguments
        /// </summary>
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

        /// <summary>
        ///  Test to check function reaction to null arguments
        /// </summary>
        [Test, TestCaseSource("InsertArgumentsProvider")]
        public void InsertEventNullError_test(string Name, string Description, DateTime DateStart, DateTime DateEnd)
        {
            Assert.AreEqual(false, _googleCalendarService.InsertEvent(Name, Description, DateStart, DateEnd));
        }

        /// <summary>
        ///  Test to check function reaction if everything is fine
        /// </summary>
        [TestCase("Lala", "Lala") ]
        public void InsertEvent_test(string Name, string Description)
        {
            var DateStart = _DateStartExample;
            var DateEnd = _DateEndExample;
            Assert.AreEqual(true, _googleCalendarService.InsertEvent(Name, Description, DateStart, DateEnd));
        }
        /// <summary>
        ///  Test to check function reaction if Date End is later than Date Start
        /// </summary>
        [TestCase("Lala", "Lala")]
        public void InsertEvent_DateComparisonTest(string Name, string Description)
        {
            var DateStart = _DateEndExample;
            var DateEnd = _DateStartExample;
            Assert.AreEqual(false, _googleCalendarService.InsertEvent(Name, Description, DateStart, DateEnd));
        }
        /// <summary>
        ///  Test to check function reaction if Date End is equal to Date Start
        /// </summary>
        [TestCase("Lala", "Lala")]
        public void InsertEvent_DatesAreEqual(string Name, string Description)
        {
            var DateStart = _DateStartExample;
            var DateEnd = _DateStartExample;
            Assert.AreEqual(false, _googleCalendarService.InsertEvent(Name, Description, DateStart, DateEnd));
        }
        /// <summary>
        ///  Test to check function reaction to null arguments
        /// </summary>
        [Test, TestCaseSource("InsertReccuringArgumentsProvider")]
        public void InsertReccuringEventNullError_test(string Name, string Description, DateTime DateStart,
            DateTime DateEnd, string HowOften, int HowMany)
        {
            Assert.AreEqual(false, _googleCalendarService.InsertReccuringEvent(Name, Description, DateStart,
                DateEnd, HowOften, HowMany ));
        }

        /// <summary>
        ///  TestCaseData Collection to check function to proper reccurence arguments
        /// </summary>
        public static IEnumerable<TestCaseData> InsertReccuringHowOftenCorrectArgumentsProvider()
        {

            yield return new TestCaseData("Lala", "Lala",  "WEEKLY", 2);
            yield return new TestCaseData("Lala", "Lala", "DAILY", 2);
            yield return new TestCaseData("Lala", "Lala",  "YEARLY", 2);
            yield return new TestCaseData("Lala", "Lala",  "MONTHLY", 2);

        }
        /// <summary>
        ///  Test to check function reaction if there are proper reccuring arguments
        /// </summary>
        [Test, TestCaseSource("InsertReccuringHowOftenCorrectArgumentsProvider")]
        public void InsertRecurringEvent_HowOftenCorrecttest(string Name,  string Description,
            string HowOften, int HowMany)
        {
            var DateStart = _DateStartExample;
            var DateEnd = _DateEndExample;

            Assert.AreEqual(true, _googleCalendarService.InsertReccuringEvent(Name, Description, DateStart,
                DateEnd, HowOften, HowMany));
        }
        /// <summary>
        ///  Test to check function reaction if there are wrong reccuring arguments
        /// </summary>
        [TestCase("Lala", "Lala", "Wrongstring", 2)]
        public void InsertRecurringEvent_HowOftenWrongtest(string Name, string Description,  string HowOften,
            int HowMany)
        {
            var DateStart = _DateStartExample;
            var DateEnd = _DateEndExample;
            Assert.AreEqual(false, _googleCalendarService.InsertReccuringEvent(Name, Description, DateStart,
                DateEnd, HowOften, HowMany));
        }
        /// <summary>
        ///  Test to check function reaction if there are proper HowMany arguments
        /// </summary>
        [TestCase("Lala", "Lala", "DAILY", 2)]
        public void InsertRecurringEvent_HowManyCorrecttest(string Name, string Description, string HowOften,
            int HowMany)
        {
            var DateStart = _DateStartExample;
            var DateEnd = _DateEndExample;
            Assert.AreEqual(true, _googleCalendarService.InsertReccuringEvent(Name, Description, DateStart,
                DateEnd, HowOften, HowMany));
        }

        /// <summary>
        ///  Test to check function reaction if there are wrong HowMany arguments
        /// </summary>
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
        /// <summary>
        ///  Test to check function reaction if Date Start is later than DateEnd
        /// </summary>
        [TestCase("Lala", "Lala", "DAILY", 2)]
        public void InsertRecurringEvent_DateComparisonTest(string Name, string Description, string HowOften,
            int HowMany)
        {
            var DateStart = _DateEndExample;
            var DateEnd = _DateStartExample;
            Assert.AreEqual(false, _googleCalendarService.InsertReccuringEvent(Name, Description, DateStart,
                DateEnd, HowOften, HowMany));
        }
        /// <summary>
        ///  Test to check function reaction if Date Start is equal to DateEnd
        /// </summary>
        [TestCase("Lala", "Lala", "DAILY", 2)]
        public void InsertRecurringEvent_DatesAreEqual(string Name, string Description, string HowOften,
            int HowMany)
        {
            var DateStart = _DateStartExample;
            var DateEnd = _DateStartExample;
            Assert.AreEqual(false, _googleCalendarService.InsertReccuringEvent(Name, Description, DateStart,
                DateEnd, HowOften, HowMany));
        }
        /// <summary>
        ///  Test to check function reaction if arguments are null
        /// </summary>
        [TestCase(null, null)]
        [TestCase("lala", null)]
        public void DeleteEvent_NullError(string Name, DateTime DateStart)
        {
            Assert.AreEqual(false, _googleCalendarService.DeleteEvent(Name, DateStart));
        }

    }
}
