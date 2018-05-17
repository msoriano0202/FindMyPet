using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using FindMyPet.DTO.Owner;

namespace FindMyPet.ConsumerConsole
{
    class Program
    {
        private const string url = "http://localhost:6189";
        private static JsonServiceClient JsonClient
        {
            get { return new JsonServiceClient(url); }
        }

        static void Main(string[] args)
        {
            AddOwner();
            //ListOwners();
        }

        private static void AddOwner()
        {
            var request = new CreateOwnerRequest
            {
                FirstName = "Tina",
                LastName = "Santiago"
            };
            var response = JsonClient.Post(request);

            Console.WriteLine(string.Format("Owner Inserted: {0}", response.Id));
            Console.ReadKey();
        }

        //private static void ListOwners()
        //{
        //    var request = new OwnerRequest();
        //    var response = JsonClient.Post(request);

        //    foreach (var owner in response)
        //    {
        //        Console.WriteLine(string.Format("{0}: {3} - {1} {2} ({4})", owner.Id, owner.FirstName, owner.LastName, owner.Gender, owner.CreatedOn));
        //    }

        //    Console.ReadKey();
        //}
    }
}
