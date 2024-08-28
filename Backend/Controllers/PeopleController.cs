using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private IPeopleService _peopleService;

        public PeopleController([FromKeyedServices("peopleService")] IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }


        //Siendo un get, cuando agregamos algo en (),
        //indicamos que va a ingresar a este metodo siempre que lo llame de
        //forma correcta
        [HttpGet("all")]
        public List<People> GetPeople() => Repository.People;

        //si se desea recibir un dato por el query de la request, se hacer asi ({id})
        //si se recibe mas de un dato: ({id}/{some})
        [HttpGet("{id}")]
        public ActionResult<People> Get(int id)
        {
            var people = Repository.People.FirstOrDefault(p => p.Id == id);

            if (people == null)
            {
                return NotFound();
            }

            return Ok(people);
        }

        [HttpGet("search/{search}")]
        public List<People> Get(string search) => 
            Repository.People.Where(p => p.Name.ToUpper().Contains(search.ToUpper())).ToList();


        [HttpPost]
        public IActionResult Add(People person)
        {
            if (!_peopleService.Validate(person))
            {
                return BadRequest();
            }

            Repository.People.Add(person);

            return NoContent();
        }


    }

    public class Repository
    {
        public static List<People> People = new List<People>
        {
            new People(){ Id = 1,Name = "Pedro",Birthdate = new DateTime(1990,12,3),
            },
            new People(){ Id = 2,Name = "Luis",Birthdate = new DateTime(1992,11,3),
            },
            new People(){ Id = 3,Name = "Ana",Birthdate = new DateTime(1998,1,8),
            },
            new People(){ Id = 4,Name = "Hugo",Birthdate = new DateTime(1995,1,30),
            }
        };
    }

    public class People
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthdate { get; set; }
    }
}
