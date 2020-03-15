using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DocumentStore.Application.Command;
using DocumentStore.Application.Dtos;
using DocumentStore.Application.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentStore.WebApi.Controllers
{
    /// <summary>
    /// Pdf document store controller
    /// </summary>
    [ApiController]
    [Route("api/v1/documents")]
    public class DocumentController : ControllerBase
    {
        private readonly IMediator mediator;

        public DocumentController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        /// <summary>
        /// Return list of uploaded Pdf documents.
        /// </summary>
        /// <param name="documentParameters"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<DocumentInfoDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] DocumentParameters documentParameters)
        {
            var documents = await mediator.Send(new GetDocumentsQuery(documentParameters.OrderBy));
            return Ok(documents);
        }

        /// <summary>
        /// Download Pdf document by location.
        /// </summary>
        /// <param name="location">Pdf Document location</param>
        /// <returns></returns>
        [Route("{location}")]
        [HttpGet]
        [ProducesResponseType(typeof(File), (int)HttpStatusCode.PartialContent)]
        public async Task<IActionResult> GetByLocation([FromRoute,Required]string location)
        {
            var document = await mediator.Send(new DownloadDocumentQuery(WebUtility.UrlDecode(location)));
            return File(document.FileData, document.ContentType, document.Name);
        }

        /// <summary>
        /// Upload new Pdf Document
        /// </summary>
        /// <param name="document">Pdf Document file data</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> AddDocument([Required]IFormFile document)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fileName = Path.GetFileName(document.FileName);
            var fileData = await ReadFileContent(document);
            var documentDto = new DocumentDto(fileName, document.ContentType, document.Length, fileData);
            await mediator.Send(new UploadDocumentCommand(documentDto));
            return Created(string.Empty, null);
        }

        /// <summary>
        /// Delete Pdf document from storage by location
        /// </summary>
        /// <param name="location">Pdf Document location</param>
        /// <returns></returns>
        [Route("{location}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Delete([FromRoute, Required]string location)
        {
            await mediator.Send(new DeleteDocumentCommand(WebUtility.UrlDecode(location)));
            return Ok();
        }

        private static async Task<byte[]> ReadFileContent(IFormFile document)
        {
            using (var stream = new MemoryStream())
            {
                await document.CopyToAsync(stream);
                return stream.ToArray();
            }
        }
    }

    public abstract class QueryStringParameters
    {
        /// <summary>
        /// Property name
        /// </summary>
        public string OrderBy { get; set; }
    }
    public class DocumentParameters : QueryStringParameters
    {
        public DocumentParameters()
        {
            OrderBy = "Name";
        }
    }
}
