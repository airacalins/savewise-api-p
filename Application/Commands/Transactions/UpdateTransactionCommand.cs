using Application.Commands.Transactions.Dtos;
using Application.Commands.Transactions.Interfaces;
using Application.Repositories.TransactionRepository;

namespace Application.Commands.Transactions
{
  public class UpdateTransactionCommand : IUpdateTransactionCommand
  {
    private readonly ITransactionRepository _transactionRepository;
    public UpdateTransactionCommand(ITransactionRepository transactionRepository)
    {
      _transactionRepository = transactionRepository;

    }
    public async Task ExecuteCommand(Guid accountId, Guid id, UpdateTransactionDto input)
    {
      var transaction = await _transactionRepository.GetById(id);

      if (transaction == null) throw new NullReferenceException();
      if (transaction.AccountId != accountId) throw new NotImplementedException();

      transaction.TransactionType = input.TransactionType;
      transaction.Amount = input.Amount;
      transaction.DateCreated = input.DateCreated;
      await _transactionRepository.SaveChangesAsync();
    }
  }
}