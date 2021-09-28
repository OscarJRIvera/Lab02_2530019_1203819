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
        public IActionResult Descomprimir([FromForm] IFormFile file)
        {
            try
            {
                string filename;
                if (file == null)
                    return BadRequest();
                else
                {
                    filename = file.FileName.Substring(file.FileName.Length - 4, 4);

                }
                string Ruta = Path.GetFullPath("ArchivosC\\" + file.FileName);
                string nombre = F.Nombres[file.FileName];
                string Ruta2 = Path.GetFullPath("ArchivosD\\" + nombre);
                FileStream archivoC = new FileStream(Ruta, FileMode.OpenOrCreate);
                file.CopyTo(archivoC);
                archivoC.Close();
                if (filename == "huff")
                {
                    F.Huff.DescompresionRecurrencias(Ruta, Ruta2);
                    FileStream ArchivoD = new FileStream(Ruta2, FileMode.OpenOrCreate);
                    FileStreamResult ArchivoD2 = new FileStreamResult(ArchivoD, "text/lzw");
                    return ArchivoD2;
                }
                else if (filename.Substring(1, 3) == "lzw")
                {

                    F.lzw.Descomprimir(Ruta, Ruta2);
                    FileStream ArchivoD = new FileStream(Ruta2, FileMode.OpenOrCreate);
                    FileStreamResult ArchivoD2 = new FileStreamResult(ArchivoD, "text/huff");
                    return ArchivoD2;
                }
                else
                {
                    string json = "solo existe descompression para archivos 'huff' o 'lzw'";
                    return BadRequest(json);
                }

            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }

        }
        [HttpPost("{tipodecomp}/compress/{name}")]
        public IActionResult Comprimir([FromForm] IFormFile file, [FromRoute] string name, string tipodecomp)
        {
            try
            {
                if (file == null)
                    return BadRequest();
                string Ruta = Path.GetFullPath("Archivoso\\" + name + file.FileName);
                FileStream archivoo = new FileStream(Ruta, FileMode.OpenOrCreate);
                file.CopyTo(archivoo);
                archivoo.Close();
                if (tipodecomp == "huff")
                {
                    string Ruta2 = Path.GetFullPath("ArchivosC\\" + name + ".huff");
                    F.Huff.Recurrencia(Ruta, Ruta2);
                    FileStream archivoC = new FileStream(Ruta2, FileMode.OpenOrCreate);
                    FileStreamResult ArchivoC2 = new FileStreamResult(archivoC, "text/huff");
                    Compresion temp = new Compresion();
                    temp.RutaComprimido = Ruta2;
                    temp.NombreOriginal = name + file.FileName;
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
                else if (tipodecomp == "lzw")
                {
                    string Ruta2 = Path.GetFullPath("ArchivosC\\" + name + ".lzw");
                    F.lzw.Comprimir(Ruta, Ruta2);
                    FileStream archivoC = new FileStream(Ruta2, FileMode.OpenOrCreate);
                    FileStreamResult ArchivoC2 = new FileStreamResult(archivoC, "text/lzw");
                    Compresion temp = new Compresion();
                    temp.RutaComprimido = Ruta2;
                    temp.NombreOriginal = name + file.FileName;
                    temp.NombreComprimido = name + ".lzw";
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
                else
                {
                    string json = "solo existe compression 'huff' o 'lzw'";
                    return BadRequest(json);
                }

            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
