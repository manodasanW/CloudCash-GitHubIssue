using CloudCash.BL.DTOs.Users;

namespace CloudCash.BL.DTOs.ExpenseIncomes
{
    public record ExpenseIncomeDetailModel : ExpenseIncomeListModel
    {
        public UserListModel User { get; set; } = new();
    }
}
