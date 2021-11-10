﻿using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MongoDB.Kennedy;
using SampleAppForMongoDBQueryHelper;

namespace MongoDB.QueryHelper.Tests.Data
{
	public class TestMongoContext : MongoDbDataContext
	{
		public TestMongoContext() : base("DemoDatabase", "127.0.0.1",27017)
		{
		}

		public MongoCollection<Person> PeopleCollection
		{
			get { return this.GetMongoCollection<Person>(); }
		}

		public IQueryable<Person> People
		{
			get { return this.PeopleCollection.AsQueryable(); }
		}

		public static void BuildTestData()
		{
			var p1 = new Person()
			{
				Age = 31,
				Name = "Bill"
			};
			var p2 = new Person()
			{
				Age = 32,
				Name = "Ted"
			};
			var p3 = new Person()
			{
				Age = 42,
				Name = "Ralph"
			};
			var p4 = new Person()
			{
				Age = 20,
				Name = "Zoe"
			};

			var mongo = new TestMongoContext();
			mongo.PeopleCollection.RemoveAll();
			mongo.Save(p1);
			mongo.Save(p2);
			mongo.Save(p3);
			mongo.Save(p4);

			p1.FriendIds.Add(p2.Id);
			p1.FriendIds.Add(p3.Id);
			p2.FriendIds.Add(p1.Id);

			mongo.Save(p1);
			mongo.Save(p2);
		}

		public static void AddIndexes()
		{
			var mongo = new TestMongoContext();
			mongo.PeopleCollection.CreateIndex(IndexKeys<Person>.Ascending(p => p.Age));
		}

		public static void DropIndexes()
		{
			var mongo = new TestMongoContext();
			mongo.PeopleCollection.DropAllIndexes();
		}
	}
}
