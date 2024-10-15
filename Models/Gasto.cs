using System.ComponentModel.DataAnnotations;

namespace FogachoReveloProyecto.Models
{
    public class Gasto
    {
        [Key]
        public int IdGasto { get; set; }
        [Required]
        public DateTime FechaRegristo { get; set; }
        [Required]
        public DateTime FechaFinal { get; set; }
        [Required]
        public CategoriaGasto Categorias { get; set; }
        [Required]
        public string? Descripcion { get; set; }
        [Required]
        public float? Valor { get; set; }
        public Estado Estados { get; set; }
        

    }
}
