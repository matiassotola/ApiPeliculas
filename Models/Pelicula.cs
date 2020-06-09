using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Models
{
    public class Pelicula
    {
        /*Si nosotros le quisieramos poner IdPelicula al campo, deberíamos renombrar y
         * agregar una data annotation para decirle a Entity Framework que esa es la
         * clave primaria de la tabla.
         * Al dejarlo con el nombre Id internamente Entity se da cuenta que es una clave primaria
         * y la vuelve incremental*/
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string RutaImagen { get; set; }
        public string Descripcion { get; set; }
        public string Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        
        public int categoriaId { get; set; }
        [ForeignKey("categoriaId")]
        public Categoria Categoria { get; set; }
    }
}
