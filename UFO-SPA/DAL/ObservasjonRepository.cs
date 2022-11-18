using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KundeApp2.Model;
using Microsoft.EntityFrameworkCore;

namespace KundeApp2.DAL
{
    //Denne klassen er hentet fra KundeApp2-med-DB filen i KundeApp2-med-DAL mappen fra canvas
    public class ObservasjonRepository : IObservasjonRepository //Klasse for å initiere CRUD metodene
    {
        private readonly ObservasjonContext _db; //Inititerer db varaiabel gjennom kontekst klassen

        public ObservasjonRepository(ObservasjonContext db) //Dependency Injection av ObservasjonContekst
        {
            _db = db;
        }

        public async Task<bool> Lagre(Observasjon innObservasjon) //Lagre metode for å lagre data i databasen
        {
            try
            {
                var nyObservasjonerad = new Observasjoner();
                nyObservasjonerad.Postkode = innObservasjon.Postkode;
                nyObservasjonerad.Beskrivelse = innObservasjon.Beskrivelse;
                nyObservasjonerad.Navn = innObservasjon.Navn;
                nyObservasjonerad.Dato = innObservasjon.Dato;
                nyObservasjonerad.Tid = innObservasjon.Tid;

                _db.Observasjoner.Add(nyObservasjonerad);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<List<Observasjon>> HentAlle() //HentAlle metode som henter all data fra databasen
        {
            try
            {
                List<Observasjon> alleObservasjoner = await _db.Observasjoner.Select(o => new Observasjon
                {
                    Id = o.Id,
                    Postkode = o.Postkode,
                    Beskrivelse = o.Beskrivelse,
                    Navn = o.Navn,
                    Dato = o.Dato,
                    Tid = o.Tid
                }).ToListAsync();
                return alleObservasjoner;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Slett(int id) //Slett metode som sletter data fra databasen med en spesifikk id
        {
            try
            {
                Observasjoner enDBObservasjon = await _db.Observasjoner.FindAsync(id);
                _db.Observasjoner.Remove(enDBObservasjon);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Observasjon> HentEn(int id) //Hent metode som henter ett objekt fra databasen med en spesifikk id
        {
            Observasjoner enObservasjon = await _db.Observasjoner.FindAsync(id);
            var hentetObservasjon = new Observasjon()
            {
                Id = enObservasjon.Id,
                Postkode = enObservasjon.Postkode,
                Beskrivelse = enObservasjon.Beskrivelse,
                Navn = enObservasjon.Navn,
                Dato = enObservasjon.Dato,
                Tid = enObservasjon.Tid
            };
            return hentetObservasjon;
        }

        public async Task<bool> Endre(Observasjon endreObservasjon) //Endre metode som endrer på dataen i databasen
        {
            try
            {
                var endreObjekt = await _db.Observasjoner.FindAsync(endreObservasjon.Id);
                endreObjekt.Postkode = endreObservasjon.Postkode;
                endreObjekt.Beskrivelse = endreObservasjon.Beskrivelse;
                endreObjekt.Navn = endreObservasjon.Navn;
                endreObjekt.Dato = endreObservasjon.Dato;
                endreObjekt.Tid = endreObservasjon.Tid;
                await _db.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}