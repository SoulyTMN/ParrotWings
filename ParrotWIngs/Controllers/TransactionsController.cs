using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ParrotWIngs.Models;

namespace ParrotWIngs.Controllers
{
    [Authorize]
    [RoutePrefix("api/Transactions")]
    public class TransactionsController : BaseApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: api/Transactions
        [Route("")]
        public IQueryable<TransactionDTO> GetTransactions()
        {
            var transactions = from t in db.Transactions
                               select new TransactionDTO()
                               {
                                   Id = t.Id,
                                   PayeeName = t.Payee.PwName,
                                   PayeeEmail = t.Payee.Email,
                                   RecipientName = t.Recipient.PwName,
                                   RecipientEmail = t.Recipient.Email,
                                   Amount = t.Amount,
                                   ResultingPayeeBalance = t.ResultingPayeeBalance,
                                   ResultingRecipientBalance = t.ResultingRecipientBalance,
                                   Date = t.Date
                               };

            return transactions;
        }

        [Route("my")]
        public IQueryable<MyTransactionDTO> GetMyTransactions()
        {
            var transactions = from t in db.Transactions.Where(x => x.PayeeId == UserIdentityId || x.RecipientId == UserIdentityId)
                               select new MyTransactionDTO()
                               {
                                   Id = t.Id,
                                   Date = t.Date,
                                   CorrespondentName = t.Recipient.PwName,
                                   Amount = t.Amount,
                                   TransactionType = t.PayeeId == UserIdentityId ? Static.TransactionTypes.Debit : Static.TransactionTypes.Credit,
                                   MyResultingBalance = t.PayeeId == UserIdentityId ? t.ResultingPayeeBalance : t.ResultingRecipientBalance
                               };

            return transactions;
        }


        // GET: api/Transactions/5
        [Route("{id:int}")]
        [ResponseType(typeof(TransactionDTO))]
        public async Task<IHttpActionResult> GetTransaction(int id)
        {
            var transaction = await db.Transactions.Include(t => t.Payee).Include(t => t.Recipient).Select(t =>
             new TransactionDTO()
             {
                 Id = t.Id,
                 PayeeName = t.Payee.PwName,
                 PayeeEmail = t.Payee.Email,
                 RecipientName = t.Recipient.PwName,
                 RecipientEmail = t.Recipient.Email,
                 Amount = t.Amount,
                 ResultingPayeeBalance = t.ResultingPayeeBalance,
                 ResultingRecipientBalance = t.ResultingRecipientBalance,
                 Date = t.Date
             }).SingleOrDefaultAsync(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }
         
        // POST: api/Transactions/my
        [Route("my")]
        [ResponseType(typeof(MyTransactionDTO))]
        public async Task<IHttpActionResult> PostMyTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (transaction.Amount < 0)
                throw new Exception("Negative amout transactions are not allowed.");
            transaction.PayeeId = UserIdentityId;
            transaction.Date = DateTime.Now;

            if (transaction.Amount < 0)
                throw new Exception("Negative amout transactions are not allowed.");

            double currentPayeeBalance = db.UserAccounts.ToList().FirstOrDefault(x => x.UserId == transaction.PayeeId).Balance;
            if (currentPayeeBalance < transaction.Amount)
                throw new Exception("Cannot commit the transaction. Payee balance is smaller than transaction amount.");
            else
            {
                transaction.ResultingPayeeBalance = currentPayeeBalance - transaction.Amount;
                transaction.ResultingRecipientBalance = db.UserAccounts.ToList().FirstOrDefault(x => x.UserId == transaction.RecipientId).Balance + transaction.Amount;
            }

            db.Transactions.Add(transaction);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TransactionExists(transaction.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }


            db.Entry(transaction).Reference(x => x.Payee).Load();
            db.Entry(transaction).Reference(x => x.Recipient).Load();
            var dto = new MyTransactionDTO()
            {
                Id = transaction.Id,
                Date = transaction.Date,
                CorrespondentName = transaction.Recipient.PwName,
                Amount = transaction.Amount,
                TransactionType = Static.TransactionTypes.Debit,
                MyResultingBalance = transaction.ResultingPayeeBalance
            };
            
            //return CreatedAtRoute("DefaultApi", new { id = transaction.Id }, dto);
            return Ok(dto);
        }

        // POST: api/Transactions
        [Route("")]
        [ResponseType(typeof(TransactionDTO))]
        public async Task<IHttpActionResult> PostTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (transaction.Amount < 0)
                throw new Exception("Negative amout transactions are not allowed."); 

            double currentPayeeBalance = db.UserAccounts.ToList().FirstOrDefault(x => x.UserId == transaction.PayeeId).Balance;
            if (currentPayeeBalance < transaction.Amount)
                throw new Exception("Cannot commit the transaction. Payee balance is smaller than transaction amount.");
            else
            {
                transaction.ResultingPayeeBalance = currentPayeeBalance - transaction.Amount;
                transaction.ResultingRecipientBalance = db.UserAccounts.ToList().FirstOrDefault(x => x.UserId == transaction.RecipientId).Balance + transaction.Amount;
            }



            db.Transactions.Add(transaction);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TransactionExists(transaction.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }


            db.Entry(transaction).Reference(x => x.Payee).Load();
            db.Entry(transaction).Reference(x => x.Recipient).Load();
            var dto = new TransactionDTO()
            {
                Id = transaction.Id,
                PayeeName = transaction.Payee.PwName,
                PayeeEmail = transaction.Payee.Email,
                RecipientName = transaction.Recipient.PwName,
                RecipientEmail = transaction.Recipient.Email,
                Amount = transaction.Amount,
                ResultingPayeeBalance = transaction.ResultingPayeeBalance,
                ResultingRecipientBalance = transaction.ResultingRecipientBalance,
                Date = transaction.Date
            };

            //return CreatedAtRoute("DefaultApi", new { id = transaction.Id }, dto);
            return Ok(dto);
        }

        // DELETE: api/Transactions/5
        [Route("{id:int}")]
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> DeleteTransaction(int id)
        {
            Transaction transaction = await db.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(transaction);
            await db.SaveChangesAsync();

            return Ok(transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(int id)
        {
            return db.Transactions.Count(e => e.Id == id) > 0;
        }
    }
}