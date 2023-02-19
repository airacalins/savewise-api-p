using Application.Contexts;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories.TransactionRepository
{
  public class TransactionRepository : ITransactionRepository
  {
    private readonly IDataContext _context;

    public TransactionRepository(IDataContext context)
    {
      _context = context;
    }

    public Task<List<Transaction>> GetAll()
    {
      return _context.Transactions.ToListAsync();
    }

    public async Task<Transaction> GetById(Guid id)
    {
      var transaction = await _context.Transactions.FirstOrDefaultAsync(transaction => transaction.Id == id);

      if (transaction == null) throw new NullReferenceException();
      return transaction;
    }

    public void Add(Transaction item)
    {
      _context.Transactions.Add(item);
    }

    public async Task Update(Transaction item)
    {
      var transaction = await GetById(item.Id);
    }

    public async Task Delete(Guid id)
    {
      var transaction = await GetById(id);

      if (transaction == null) throw new NullReferenceException();
      transaction.IsArchived = true;
    }

    public async Task SaveChangesAsync()
    {
      await _context.SaveChangesAsync();
    }
  }
}