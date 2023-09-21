using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApi_MySQL.Models
{
    [Table("roles")]
    public class Role
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(50)]
        public string name { get; set; }

        public ICollection<Credential>? credential { get; set; }
    }
}
