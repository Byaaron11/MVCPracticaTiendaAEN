using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPracticaTiendaAEN.Models
{
    [Table("vistapedidos")]
    public class VistaPedido
    {
        [Key]
        [Column("IDVISTAPEDIDOS")]
        public int IdPedido { get; set; }
        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Apellidos")]
        public string Apellidos { get; set; }
        [Column("Titulo")]
        public string Titulo { get; set; }
        [Column("PRECIO")]
        public int Precio { get; set; }
        [Column("Portada")]
        public string Portada { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
        [Column("PRECIOFINAL")]
        public int PrecioFinal { get; set; }

    }
}
