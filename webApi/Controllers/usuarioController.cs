using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using webApi.Models;

namespace webApi.Controllers
{
    public class usuarioController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/usuario
        public IQueryable<usuario> Getusuario()
        {
            return db.usuario;
        }

        // GET: api/usuario/5
        [ResponseType(typeof(usuario))]
        public IHttpActionResult Getusuario(int id)
        {
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // PUT: api/usuario/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putusuario(int id, usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.id)
            {
                return BadRequest();
            }

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usuarioExists(id))
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

        // POST: api/usuario
        [ResponseType(typeof(usuario))]
        public IHttpActionResult Postusuario(usuario usuario)
        {
            /*  if (!ModelState.IsValid)
              {
                  return BadRequest(ModelState);
              }

              db.usuario.Add(usuario);
              db.SaveChanges();

              return CreatedAtRoute("DefaultApi", new { id = usuario.id }, usuario);
          */
            var result = (from u in db.usuario
                          where u.userName == usuario.userName 
                          where u.password == usuario.password
                          select u).FirstOrDefault();
            return Ok(result);
        }

        // DELETE: api/usuario/5
        [ResponseType(typeof(usuario))]
        public IHttpActionResult Deleteusuario(int id)
        {
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.usuario.Remove(usuario);
            db.SaveChanges();

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool usuarioExists(int id)
        {
            return db.usuario.Count(e => e.id == id) > 0;
        }
    }
}