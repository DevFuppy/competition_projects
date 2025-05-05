using static BCrypt.Net.BCrypt;

namespace EFcoreRepoPractice.Application
{
    public static class PasswordHasher
    {
        public static string GenerateHashPwd(string inputPwd) => HashPassword(inputPwd,workFactor:12);

        public static bool VerifyHashPwd(string oriPwd, string HashedPwd) => Verify(oriPwd, HashedPwd);


    }
}
