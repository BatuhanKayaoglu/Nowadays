using AutoMapper;
using AutoMapper.Internal;
using Nowadays.Common.ResponseViewModel;
using Nowadays.Common.ViewModels;
using Nowadays.Entity.Models;
using Nowadays.Entity.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddCompanyViewModel, Company>().ReverseMap();
            CreateMap<AddProjectViewModel, Company>().ReverseMap();

        }
    }
}
