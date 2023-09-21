using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApi_MySQL.Models
{
    [Table("credentials")]
    public class Credential
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("user_group_id")]
        [ForeignKey("UserGroup")]
        public int userGroupId { get; set; }

        [Column("role_id")]
        [ForeignKey("Role")]
        public int roleId { get; set; }

        public UserGroup userGroup { get; set; }
        public Role role { get; set; }
    }
}
