using AspNetCoreHero.Results;
using AutoMapper;
using Finbuckle.MultiTenant;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TechnicalTest.Domain.Entities;
namespace TechnicalTest.Application.Features.Security.Organizations.Commands.Create
{
    public class CreateOrganizationCommand : IRequest<Result<string>>
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
    }

    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, Result<string>>
    {
        private readonly IMultiTenantStore<Organization> _repository;
        private readonly IMapper _mapper;
        public CreateOrganizationCommandHandler(
            IMultiTenantStore<Organization> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Result<string>> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
        {
            string result = string.Empty;
            string msg = string.Empty;
            try
            {
                var entity = await _repository.TryGetByIdentifierAsync(request.Identifier);

                if (entity == null)
                {
                    entity = _mapper.Map<Organization>(request);

                    await _repository.TryAddAsync(entity);
                }

                result = entity.Id;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    msg = ex.InnerException.Message;
                }
                else
                {
                    msg = ex.Message;
                }
            }

            if (!string.IsNullOrEmpty(msg))
            {
                return await Result<string>.FailAsync(msg);
            }
            else
            {
                if (string.IsNullOrEmpty(result))
                {
                    return await Result<string>.FailAsync("Error registering.");
                }
                else
                {
                    return await Result<string>.SuccessAsync(data: result);
                }
            }
        }
    }
}
