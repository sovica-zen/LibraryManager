namespace LibraryManagementSystem.Models {
    public class Book {
        public int ID { get; set; }
        public string? Title { get; set; }
        public int? PublicationYear { get; set; }

        public int? AuthorId { get; set; }
    }
}
