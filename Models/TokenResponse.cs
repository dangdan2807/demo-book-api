﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApi_MySQL.Models
{
    [Table("token_responses")]
    public class TokenResponse
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("access_token")]
        [StringLength(255)]
        public string accessToken { get; set; } = "";

        [Column("refresh_token")]
        [StringLength(255)]
        public string refreshToken { get; set; } = "";

        [Column("expiration_time")]
        public DateTime expRefreshToken { get; set; }

        [Column("is_revoke")]
        public bool isRevoke { get; set; } = false;

        [ForeignKey("User")]
        [Column("user_id")]
        [Required]
        public int userId { get; set; }

        public User? User { get; set; }
    }
}
