using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Connector;
using WebApi.Model;
using System;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Administrator")]
    public class ValuesController : ControllerBase
    {

        

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            Conn connection = new Conn();
            return connection.getList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<User>> Get(int id)
        {
            Conn connection = new Conn();
            return connection.getById(id);
        }


        // POST api/values
        [HttpPost]
        public void Post( string Isim, string Soyisim, string Telefon, string Telefon2, string Adres, string Eposta)
        {
            Conn cnn = new Conn();           
            cnn.postList( Isim, Soyisim, Telefon, Telefon2, Adres, Eposta) ;     
        }


        // PUT api/values/5
        [HttpPut("{id, Isim,  Soyisim, Telefon, Telefon2, Adres, Eposta}")]
        public void Put(int id, string Isim, string Soyisim, string Telefon, string Telefon2, string Adres, string Eposta)
        {
            Conn connection = new Conn();
            connection.putList( id, Isim,  Soyisim, Telefon, Telefon2, Adres,  Eposta);
        }


        // DELETE api/values/5
        [HttpDelete()]
        public void Delete(int id)
        {
            Conn connection = new Conn();
            connection.deleteById(id);
        }
    }
}
