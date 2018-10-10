using DogService.Models;
using SimpleProxyClient.Attributes;
using SimpleProxyClient.Attributes.PropertyShortAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogService.Abstractions
{
    [ControllerBinding("Dog")]
    public interface IDog
    {
        [MethodBinding(null, InterceptorType = InterceptorType.RestHttpInterceptor, HttpMethod = HttpMethod.Get)]
        Task<string> Name();

        [MethodBinding(null, InterceptorType = InterceptorType.RestHttpInterceptor, HttpMethod = HttpMethod.Get)]
        Task<int> Age();

        [MethodBinding]
        Task<DogInfo> Bark([FromBody] BarkRequest req);

        [MethodBinding]
        DogInfo BarkSync([FromBody] BarkRequest req);

        [MethodBinding]
        Task MakeSoundAsync();

        [MethodBinding]
        void MakeSound();
    }
}
