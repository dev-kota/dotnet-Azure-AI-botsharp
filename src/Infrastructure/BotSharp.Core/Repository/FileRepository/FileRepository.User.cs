using BotSharp.Abstraction.Users.Enums;
using BotSharp.Abstraction.Users.Models;
using System.IO;

namespace BotSharp.Core.Repository;

public partial class FileRepository
{
    public User? GetUserByEmail(string email)
    {
        return Users.FirstOrDefault(x => x.Email == email.ToLower());
    }

    public User? GetUserById(string id = null)
    {
        return Users.FirstOrDefault(x => x.Id == id || (x.ExternalId != null && x.ExternalId == id));
    }

    public User? GetUserByUserName(string userName = null)
    {
        return Users.FirstOrDefault(x => x.UserName == userName.ToLower());
    }

    public void CreateUser(User user)
    {
        var userId = Guid.NewGuid().ToString();
        user.Id = userId;
        user.Role = UserRole.Admin;
        var dir = Path.Combine(_dbSettings.FileRepository, "users", userId);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        var path = Path.Combine(dir, "user.json");
        File.WriteAllText(path, JsonSerializer.Serialize(user, _options));
    }
}
