using Microsoft.AspNetCore.Http;

namespace ApiPeliculas.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Este es un método capturador de errores
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public static void AddAplicationError(this HttpResponse response, string message)
        {
            // Vamos a capturar el error en "Application-Error"
            response.Headers.Add("Application-Error", message);
            
            //Las siguinetes líneas son para que la línea de Application-Error funcione.

            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            // Es * para que el origen de acceso sea cualquiera, todos.
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
