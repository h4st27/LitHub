using LitHub.Dtos;
using LitHub.Models;

namespace LitHub.Services.UserService
{
    public interface IUserService
    {
        string Login(AuthPayloadDto authPayload);
        string Registrate(UserData user);
        UserData ValidateUser(AuthPayloadDto user);
    }
}
