using AutoMapper;
using MVC.Models;
using Service.Models;

namespace MVC.AutoMapper
{
    public class VehicleProfil : Profile
    {
        public VehicleProfil()
        {
            CreateMap<VehicleMake, VehicleMakeDto>().ReverseMap();
            CreateMap<VehicleModel, VehicleModelDto>().ReverseMap();
        }
    }
}
