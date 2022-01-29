using mongoApp.Entity;
using mongoApp.Enums;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongoApp
{
	class Program
	{
		public static List<Book> GetBooks()
		{
			var booksList = new List<Book>()
			{
				new Book("Hobbit", "Tolkien", 5, "fantasy", 2014),
				new Book("Lord of the rings", "Tolkien", 3, "fantasy", 2015),
				new Book("Kolobok", null, 10, "kids", 2000),
				new Book("Repka", null, 11, "kids", 2000),
				new Book("Dyadya Stiopa", "Mihalkov", 1, "kids", 2001)
			};

			return booksList;
		}

		static void Main(string[] args)
		{
			//change password or conn string to local !!!
			MongoHelper helper = new MongoHelper(MongoClientSettings.FromConnectionString("mongodb+srv://andrey:<password>@cluster0.ypfkr.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"),
				"mongoApp",
				"books");


			foreach (Book book in GetBooks())
			{
				Console.WriteLine($"Adding book with name - {book.Name}");
				helper.AddBook(book);
			}


			var copys = helper.GetBooksWithManyCopys();
			Console.WriteLine($"GetBooksWithManyCopys - return {copys}");


			var minCountBook = helper.GetBookWithMinMaxCount(SortEnum.Min);
			var maxCountBook = helper.GetBookWithMinMaxCount(SortEnum.Max);
			Console.WriteLine($"Min count book - {minCountBook.Name}");
			Console.WriteLine($"Max count book - {maxCountBook.Name}");


			var authors = helper.GetAuthors();
			foreach(var author in authors)
				Console.WriteLine($"{author ?? "<Empty>"} - returned from collection");


			helper.IncreaseBookCount();
			helper.AddAdditionalGenre();
			helper.AddAdditionalGenre();


			Console.WriteLine("Deleting all books");
			helper.DeleteAllBooks();


			Console.ReadLine();
		}
	}
}
