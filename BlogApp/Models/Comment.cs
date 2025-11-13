using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yorum metni zorunludur!")]
        [StringLength(500, ErrorMessage = "Yorum maksimum 500 karakter olabilir!")]
        public string Text { get; set; } = string.Empty;

        public int PostId { get; set; }
        public Post? Post { get; set; }
    }
}