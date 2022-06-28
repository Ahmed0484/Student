using api.DataModels;
using api.DTOs;
using AutoMapper;

namespace api.Profiles.AfterMaps
{
    public class UpdateStudentAfterMap : IMappingAction<UpdateStudentDTO, Student>
    {
        public void Process(UpdateStudentDTO source, Student destination, ResolutionContext context)
        {
            destination.Address = new Address()
            {
                PhysicalAddress = source.PhysicalAddress,
                PostalAddress = source.PostalAddress
            };
        }
    }
}
