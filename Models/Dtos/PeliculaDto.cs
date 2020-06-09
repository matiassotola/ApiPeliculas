﻿using System.ComponentModel.DataAnnotations;
using static ApiPeliculas.Models.Pelicula;

namespace ApiPeliculas.Models.Dtos
{
    public class PeliculaDto
    {        
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El ruta imágen es obligatorio.")]
        public string RutaImagen { get; set; }
        [Required(ErrorMessage = "El descripción es obligatorio.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El duración es obligatorio.")]
        public string Duracion { get; set; }        
        public TipoClasificacion Clasificacion { get; set; }                
        public int categoriaId { get; set; }        
        public Categoria Categoria { get; set; }
    }
}
