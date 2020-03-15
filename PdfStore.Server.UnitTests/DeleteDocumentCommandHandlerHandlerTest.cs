using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentStore.Application.Command;
using DocumentStore.Application.CommandHandlers;
using DocumentStore.Application.Validation;
using DocumentStore.Infrastructure;
using DocumentStore.Infrastructure.Models;
using Moq;
using NUnit.Framework;

namespace DocumentStore.Application.UnitTests
{
    public class DeleteDocumentCommandHandlerTest
    {
        private DeleteDocumentCommandHandler handler;

        private Mock<IFileRepository> fileRepository;
        private Mock<IDomainRepository> domainRepository;

        [SetUp]
        public void Setup()
        {
            fileRepository = new Mock<IFileRepository>();
            domainRepository = new Mock<IDomainRepository>();
            handler = new DeleteDocumentCommandHandler(fileRepository.Object, domainRepository.Object);
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenFileDoesNtoExist()
        {
            string location = "testLocation";
            domainRepository.Setup(repository => repository.GetDocumentByLocation(location)).ReturnsAsync(() => null);
            var exception = Assert.ThrowsAsync<ValidationException>(() =>
                handler.Handle(new DeleteDocumentCommand(location), CancellationToken.None));
            Assert.That(exception.Message, Is.EqualTo($"Document with Location: {location} doesn't exist."));
        }

        [Test]
        public async Task ShouldDeleteFile()
        {
            string location = "testLocation";
            var document = new Document(Guid.Empty, "test.pdf", location, 1024, string.Empty);
            domainRepository.Setup(repository => repository.GetDocumentByLocation(location)).ReturnsAsync(document);

            await handler.Handle(new DeleteDocumentCommand(location), CancellationToken.None);

            fileRepository.Verify(repository => repository.DeleteFile(document.Name, CancellationToken.None),Times.Once);
            domainRepository.Verify(repository => repository.Delete(document, CancellationToken.None), Times.Once);
        }
    }
}