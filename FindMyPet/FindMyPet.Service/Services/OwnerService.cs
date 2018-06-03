using FindMyPet.DTO.Owner;
using FindMyPet.MyServiceStack.DataAccess;
using FindMyPet.MyServiceStack.Providers;
using FindMyPet.TableModel;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindMyPet.MyServiceStack.Services
{
    public class OwnerService : Service
    {
        private readonly IOwnerProvider _ownerProvider;

        public OwnerService(IOwnerProvider ownerProvider)
        {
            if(ownerProvider == null)
                throw new ArgumentNullException(nameof(ownerProvider));

            _ownerProvider = ownerProvider;
        }

        public async Task<Owner> Get(OwnerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _ownerProvider.GetOwnerAsync(request)
                                       .ConfigureAwait(false);
        }

        public async Task<Owner> Post(CreateOwnerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _ownerProvider.CreateOwnerAsync(request)
                                       .ConfigureAwait(false);
        }

        public async Task<Owner> Put(UpdateOwnerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _ownerProvider.UpdateOwnerAsync(request)
                                       .ConfigureAwait(false);
        }

        public async Task<List<Owner>> Post(SearchOwnerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _ownerProvider.SearchOwnersAsync(request)
                                       .ConfigureAwait(false);
        }
    }
}