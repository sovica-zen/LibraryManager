using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models {
    public class Book {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }
        [Required]
        [StringLength(255)]
        public required string Title { get; set; }
        [Range(-5000, 2200)]
        public int? PublicationYear { get; set; }
        [Required]
        public required int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public required Author Author { get; set; }
    }
}
