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

    /// <summary>
    ///  Active = 1 / Lost = 2 / Found = 3
    /// </summary>
    public enum PetStatusEnum
    {
        Active = 1,
        Lost = 2,
        Found = 3
    }

    /// <summary>
    /// Lost = 1 / Abandom = 2
    /// </summary>
    public enum AlertTypeEnum
    {
        Lost = 1,
        Abandom = 2
    }

    /// <summary>
    /// Active = 1 / Deleted = 2
    /// </summary>
    public enum AlertStatusEnum
    {
        Active = 1,
        Deleted = 2
    }
}
