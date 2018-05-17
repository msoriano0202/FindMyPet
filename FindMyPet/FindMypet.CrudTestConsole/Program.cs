using FindMyPet.TableModel;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using System;
using System.Configuration;
using System.Data;
using System.Linq;

namespace FindMypet.CrudTestConsole
{
    public class FullOwnerPet
    {
        public int OwnerId { get; set; }
        public int PetId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //InsertOwner("Miguel Martin", "Soriano Talaverano", "M", "miguel_mst@hotmail.com");
                //InsertOwner("Emanuel Ricardo", "Soriano Talaverano", "M", "emanuelsoriano11@hotmail.com");
                //ListAllOwners();
                //InsertPet(2, "Marti Soriano", new DateTime(2009, 8, 29));
                //InsertPet(3, "Canuto Soriano", new DateTime(2013, 3, 1));
                //ListAllPets();
                //ListPetsByOwnerId(2);
                //ListPetsByOwnerId(3);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
                System.Console.Read();
            }
        }

        private static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["FindMyPet"].ConnectionString; }
        }

        private static void InsertOwner(string firstName, string lastName, string gender, string email)
        {
            //create the db factory with OrmLite
            var dbFactory = new OrmLiteConnectionFactory(ConnectionString, SqlServerDialect.Provider);
            long ownerId = 0;

            using (var db = dbFactory.OpenDbConnection())
            {
                using (IDbTransaction trans = db.OpenTransaction(IsolationLevel.ReadCommitted))
                {
                    ownerId = db.Insert(new OwnerTable
                    {
                        Code = Guid.NewGuid(),
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        CreatedOn = DateTime.Now
                    }, selectIdentity: true);

                    trans.Commit();
                }
            }

            System.Console.WriteLine(string.Format("Owner has been inserted. Id: {0}", ownerId.ToString()));
            System.Console.Read();
        }

        private static void ListAllOwners()
        {
            //create the db factory with OrmLite
            var dbFactory = new OrmLiteConnectionFactory(ConnectionString, SqlServerDialect.Provider);

            using (var db = dbFactory.OpenDbConnection())
            {
                var owners = db.Select(db.From<OwnerTable>());
                foreach (var owner in owners)
                {
                    System.Console.WriteLine(string.Format("{0}: {1} {2} ({3})", owner.Id, owner.FirstName, owner.LastName));
                }
            }
            
            System.Console.Read();
        }

        private static void InsertPet(int ownerId, string name, DateTime dateOfBirth)
        {
            //create the db factory with OrmLite
            var dbFactory = new OrmLiteConnectionFactory(ConnectionString, SqlServerDialect.Provider);
            long petId = 0;

            using (var db = dbFactory.OpenDbConnection())
            {
                using (IDbTransaction trans = db.OpenTransaction(IsolationLevel.ReadCommitted))
                {
                    var createdOn = DateTime.Now;

                    petId = db.Insert(new PetTable
                    {
                        Code = Guid.NewGuid(),
                        Name = name,
                        DateOfBirth = dateOfBirth,
                        CreatedOn = createdOn
                    }, selectIdentity: true);

                    db.Insert(new OwnerPetTable { OwnerTableId = ownerId, PetTableId = (int)petId, CreatedOn = createdOn });

                    trans.Commit();
                }
            }

            System.Console.WriteLine(string.Format("Pet has been inserted. Id: {0}", petId.ToString()));
            System.Console.Read();
        }

        private static void ListAllPets()
        {
            //create the db factory with OrmLite
            var dbFactory = new OrmLiteConnectionFactory(ConnectionString, SqlServerDialect.Provider);

            using (var db = dbFactory.OpenDbConnection())
            {
                var query = db.Select<FullOwnerPet>(db
                                .From<OwnerTable>()
                                .Join<OwnerTable, OwnerPetTable>((o, op) => o.Id == op.OwnerTableId)
                                .Join<OwnerPetTable, PetTable>((op, p) => op.PetTableId == p.Id)
                                .Where<OwnerTable>(o => o.Id > 0))
                                .ToList();

                query.PrintDump();
                foreach (var item in query)
                {
                    System.Console.WriteLine(string.Format("Owner: {0} {1} -> Pet: {2}", item.FirstName, item.LastName, item.Name));
                }
            }

            System.Console.Read();
        }

        private static void ListPetsByOwnerId(int ownerId)
        {
            //create the db factory with OrmLite
            var dbFactory = new OrmLiteConnectionFactory(ConnectionString, SqlServerDialect.Provider);

            using (var db = dbFactory.OpenDbConnection())
            {
                var query = db.From<PetTable>()
                                .Join<PetTable, OwnerPetTable>((p, op) => p.Id == op.PetTableId)
                                .Where<OwnerPetTable>(op => op.OwnerTableId == ownerId);
                var results = db.Select(query);

                results.PrintDump();
                foreach (var item in results)
                {
                    System.Console.WriteLine(string.Format("Pet: {0}", item.Name));
                }
            }

            System.Console.Read();
        }
    }
}
