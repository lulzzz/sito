using System;
using System.IO;
using System.Linq;
using AutoMapper;
using Maddalena.ML.Adapters.AdventureWorks;
using Newtonsoft.Json;
using Address = Maddalena.ML.Adapters.AdventureWorks.Address;

namespace Maddalena.Ai
{
    internal class Program
    {
        public static T[] Load<T>()
        {
            return JsonConvert.DeserializeObject<T[]>(File.ReadAllText($"./json/{typeof(T).Name}"));
        }

        public static T[] Shuffle<T>(T[] array)
        {
            var r = new Random();

            for (int i = 0; i < array.Length; i++)
            {
                var pos = r.Next(array.Length);

                var k = array[i];
                array[i] = array[pos];
                array[pos] = k;
            }

            return array;
        }

        private static void Main(string[] args)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Address, Maddalena.ML.Model.Address>();

            });

            var states = Load<StateProvince>();
            var address = Shuffle(Load<Address>());
            var addressTypes = Load<AddressType>();



            var emails = Load<EmailAddress>();
            var customers = Load<Customer>();
            var people = Load<Person>();

            foreach (var x in customers)
            {
                var person = people.First(p => p.BusinessEntityID == x.PersonID);

                var customer = new Maddalena.ML.Model.Customer()
                {
                    CreatedAt = x.ModifiedDate,
                };
            }
        }
    }
}