using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalletsApiCore.Models
{
    public class v_persona
    {
        [Key]
        [Column("id")]
        public Guid id { get; set; }
        [Column("nombre")]
        public string nombre { get; set; }
    }
}
