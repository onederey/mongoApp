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
	public class MongoHelper
	{
		private readonly IMongoCollection<Book> _booksCollection;

		public MongoHelper(MongoClientSettings settings, string dbName, string collectionName)
		{
			var mongoClient = new MongoClient(settings);
			var mongoDatabase = mongoClient.GetDatabase(dbName);
			_booksCollection = mongoDatabase.GetCollection<Book>(collectionName);
		}

		public void AddBook(Book book) => _booksCollection.InsertOne(book);

		public long GetBooksWithManyCopys()
		{
			return _booksCollection
				.Find(x => x.Count > 1)
				.Project(x => x.Name)
				.SortBy(x => x.Name)
				.Limit(3)
				.CountDocuments();
		}

		public Book GetBookWithMinMaxCount(SortEnum sort)
		{
			switch(sort)
			{
				case SortEnum.Min:
					return _booksCollection
						.Find(x => x.Count.HasValue)
						.SortBy(x => x.Count)
						.First();
				case SortEnum.Max:
					return _booksCollection
						.Find(x => x.Count.HasValue)
						.SortByDescending(x => x.Count)
						.First();
			}

			return null;
		}

		public ICollection<string> GetAuthors()
		{
			return _booksCollection
				.Distinct(x => x.Author, FilterDefinition<Book>.Empty)
				.ToList();
		}

		public ICollection<Book> GetBooksWithoutAuthors()
		{
			return _booksCollection
				.Find(x => string.IsNullOrEmpty(x.Author))
				.ToList();
		}

		public void IncreaseBookCount() => _booksCollection
			.UpdateMany(x => x.Count.HasValue, Builders<Book>.Update.Inc("Count", 1));


		public void AddAdditionalGenre() => _booksCollection
			.UpdateMany(x => x.Genre.Contains("fantasy") && !x.Genre.Contains("favority"), Builders<Book>.Update.Push("Genre", "favority"));

		public void DeleteBooksWithCount() => _booksCollection.DeleteMany(x => x.Count < 3);

		public void DeleteAllBooks() => _booksCollection.DeleteMany(x => !string.IsNullOrEmpty(x.Name));
	}
}
