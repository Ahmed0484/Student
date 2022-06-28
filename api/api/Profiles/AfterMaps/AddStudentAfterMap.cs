using api.DataModels;
using api.DTOs;
using AutoMapper;

namespace api.Profiles.AfterMaps
{
    public class AddStudentAfterMap : IMappingAction<AddStudentDTO,Student>
    {
        public void Process(AddStudentDTO source,Student destination, ResolutionContext context)
        {
            destination.Id = Guid.NewGuid();
            destination.Address = new DataModels.Address()
            {
                Id = Guid.NewGuid(),
                PhysicalAddress = source.PhysicalAddress,
                PostalAddress = source.PostalAddress
            };
        }
    }
}
