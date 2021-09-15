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
            try
            {
                return F.Historial;
            }
            catch
            {
                return null;
            }

        }
        [HttpPost("decompress")]
        public IActionResult Descomprimir([FromForm] IFormFile File)
        {
            try
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
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }

        }
        [HttpPost("compress/{name}")]
        public IActionResult Comprimir([FromForm] IFormFile File, [FromRoute] string name)
        {
            try
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
                FileStream archivoo2 = new FileStream(Ruta, FileMode.Open);
                double razon = (Convert.ToDouble(archivoC.Length) / Convert.ToDouble(archivoo2.Length));
                double factor = (Convert.ToDouble(archivoo2.Length) / Convert.ToDouble(archivoC.Length));
                double porc = (1 - razon) * 100;
                string porcentaje = "" + porc + "%";
                temp.RazonDeCompresion = razon;
                temp.FactorDeCompresion = factor;
                temp.PorcentajeDeReduccion = porcentaje;
                F.Historial.Add(temp);

                return ArchivoC2;
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }

        }
    }
}
