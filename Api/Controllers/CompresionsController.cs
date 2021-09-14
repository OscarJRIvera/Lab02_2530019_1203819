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
        public IActionResult Descomprimir([FromForm] IFormFile File)
        {
            string Ruta = Path.GetFullPath("ArchivosC\\" + File.FileName);
            string nombre = F.Nombres[File.FileName];
            string Ruta2 = Path.GetFullPath("ArchivosD\\" + nombre);
            FileStream archivoC = new FileStream(Ruta, FileMode.OpenOrCreate);
            File.CopyTo(archivoC);
            archivoC.Close();
            F.Huff.DescompresionRecurrencias(Ruta, Ruta2);
            FileStream ArchivoD = new FileStream(Ruta2, FileMode.OpenOrCreate);
            FileStreamResult ArchivoD2 = new FileStreamResult(ArchivoD, "text/huff");
            return ArchivoD2;
        }
        [HttpPost("compress/{name}")]
        public IActionResult Comprimir([FromForm] IFormFile File, [FromRoute] string name)
        {
            string Ruta = Path.GetFullPath("Archivoso\\" + name + File.FileName);
            string Ruta2 = Path.GetFullPath("ArchivosC\\" + name + ".huff");
            FileStream archivoo = new FileStream(Ruta, FileMode.OpenOrCreate);
            File.CopyTo(archivoo);
            archivoo.Close();
            F.Huff.Recurrencia(Ruta, Ruta2);
            FileStream archivoC = new FileStream(Ruta2, FileMode.OpenOrCreate);
            FileStreamResult ArchivoC2 = new FileStreamResult(archivoC, "text/huff");
            Compresion temp = new Compresion();
            temp.RutaComprimido = Ruta2;
            temp.NombreOriginal = name + File.FileName;
            temp.NombreComprimido = name + ".huff";
            F.Nombres.Add(temp.NombreComprimido, temp.NombreOriginal);
            return ArchivoC2;
        }
    }
}
