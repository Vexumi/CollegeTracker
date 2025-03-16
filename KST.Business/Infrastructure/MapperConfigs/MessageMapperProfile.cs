using AutoMapper;
using KST.Business.ViewModels;
using KST.DataAccess.Models;

namespace KST.Business.Infrastructure.MapperConfigs;

public class MessageMapperProfile: Profile
{
    public MessageMapperProfile()
    {
        CreateMap<MessageDTO, Message>();
    }
}