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
    /// Male = 1 / Female = 2
    /// </summary>
    public enum PetSexTypeEnum
    {
        Male = 1,
        Female = 2
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
        Abandom = 2,
        Injured = 3,
        Found = 4,
        Adoption = 5
    }

    /// <summary>
    /// Active = 1 / Deleted = 2
    /// </summary>
    public enum AlertStatusEnum
    {
        Active = 1,
        Closed = 2,
        Reported = 3,
        Deleted = 4
    }

    /// <summary>
    /// Pending = 0 / Approved = 1 / Rejected = 2
    /// </summary>
    public enum ApproveStatusEnum
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

    /// <summary>
    ///  Welcome = 0 / ReSendConfirmation = 1 / PetShare = 2
    /// </summary>
    public enum EmailTypeEnum
    {
        Welcome = 0,
        ReSendConfirmation = 1,
        PetShare = 2
    }

    public enum EmailViewerTypeEnum
    {
        Browser = 0,
        Email = 1
    }
}
