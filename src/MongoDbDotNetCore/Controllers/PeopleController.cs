using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbDotNetCore.Models;
using System.Threading.Tasks;

namespace MongoDbDotNetCore.Controllers
{
    public class PeopleController : Controller
    {
        private IMongoCollection<Person> people;

        public PeopleController(MongoClient client)
        {
            var database = client.GetDatabase("mongodbdotnetcore");
            people = database.GetCollection<Person>(nameof(people));
        }
        // GET: People
        public async Task<ActionResult> Index()
        {
            var allPeopleCursor = await people.FindAsync(FilterDefinition<Person>.Empty);
            var allPeople = await allPeopleCursor.ToListAsync();
            return base.View(allPeople);
        }

        // GET: People/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var person = await people.FindAsync(Builders<Person>.Filter.Eq(p => p.Id, ObjectId.Parse(id)));
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Person person)
        {
            await people.InsertOneAsync(person);
            return RedirectToAction(nameof(Index));
        }

        // GET: People/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var personCursor = await people.FindAsync(Builders<Person>.Filter.Eq(p => p.Id, ObjectId.Parse(id)));
            var person = await personCursor.FirstOrDefaultAsync();
            return base.View(person);
        }

        // POST: People/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Person person)
        {
            if (!ModelState.IsValid)
                return View();
            person.Id = ObjectId.Parse(id);
            await people.ReplaceOneAsync(Builders<Person>.Filter.Eq(p => p.Id, person.Id), person);
            return RedirectToAction(nameof(Index));
        }

        // GET: People/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            await people.DeleteOneAsync(Builders<Person>.Filter.Eq(p => p.Id, ObjectId.Parse(id)));
            return RedirectToAction(nameof(Index));
        }
    }
}