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
        public double ValorPagado { get; set; }
        public Estado Estados { get; set; }


        //Este metodo se utiliza para calcular el valor del gasto
        public double CalcularValorGasto(double valorPago)
        {
            if (Valor > 0 && valorPago > 0)
            {
                ValorPagado += valorPago;
                double nuevoValor = (double)(Valor - ValorPagado);
                Valor = nuevoValor < 0 ? 0 : nuevoValor; // Asegurar que no sea negativo
                ActualizacionPagos(); // Actualizamos el estado tras cada cálculo
                return (double)Valor;
            }
            return 0;
        }

        //Este metodo se utiliza para cambiar el estado de los gastos
        public void ActualizacionPagos()
        {
            DateTime fechaActual = DateTime.Today;  // Solo tomamos la fecha, sin la hora
            DateTime fechaFinalSinHora = FechaFinal.Date; // Comparamos solo la fecha, sin la hora

            if (Valor != null && ValorPagado == Valor) // Si ya está todo pagado
            {
                Estados = Estado.Finalizado;
            }
            else if (fechaActual > fechaFinalSinHora && Valor > 0) // Fecha ha pasado y no se ha pagado completamente
            {
                Estados = Estado.Atrasado;
            }
            else if (fechaActual <= fechaFinalSinHora && Valor > 0) // Todavía dentro de la fecha final, pero no se ha pagado todo
            {
                Estados = Estado.Pendiente;
            }
        }
        //Este metodo se utiliza para validar el estado finalizado
        public void ValidarValor() 
        {
            ActualizacionPagos();
        }
    }
}
