using System;
using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models
{
    public class Categoria
    {
        /*Si nosotros le quisieramos poner IdCategoria al campo, deberíamos renombrar y
         * agregar una data annotation para decirle a Entity Framework que esa es la
         * clave primaria de la tabla.
         * Al dejarlo con el nombre Id internamente Entity se da cuenta que es una clave primaria
         * y la vuelve incremental*/
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
