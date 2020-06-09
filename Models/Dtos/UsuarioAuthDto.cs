using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class UsuarioAuthDto
    {        
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="El usuario es obligatorio.")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "El password es obligatorio.")]
        [StringLength(10,MinimumLength =4, ErrorMessage ="La contraseña debe estár entre 4 y caracteres.")]
        public string Password { get; set; }
    }
}
