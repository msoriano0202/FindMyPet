using ServiceStack;

namespace FindMyPet.DTO.Owner
{
    [Route("/owner/{Id}", "GET")]
    public class OwnerRequest : IReturn<Owner>
    {
        public int Id { get; set; }
    }
}
