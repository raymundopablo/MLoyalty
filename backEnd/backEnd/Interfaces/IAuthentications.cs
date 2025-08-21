using backEnd.clases;

namespace backEnd.Interfaces
{
    public interface IAuthentications
    {
        string GenerateToken(cs_AspNetUser input);
    }
}
