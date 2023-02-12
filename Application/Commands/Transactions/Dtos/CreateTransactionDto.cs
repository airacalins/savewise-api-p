using System.ComponentModel.DataAnnotations.Schema;
using Application.Commands.Accounts.Dtos;
using Domain;
using Domain.Enums;

namespace Application.Commands.Transactions.Dtos
{
  public class CreateTransactionDto
  {
    public Guid AccountId { get; set; }
    public TransactionType TransactionType { get; set; }
    public double Amount { get; set; }
    public DateTime DateCreated { get; set; }

    public Transaction ToTransactionEntity()
    {
      var transaction = new Transaction
      {
        AccountId = AccountId,
        TransactionType = TransactionType,
        Amount = Amount,
        DateCreated = DateCreated,
      };

      return transaction;
    }
  }
}