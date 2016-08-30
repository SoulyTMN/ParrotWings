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
                                   Date = t.Date
                               };

            return transactions;
        }

        [Route("my")]
        public IQueryable<TransactionDTO> GetMyTransactions()
        {
            var transactions = from t in db.Transactions.Where(x => x.PayeeId == UserIdentityId || x.RecipientId == UserIdentityId)
                               select new TransactionDTO()
                               {
                                   Id = t.Id,
                                   PayeeName = t.Payee.PwName,
                                   PayeeEmail = t.Payee.Email,
                                   RecipientName = t.Recipient.PwName,
                                   RecipientEmail = t.Recipient.Email,
                                   Amount = t.Amount,
                                   Date = t.Date
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
                 Date = t.Date
             }).SingleOrDefaultAsync(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // PUT: api/Transactions/5
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.Id)
            {
                return BadRequest();
            }

            db.Entry(transaction).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Transactions
        [Route("")]
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> PostTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                Date = transaction.Date
            };

            return CreatedAtRoute("DefaultApi", new { id = transaction.Id }, dto);
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