using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandomController : ControllerBase
    {
        private IRandomService _randomServiceSingleton;
        private IRandomService _randomServiceScoped;
        private IRandomService _randomServiceTranscient;

        private IRandomService _randomService2Singleton;
        private IRandomService _randomService2Scoped;
        private IRandomService _randomService2Transcient;

        public RandomController(
            [FromKeyedServices("randomSingleton")] IRandomService randomServiceSingleton,
            [FromKeyedServices("randomScoped")] IRandomService randomServiceScoped,
            [FromKeyedServices("randomTransient")] IRandomService randomServiceTransient,
            [FromKeyedServices("randomSingleton")] IRandomService randomService2Singleton,
            [FromKeyedServices("randomScoped")] IRandomService randomService2Scoped,
            [FromKeyedServices("randomTransient")] IRandomService randomService2Transient
            )
        {
            _randomServiceSingleton = randomServiceSingleton;
            _randomServiceScoped = randomServiceScoped;
            _randomServiceTranscient = randomServiceTransient;
            _randomService2Singleton = randomService2Singleton;
            _randomService2Scoped = randomService2Scoped;
            _randomService2Transcient = randomService2Transient;
        }

        [HttpGet]
        public ActionResult<Dictionary<string, int>> Get()
        {
            var result = new Dictionary<string, int>();

            result.Add("Singleton 1", _randomServiceSingleton.Value);
            result.Add("Scoped 1", _randomServiceScoped.Value);
            result.Add("Transcient 1", _randomServiceTranscient.Value);

            result.Add("Singleton 2", _randomService2Singleton.Value);
            result.Add("Scoped 2", _randomService2Scoped.Value);
            result.Add("Transcient 2", _randomService2Transcient.Value);

            return result;
        }

    }
}
