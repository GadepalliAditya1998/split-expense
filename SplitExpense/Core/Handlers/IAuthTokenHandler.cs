using SplitExpense.Core.Models;

namespace SplitExpense.Core.Handlers
{
    public interface IAuthTokenHandler
    {
        public TokenUserInfo? GetTokenUserInfo(string token);
    }
}
