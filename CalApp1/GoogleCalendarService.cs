using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CalApp1.Services
{
    /// <summary>
    /// Class which services the interaction between the User of the App and Google Calendar API
    /// </summary>
    public class GoogleCalendarService
    {
        /// <summary>
        /// The method gives credential of this Google Calendar Service Account
        /// </summary>
        private static ServiceAccountCredential GetCredential()
        {
            string[] Scopes = { CalendarService.Scope.Calendar };

            string jsonFile = "calapp1-a06384fe507e.json";
            ServiceAccountCredential credential;

            using (var stream =
                new FileStream(jsonFile, FileMode.Open, FileAccess.Read))
            {
                var confg = Google.Apis.Json.NewtonsoftJsonSerializer.Instance.Deserialize<JsonCredentialParameters>(stream);
                credential = new ServiceAccountCredential(
                   new ServiceAccountCredential.Initializer(confg.ClientEmail)
                   {
                       Scopes = Scopes
                   }.FromPrivateKey(confg.PrivateKey));
            }

            return credential;

        }
        /// <summary>
        /// The method gives google calendar Id
        /// </summary>
        private static string GetCalendarId()
        {
            string calendarId = @"calapp553@gmail.com";

            return calendarId;
        }
        /// <summary>
        /// The method gives Google Calendar Service
        /// </summary>
        private static CalendarService GetService()
        {

            var credential = GetCredential();

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Calendar API Sample",
            });
            return service;
        }

        /// <summary>
        /// Function to send all the details and create a Google Calendar event
        /// </summary>
        /// <param name="Name"> - contains info about the event summary</param>
        /// <param name="Description"> - contains info about the event description</param>
        /// <param name="DateStart"> - contains info about the event Start Time of the event</param>
        /// <param name="DateEnd"> - contains info about the event End Time of the event </param>
        public bool InsertEvent(string Name, string Description, DateTime DateStart, DateTime DateEnd)
        {

            if (Name == null || Description == null)
                return false;

            if (DateTime.Compare(DateStart, DateEnd) >= 0)
                return false;

            var calendarId = GetCalendarId();

            var service = GetService();

            var EventId = Guid.NewGuid().ToString("N");

            var myevent = new Event()
            {
                Id = EventId,
                Summary = Name,
                Location = null,
                Description = Description,
                Start = new EventDateTime()
                {
                    DateTime = DateStart,
                    TimeZone = "Europe/Warsaw",
                },
                End = new EventDateTime()
                {
                    DateTime = DateEnd,
                    TimeZone = "Europe/Warsaw",
                },
               
            };

            var InsertRequest = service.Events.Insert(myevent, calendarId);

            try
            {
                InsertRequest.Execute();
            }
            catch (Exception)
            {
                try
                {
                    //Insert/Update new Event
                    service.Events.Update(myevent, calendarId, myevent.Id).Execute();
                }
                catch (Exception)
                {
                    //can't Insert/Update new Event 
                    return false;

                }
            }

            return true;
        }

        /// <summary>
        /// Function to send all the details and create a Google Calendar event
        /// </summary>
        /// <param name="Name"> - contains info about the event summary</param>
        /// <param name="Description"> - contains info about the event description</param>
        /// <param name="DateStart"> - contains info about the event Start Time of the event</param>
        /// <param name="DateEnd"> - contains info about the event End Time of the event </param>
        /// /// <param name="HowOften"> - contains info abbout the reccurence type, weekly, daily etc
        /// It accepts only "DAILY", "MONTHLY", "YEARLY", "WEEKLY"</param>
        /// /// <param name="HowMany"> - contains info about how many times it should occur
        /// for ex "DAILY", 2 - event will occur in 2 consecutive days and never happen again</param>
        public bool InsertReccuringEvent(string Name, string Description, DateTime DateStart, DateTime DateEnd,
            string HowOften, int HowMany)
        {
            if (HowMany <= 0)
                return false;

            if (Name == null || Description == null)
                return false;

            
            if (!(HowOften == "DAILY" || HowOften == "WEEKLY" || HowOften == "MONTHLY" || HowOften == "YEARLY"))
                return false;
            
            if (DateTime.Compare(DateStart, DateEnd) >= 0)
                return false;


            String Reccurence = String.Format("RRULE:FREQ={0};COUNT={1}", HowOften, HowMany);


            var calendarId = GetCalendarId();

            var service = GetService();

            var EventId = Guid.NewGuid().ToString("N");

            var ListOfOneEvent = new List<Event>() {
                new Event(){
                    Id = EventId,
                    Summary = Name,
                    Location = null,
                    Description = Description,
                    Start = new EventDateTime()
                    {
                        DateTime = DateStart,
                        TimeZone = "Europe/Warsaw",
                    },
                    End = new EventDateTime()
                    {
                        DateTime = DateEnd,
                        TimeZone = "Europe/Warsaw",
                    },
                    Recurrence = new List<string> { Reccurence },

                }
             };

            var myevent = ListOfOneEvent.Find(x => x.Id == EventId);

            var InsertRequest = service.Events.Insert(myevent, calendarId);

            try
            {
                InsertRequest.Execute();
            }
            catch (Exception)
            {
                try
                {
                    //Insert/Update new Event
                    service.Events.Update(myevent, calendarId, myevent.Id).Execute();
                }
                catch (Exception)
                {
                    //can't Insert/Update new Event 
                    return false;

                }
            }

            return true;
        }

        /// <summary>
        /// Function to get event by name and date from API and remove it
        /// </summary>
        /// <param name="Name"> - contains info about the event summary</param>
        /// <param name="WhenStarts"> - contains info about when the event happens</param>

        public bool DeleteEvent(string Name, DateTime WhenStarts)
        {

            if (Name == null || WhenStarts == null)
                return false;


            var calendarId = GetCalendarId();

            var service = GetService();

            EventsResource.ListRequest listRequest = service.Events.List(calendarId);
            Events events = listRequest.Execute();

            if (!(events.Items != null && events.Items.Count > 0))
                return false;

            
            var myevent = events
                .Items
                .Where(c => c.Start.DateTime == WhenStarts)
                .Where(d=>d.Summary==Name)
                .FirstOrDefault();

            if (myevent == null)
                return false;

            var DeleteRequest = service.Events.Delete(calendarId,myevent.Id);

            try
            {
                DeleteRequest.Execute();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        /*
        public static void Initialize()
        {
            
            var calendarId = GetCalendarId();

            var service = GetService();

            var calendar = service.Calendars.Get(calendarId).Execute();

          
   
            //this is a listing of events, we probably won't use it
            /*
            // Define parameters of request.
            EventsResource.ListRequest listRequest = service.Events.List(calendarId);
            listRequest.TimeMin = DateTime.Now;
            listRequest.ShowDeleted = false;
            listRequest.SingleEvents = true;
            listRequest.MaxResults = 10;
            listRequest.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = listRequest.Execute();
            Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                }
            }
            else
            {
                Console.WriteLine("No upcoming events found.");
            }
            Console.WriteLine("click for more .. ");
            Console.Read();

            var myevent = DB.Find(x => x.Id == "eventid" + 1);

            var InsertRequest = service.Events.Insert(myevent, calendarId);

            try
            {
                InsertRequest.Execute();
            }
            catch (Exception)
            {
                try
                {
                    service.Events.Update(myevent, calendarId, myevent.Id).Execute();
                    Console.WriteLine("Insert/Update new Event ");
                    Console.Read();

                }
                catch (Exception)
                {
                    Console.WriteLine("can't Insert/Update new Event ");

                }
            }
            
        }
        */


    }
    
}