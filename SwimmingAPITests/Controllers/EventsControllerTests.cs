using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SwimmingAPI.Controllers;
using SwimmingAPI.Models;
using SwimmingAPI.Repo.Interfaces;
using TimeSpan = System.TimeSpan;

namespace SwimmingAPITests.Controllers
{
    [TestClass()]
    public class EventsControllerTests
    {
        

        [TestMethod()]
        public void AddEventBadModelReturnsModelErrorState()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var addEvent = new EventAddModel();
            
            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);
            sut.ModelState.AddModelError("Round","Round Is Required");


            //Action
            var res = sut.AddEvent(addEvent);

            //Assert
            res.Should().BeOfType<InvalidModelStateResult>();
            res.As<InvalidModelStateResult>().ModelState.IsValid.Should().BeFalse();

        }

        [TestMethod()]
        public void AddEventInvalidGenderReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var addEvent = new EventAddModel()
            {
                EventAge = "21",
                EventCode = "21",
                EventGender = "TEST FAIL",
                MeetId = 1,
                Round = "F"
            };

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);
           

            //Action
            var res = sut.AddEvent(addEvent);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Gender entered is not valid it must be either M, F, or Mix");
            
        }

        [TestMethod()]
        public void AddEventInvalidEventCodeReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var addEvent = new EventAddModel()
            {
                EventAge = "21",
                EventCode = "35",
                EventGender = "M",
                MeetId = 1,
                Round = "F"
            };

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);


            //Action
            var res = sut.AddEvent(addEvent);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Event Code is not valid and must be match the neutral file format standard");

        }

        [TestMethod()]
        public void AddEventInvalidRoundReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var addEvent = new EventAddModel()
            {
                EventAge = "21",
                EventCode = "01",
                EventGender = "M",
                MeetId = 1,
                Round = "BAD ROUND"
            };

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);


            //Action
            var res = sut.AddEvent(addEvent);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Round must be one of the following H, F, C, S, B according to the Neutral file format");

        }

        [TestMethod()]
        public void AddEventValidModelAndDatabaseReturnsOk()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var addEvent = new EventAddModel()
            {
                EventAge = "21",
                EventCode = "01",
                EventGender = "M",
                MeetId = 1,
                Round = "F"
            };
            mockEventsRepo.Setup(MER => MER.AddEvent(addEvent)).Returns(true);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);


            //Action
            var res = sut.AddEvent(addEvent);

            //Assert
            res.Should().BeOfType<OkResult>();
            

        }

        [TestMethod()]
        public void AddEventValidModelAndFailedDatabaseReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var addEvent = new EventAddModel()
            {
                EventAge = "21",
                EventCode = "01",
                EventGender = "M",
                MeetId = 1,
                Round = "F"
            };
            mockEventsRepo.Setup(MER => MER.AddEvent(addEvent)).Returns(false);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);


            //Action
            var res = sut.AddEvent(addEvent);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Failed to add to database");


        }

        [TestMethod()]
        public void GetEventsReturnsOkWithEvents()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var listOfEvents = new List<Event>
            {
                new Event()
                {
                    EventId = 1,
                    EventAge = "15",
                    EventCode = "01",
                    EventGender = "Mix",
                    MeetId = 2,
                    Round = "F"
                },
                new Event()
                {
                    EventId   = 2,
                    EventAge = "16",
                    EventCode = "02",
                    EventGender = "F",
                    MeetId = 2,
                    Round = "F"
                }
            };
            mockEventsRepo.Setup(mER => mER.GetEvents()).Returns(listOfEvents);

            var sut = new EventsController(mockEventsRepo.Object,mockEventResultsRepo.Object,mockUserRepo.Object);

            //Action
            var res = sut.GetEvents();

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<Event>>>();
            res.As<OkNegotiatedContentResult<List<Event>>>().Content.Should().BeEquivalentTo(listOfEvents);

        }

        [TestMethod()]
        public void GetEventsByEventCodeValidEventCodeReturnsOk()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var listOfEvents = new List<Event>
            {
                new Event()
                {
                    EventId = 1,
                    EventAge = "15",
                    EventCode = "01",
                    EventGender = "Mix",
                    MeetId = 2,
                    Round = "F"
                },
                new Event()
                {
                    EventId   = 2,
                    EventAge = "16",
                    EventCode = "01",
                    EventGender = "F",
                    MeetId = 2,
                    Round = "F"
                }
            };
            mockEventsRepo.Setup(mER => mER.GetEventsByEventCode("01")).Returns(listOfEvents);

            var sut = new EventsController(mockEventsRepo.Object,mockEventResultsRepo.Object,mockUserRepo.Object);
            

            //Action
            var res = sut.GetEventsByEventCode("01");

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<Event>>>();
            res.As<OkNegotiatedContentResult<List<Event>>>().Content.Should().BeEquivalentTo(listOfEvents);
        }


        [TestMethod()]
        public void GetEventsByEventCodeInvalidEventCodeReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);


            //Action
            var res = sut.GetEventsByEventCode("55");

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Event Code is invalid");
        }

        [TestMethod()]
        public void GetEventsByGenderInvalidGenderReturnsBadResult()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            

            //Action
            var res = sut.GetEventsByGender("TEST");

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Gender entered is not valid it must be either M, F, or Mix");
        }

        [TestMethod()]
        public void GetEventsByGenderValidGenderReturnsOk()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            var listOfEvents = new List<Event>
            {
                new Event()
                {
                    EventId = 1,
                    EventAge = "15",
                    EventCode = "01",
                    EventGender = "M",
                    MeetId = 2,
                    Round = "F"
                },
                new Event()
                {
                    EventId   = 2,
                    EventAge = "16",
                    EventCode = "01",
                    EventGender = "M",
                    MeetId = 2,
                    Round = "F"
                }
            };
            mockEventsRepo.Setup(mER => mER.GetEventsByGender("M")).Returns(listOfEvents);

            //Action
            var res = sut.GetEventsByGender("M");

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<Event>>>();
            res.As<OkNegotiatedContentResult<List<Event>>>().Content.Should().BeEquivalentTo(listOfEvents);
        }

        [TestMethod()]
        public void GetEventsByAgeReturnsOkWithEvents()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var listOfEvents = new List<Event>
            {
                new Event()
                {
                    EventId = 1,
                    EventAge = "15",
                    EventCode = "01",
                    EventGender = "Mix",
                    MeetId = 2,
                    Round = "F"
                },
                new Event()
                {
                    EventId   = 2,
                    EventAge = "15",
                    EventCode = "02",
                    EventGender = "F",
                    MeetId = 2,
                    Round = "F"
                }
            };
            mockEventsRepo.Setup(mER => mER.GetEventsByAge("15")).Returns(listOfEvents);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            //Action
            var res = sut.GetEventsByAge("15");

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<Event>>>();
            res.As<OkNegotiatedContentResult<List<Event>>>().Content.Should().BeEquivalentTo(listOfEvents);
        }

        [TestMethod]
        public void GetEventsByAgeAndGenderInvalidGenderReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var sut = new EventsController(mockEventsRepo.Object,mockEventResultsRepo.Object,mockUserRepo.Object);

            //Action
            var res = sut.GetEventsByAgeAndGender("21", "TEST");

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should()
                .Be("Gender entered is not valid it must be either M, F, or Mix");
        }

        [TestMethod]
        public void GetEventsByAgeAndGenderValidGenderReturnsOk()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var listOfEvents = new List<Event>
            {
                new Event()
                {
                    EventId = 1,
                    EventAge = "15",
                    EventCode = "01",
                    EventGender = "Mix",
                    MeetId = 2,
                    Round = "F"
                },
                new Event()
                {
                    EventId   = 2,
                    EventAge = "15",
                    EventCode = "02",
                    EventGender = "F",
                    MeetId = 2,
                    Round = "F"
                }
            };
            mockEventsRepo.Setup(mER => mER.GetEventsByAgeAndGender("21", "M")).Returns(listOfEvents);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            //Action
            var res = sut.GetEventsByAgeAndGender("21", "M");

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<Event>>>();
            res.As<OkNegotiatedContentResult<List<Event>>>().Content.Should().BeEquivalentTo(listOfEvents);
        }

        [TestMethod()]
        public void GetEventsByAgeAndEventCodeInvalidEventCodeReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var sut = new EventsController(mockEventsRepo.Object,mockEventResultsRepo.Object,mockUserRepo.Object);
            
            //Action
            var res = sut.GetEventsByAgeAndEventCode("18", "55");

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should()
                .Be("Event Code is not valid and must be match the neutral file format standard");

        }

        [TestMethod()]
        public void GetEventsByAgeAndEventCodeValidReturnsOk()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var listOfEvents = new List<Event>
            {
                new Event()
                {
                    EventId = 1,
                    EventAge = "15",
                    EventCode = "01",
                    EventGender = "Mix",
                    MeetId = 2,
                    Round = "F"
                },
                new Event()
                {
                    EventId   = 2,
                    EventAge = "15",
                    EventCode = "02",
                    EventGender = "F",
                    MeetId = 2,
                    Round = "F"
                }
            };
            mockEventsRepo.Setup(mER => mER.GetEventsByAgeAndEventCode("18", "01")).Returns(listOfEvents);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            //Action
            var res = sut.GetEventsByAgeAndEventCode("18", "01");

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<Event>>>();
            res.As<OkNegotiatedContentResult<List<Event>>>().Content.Should().BeEquivalentTo(listOfEvents);

        }

        [TestMethod()]
        public void GetEventsByEventCodeAndGenderInvalidEventCodeReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var sut = new EventsController(mockEventsRepo.Object,mockEventResultsRepo.Object,mockUserRepo.Object);

            //Action
            var res = sut.GetEventsByEventCodeAndGender("55", "M");
            
            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should()
                .Be("Event Code is not valid and must be match the neutral file format standard");
            
        }

        [TestMethod()]
        public void GetEventsByEventCodeAndGenderInvalidGenderReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            //Action
            var res = sut.GetEventsByEventCodeAndGender("01", "TEST");

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should()
                .Be("Gender entered is not valid it must be either M, F, or Mix");

        }

        [TestMethod()]
        public void GetEventsByEventCodeAndGenderValidReturnsOk()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var listOfEvents = new List<Event>
            {
                new Event()
                {
                    EventId = 1,
                    EventAge = "15",
                    EventCode = "01",
                    EventGender = "Mix",
                    MeetId = 2,
                    Round = "F"
                },
                new Event()
                {
                    EventId   = 2,
                    EventAge = "15",
                    EventCode = "02",
                    EventGender = "F",
                    MeetId = 2,
                    Round = "F"
                }
            };

            mockEventsRepo.Setup(mER => mER.GetEventsByEventCodeAndGender("01", "M")).Returns(listOfEvents);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            //Action
            var res = sut.GetEventsByEventCodeAndGender("01", "M");

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<Event>>>();
            res.As<OkNegotiatedContentResult<List<Event>>>().Content.Should().BeEquivalentTo(listOfEvents);

        }

        [TestMethod()]
        public void GetEventsByAgeGenderAndEventCodeInvalidEventCodeReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var sut = new EventsController(mockEventsRepo.Object,mockEventResultsRepo.Object,mockUserRepo.Object);

            //Action
            var res = sut.GetEventsByAgeGenderAndEventCode("18", "M", "55");

            //assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should()
                .Be("Event Code is not valid and must be match the neutral file format standard");
        }

        [TestMethod()]
        public void GetEventsByAgeGenderAndEventCodeInvalidGenderReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            //Action
            var res = sut.GetEventsByAgeGenderAndEventCode("18", "TEST", "01");

            //assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should()
                .Be("Gender entered is not valid it must be either M, F, or Mix");
        }

        [TestMethod()]
        public void GetEventsByAgeGenderAndEventCodeValidReturnsOk()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var listOfEvents = new List<Event>
            {
                new Event()
                {
                    EventId = 1,
                    EventAge = "15",
                    EventCode = "01",
                    EventGender = "Mix",
                    MeetId = 2,
                    Round = "F"
                },
                new Event()
                {
                    EventId   = 2,
                    EventAge = "15",
                    EventCode = "02",
                    EventGender = "F",
                    MeetId = 2,
                    Round = "F"
                }
            };

            mockEventsRepo.Setup(mER => mER.GetEventsByAgeGenderAndEventCode("18", "M", "01")).Returns(listOfEvents);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            //Action
            var res = sut.GetEventsByAgeGenderAndEventCode("18", "M", "01");

            //assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<Event>>>();
            res.As<OkNegotiatedContentResult<List<Event>>>().Content.Should().BeEquivalentTo(listOfEvents);
        }


        [TestMethod()]
        public void GetResultForEventInvalidEventIdReturnsBadRequest()
        {
            //Setup
            const int eventId = 1;
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            mockEventsRepo.Setup(mER => mER.GetEvent(eventId)).Returns((Event) null);

            var sut = new EventsController(mockEventsRepo.Object,mockEventResultsRepo.Object,mockUserRepo.Object);

            //Action
            var res = sut.GetResultsForEvent(eventId);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Invalid event Id, event not found");

        }

        [TestMethod()]
        public void GetResultForEventValidReturnsOkWithEventResults()
        {
            //Setup
            const int eventId = 1;
            const int meetId = 1;
            
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var _event = new Event()
            {
                EventAge = "Junior",
                EventCode = "01",
                EventGender = "Mix",
                EventId = eventId,
                MeetId = meetId,
                Round = "F"
            };
            var user = new ApplicationUser()
            {
                Address = "TestAddress",
                Club = "testClub",
                DateOfBirth = new DateTime(1996, 08, 19),
                FamilyName = "Smith",
                GivenName = "John",
                Gender = "M",
                Id = "a1",
                Email="testUser@test.com",
                PhoneNumber="07700900000",
                UserName="testUser@test.com"
            };
            var eventResults = new List<EventResult>()
            {
                new EventResult()
                {
                    Event = _event,
                    EventId = eventId,
                    EventResultId = 1,
                    UserId = "a1",
                    Time = new TimeSpan(0, 0, 2, 5, 100)
                }
            };


            mockEventsRepo.Setup(mER => mER.GetEvent(eventId)).Returns(_event);
            mockEventResultsRepo.Setup(mERR => mERR.GetEventResultsFromEventId(eventId)).Returns(eventResults);
            mockUserRepo.Setup(mUR => mUR.GetUser("a1")).Returns(user);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            //Action
            var res = sut.GetResultsForEvent(eventId);

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<EventResultView>>>();
            var expectedContent = new List<EventResultView>
            {
                new EventResultView()
                {
                    Club = user.Club,
                    DateOfBirth = user.DateOfBirth,
                    EventCode = _event.EventCode,
                    FamilyName = user.FamilyName,
                    Gender = user.Gender,
                    GivenName = user.GivenName,
                    Round = _event.Round,
                    Time = eventResults[0].Time.ToString("mmssff")
                }
            };
            res.As<OkNegotiatedContentResult<List<EventResultView>>>().Content.Should().BeEquivalentTo(expectedContent);
        }


        [TestMethod()]
        public void GetResultForEventFormattedInvalidEventIdReturnsBadRequest()
        {
            //Setup
            const int eventId = 1;
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            mockEventsRepo.Setup(mER => mER.GetEvent(eventId)).Returns((Event)null);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            //Action
            var res = sut.GetResultsForEventFormatted(eventId);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Invalid event Id, event not found");

        }

        [TestMethod()]
        public void GetResultForEventFormattedValidReturnsOkWithEventResults()
        {
            //Setup
            const int eventId = 1;
            const int meetId = 1;

            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var _event = new Event()
            {
                EventAge = "Junior",
                EventCode = "01",
                EventGender = "Mix",
                EventId = eventId,
                MeetId = meetId,
                Round = "F"
            };
            var user = new ApplicationUser()
            {
                Address = "TestAddress",
                Club = "testClub",
                DateOfBirth = new DateTime(1996, 08, 19),
                FamilyName = "Smith",
                GivenName = "John",
                Gender = "M",
                Id = "a1",
                Email = "testUser@test.com",
                PhoneNumber = "07700900000",
                UserName = "testUser@test.com"
            };
            var eventResults = new List<EventResult>()
            {
                new EventResult()
                {
                    Event = _event,
                    EventId = eventId,
                    EventResultId = 1,
                    UserId = "a1",
                    Time = new TimeSpan(0, 0, 2, 5, 100)
                }
            };


            mockEventsRepo.Setup(mER => mER.GetEvent(eventId)).Returns(_event);
            mockEventResultsRepo.Setup(mERR => mERR.GetEventResultsFromEventId(eventId)).Returns(eventResults);
            mockUserRepo.Setup(mUR => mUR.GetUser("a1")).Returns(user);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            //Action
            var res = sut.GetResultsForEventFormatted(eventId);

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<string>>>();
            var expectedContent = new List<EventResultView>
            {
                new EventResultView()
                {
                    Club = user.Club,
                    DateOfBirth = user.DateOfBirth,
                    EventCode = _event.EventCode,
                    FamilyName = user.FamilyName,
                    Gender = user.Gender,
                    GivenName = user.GivenName,
                    Round = _event.Round,
                    Time = eventResults[0].Time.ToString("mmssff")
                }
            };
            res.As<OkNegotiatedContentResult<List<string>>>().Content.Should().BeEquivalentTo(FormatEventResults(expectedContent));
        }

        [TestMethod()]
        public void GetResultsForMeetInvalidMeetIdReturnsBadRequest()
        {
            //Setup
            const int meetId = 1;
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            mockEventsRepo.Setup(mER => mER.GetEventsByMeetId(meetId)).Returns(new List<Event>());
            
            var sut = new EventsController(mockEventsRepo.Object,mockEventResultsRepo.Object,mockUserRepo.Object);

            //Action
            var res = sut.GetResultsForMeet(meetId);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("No events for that meet");
        }

        [TestMethod()]
        public void GetResultsForMeetValidReturnsOk()
        {
            //Setup
            const int meetId = 1;
            const int eventId = 1;
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var _event = new Event()
            {
                EventAge = "Junior",
                EventCode = "01",
                EventGender = "Mix",
                EventId = eventId,
                MeetId = meetId,
                Round = "F"
            };
            var user = new ApplicationUser()
            {
                Address = "TestAddress",
                Club = "testClub",
                DateOfBirth = new DateTime(1996, 08, 19),
                FamilyName = "Smith",
                GivenName = "John",
                Gender = "M",
                Id = "a1",
                Email = "testUser@test.com",
                PhoneNumber = "07700900000",
                UserName = "testUser@test.com"
            };
            var eventResults = new List<EventResult>()
            {
                new EventResult()
                {
                    Event = _event,
                    EventId = eventId,
                    EventResultId = 1,
                    UserId = "a1",
                    Time = new TimeSpan(0, 0, 2, 5, 100)
                }
            };

            mockEventsRepo.Setup(mER => mER.GetEventsByMeetId(meetId)).Returns(new List<Event>{_event});
            mockEventResultsRepo.Setup(mERR => mERR.GetEventResultsFromEventId(eventId)).Returns(eventResults);
            mockUserRepo.Setup(mUR => mUR.GetUser("a1")).Returns(user);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            //Action
            var res = sut.GetResultsForMeet(meetId);

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<EventResultView>>>();
            var expectedContent = new List<EventResultView>
            {
                new EventResultView()
                {
                    Club = user.Club,
                    DateOfBirth = user.DateOfBirth,
                    EventCode = _event.EventCode,
                    FamilyName = user.FamilyName,
                    Gender = user.Gender,
                    GivenName = user.GivenName,
                    Round = _event.Round,
                    Time = eventResults[0].Time.ToString("mmssff")
                }
            };
            res.As<OkNegotiatedContentResult<List<EventResultView>>>().Content.Should().BeEquivalentTo(expectedContent);
        }


        [TestMethod()]
        public void GetResultsForMeetFormattedInvalidMeetIdReturnsBadRequest()
        {
            //Setup
            const int meetId = 1;
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            mockEventsRepo.Setup(mER => mER.GetEventsByMeetId(meetId)).Returns(new List<Event>());

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            //Action
            var res = sut.GetResultsForMeetFormatted(meetId);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("No events for that meet");
        }

        [TestMethod()]
        public void GetResultsForMeetFormattedValidReturnsOk()
        {
            //Setup
            const int meetId = 1;
            const int eventId = 1;
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var _event = new Event()
            {
                EventAge = "Junior",
                EventCode = "01",
                EventGender = "Mix",
                EventId = eventId,
                MeetId = meetId,
                Round = "F"
            };
            var user = new ApplicationUser()
            {
                Address = "TestAddress",
                Club = "testClub",
                DateOfBirth = new DateTime(1996, 08, 19),
                FamilyName = "Smith",
                GivenName = "John",
                Gender = "M",
                Id = "a1",
                Email = "testUser@test.com",
                PhoneNumber = "07700900000",
                UserName = "testUser@test.com"
            };
            var eventResults = new List<EventResult>()
            {
                new EventResult()
                {
                    Event = _event,
                    EventId = eventId,
                    EventResultId = 1,
                    UserId = "a1",
                    Time = new TimeSpan(0, 0, 2, 5, 100)
                }
            };

            mockEventsRepo.Setup(mER => mER.GetEventsByMeetId(meetId)).Returns(new List<Event> { _event });
            mockEventResultsRepo.Setup(mERR => mERR.GetEventResultsFromEventId(eventId)).Returns(eventResults);
            mockUserRepo.Setup(mUR => mUR.GetUser("a1")).Returns(user);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);

            //Action
            var res = sut.GetResultsForMeetFormatted(meetId);

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<string>>>();
            var expectedContent = new List<EventResultView>
            {
                new EventResultView()
                {
                    Club = user.Club,
                    DateOfBirth = user.DateOfBirth,
                    EventCode = _event.EventCode,
                    FamilyName = user.FamilyName,
                    Gender = user.Gender,
                    GivenName = user.GivenName,
                    Round = _event.Round,
                    Time = eventResults[0].Time.ToString("mmssff")
                }
            };
            res.As<OkNegotiatedContentResult<List<string>>>().Content.Should().BeEquivalentTo(FormatEventResults(expectedContent));
        }

        [TestMethod()]
        public void AddResultWithInvalidModelReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var result = new AddResultModel();

            var sut = new EventsController(mockEventsRepo.Object,mockEventResultsRepo.Object,mockUserRepo.Object);
            sut.ModelState.AddModelError("EventId","EventId is required");

            //Action
            var res = sut.AddResult(result);

            //Assert
            res.Should().BeOfType<InvalidModelStateResult>();
            res.As<InvalidModelStateResult>().ModelState.IsValid.Should().BeFalse();
        }

        [TestMethod()]
        public void AddResultWithValidDbReturnsOk()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var result = new AddResultModel()
            {
                EventId = 1,
                Hundreths = 100,
                Minutes = 20,
                Seconds = 30,
                UserId = "a1"
            };

            mockEventResultsRepo.Setup(mERR => mERR.AddEventResult(result)).Returns(true);
            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);
           

            //Action
            var res = sut.AddResult(result);

            //Assert
            res.Should().BeOfType<OkResult>();
        }

        [TestMethod()]
        public void AddResultWithInvalidDbReturnsBadResult()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();
            var result = new AddResultModel()
            {
                EventId = 1,
                Hundreths = 100,
                Minutes = 20,
                Seconds = 30,
                UserId = "a1"
            };

            mockEventResultsRepo.Setup(mERR => mERR.AddEventResult(result)).Returns(false);
            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);


            //Action
            var res = sut.AddResult(result);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Unable To add to database");
        }

        [TestMethod()]
        public void EditEventsInvalidModelReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var model = new EditEventModel();

            var sut = new EventsController(mockEventsRepo.Object,mockEventResultsRepo.Object,mockUserRepo.Object);
            sut.ModelState.AddModelError("EventId","EventId is invalid");

            //Action
            var res = sut.EditEvent(model);

            //Assert
            res.Should().BeOfType<InvalidModelStateResult>();
            res.As<InvalidModelStateResult>().ModelState.IsValid.Should().BeFalse();
        }

        [TestMethod()]
        public void EditEventsInvalidGenderReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var model = new EditEventModel()
            {
                EventAge = "Junior",
                EventCode = "01",
                EventGender = "TEST",
                EventId = 1,
                MeetId = 1,
                Round = "F"
            };

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);
            

            //Action
            var res = sut.EditEvent(model);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should()
                .Be("Gender entered is not valid it must be either M, F, or Mix");
        }

        [TestMethod()]
        public void EditEventsInvalidEventCodeReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var model = new EditEventModel()
            {
                EventAge = "Junior",
                EventCode = "55",
                EventGender = "M",
                EventId = 1,
                MeetId = 1,
                Round = "F"
            };

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);


            //Action
            var res = sut.EditEvent(model);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should()
                .Be("Event Code is not valid and must be match the neutral file format standard");
        }

        [TestMethod()]
        public void EditEventsInvalidRoundReturnsBadRequest()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var model = new EditEventModel()
            {
                EventAge = "Junior",
                EventCode = "01",
                EventGender = "M",
                EventId = 1,
                MeetId = 1,
                Round = "T"
            };

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);


            //Action
            var res = sut.EditEvent(model);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should()
                .Be("Round must be one of the following H, F, C, S, B according to the Neutral file format");
        }

        [TestMethod()]
        public void EditEventsValidReturnsOk()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var model = new EditEventModel()
            {
                EventAge = "Junior",
                EventCode = "01",
                EventGender = "M",
                EventId = 1,
                MeetId = 1,
                Round = "F"
            };

            mockEventsRepo.Setup(mER => mER.EditEvent(model)).Returns(true);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);


            //Action
            var res = sut.EditEvent(model);

            //Assert
            res.Should().BeOfType<OkResult>();
            
        }

        [TestMethod()]
        public void EditEventsInvalidDbReturnsBadResult()
        {
            //Setup
            var mockEventsRepo = new Mock<IEventRepo>();
            var mockEventResultsRepo = new Mock<IEventResultsRepo>();
            var mockUserRepo = new Mock<IUserRepo>();

            var model = new EditEventModel()
            {
                EventAge = "Junior",
                EventCode = "01",
                EventGender = "M",
                EventId = 1,
                MeetId = 1,
                Round = "F"
            };

            mockEventsRepo.Setup(mER => mER.EditEvent(model)).Returns(false);

            var sut = new EventsController(mockEventsRepo.Object, mockEventResultsRepo.Object, mockUserRepo.Object);


            //Action
            var res = sut.EditEvent(model);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Editing event failed");

        }


        private static List<string> FormatEventResults(IEnumerable<EventResultView> eventResultViews)
        {
            return eventResultViews.Select(eventResult => eventResult.Gender + " , " + eventResult.FamilyName + " , " + eventResult.GivenName + " , " + eventResult.Club + " , " + eventResult.DateOfBirth.ToString("DDMMyy") + " , " + eventResult.Time + " , " + eventResult.EventCode + " , " + eventResult.Round).ToList();
        }
    }
}