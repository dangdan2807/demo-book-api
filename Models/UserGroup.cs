using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApi_MySQL.Models
{
    [Table("user_groups")]
    public class UserGroup
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(50)]
        public string name { get; set; } = null!;


        [ForeignKey("User")]
        [Column("user_id")]
        public int userId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Credential>? credential { get; set; }

    }
}
