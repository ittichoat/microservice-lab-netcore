using BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MyOpenTextECM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _service;

        public DocumentController(IDocumentService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var doc = await _service.GetDocumentAsync(id);
            return doc != null ? Ok(doc) : NotFound();
        }
    }
}


