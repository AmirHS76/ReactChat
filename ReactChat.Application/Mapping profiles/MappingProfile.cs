using AutoMapper;
using ReactChat.Application.Dtos.MessageDto;
using ReactChat.Application.Dtos.User;
using ReactChat.Core.Entities.Message;
using ReactChat.Core.Entities.User;

namespace ReactChat.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BaseUser, UserDTO>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (int?)src.Id))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.UserRole.ToString()));

            CreateMap<PrivateMessage, MessageDTO>()
                .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.SenderName))
                .ForMember(dest => dest.Recipient, opt => opt.MapFrom(src => src.ReceiverName))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.MessageDateTime))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Message));
        }
    }
}
