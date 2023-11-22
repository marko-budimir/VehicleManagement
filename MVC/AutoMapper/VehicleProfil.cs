using AutoMapper;
using MVC.Models;
using Service.Models;
using Service.Utilities;

namespace MVC.AutoMapper
{
    public class VehicleProfil : Profile
    {
        public VehicleProfil()
        {
            CreateMap<VehicleMake, VehicleMakeDto>().ReverseMap();
            CreateMap<VehicleModel, VehicleModelDto>().ReverseMap();
            CreateMap<PagedList<VehicleMake>, VehicleMakeViewModel>()
                .ForMember(dest => dest.VehicleMakes, opt => opt.MapFrom(src => src.Items));
        }
    }
}
