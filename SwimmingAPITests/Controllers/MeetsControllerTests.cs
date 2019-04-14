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

namespace SwimmingAPITests.Controllers
{
    [TestClass()]
    public class MeetsControllerTests
    {
        

        [TestMethod()]
        public void AddMeetInvalidSwimmingLengthResultsInBadRequest()
        {
            //Setup
            var mockMeetRepo = new Mock<IMeetRepo>();
            var meet = new AddMeetModel();
            var sut = new MeetsController(mockMeetRepo.Object);
            
            //Action
            var res = sut.AddMeet(meet);

            //assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Pool Length must match the neutral file format code");
        }

        [TestMethod()]
        public void AddMeetInvalidModelLengthResultsInInvalidModelStateResult()
        {
            //Setup
            var mockMeetRepo = new Mock<IMeetRepo>();
            var meet = new AddMeetModel();
            var sut = new MeetsController(mockMeetRepo.Object);
            sut.ModelState.AddModelError("MeetVenue","Meet venue is required");

            //Action
            var res = sut.AddMeet(meet);

            //assert
            res.Should().BeOfType<InvalidModelStateResult>();
            res.As<InvalidModelStateResult>().ModelState.IsValid.Should().BeFalse();
        }


        [TestMethod()]
        public void AddMeetValidMeetAndSuccessfulAddToDbReturnsOk()
        {
            //Setup
            var mockMeetRepo = new Mock<IMeetRepo>();
            
            var meet = new AddMeetModel()
            {
                MeetDate = new DateTime(),
                MeetName = "Test Meet Name",
                MeetVenue = "Test Meet Venue",
                PoolLength = "11"
            };
            mockMeetRepo.Setup(mMR => mMR.AddMeet(meet)).Returns(true);
            var sut = new MeetsController(mockMeetRepo.Object);
            
            //Action
            var res = sut.AddMeet(meet);

            //assert
            res.Should().BeOfType<OkResult>();
        }

        [TestMethod()]
        public void AddMeetValidMeetAndUnSuccessfulAddToDbReturnsBadRequestErrorMessage()
        {
            //Setup
            var mockMeetRepo = new Mock<IMeetRepo>();

            var meet = new AddMeetModel()
            {
                MeetDate = new DateTime(),
                MeetName = "Test Meet Name",
                MeetVenue = "Test Meet Venue",
                PoolLength = "11"
            };
            mockMeetRepo.Setup(mMR => mMR.AddMeet(meet)).Returns(false);
            var sut = new MeetsController(mockMeetRepo.Object);

            //Action
            var res = sut.AddMeet(meet);

            //assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Not able to add meet to db");
        }

        [TestMethod()]
        public void UpdateMeetInvalidModelReturnsInvalidModelStateResult()
        {
            //Setup
            var mockMeetsRepo = new Mock<IMeetRepo>();
            var meet = new Meet();
            var sut = new MeetsController(mockMeetsRepo.Object);
            sut.ModelState.AddModelError("MeetName","MeetName is required");

            //action
            var res = sut.UpdateMeet(meet);
            
            //assert
            res.Should().BeOfType<InvalidModelStateResult>();
            res.As<InvalidModelStateResult>().ModelState.IsValid.Should().BeFalse();
        }

        [TestMethod()]
        public void UpdateMeetPoolLengthNotValidReturnsBadRequest()
        {
            //Setup
            var mockMeetsRepo = new Mock<IMeetRepo>();
            var meet = new Meet()
            {
                PoolLength = "1000"
            };
            var sut = new MeetsController(mockMeetsRepo.Object);
            

            //action
            var res = sut.UpdateMeet(meet);

            //assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Pool Length must match the neutral file format code");
        }

        [TestMethod()]
        public void UpdateMeetValidMeetAndDbSuccessfulReturnsOk()
        {
            //Setup
            var mockMeetsRepo = new Mock<IMeetRepo>();
            var meet = new Meet()
            {
                MeetDate = new DateTime(),
                MeetName = "Test Meet",
                MeetId = 1,
                MeetVenue = "Test Venue",
                PoolLength = "11"
            };
            mockMeetsRepo.Setup(MMR => MMR.UpdateMeet(meet)).Returns(true);
            var sut = new MeetsController(mockMeetsRepo.Object);

            //Action
            var res = sut.UpdateMeet(meet);

            //Assert
            res.Should().BeOfType<OkResult>();

        }

        [TestMethod()]
        public void UpdateMeetValidMeetAndDbUnSuccessfulReturnsBadRequest()
        {
            //Setup
            var mockMeetsRepo = new Mock<IMeetRepo>();
            var meet = new Meet()
            {
                MeetDate = new DateTime(),
                MeetName = "Test Meet",
                MeetId = 1,
                MeetVenue = "Test Venue",
                PoolLength = "11"
            };
            mockMeetsRepo.Setup(MMR => MMR.UpdateMeet(meet)).Returns(false);
            var sut = new MeetsController(mockMeetsRepo.Object);

            //Action
            var res = sut.UpdateMeet(meet);

            //Assert
            res.Should().BeOfType<BadRequestErrorMessageResult>();
            res.As<BadRequestErrorMessageResult>().Message.Should().Be("Meet failed to be updated");

        }

        [TestMethod()]
        public void GetMeetsReturnsOkWithMeetsFromRepo()
        {
            //Setup
            var listOfMeets = new List<Meet>
            {
                new Meet()
                {
                    MeetDate = DateTime.Now,
                    MeetName = "Test Meet 1",
                    MeetId = 1,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "11"
                },
                new Meet()
                {
                    MeetDate = DateTime.Now,
                    MeetName = "Test Meet 2",
                    MeetId = 2,
                    MeetVenue = "Test Venue 2",
                    PoolLength = "21"
                }
            };
            var mockMeetsRepo = new Mock<IMeetRepo>();
            mockMeetsRepo.Setup(MMR => MMR.GetAllMeets()).Returns(listOfMeets);
            var sut = new MeetsController(mockMeetsRepo.Object);

            //Action
            var res = sut.GetMeets();

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<Meet>>>();
            res.As<OkNegotiatedContentResult<List<Meet>>>().Content.Should().BeEquivalentTo(listOfMeets);

        }

        [TestMethod()]
        public void GetMeetsFormattedGetsMeetsFromRepoAndFormatsThem()
        {
            //Setup
            var listOfMeets = new List<Meet>
            {
                new Meet()
                {
                    MeetDate = DateTime.Now,
                    MeetName = "Test Meet 1",
                    MeetId = 1,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "11"
                },
                new Meet()
                {
                    MeetDate = DateTime.Now,
                    MeetName = "Test Meet 2",
                    MeetId = 2,
                    MeetVenue = "Test Venue 2",
                    PoolLength = "21"
                }
            };
            var mockMeetsRepo = new Mock<IMeetRepo>();
            mockMeetsRepo.Setup(MMR => MMR.GetAllMeets()).Returns(listOfMeets);
            var sut = new MeetsController(mockMeetsRepo.Object);

            //Action
            var res = sut.GetMeetsFormatted();

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<string>>>();
            res.As<OkNegotiatedContentResult<List<string>>>().Content.Should().BeEquivalentTo(FormatMeets(listOfMeets));
        }

        [TestMethod()]
        public void GetMeetsFormattedWithVenueReturnsMeetsFromVenue()
        {
            //Setup
            var listOfMeets = new List<Meet>
            {
                new Meet()
                {
                    MeetDate = DateTime.Now,
                    MeetName = "Test Meet 1",
                    MeetId = 1,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "11"
                },
                new Meet()
                {
                    MeetDate = DateTime.Now,
                    MeetName = "Test Meet 2",
                    MeetId = 2,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "21"
                }
            };
            var mockMeetsRepo = new Mock<IMeetRepo>();
            mockMeetsRepo.Setup(MMR => MMR.GetMeets("Test Venue 1")).Returns(listOfMeets);
            var sut = new MeetsController(mockMeetsRepo.Object);

            //Action
            var res = sut.GetMeetsFormatted("Test Venue 1");

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<string>>>();
            res.As<OkNegotiatedContentResult<List<string>>>().Content.Should().BeEquivalentTo(FormatMeets(listOfMeets));
        }

        [TestMethod()]
        public void GetMeetsFormattedByStartAndEndDateReturnsMeetsWithinStartAndEndDate()
        {
            //Setup
            var listOfMeets = new List<Meet>
            {
                new Meet()
                {
                    MeetDate = new DateTime(2019,04,14),
                    MeetName = "Test Meet 1",
                    MeetId = 1,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "11"
                },
                new Meet()
                {
                    MeetDate = new DateTime(2019,04,13),
                    MeetName = "Test Meet 2",
                    MeetId = 2,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "21"
                }
            };
            var mockMeetsRepo = new Mock<IMeetRepo>();
            mockMeetsRepo.Setup(MMR => MMR.GetMeets(new DateTime(2019,04,12),new DateTime(2019,04,15))).Returns(listOfMeets);
            var sut = new MeetsController(mockMeetsRepo.Object);

            //Action
            var res = sut.GetMeetsFormatted(new DateTime(2019, 04, 12), new DateTime(2019, 04, 15));

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<string>>>();
            res.As<OkNegotiatedContentResult<List<string>>>().Content.Should().BeEquivalentTo(FormatMeets(listOfMeets));
        }

        [TestMethod()]
        public void GetMeetsFormattedWithOneDateReturnsMeetsFormattedOnThatDate()
        {
            //Setup
            var listOfMeets = new List<Meet>
            {
                new Meet()
                {
                    MeetDate = DateTime.Today,
                    MeetName = "Test Meet 1",
                    MeetId = 1,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "11"
                },
                new Meet()
                {
                    MeetDate = DateTime.Today,
                    MeetName = "Test Meet 2",
                    MeetId = 2,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "21"
                }
            };
            var mockMeetsRepo = new Mock<IMeetRepo>();
            mockMeetsRepo.Setup(MMR => MMR.GetMeets(DateTime.Today)).Returns(listOfMeets);
            var sut = new MeetsController(mockMeetsRepo.Object);

            //Action
            var res = sut.GetMeetsFormatted(DateTime.Today);

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<string>>>();
            res.As<OkNegotiatedContentResult<List<string>>>().Content.Should().BeEquivalentTo(FormatMeets(listOfMeets));
        }

        [TestMethod()]
        public void GetMeetsWithStartAndEndDateReturnsMeets()
        {
            //Setup
            var listOfMeets = new List<Meet>
            {
                new Meet()
                {
                    MeetDate = new DateTime(2019,04,14),
                    MeetName = "Test Meet 1",
                    MeetId = 1,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "11"
                },
                new Meet()
                {
                    MeetDate = new DateTime(2019,04,13),
                    MeetName = "Test Meet 2",
                    MeetId = 2,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "21"
                }
            };
            var mockMeetsRepo = new Mock<IMeetRepo>();
            mockMeetsRepo.Setup(MMR => MMR.GetMeets(new DateTime(2019, 04, 12), new DateTime(2019, 04, 15))).Returns(listOfMeets);
            var sut = new MeetsController(mockMeetsRepo.Object);

            //Action
            var res = sut.GetMeets(new DateTime(2019, 04, 12), new DateTime(2019, 04, 15));

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<Meet>>>();
            res.As<OkNegotiatedContentResult<List<Meet>>>().Content.Should().BeEquivalentTo(listOfMeets);
        }

        [TestMethod()]
        public void GetMeetsSingleDate()
        {
            //Setup
            var listOfMeets = new List<Meet>
            {
                new Meet()
                {
                    MeetDate = DateTime.Today,
                    MeetName = "Test Meet 1",
                    MeetId = 1,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "11"
                },
                new Meet()
                {
                    MeetDate = DateTime.Today,
                    MeetName = "Test Meet 2",
                    MeetId = 2,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "21"
                }
            };
            var mockMeetsRepo = new Mock<IMeetRepo>();
            mockMeetsRepo.Setup(MMR => MMR.GetMeets(DateTime.Today)).Returns(listOfMeets);
            var sut = new MeetsController(mockMeetsRepo.Object);

            //Action
            var res = sut.GetMeets(DateTime.Today);

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<Meet>>>();
            res.As<OkNegotiatedContentResult<List<Meet>>>().Content.Should().BeEquivalentTo(listOfMeets);
        }

        [TestMethod()]
        public void GetMeetsWithVenueName()
        {
            //Setup
            var listOfMeets = new List<Meet>
            {
                new Meet()
                {
                    MeetDate = DateTime.Now,
                    MeetName = "Test Meet 1",
                    MeetId = 1,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "11"
                },
                new Meet()
                {
                    MeetDate = DateTime.Now,
                    MeetName = "Test Meet 2",
                    MeetId = 2,
                    MeetVenue = "Test Venue 1",
                    PoolLength = "21"
                }
            };
            var mockMeetsRepo = new Mock<IMeetRepo>();
            mockMeetsRepo.Setup(MMR => MMR.GetMeets("Test Venue 1")).Returns(listOfMeets);
            var sut = new MeetsController(mockMeetsRepo.Object);

            //Action
            var res = sut.GetMeets("Test Venue 1");

            //Assert
            res.Should().BeOfType<OkNegotiatedContentResult<List<Meet>>>();
            res.As<OkNegotiatedContentResult<List<Meet>>>().Content.Should().BeEquivalentTo(listOfMeets);
        }

        private static List<string> FormatMeets(IEnumerable<Meet> meets)
        {
            return meets.Select(meet => meet.MeetName + " , " + meet.MeetVenue + " , " + meet.MeetDate.ToString("ddMMy") + " , " + meet.PoolLength).ToList();
        }
    }
}