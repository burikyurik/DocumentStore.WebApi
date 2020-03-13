using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentStore.Application.Query;
using DocumentStore.Application.QueryHandlers;
using DocumentStore.Application.Validation;
using DocumentStore.Infrastructure;
using DocumentStore.Infrastructure.Models;
using Moq;
using NUnit.Framework;

namespace DocumentStore.Application.UnitTests
{
    public class GetDocumentQueryHandlerTest
    {
        private GetDocumentQueryHandler handler;
        private Mock<IFileRepository> fileRepository;
        private Mock<IDomainRepository> domainRepository;
        [SetUp]
        public void Setup()
        {
            fileRepository = new Mock<IFileRepository>();
            domainRepository = new Mock<IDomainRepository>();
            handler = new GetDocumentQueryHandler(fileRepository.Object, domainRepository.Object);
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenFileDoesntExist()
        {
            string location = "testLocation";
            domainRepository.Setup(repository => repository.GetDocumentByLocation(location)).ReturnsAsync(() => null);
            var exception = Assert.ThrowsAsync<ValidationException>(() =>
                handler.Handle(new DownloadDocumentQuery(location), CancellationToken.None));
            Assert.That(exception.Message, Is.EqualTo($"Document with Location: {location} doesn't exist."));
        }

        [Test]
        public async Task ShouldReturnDocumentDtoWhenExist()
        {
            string location = "testLocation";
            byte[] fileData = new byte[1024];
            var document = new Document(Guid.Empty, "test.pdf",location, fileData.Length,String.Empty);
            domainRepository.Setup(repository => repository.GetDocumentByLocation(location)).ReturnsAsync(document);
            fileRepository.Setup(repository => repository.GetFile(location)).ReturnsAsync(fileData);

            var dto = await handler.Handle(new DownloadDocumentQuery(location), CancellationToken.None);
            Assert.NotNull(dto);
            Assert.That(dto.Name, Is.EqualTo(document.Name));
            Assert.That(dto.ContentType, Is.EqualTo(document.ContentType));
            Assert.That(dto.Length, Is.EqualTo(document.FileSize));
        }
    }
}