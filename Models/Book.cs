using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models {
    public class Book {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }
        public required string Title { get; set; }
        public int? PublicationYear { get; set; }
        public required int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public required Author Author { get; set; }
    }
}
