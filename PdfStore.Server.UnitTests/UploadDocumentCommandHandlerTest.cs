using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentStore.Application.Command;
using DocumentStore.Application.CommandHandlers;
using DocumentStore.Application.Dtos;
using DocumentStore.Infrastructure;
using DocumentStore.Infrastructure.Models;
using Moq;
using NUnit.Framework;

namespace DocumentStore.Application.UnitTests
{
    public class UploadDocumentCommandHandlerTest
    {
        private UploadDocumentCommandHandler handler;
        private UploadDocumentCommandValidator validator;
        private Mock<IFileRepository> fileRepository;
        private Mock<IDomainRepository> domainRepository;

        [SetUp]
        public void Setup()
        {
            fileRepository = new Mock<IFileRepository>();
            domainRepository = new Mock<IDomainRepository>();
            validator = new UploadDocumentCommandValidator();
            handler = new UploadDocumentCommandHandler(fileRepository.Object, domainRepository.Object, validator);
            
        }

        [TestCase(1, false)]
        [TestCase(-1, true)]
        [TestCase(0, true)]
        public void ShouldValidateFileSizeMoreThen5Mb(long fileSizeDifference, bool expectedIsValid)
        {
            const long maxFileSize = 5242880;//5Mb
            var document = new DocumentDto("test.pdf", "testContentType", maxFileSize + fileSizeDifference, new byte[maxFileSize + fileSizeDifference]);
            var validation = validator.Validate(new UploadDocumentCommand(document));
            Assert.That(validation.IsValid,Is.EqualTo(expectedIsValid));
            if(!expectedIsValid)
                Assert.That(validation.Errors.First().ErrorMessage, Is.EqualTo($"File too large, limit is {maxFileSize} Byte"));
        }

        [TestCase(".png",false)]
        [TestCase(".doc",false)]
        [TestCase(".png",false)]
        [TestCase(".pdf", true)]
        public void ShouldValidateFileType(string fileExtension, bool expectedIsValid)
        {
            var document = new DocumentDto($"test{fileExtension}", "testContentType", 100, new byte[100]);
            var validation = validator.Validate(new UploadDocumentCommand(document));
            Assert.That(validation.IsValid,Is.EqualTo(expectedIsValid));
            if(!expectedIsValid)
                Assert.That(validation.Errors.First().ErrorMessage, Is.EqualTo($"Not Allowed File Extension. Allowed Extension .pdf"));
        }

        [Test]
        public async Task ShouldUploadFile()
        {
            var document = new DocumentDto("test", "testContentType", 100, new byte[100]);
            string location = "testLocation";
            await handler.Handle(new UploadDocumentCommand(document), CancellationToken.None);
            fileRepository
                .Setup(repository => repository.UploadFile(document.Name, document.FileData, document.ContentType))
                .ReturnsAsync(location);
            fileRepository.Verify(repository => repository.UploadFile(document.Name, document.FileData,document.ContentType), Times.Once);
            domainRepository.Verify(repository => repository.Add(It.IsNotNull<Document>(), CancellationToken.None), Times.Once);
        }
    }
}