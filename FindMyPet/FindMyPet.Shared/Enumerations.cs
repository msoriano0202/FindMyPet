using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.Shared
{
    public enum EnvironmentEnum
    {
        Local = 0,
        Development = 1,
        Production = 2
    }

    public enum PetStatusEnum
    {
        Active = 1,
        Lost = 2,
        Found = 3
    }
}
