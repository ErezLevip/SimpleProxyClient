using Castle.DynamicProxy;
using DogService.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SimpleProxyClient;
using SimpleProxyClient.Abstractions;
using SimpleProxyClient.Models;
using SimpleProxyClient.Serializers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Preconditions(out IServiceProxyClient<IDog> dogProxyClient, out Dictionary<string, string> headers);

            int count = 1;

            TestHttpClient(count, out List<Task<long>> httpCallTasks, out Task<long> httpTask);
            var proxyCallTasks = CreateProxyTasks(dogProxyClient, headers, count);
            var interceptorTask = WaitAndMesureTask(proxyCallTasks);

            Console.WriteLine($"interceptor is {interceptorTask.Id}");
            Task.WhenAll(httpTask, interceptorTask).GetAwaiter().GetResult();

            var proxyAvg = proxyCallTasks.Select(t => t.Result).Average();
            Console.WriteLine($"avg time interceptor is {proxyAvg}");

            var httpAvg = httpCallTasks.Select(t => t.Result).Average();
            Console.WriteLine($"avg time http is {httpAvg}");
            Console.ReadLine();
        }

        private static List<Task<long>> CreateProxyTasks(IServiceProxyClient<IDog> dogProxyClient, Dictionary<string, string> headers, int count)
        {
            var globalInterceptors = CreateGlobalInterceptors();
            var proxyCallTasks = new List<Task<long>>();
            for (int i = 0; i < count; i++)
            {
                proxyCallTasks.Add(ExecuteProxyClient(dogProxyClient, headers, globalInterceptors));
            }

            return proxyCallTasks;
        }

        private static IOrderedEnumerable<IProxyInterceptor> CreateGlobalInterceptors()
        {
            return new Dictionary<int, IProxyInterceptor>
            {
                {0,new ExceptionInterceptor() }
            }.OrderBy(o => o.Key).Select(o => o.Value).OrderBy(e => e);
        }

        private static void TestHttpClient(int count, out List<Task<long>> httpCallTasks, out Task<long> httpTask)
        {
            httpCallTasks = new List<Task<long>>();
            for (int i = 0; i < count; i++)
            {
                httpCallTasks.Add(ExecuteHttpClient());
            }

            httpTask = WaitAndMesureTask(httpCallTasks);
        }

        private static Task<long> WaitAndMesureTask(List<Task<long>> calls)
        {
            return Task.Run(async () =>
            {
                var sw = new Stopwatch();
                sw.Start();
                await Task.WhenAll(calls.ToArray());
                sw.Stop();
                return sw.ElapsedMilliseconds;
            });
        }

        private static void Preconditions(out IServiceProxyClient<IDog> dogProxyClient, out Dictionary<string, string> headers)
        {
            var collection = new ServiceCollection()
            .AddScoped<ISerializer, JsonNetSerializer>()
            .AddScoped((IServiceProvider p) =>
            {
                return new ServiceInformation<IDog>
                {
                    BaseUrl = "https://localhost:5001/api/",
                    Ttl = TimeSpan.FromSeconds(30)
                };
            })
            .AddSimpleProxyClient<IDog>()
            .BuildServiceProvider();

            dogProxyClient = collection.GetService<IServiceProxyClient<IDog>>();
            headers = new Dictionary<string, string>
            {
                {"color","blue" }
            };
        }

        private static Task<long> ExecuteHttpClient()
        {
            return Task.Run(async () =>
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                string name;
                using (HttpClient hc = new HttpClient())
                {
                    var req = JsonConvert.SerializeObject(new DogService.Models.BarkRequest
                    {
                        Name = "Thor"
                    });
                    var content = new StringContent(req, Encoding.UTF8, "application/json");
                    var res = await hc.PostAsync("https://localhost:5001/api/Dog/Name", content);
                    name = await res.Content.ReadAsStringAsync();
                }

                st.Stop();
                var time = st.ElapsedMilliseconds;
                Console.WriteLine(time);
                return time;
            });
        }

        private static Task<long> ExecuteProxyClient(IServiceProxyClient<IDog> dogProxyClient, Dictionary<string, string> headers, IOrderedEnumerable<IProxyInterceptor> globalInterceptors)
        {
            return Task.Run(async () =>
            {
                var dog = dogProxyClient.GetService(headers, globalInterceptors);
                Stopwatch st = new Stopwatch();
                st.Start();
                var name = await dog.Bark(new DogService.Models.BarkRequest
                {
                    Name = "thor"
                });
                st.Stop();
                return st.ElapsedMilliseconds;
            });
        }
    }
}
