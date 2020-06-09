namespace ApiPeliculas.Models.Dtos
{
    public class UsuarioDto
    {
        public string UsuarioAcceso { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}
