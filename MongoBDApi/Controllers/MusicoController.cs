using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MusicoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Musico> _musicoCollection;

        public MusicoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _musicoCollection = _mongoDB.DB.GetCollection<Musico>(typeof(Musico).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarMusico([FromBody] MusicoDto dto)
        {
            var NewMusico = new Musico(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _musicoCollection.InsertOne(NewMusico);
            
            return StatusCode(201, "Musico adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterMusicos()
        {
            var Musicos = _musicoCollection.Find(Builders<Musico>.Filter.Empty).ToList();
            
            return Ok(Musicos);
        }

        [HttpPut]
        public ActionResult AtualizarMusico([FromBody] MusicoDto dto)
        {
            var musico = new Musico(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);
            _musicoCollection.UpdateOne(Builders<Musico>.Filter.Where(_ => _.DataNascimento == dto.DataNascimento), Builders<Musico>.Update.Set("sexo", dto.Sexo));

            return Ok("Atualizado com sucesso!!!");
        }

        [HttpDelete("{dataNasc}")]
        public ActionResult DeletarMusico(DateTime  dataNasc)
        {
            _musicoCollection.DeleteOne(Builders<Musico>.Filter.Where(_ => _.DataNascimento == dataNasc));

            return Ok("Deletado com sucesso!!!");
        }
    }
}
