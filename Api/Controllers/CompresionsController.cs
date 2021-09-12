using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
namespace Api.Controllers
{
    [Route("Api")]
    [ApiController]
    public class CompresionsController : Controller
    {
        private readonly Models.Data.Singleton F = Models.Data.Singleton.Instance;
        [HttpGet]
        public string Get()
        {
            string xd = "Lab02";

            return xd;
        }
        [HttpGet("compressions")]
        public IEnumerable<Compresion> reccorrerarchivos()
        {
            return F.Historial;
        }
        [HttpPost("decompress")]
        [HttpPost("compress/{name}")]
        public IActionResult Compress([FromForm] IFormFile File,[FromForm] string name)
        {


        }
    }
}
