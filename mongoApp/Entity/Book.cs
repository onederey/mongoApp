using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongoApp.Entity
{
	public class Book
	{
		public Book(string name, string author, int? count, string genre, int? year)
		{
			Name = name;
			Author = author;
			Count = count;
			Genre = new List<string>()
			{
				genre
			};
			Year = year;
		}
		[BsonId]
		public ObjectId Id { get; set; }
		public string Name { get; set; }
		public string Author { get; set; }
		public int? Count { get; set; }
		public ICollection<string> Genre { get; set; }
		public int? Year { get; set; }
	}
}
