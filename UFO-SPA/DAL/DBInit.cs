using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KundeApp2.DAL
{
    public static class DBInit
    {
        public static void Seed(IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.CreateScope();
            
            var db = serviceScope.ServiceProvider.GetService<ObservasjonContext>();

            // må slette og opprette databasen hver gang når den skal initieres (seed`es)
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var observasjon1 = new Observasjoner //Dummy data #1
            {
                Id = 1,
                Postkode = "0001",
                Beskrivelse = "Grønn UFO",
                Navn = "Ola Nordmann",
                Dato = "1 januar 2022",
                Tid = "22:30"
            };

            var observasjon2 = new Observasjoner //Dummy data #2
            {
                Id = 2,
                Postkode = "0002",
                Beskrivelse = "Blå UFO",
                Navn = "Sam Møller",
                Dato = "2 januar 2022",
                Tid = "20:00"
            };
            //Legger til dummy dataen i databasen
            db.Observasjoner.Add(observasjon1);
            db.Observasjoner.Add(observasjon2);

            db.SaveChanges();
        }
    }  
}
