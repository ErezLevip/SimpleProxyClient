using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogService.Abstractions;
using DogService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogController : ControllerBase
    {
        private readonly IDog _dog;
        public DogController(IDog dog)
        {
            _dog = dog;
        }

        [HttpGet]
        [Route("Name")]
        public async Task<ActionResult<string>> Name()
        {
            return await _dog.Name();
        }

        [HttpGet]
        [Route("Age")]
        public async Task<ActionResult<int>> Age()
        {
            return await _dog.Age();
        }

        [HttpPost]
        [Route("Bark")]
        public async Task<ActionResult<DogInfo>> Bark( BarkRequest req)
        {
            return await _dog.Bark(req);
        }

        [HttpPost]
        [Route("BarkSync")]
        public ActionResult<DogInfo> BarkSync(BarkRequest req)
        {
            return _dog.BarkSync(req);
        }

        [HttpPost]
        [Route("MakeSoundAsync")]
        public async Task<ActionResult> MakeSoundAsync()
        {
            await _dog.MakeSoundAsync();
            return Ok(); 
        }
        [HttpPost]
        [Route("MakeSound")]
        public ActionResult MakeSound()
        {
            _dog.MakeSound();
            return Ok();
        }
    }
}
