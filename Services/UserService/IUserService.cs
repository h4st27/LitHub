using Libra.Dtos;
using Libra.Models;

namespace Libra.Services.UserService
{
    public interface IUserService
    {
        string Login(AuthPayloadDto authPayload);
        string Registrate(UserData user);
        UserData ValidateUser(AuthPayloadDto user);
    }
}
