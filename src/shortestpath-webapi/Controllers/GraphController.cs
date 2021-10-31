using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shortestpath_webapi.Services;

namespace shortestpath_webapi.Controllers
{
    [ApiController]
    public class GraphController : ControllerBase
    {
        private readonly IGraphSearchService _graphSearchService;

        public GraphController(IGraphSearchService graphSearchService)
        {
            _graphSearchService = graphSearchService;
        }

        [HttpPost("/connectNode")]
        public IActionResult ConnectNodes([FromBody] RequestModel request)
        {
            if (!request.IsValid())
                return BadRequest();
            
            _graphSearchService.AddConnection(char.ToLower(request.From), char.ToLower(request.To));

            return Ok();
        }

        [HttpGet("/path")]
        public IActionResult ShortestPath([FromQuery] char from, [FromQuery] char to)
        {
            var requestModel = new RequestModel(from, to);

            if (!requestModel.IsValid())
                return BadRequest();

            var result = _graphSearchService.ShortestPath(char.ToLower(from), char.ToLower(to));

            return Ok( new ResponseModel { Path = result});
        }
    }

    public class ResponseModel
    {
        public string Path { get; set; }
    }

    public class RequestModel
    {
        public char From { get; set; }
        public char To { get; set; }

        public RequestModel(char from, char to)
        {
            From = from;
            To = to;
        }

        public bool IsValid()
        {
            return From != To && char.IsLetter(From) && char.IsLetter(To);
        }
    }
}