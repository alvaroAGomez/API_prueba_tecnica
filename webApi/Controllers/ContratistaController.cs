using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using webApi.Models;

namespace webApi.Controllers
{
    public class ContratistaController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/Contratista
        public System.Object GetContratista()
        {
            var result = (from c in db.Contratista
                          join e in db.Empresas on c.idEmpresa equals e.id
                          join u in db.usuario on c.idUsuario equals u.id
                          where u.status == "operativo"

                          select new {
                              c.nombre, c.apellido, c.dni, c.cuil, c.email, c.celular,
                              c.legajo, c.Empresas, c.usuario, c.idEmpresa, c.idUsuario,
                              e.id
                          }
                          ).ToList();

            return result;
        }
        
        // GET: api/Contratista/5
        [ResponseType(typeof(Contratista))]
        public System.Object GetContratista(int id)
        {
            var result = (from c in db.Contratista
                          join e in db.Empresas on c.idEmpresa equals e.id
                          join u in db.usuario on c.idUsuario equals u.id
                          where c.dni == id

                          select new
                          {
                              c.nombre,
                              c.apellido,
                              c.dni,
                              c.cuil,
                              c.email,
                              c.celular,
                              c.legajo,
                              c.Empresas,
                              c.usuario,
                              c.idEmpresa,
                              c.idUsuario,
                              e.id
                          }
                          ).FirstOrDefault();

            return result;

        }

        // PUT: api/Contratista/5
        [ResponseType(typeof(void))]
        public System.Object PutContratista(int id, Contratista contratista)
        {
            Contratista result = (from c in db.Contratista
                               where c.dni == id
                               select c).SingleOrDefault();

          
            if (result != default(Contratista))
            {
                result.nombre = contratista.nombre;
                result.apellido = contratista.apellido;
                result.legajo = contratista.legajo;
                result.celular = contratista.celular;
                result.userName = contratista.userName;
                result.password = contratista.password;
                result.email = contratista.email;
                result.idEmpresa = contratista.idEmpresa;
                db.SaveChanges();
            }          

            var resultUsuario = (from u in db.usuario
                                  where u.id == contratista.idUsuario
                                  select u).FirstOrDefault();

                resultUsuario.userName = contratista.userName;
                resultUsuario.password = contratista.password;
                db.SaveChanges();       

            return Ok();
        }

        // POST: api/Contratista
        [ResponseType(typeof(Contratista))]
        public IHttpActionResult PostContratista(Contratista contratista)
        {
            usuario nuevoUsuario = new usuario();
            nuevoUsuario.userName = contratista.userName;
            nuevoUsuario.password = contratista.password;
            nuevoUsuario.status = "Operativo";//contratista.status;
            nuevoUsuario.superSu = "no";//contratista.superSu;
            db.usuario.Add(nuevoUsuario);
            db.SaveChanges();

            var ultimoId = db.usuario.Max(x => x.id);
           

            Contratista nuevoContratista = new Contratista();
            nuevoContratista.idUsuario = ultimoId;
            nuevoContratista.nombre = contratista.nombre;
            nuevoContratista.apellido = contratista.apellido;
            nuevoContratista.legajo = contratista.legajo;
            nuevoContratista.dni = contratista.dni;
            nuevoContratista.cuil = contratista.cuil;
            nuevoContratista.email = contratista.email;
            nuevoContratista.celular = contratista.celular;
            nuevoContratista.idEmpresa = contratista.idEmpresa;

            db.Contratista.Add(nuevoContratista);
            db.SaveChanges();

            return Ok();
        }

        // DELETE: api/Contratista/5
        [ResponseType(typeof(Contratista))]
        public System.Object DeleteContratista(int id)
        {
            var result = (from u in db.usuario
                         where u.id == id
                         select u).FirstOrDefault();

            result.status = "No Operativo";

            db.SaveChanges();

            return Ok(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContratistaExists(int id)
        {
            return db.Contratista.Count(e => e.dni == id) > 0;
        }
    }
}