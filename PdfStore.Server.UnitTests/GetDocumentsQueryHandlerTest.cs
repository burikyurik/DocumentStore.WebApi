using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentStore.Application.Query;
using DocumentStore.Application.QueryHandlers;
using DocumentStore.Infrastructure;
using DocumentStore.Infrastructure.Models;
using Moq;
using NUnit.Framework;

namespace DocumentStore.Application.UnitTests
{
    public class GetDocumentsQueryHandlerTest
    {
        private GetDocumentsQueryHandler handler;
        private ISortHelper<Document> sortHelper;
        private Mock<IDomainRepository> domainRepository;
        [SetUp]
        public void Setup()
        {
            sortHelper = new SortHelper<Document>();
            domainRepository = new Mock<IDomainRepository>();
            handler = new GetDocumentsQueryHandler(domainRepository.Object, sortHelper);
        }

        [Test]
        public async Task ShouldReturnDocumentInfoDtoList()
        {
            var documents = new[]
            {
                new Document(Guid.NewGuid(), "tes", "testLocation", 1024, string.Empty)
            };
            domainRepository.Setup(repository => repository.GetDocuments(It.IsAny<string>())).ReturnsAsync(documents);
            var dtos = await handler.Handle(new GetDocumentsQuery("Name"), CancellationToken.None);
            Assert.That(dtos.Count,Is.EqualTo(documents.Length));
        }

        [TestCase("name", "Name ascending")]
        [TestCase("Name desc", "Name descending")]
        [TestCase("Location", "Location ascending")]
        [TestCase("location desc", "Location descending")]
        [TestCase("fileSize", "FileSize ascending")]
        [TestCase("FileSize desc", "FileSize descending")]
        [TestCase("WrongPropertyName", "")]
        [TestCase("", "")]
        public async Task ShouldCreateCreateSortQuery(string queryParameter, string expectedSortQuery)
        {
            var sortQuery = sortHelper.CreateSortQuery(queryParameter);
            Assert.That(sortQuery, Is.EqualTo(expectedSortQuery));
        }
    }
}