using Microsoft.EntityFrameworkCore;


namespace LibraryManagementSystem.Data {
    public class LibraryContext : DbContext{
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options) { }

        public DbSet<Models.Author> Authors { get; set; }
        public DbSet<Models.Book> Books { get; set; }
    }
}
