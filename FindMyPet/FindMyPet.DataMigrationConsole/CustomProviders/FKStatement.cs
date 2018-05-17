using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DataMigrationConsole.CustomProviders
{
    /// <summary>
    /// For generate SQL string for drop and re-recreate foreign keys 
    /// </summary>
    public class FKStatement
    {
        public string ParentObject { get; set; }
        public string ReferenceObject { get; set; }
        public string DropStatement { get; set; }
        public string CreateStatement { get; set; }
    }
}
