using FindMyPet.DataMigrationConsole.CustomProviders;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FindMyPet.DataMigrationConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //get the connection string and other settings from app.config
                var connection = ConfigurationManager.ConnectionStrings["FindMyPet"].ConnectionString;

                var isUpdateAll = Convert.ToBoolean(ConfigurationManager.AppSettings["UpdateAll"]);
                var updateTables = ConfigurationManager.AppSettings["UpdateTables"].Split(',').ToList();
                var nameSpace = ConfigurationManager.AppSettings["ModelNamespace"];

                //load the assembly for dynamic to load model
                var asm = Assembly.Load(nameSpace);

                //dynamic get the models by namespace
                var models = asm.GetTypes().Where(p =>
                     p.Namespace == nameSpace
                ).ToList();

                models = GetTablesInOrder(models);

                List<object> objects = new List<object>();
                foreach (var model in models)
                {
                    objects.Add(Activator.CreateInstance(model));
                }

                //create the db factory with OrmLite
                var dbFactory = new OrmLiteConnectionFactory(connection, SqlServerDialect.Provider);

                using (var db = dbFactory.OpenDbConnection())
                {
                    using (IDbTransaction trans = db.OpenTransaction(IsolationLevel.ReadCommitted))
                    {
                        foreach (var o in objects)
                        {
                            var model = o.GetType();

                            if (isUpdateAll || (updateTables.Where(t => t == model.Name).Any() && !isUpdateAll))
                            {
                                //dynamic to call the UpdateTable method so that can support all models
                                Migration m = new Migration();
                                MethodInfo method = typeof(Migration).GetMethod("UpdateTable");
                                MethodInfo generic = method.MakeGenericMethod(model);
                                generic.Invoke(m, new object[] { db, new MSSqlProvider() });
                            }
                        }
                        trans.Commit();
                    }
                }

                System.Console.WriteLine("Database has been updated!");
                System.Console.Read();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
                System.Console.Read();
                //throw ex;
            }
        }

        public static List<Type> GetTablesInOrder(List<Type> models)
        {
            var orderedTables = new List<string>
            {
                "OwnerTableModel",
                "OwnerSettingTableModel",
                "PetTableModel",
                "OwnerPetTableModel",
                "PetImageTableModel",
                "PetAlertTableModel",
                "ParameterGroupTableModel",
                "ParameterValueTableModel"
            };

            var newOrderedList = new List<Type>();

            foreach (var table in orderedTables)
            {
                var found = models.SingleOrDefault(x => x.Name == table);
                newOrderedList.Add(found);
            }

            return newOrderedList;
        }
    }
}
