using bookDemo.Models;

namespace bookDemo.Data
{
    public static class ApplicationContext
    {
        public static List<Book> Books { get; set; }
        static ApplicationContext()
        {
            Books = new List<Book>()
            {
                new Book(){Id=1,Title = "Martin Eden",Price = 75},
                new Book(){Id=2,Title = "Vişne Bahçesi",Price = 105},
                new Book(){Id=3,Title = "Kamelyalı Kadın",Price = 75}
            };
        }
    }
}
