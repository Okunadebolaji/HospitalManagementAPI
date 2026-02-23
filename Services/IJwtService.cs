using HospitalManagementAPI.Models;
namespace HospitalManagementAPI.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}
