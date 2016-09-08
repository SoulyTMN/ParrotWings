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
    public class UserAccountsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/UserAccounts
        public IQueryable<UserAccountDTO> GetUserAccounts()
        {
            var userAccounts = from ua in db.UserAccounts
                               select new UserAccountDTO()
                               {
                                   Id = ua.Id,
                                   Balance = ua.Balance,
                                   UserName = ua.User.PwName,
                                   UserEmail = ua.User.Email
                               };

            return userAccounts;
        }

        // GET: api/UserAccounts/5
        [ResponseType(typeof(UserAccountDTO))]
        public async Task<IHttpActionResult> GetUserAccount(int id)
        {
            var userAccount = await db.UserAccounts.Include(ua => ua.User).Select(ua =>
            new UserAccountDTO()
            {
                Id = ua.Id,
                Balance = ua.Balance,
                UserName = ua.User.PwName,
                UserEmail = ua.User.Email
            }).SingleOrDefaultAsync(ua => ua.Id == id);
            if (userAccount == null)
            {
                return NotFound();
            }

            return Ok(userAccount);
        }

        // PUT: api/UserAccounts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserAccount(int id, UserAccount userAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userAccount.Id = GetUserAccountId(userAccount.UserId);

            try
            {
                db.Entry(userAccount).State = EntityState.Modified;
            }
            catch (Exception e)
            {

            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAccountExists(id))
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

        // POST: api/UserAccounts
        [AllowAnonymous]
        [ResponseType(typeof(UserAccount))]
        public async Task<IHttpActionResult> PostUserAccount(UserAccount userAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserAccounts.Add(userAccount);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserAccountExists(userAccount.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            db.Entry(userAccount).Reference(x => x.User).Load();
            var dto = new UserAccountDTO()
            {
                Id = userAccount.Id,
                Balance = userAccount.Balance,
                UserName = userAccount.User.PwName,
                UserEmail = userAccount.User.Email
            };

            return CreatedAtRoute("DefaultApi", new { id = userAccount.Id }, dto);
        }

        // DELETE: api/UserAccounts/5
        [ResponseType(typeof(UserAccount))]
        public async Task<IHttpActionResult> DeleteUserAccount(int id)
        {
            UserAccount userAccount = await db.UserAccounts.FindAsync(id);
            if (userAccount == null)
            {
                return NotFound();
            }

            db.UserAccounts.Remove(userAccount);
            await db.SaveChangesAsync();

            return Ok(userAccount);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserAccountExists(int id)
        {
            return db.UserAccounts.AsNoTracking().Count(e => e.Id == id) > 0;
        }

        private int GetUserAccountId(string userID)
        {
            return db.UserAccounts.AsNoTracking().ToList().FirstOrDefault(x => x.UserId == userID).Id;
        }
    }
}