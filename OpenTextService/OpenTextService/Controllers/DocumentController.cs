using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace OpenTextAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument(
            [FromHeader(Name = "Authorization")] string authorizationHeader,
            UploadFileRequest form)
        {
            var token = authorizationHeader?.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase)?.Trim();
            if (string.IsNullOrEmpty(token))
                return Unauthorized("Missing Bearer Token");

            var nodeId = await _documentService.UploadDocumentAsync(token, form);
            return Ok(new { NodeId = nodeId });
        }

        [HttpPut("metadata/{nodeId}")]
        public async Task<IActionResult> AttachMetadata(
            [FromHeader(Name = "Authorization")] string authorizationHeader,
            int nodeId,
            [FromBody] MetadataDto metadata)
        {
            var token = authorizationHeader?.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase)?.Trim();
            if (string.IsNullOrEmpty(token))
                return Unauthorized("Missing Bearer Token");

            await _documentService.AttachMetadataAsync(token, nodeId, metadata);
            return NoContent();
        }

        [HttpGet("{nodeId}")]
        public async Task<IActionResult> GetDocument(
            [FromHeader(Name = "Authorization")] string authorizationHeader,
            int nodeId)
        {
            var token = authorizationHeader?.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase)?.Trim();
            if (string.IsNullOrEmpty(token))
                return Unauthorized("Missing Bearer Token");

            var doc = await _documentService.GetDocumentAsync(token, nodeId);
            return Ok(doc);
        }

        [HttpDelete("{nodeId}")]
        public async Task<IActionResult> DeleteDocument(
            [FromHeader(Name = "Authorization")] string authorizationHeader,
            int nodeId)
        {
            var token = authorizationHeader?.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase)?.Trim();
            if (string.IsNullOrEmpty(token))
                return Unauthorized("Missing Bearer Token");

            await _documentService.DeleteDocumentAsync(token, nodeId);
            return NoContent();
        }
    }
}


