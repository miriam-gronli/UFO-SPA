using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KundeApp2.DAL;
using KundeApp2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KundeApp2.Controllers
{
    [ApiController]
    //Denne klassen er hentet fra KundeApp2-med-DB filen i KundeApp2-med-DAL mappen fra canvas
    [Route("api/[controller]")]
    public class ObservasjonController : ControllerBase
    {
        private IObservasjonRepository _db; //Initierer IObservasjoRepository db variabel

        private ILogger<ObservasjonController> _log; //Initierer IILoggerFactory i controllern

        //Dependency Injection av IObservasjonRepository
        //ILogger blir tatt inn i controllern
        public ObservasjonController(IObservasjonRepository db, ILogger<ObservasjonController> log)
        {
            _db = db;
            _log = log;
        }


        //Følgende asynkrone CRUD metoder blir initialisert og returnerer metodene i IObservasjonRepository
        [HttpPost]
        public async Task<ActionResult> Lagre(Observasjon innObservasjon)
        {

            //_log.LogInformation("En ny observasjon har blitt lagret."); //Logger til fil dersom en ny observasjon har blitt lagret
            //return await _db.Lagre(innObservasjon);

            if (ModelState.IsValid)
            {
                bool returOK = await _db.Lagre(innObservasjon);
                if (!returOK)
                {
                    _log.LogInformation("Observasjonen kunne ikke lagres!");
                    return BadRequest();
                }
                return Ok(); // kan ikke returnere noe tekst da klient prøver å konvertere deene som en Json-streng
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> HentAlle()
        {
            //_log.LogInformation("Alle observasjoner har blitt hentet."); //Logger til fil at alle observasjoner har blitt hentet
            //return await _db.HentAlle();

            List<Observasjon> alleObservasjoner = await _db.HentAlle();
            return Ok(alleObservasjoner);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Slett(int id)
        {
            //_log.LogInformation("En observasjon har blitt slettet."); //Logger til fil dersom en observasjon har blitt slettet
            //return await _db.Slett(id);

            bool returOK = await _db.Slett(id);
            if (!returOK)
            {
                _log.LogInformation("Sletting av Observasjonen ble ikke utført");
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> HentEn(int id)
        {
            //_log.LogInformation("En observasjon har blitt hentet."); //Logger til fil dersom bare en observasjon har blitt hentet
            //return await _db.HentEn(id);

            if (ModelState.IsValid)
            {
                Observasjon observasjonen = await _db.HentEn(id);
                if (observasjonen == null)
                {
                    _log.LogInformation("Fant ikke Observasjonen");
                    return NotFound();
                }
                return Ok(observasjonen);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult> Endre(Observasjon endreObservasjon)
        {
            //_log.LogInformation("En observasjon har blitt endret."); //Logger til fil dersom en observasjon har blitt endret
            //return await _db.Endre(endreObservasjon);

            if (ModelState.IsValid)
            {
                bool returOK = await _db.Endre(endreObservasjon);
                if (!returOK)
                {
                    _log.LogInformation("Endringen kunne ikke utføres");
                    return NotFound();
                }
                return Ok();
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest();
        }
    }
}
