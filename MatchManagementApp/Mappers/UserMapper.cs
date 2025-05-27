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

        public static UserRegistrationViewModel ToRegistrationViewModel(UserReadDto dto)
        {
            return new UserRegistrationViewModel
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