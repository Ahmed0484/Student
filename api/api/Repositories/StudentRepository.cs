using api.DataModels;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentAdminContext ctx;

        public StudentRepository(StudentAdminContext ctx)
        {
            this.ctx = ctx;
        }
        public async Task<List<Student>> GetStudentsAsync()
        {
            return await ctx.Students.Include(nameof(Gender)).Include(nameof(Address)).ToListAsync();
        }

        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            return await ctx.Students
                .Include(nameof(Gender)).Include(nameof(Address))
                .FirstOrDefaultAsync(x => x.Id == studentId);
        }

        public async Task<List<Gender>> GetGendersAsync()
        {
            return await ctx.Genders.ToListAsync();
        }

        public async Task<bool> Exists(Guid studentId)
        {
            return await ctx.Students.AnyAsync(x => x.Id == studentId);
        }

        public async Task<Student> UpdateStudent(Guid studentId, Student student)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if (existingStudent != null)
            {
                existingStudent.FirstName = student.FirstName;
                existingStudent.LastName = student.LastName;
                existingStudent.DateOfBirth = student.DateOfBirth;
                existingStudent.Email = student.Email;
                existingStudent.Mobile = student.Mobile;
                existingStudent.GenderId = student.GenderId;
                existingStudent.Address.PhysicalAddress = student.Address.PhysicalAddress;
                existingStudent.Address.PostalAddress = student.Address.PostalAddress;

                await ctx.SaveChangesAsync();
                return existingStudent;
            }

            return null;
        }

        public async Task<Student> DeleteStudent(Guid studentId)
        {
            var student = await GetStudentAsync(studentId);

            if (student != null)
            {
                ctx.Students.Remove(student);
                await ctx.SaveChangesAsync();
                return student;
            }

            return null;
        }

        public async Task<Student> AddStudent(Student request)
        {
            var student = await ctx.Students.AddAsync(request);
            await ctx.SaveChangesAsync();
            return student.Entity;
        }

        public async Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl)
        {
            var student = await GetStudentAsync(studentId);

            if (student != null)
            {
                student.ProfileImageUrl = profileImageUrl;
                await ctx.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
