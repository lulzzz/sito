using System;
using System.IO;
using System.Linq;
using Elasticsearch.Net;
using Maddalena.ML.Adapters.AdventureWorks;
using Maddalena.ML.Geocoding;
using Nest;
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

        public static Geocoder Geocoder = new Geocoder("AIzaSyDQ1tMYT-uoxCHDqN_sSdU95Ldk51naLFA");

        public class Geo
        {
            [Number(NumberType.Double, Name = "lat")]
            public double Latitude { get; set; }

            [Number(NumberType.Double, Name = "lng")]
            public double Longitude { get; set; }
        }

        class Test
        {
            public int Id { get; set; }

            [Nest.GeoPoint]
            public Geo Geo { get; set; }

            [Nest.Ip]
            public string Ip { get; set; }

            [Nest.Date]
            public DateTime Date { get; set; }
        }


        private static void Main(string[] args)
        {
            var states = Load<StateProvince>();
            var address = Load<Address>();
            var addressTypes = Load<AddressType>();

            foreach (var s in address)
            {
                var state = states.First(x => x.StateProvinceID == s.StateProvinceID);
                var str = $"{s.AddressLine1} {s.AddressLine2} {s.City} - {s.PostalCode} - {state.Name}";

                var addrTask = Geocoder.GeocodeAsync(str);
                addrTask.Wait();
                var add = addrTask.Result.FirstOrDefault();

                if(add == null) continue;

                add.Type = Enum.Parse<ML.Model.AddressType>(addressTypes
                    .First(x => x.AddressTypeID == s.AddressID).Name.Replace(" ", ""));

                add.Id = Guid.NewGuid().ToString();
            }


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