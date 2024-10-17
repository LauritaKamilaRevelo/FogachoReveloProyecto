using System.ComponentModel.DataAnnotations;

namespace FogachoReveloProyecto.Models
{
    public class Gasto
    {
        //Atributos
        [Key]
        public int IdGasto { get; set; }
        [Required]
        public DateTime FechaRegristo { get; set; }
        [Required]
        public DateTime FechaFinal { get; set; }
        [Required]
        public Categoria Categorias { get; set; }
        [Required]
        public string? Descripcion { get; set; }
        [Required]
        public double? Valor { get; set; }
        [Required]
        public double? ValorPagado { get; set; }
        public Estado Estados { get; set; }


        //Este metodo se utiliza para calcular el valor del gasto
        public double CalcularValorGasto(double NuevoValor)
        {
            if (Valor > 0 && ValorPagado > 0)
            {
                NuevoValor = (double)(Valor - ValorPagado);
                return (double) (Valor = NuevoValor);
            }
            return 0;
        }

        //Este metodo se utiliza para cambiar el estado de los gastos
        public void ActualizacionPagos() 
        {
            var FechaActual = DateTime.Today;
            if(Valor == 0){ 
                Estados = Estado.Finalizado;
            }
            if (FechaActual <= FechaFinal) {
                Estados = Estado.Pendiente;
            } else 
            { 
                Estados = Estado.Atrasado;
            }
        }
        //Este metodo se utiliza para validar el estado finalizado
        public void ValidarValor() 
        {
            if (Valor == 0) {
                Estados = Estado.Finalizado;
            } else if (Estados == Estado.Finalizado) {
                ActualizacionPagos();
            }
        }
    }
}
