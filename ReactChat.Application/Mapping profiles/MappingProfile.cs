using AutoMapper;
using ReactChat.Application.Dtos.MessageDto;
using ReactChat.Core.Entities.Login;
using ReactChat.Core.Entities.Messages;
using ReactChat.Dtos.Users;

namespace ReactChat.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BaseUser, UserDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (int?)src.Id))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<PrivateMessage, MessageDto>()
                .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.SenderName))
                .ForMember(dest => dest.Recipient, opt => opt.MapFrom(src => src.ReceiverName))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.MessageDateTime))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Message));
        }
    }
}
