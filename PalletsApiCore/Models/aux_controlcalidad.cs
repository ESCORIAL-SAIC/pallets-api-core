using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalletsApiCore.Models
{
    [Table("aux_controlcalidad", Schema = "public")]
    public partial class aux_controlcalidad
    {
        [Key]
        public int etiqueta { get; set; }
        public Guid reparador_empleado_id{ get; set; }
        public string reparador_empleado_n { get; set; }
        public bool reparador_estado { get; set; }
        public Guid reparador_falla_id { get; set; }
        public string reparador_falla_n { get; set; }
    }
}
