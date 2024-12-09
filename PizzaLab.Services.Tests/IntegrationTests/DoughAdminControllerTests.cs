namespace PizzaLab.Services.Tests.IntegrationTests
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using NUnit.Framework;
    using PizzaLab.Data;
    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Web.Areas.Admin.Controllers;
    using PizzaLab.Web.ViewModels.Dough;

    internal class DoughAdminControllerTests
    {

        private DoughController doughController;
        private Mock<IDoughService> doughServiceMock;

        [TearDown]
        public void Teardown()
        {
            doughController.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            var doughService = new Mock<IDoughService>();
            doughController = new DoughController(doughService.Object);
        }

        [Test]
        public async Task Add_ValidModel_RedirectsToOptions()
        {
            var model = new AddDoughViewModel { Name = "Test Dough"};

            var result = await doughController.Add(model) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo(nameof(DoughController.Options)));
        }

        [Test]
        public async Task Remove_ReturnsViewWithModel()
        {
            var expectedDoughs = new List<DoughViewModel>
            {
                new DoughViewModel { Id = 1, Name = "Dough 1" },
                new DoughViewModel { Id = 2, Name = "Dough 2" }
            };

            var doughServiceMock = new Mock<IDoughService>();
            doughServiceMock
                .Setup(s => s.GetAllDoughsAsync())
                .ReturnsAsync(expectedDoughs);

            doughController = new DoughController(doughServiceMock.Object);

            var result = await doughController.Remove() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.TypeOf<List<DoughViewModel>>());

            var model = result.Model as List<DoughViewModel>;
            Assert.That(model, Has.Count.EqualTo(2));
            Assert.That(model[0].Name, Is.EqualTo("Dough 1"));
            Assert.That(model[1].Name, Is.EqualTo("Dough 2"));
        }

        [Test]
        public async Task RemoveDough_Get_ReturnsViewWithModel()
        {
            var doughService = new Mock<IDoughService>();
            doughService.Setup(s => s.GetDoughByIdAsync(It.IsAny<int>()))
                        .ReturnsAsync((int doughId) => new DoughViewModel { Id = doughId, Name = "Test Dough" });

            doughController = new DoughController(doughService.Object);

            var doughId = 1;

            var result = await doughController.RemoveDough(doughId) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.TypeOf<DoughViewModel>());

            var model = result.Model as DoughViewModel;
            Assert.That(model.Id, Is.EqualTo(doughId));
            Assert.That(model.Name, Is.EqualTo("Test Dough"));
        }

        [Test]
        public async Task RemoveDough_Get_InvalidId_RedirectsToError404()
        {

            var result = await doughController.RemoveDough(123) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.AreEqual("Error404", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }
    }

}