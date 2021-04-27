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
    
    public class GoogleCalendarService
    {
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
        private static string GetCalendarId()
        {
            string calendarId = @"calapp553@gmail.com";

            return calendarId;
        }

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

        public bool InsertEvent(string Name, string Description, DateTime DateStart, DateTime DateEnd)
        {
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


        public bool InsertReccuringEvent(string Name, string Description, DateTime DateStart, DateTime DateEnd,
            string HowOften, int HowMany)
        {
            if (HowMany <= 0)
                return false;

            if (!(HowOften == "DAILY" || HowOften == "WEEKLY" || HowOften == "MONTHLY" || HowOften == "YEARLY"))
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
            */
        }


        static List<Event> DB =
             new List<Event>() {
                new Event(){
                    Id = "eventid" + 1,
                    Summary = "Google I/O 2015",
                    Location = "800 Howard St., San Francisco, CA 94103",
                    Description = "A chance to hear more about Google's developer products.",
                    Start = new EventDateTime()
                    {
                        DateTime = new DateTime(2021, 01, 13, 15, 30, 0),
                        TimeZone = "Europe/Warsaw",
                    },
                    End = new EventDateTime()
                    {
                        DateTime = new DateTime(2021, 01, 14, 15, 30, 0),
                        TimeZone = "Europe/Warsaw",
                    },
                     Recurrence = new List<string> { "RRULE:FREQ=DAILY;COUNT=2" },

                }
             };
    }
    
}