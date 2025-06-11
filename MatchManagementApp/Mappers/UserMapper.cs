    public static class UserMapper
    {
        public static UserDto ToCreateDto(UserViewModel vm)
        {
            return new UserDto
            {
                Username = vm.Username,
                Email = vm.Email,
                Password = vm.Password,
                Age = vm.Age,
                Gender = vm.Gender
            };
        }

        public static UserViewModel ToUserViewModel(UserDto dto)
        {
            return new UserViewModel
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                Age = dto.Age,
                Gender = dto.Gender
            };
        }
    }