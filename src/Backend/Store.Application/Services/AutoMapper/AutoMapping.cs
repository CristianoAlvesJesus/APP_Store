using AutoMapper;
using Sqids;
using Store.Communication.Requests;
using Store.Communication.Responses;
using Store.Domain.Entities;

namespace Store.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        private readonly SqidsEncoder<long> _idEnconder;
        public AutoMapping(SqidsEncoder<long> idEnconder)
        {
            _idEnconder = idEnconder;

            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            CreateMap<RequestRegisterUserJson, User>().ForMember(userDest => userDest.Password, opt => opt.Ignore());
            CreateMap<RequestTransactionJson, Transaction>();
        }
        private void DomainToResponse()
        {
            CreateMap<User, ResponseUserProfileJson>();
            CreateMap<Transaction, ResponseRegiteredTransactionJson>()
                .ForMember(dest => dest.Id, config => config.MapFrom(source => _idEnconder.Encode(source.Id)));


            CreateMap<Domain.Entities.Transaction, ResponseRegiteredTransactionJson>()
                .ForMember(dest => dest.Id, config => config.MapFrom(source => _idEnconder.Encode(source.Id)));

            CreateMap<Domain.Entities.Transaction, ResponseShortTransactionJson>()
                .ForMember(dest => dest.Id, config => config.MapFrom(source => _idEnconder.Encode(source.Id)));

            CreateMap<Domain.Entities.Transaction, ResponseTransactionJson>()
                .ForMember(dest => dest.Id, config => config.MapFrom(source => _idEnconder.Encode(source.Id)));
        }
    }
}
