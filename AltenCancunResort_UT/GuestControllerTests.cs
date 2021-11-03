using System;
using Xunit;
using Moq;
using AltenCancunResort.Services;
using AltenCancunResort.Models;
using AltenCancunResort.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AltenCancunResort_UT
{
    public class GuestControllerTests
    {
        [Fact(DisplayName = "The user has to be presented with the created Guest via URL.")]
        public void CreateGuestTest()
        {
            var mockServiceGuest = new Mock<IGuestService>();
            var mockServiceLogger = new Mock<ILogger<GuestController>>();
            var idInputObj = new CreateGuestInput
            {
                FirstName = "First",
                LastName = "Last"
            };
            var guestOutputTask = Task<GuestOutput>.Run(()=>{
                var guestOutput = new GuestOutput
                {
                    GuestID = 123,
                    FirstName = "First",
                    LastName = "Last",
                    Active = true
                };
                return guestOutput;
            });
            mockServiceGuest.Setup(guestService => guestService.CreateGuest(idInputObj)).Returns(guestOutputTask);
            var controller = new GuestController(mockServiceGuest.Object, mockServiceLogger.Object);

            var result = controller.Create(idInputObj);
            
            Assert.IsType(typeof(CreatedAtRouteResult), result.Result);
            var actionResult = result.Result as CreatedAtRouteResult;
            Assert.True(actionResult.RouteName == "GetByID");
            Assert.True(actionResult.RouteValues.ContainsKey("GuestID"));
        }

        [Fact(DisplayName = "The user has to receive an ok response if guest exists.")]
        public void GetGuestByIDTest()
        {
            var mockServiceGuest = new Mock<IGuestService>();
            var mockServiceLogger = new Mock<ILogger<GuestController>>();
            var idInputObj = new GuestIdInput
            {
                GuestID = 224
            };
            var guestOutputTask = Task<GuestOutput>.Run(()=>{
                var guestOutput = new GuestOutput
                {
                    GuestID = 224,
                    FirstName = "First",
                    LastName = "Last",
                    Active = true
                };
                return guestOutput;
            });
            mockServiceGuest.Setup(guestService => guestService.GetGuestById(idInputObj)).Returns(guestOutputTask);
            var controller = new GuestController(mockServiceGuest.Object, mockServiceLogger.Object);

            var result = controller.GetByID(idInputObj);
            
            Assert.IsType(typeof(OkObjectResult), result.Result);
        }

        [Fact(DisplayName = "The user has to receive a notfound response if guest does not exist.")]
        public void GetNotFoundGuestByIdTest()
        {
            var mockServiceGuest = new Mock<IGuestService>();
            var mockServiceLogger = new Mock<ILogger<GuestController>>();
            var idInputObj = new GuestIdInput
            {
                GuestID = 224
            };
            var guestOutputTask = Task<GuestOutput>.Run(()=>{
                GuestOutput output = null;
                return output;
            });
            mockServiceGuest.Setup(guestService => guestService.GetGuestById(idInputObj)).Returns(guestOutputTask);
            var controller = new GuestController(mockServiceGuest.Object, mockServiceLogger.Object);

            var result = controller.GetByID(idInputObj);
            
            Assert.IsType(typeof(NotFoundResult), result.Result);
        }
    
        [Fact(DisplayName = "The user has to receive a no content result if update was successful.")]
        public void UpdateGuestStatusTest()
        {
            var mockServiceGuest = new Mock<IGuestService>();
            var mockServiceLogger = new Mock<ILogger<GuestController>>();
            var updateGuestInput = new UpdateGuestInput
            {
                GuestID = 231,
                Status = false
            };
            var guestOutputTask = Task<UpdateGuestStatusOutput>.Run(()=>{
                var output = new UpdateGuestStatusOutput{
                    IsGuestUpdated = true
                };
                return output;
            });
            mockServiceGuest.Setup(guestService => guestService.UpdateGuestStatus(It.IsAny<UpdateGuestInput>())).Returns(guestOutputTask);
            var controller = new GuestController(mockServiceGuest.Object, mockServiceLogger.Object);

            var result = controller.UpdateGuestStatus(It.IsAny<UpdateGuestInput>());
            
            Assert.IsType(typeof(NoContentResult), result.Result);
        }

        [Fact(DisplayName = "The user has to receive a bad request response if guest does not exist.")]
        public void GuestNotFoundUpdateStatusTest()
        {
            var mockServiceGuest = new Mock<IGuestService>();
            var mockServiceLogger = new Mock<ILogger<GuestController>>();
            var updateGuestInput = new UpdateGuestInput
            {
                GuestID = 231,
                Status = false
            };
            var guestOutputTask = Task<UpdateGuestStatusOutput>.Run(()=>{
                UpdateGuestStatusOutput output = null;
                return output;
            });
            mockServiceGuest.Setup(guestService => guestService.UpdateGuestStatus(It.IsAny<UpdateGuestInput>())).Returns(guestOutputTask);
            var controller = new GuestController(mockServiceGuest.Object, mockServiceLogger.Object);

            var result = controller.UpdateGuestStatus(It.IsAny<UpdateGuestInput>());
            
            Assert.IsType(typeof(BadRequestObjectResult), result.Result);
        }
    }
}
