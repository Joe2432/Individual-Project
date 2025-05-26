using MatchManagementApp.Core.DTOs;
namespace MatchManagementApp.UI.Mappers
{
    public static class UserMapper
    {
        public static UserCreateDto ToCreateDto(UserRegistrationViewModel vm)
        {
            return new UserCreateDto
            {
                Username = vm.Username,
                Email = vm.Email,
                Password = vm.Password,
                Age = vm.Age,
                Gender = vm.Gender
            };
        }
    }
}