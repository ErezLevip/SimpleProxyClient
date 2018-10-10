using DogService.Abstractions;
using DogService.Models;
using SimpleProxyClient.Attributes.PropertyShortAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogService
{
    public class Dog : IDog
    {
        public Task<int> Age()
        {
            return Task.FromResult(3);
        }

        public Task<string> Name()
        {
            return Task.FromResult("Thor");
        }
        public Task<DogInfo> Bark(BarkRequest req)
        {
            return Task.FromResult(new DogInfo
            {
                Name = req.Name,
                Message = $"Woof Woof im {req.Name}"
            });
        }

        public DogInfo BarkSync([FromBody] BarkRequest req)
        {
            return new DogInfo
            {
                Name = req.Name,
                Message = $"Woof Woof im {req.Name}"
            };
        }

        public Task MakeSoundAsync()
        {
            Console.WriteLine("Woof Woof");
            return Task.CompletedTask;
        }

        public void MakeSound()
        {
            Console.WriteLine("Woof Woof");
        }
    }
}
