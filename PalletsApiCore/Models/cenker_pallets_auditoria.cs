using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalletsApiCore.Models
{
    [Table("cenker_pallets_auditoria", Schema = "web")]
    public class cenker_pallets_auditoria
    {
        [Key]
        [Column("id")]
        public Guid id { get; set; }
        [Column("fecha")]
        public DateTime fecha { get; set; }
        [Column("evento", TypeName = "character varying")]
        public string evento { get; set; }
        [Column("objeto", TypeName = "character varying")]
        public string objeto { get; set; }
        [Column("elemento_asociado")]
        public Guid elemento_asociado { get; set; }
        [Column("valor_anterior", TypeName = "character varying")]
        public string valor_anterior { get; set; }
        [Column("valor_actual", TypeName = "character varying")]
        public string valor_actual { get; set; }
        [Column("usuario", TypeName = "character varying")]
        public string usuario { get; set; }
    }
}
