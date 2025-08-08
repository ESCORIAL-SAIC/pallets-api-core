#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalletsApiCore.Models
{
    [Table("aux_controlcalidad", Schema = "public")]
    public partial class aux_controlcalidad
    {
        [Key]
        public Guid id { get; set; }
        public Guid puestocontrol_id { get; set; }
        public string puestocontrol_n { get; set; }
        public int etiqueta { get; set; }
        public DateTime controlador_fechahora { get; set; }
        public Guid controlador_empleado_id { get; set; }
        public string controlador_empleado_n { get; set; }
        public bool controlador_estado { get; set; }
        public Guid secundario_emplado_id { get; set; }
        public string secundario_emplado_n { get; set; }
    }
}
