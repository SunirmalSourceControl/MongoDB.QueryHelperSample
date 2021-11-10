using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB;
using System.Threading.Tasks;

namespace SampleAppForMongoDBQueryHelper
{
    public class PersonService
    {
        private readonly IMongoCollection<Person> _perston;
        public PersonService(ISchoolDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _perston = database.GetCollection<Person>("IndexTest");
        }
        /// <summary>
        /// 
        /// </summary>
        public void PrintIndexes()
        {
            var indexes = _perston.Indexes.List();
            foreach (var index in indexes.ToList())
            {
                Console.WriteLine(index.ToString());
            }
        }
        public void FindBySalary()
        {
            FilterDefinition<Person> fd = "{Salary: -2037871840 }";
            //var cursor = _perston.AsQueryable<Person>()
            //  .Where(x => x.Salary == -2037871840);
            var cursor = from p in _perston.AsQueryable<Person>()
                         where p.Salary == -2037871840
                         select p;

            Console.WriteLine(cursor.ToMongoQueryText());

        }
        public async Task<List<Person>> GetAllAsync()
        {
            var indexes = await _perston.Indexes.ListAsync();
            foreach (var index in indexes.ToList())
            {
                Console.WriteLine(index.ToString());
            }
            return await _perston.Find(s => true).ToListAsync();
        }
        public async void CreateAsync()
        {
            Random random = new Random();
            var name = "NAME-";
            for (int i = 0; i < 10000; i++)
            {
                name += i.ToString();
                var salary = 10000 * random.Next();
                Person p = new Person
                {
                    Name = name,
                    Salary = salary
                };
                await _perston.InsertOneAsync(p);
            }
        }
    }
}
