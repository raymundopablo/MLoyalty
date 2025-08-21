using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace backEnd.Helpers
{
    public class FileHelper
    {
        public string SubirArchivo(string Host, string Carpeta, string NombreArchivo, IFormFile archivo)
        {
            string extension = string.Empty;

            string path = Path.Combine(Host, Carpeta);

            string rutaArchivo = Carpeta + "/" + NombreArchivo;

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (FileStream stream = new(path + "/" + NombreArchivo, FileMode.Create))
                {
                    archivo.CopyTo(stream);
                    stream.Close();
                }

                return rutaArchivo;

            }
            catch (Exception ex)
            {

                string msj = ex.Message;
            }


            return "";
        }


        public void EliminarArchivo(string Host, string RutaArchivo)
        {
            string path = Path.Combine(Host, RutaArchivo);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
