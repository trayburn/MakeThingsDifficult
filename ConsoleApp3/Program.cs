using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sampleData = new ModelDTO
            {
                Registers = new Collection<TestRegisterDTO>()
                {
                    new TestRegisterDTO { Name = "Test 1" },
                    new TestRegisterDTO { Name = "Test 2" },
                    new TestRegisterDTO { Name = "Test 3" },
                    new TestRegisterDTO { Name = "Test 4" }
                }
            };

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ModelDTO, ModelDomain>();
                cfg.CreateMap<TestRegisterDTO, TestRegisterDomain>();
                cfg.CreateMap<Collection<TestRegisterDTO>, IMakeThingsDifficult>().ConstructUsing((src, ctx) =>
                {
                    var r = new MakeThingsDifficult();
                    foreach (var item in src)
                    {
                        r.Add(ctx.Mapper.Map<TestRegisterDomain>(item));
                    }
                    return r;
                });
            }).CreateMapper();

            var result = mapper.Map<ModelDomain>(sampleData);

            Console.WriteLine(result.Registers.Count());
            Console.ReadLine();
        }
    }


    public class ModelDTO
    {
        public Collection<TestRegisterDTO> Registers { get; set; }
    }

    public class TestRegisterDTO
    {
        public string Name { get; set; }
    }

    public class ModelDomain
    {
        public IMakeThingsDifficult Registers { get; set; }
    }

    public interface IMakeThingsDifficult : ICollection<ITestRegisterDomain> { }
    public class MakeThingsDifficult : Collection<ITestRegisterDomain>, IMakeThingsDifficult { }

    public class TestRegisterDomain : ITestRegisterDomain
    {
        public string Name { get; set; }
    }

    public interface ITestRegisterDomain
    {
        string Name { get; set; }
    }
}
