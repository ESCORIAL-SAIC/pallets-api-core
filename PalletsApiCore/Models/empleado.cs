using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalletsApiCore.Models
{
    [Table("empleado", Schema = "public")]
    public class empleado
    {
        [Key]
        [Column("id")]
        public Guid id { get; set; }
        [Column("boextension_id")]
        public Guid boextension_id { get; set; }
        [Column("enteasociado_id")]
        public Guid enteasociado_id { get; set; }
        [Column("activestatus")]
        public int activestatus { get; set; }
    }
}
