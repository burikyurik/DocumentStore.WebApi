using System.Threading;
using System.Threading.Tasks;
using DocumentStore.Application.Command;
using DocumentStore.Application.Dtos;
using DocumentStore.Application.Query;
using DocumentStore.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DocumentStore.WebApi.UnitTests
{
    public class DocumentControllerTests
    {
        private DocumentController controller;
        private Mock<IMediator> mediator;
        [SetUp]
        public void Setup()
        {
            mediator=new Mock<IMediator>();
            controller=new DocumentController(mediator.Object);
        }

        [Test]
        public async Task GetDocumentsShouldSendGetDocumentsQuery()
        {
            string orderParameter = "name";
            IActionResult response =await controller.Get(new DocumentParameters {OrderBy = orderParameter});
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            mediator.Verify(x => x.Send(It.IsAny<GetDocumentsQuery>(), It.IsAny<CancellationToken>()),Times.Once);
        }

        [Test]
        public async Task GetByLocationShouldSendDownloadDocumentQuery()
        {
            mediator.Setup(x => x.Send(It.IsAny<DownloadDocumentQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DocumentDto("test", "image/jpeg", 100, new byte[100]));
            IActionResult response = await controller.GetByLocation("testLocation");
            Assert.That(response, Is.InstanceOf<FileResult>());
            mediator.Verify(x => x.Send(It.IsAny<DownloadDocumentQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task AddDocumentShouldSendUploadDocumentCommand()
        {
            IActionResult response = await controller.AddDocument(Mock.Of<IFormFile>());
            Assert.That(response, Is.InstanceOf<CreatedResult>());
            mediator.Verify(x => x.Send(It.IsAny<UploadDocumentCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteShouldSendDeleteDocumentCommand()
        {
            IActionResult response = await controller.Delete("testLocation");
            Assert.That(response, Is.InstanceOf<OkResult>());
            mediator.Verify(x => x.Send(It.IsAny<DeleteDocumentCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}