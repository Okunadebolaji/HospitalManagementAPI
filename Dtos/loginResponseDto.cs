namespace HospitalManagementAPI.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new UserDto();
        public List<MenuDto> Menus { get; set; } = new List<MenuDto>();
    }
}