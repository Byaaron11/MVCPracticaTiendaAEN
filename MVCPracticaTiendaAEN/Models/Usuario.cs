using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPracticaTiendaAEN.Models
{
    [Table("USuarios")]
    public class Usuario
    {
        [Key]
        [Column("IDUSUARIO")]
        public int Idusuario { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("Apellidos")]
        public string Apellidos { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Pass")]
        public string Pass { get; set; }
        [Column("Foto")]
        public string Foto { get; set; }
    }
}
