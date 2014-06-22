using AutoMapper;
using SspEngine.DomainModel;

namespace SspEngineClient
{
    public class RiskProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<RiskAddress, Address>()
                .ForMember(dest => dest.Postcode, opt => opt.MapFrom(src => Postcode.Parse(src.Postcode)));

            Mapper.CreateMap<Risk, SspEngine.DomainModel.Risk>()
                .ForMember(dest => dest.KeptPostcode, opt => opt.MapFrom(src => Postcode.Parse(src.KeptPostcode)));
        }
    }
}