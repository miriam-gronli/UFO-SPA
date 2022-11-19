using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KundeApp2.Controllers;
using KundeApp2.DAL;
using KundeApp2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UFO_Test
{
    public class ObservasjonTest
    {
        //private const string _loggetInn = "loggetInn";
        //private const string _ikkeLoggetInn = "";

        private readonly Mock<IObservasjonRepository> mockRep = new Mock<IObservasjonRepository>();
        private readonly Mock<ILogger<ObservasjonController>> mockLog = new Mock<ILogger<ObservasjonController>>();

        //Logginn
        //private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        //private readonly MockHttpSession mockSession = new MockHttpSession();

        //HentAlle OK

        [Fact]
        public async Task HentAlleLoggInnOK()
        {

            var obs1 = new Observasjon
            {
                Id = 1,
                Navn = "Test Case",
                Postkode = "0582",
                Beskrivelse = "OMG, det var en UFO!",
                Dato = "19.07.21",
                Tid = "Midnight"
            };
            var obs2 = new Observasjon
            {
                Id = 2,
                Navn = "Nut Case",
                Postkode = "6103",
                Beskrivelse = "Helt seriøst ass, ble kidnappet. Mye grønt lys.",
                Dato = "21.10.11",
                Tid = "23.16"
            };
            var obs3 = new Observasjon
            {
                Id = 3,
                Navn = "Ove",
                Postkode = "8065",
                Beskrivelse = "Lilla lys bak sukkertoppen.",
                Dato = "12.02.86",
                Tid = "16:00"
            };

            var obsListe = new List<Observasjon>();
            obsListe.Add(obs1);
            obsListe.Add(obs2);
            obsListe.Add(obs3);

            mockRep.Setup(o => o.HentAlle()).ReturnsAsync(() => obsListe);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object); 

            //mockSession[_loggetInn] = _loggetInn;
            //mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            //obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await obsController.HentAlle() as OkObjectResult;

            //Assert.Equal((int)HttpStatusCode.OK,resultat.StatusCode);
            Assert.Equal<List<Observasjon>>((List<Observasjon>)resultat.Value, obsListe);
        }

//HentAlleTomListe
        /* Får ikke denne til å fungere
         *      [Fact]
                public async Task HentAlleTomListeInnOK()
                {
                    var obsListe = new List<Observasjon>();

                    mockRep.Setup(o => o.HentAlle()).ReturnsAsync(() => null);
                    var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);  // Husk å mocke logg her 

                    //mockSession[_loggetInn] = _loggetInn;
                    //mockHttpContext.Setup(s => s.Session).Returns(mockSession);
                    //obsController.ControllerContext.HttpContext = mockHttpContext.Object;

                    var resultat = await obsController.HentAlle() as NotFoundObjectResult;

                    //Assert.Equal((int)HttpStatusCode.OK,resultat.StatusCode);
                    Assert.(resultat.Value);
                } */

/* Loginn avhening
         [Fact]
         public async Task HentAlleIkkeLoggetInn()
         {
             // Arrange

             mockRep.Setup(o => o.HentAlle()).ReturnsAsync(It.IsAny<List<Observasjon>>());

             var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

             mockSession[_loggetInn] = _ikkeLoggetInn;
             mockHttpContext.Setup(s => s.Session).Returns(mockSession);
             kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

             // Act
             var resultat = await obsController.HentAlle() as UnauthorizedObjectResult;

             // Assert 
             Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
             Assert.Equal("Ikke logget inn", resultat.Value);
         }
         */

        [Fact]
        public async Task LagreLoggetInnOK()
        {
            //Arrange
            /*ar innObs = new Observasjon
            {
                Id = 1,
                Navn = "Test Case",
                Postkode = "0582",
                Beskrivelse = "OMG, det var en UFO!",
                Dato = "19.07.21",
                Tid = "Midnight"
            };

            var mock = new Mock<IObservasjonRepository>();
            mock.Setup(o => o.Lagre(innObs)).ReturnsAsync(true);
            */
            mockRep.Setup(o => o.Lagre(It.IsAny<Observasjon>())).ReturnsAsync(true);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            //mockSession[_loggetInn] = _loggetInn;
            //mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            //obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await obsController.Lagre(It.IsAny<Observasjon>()) as OkObjectResult;

            //Assert

            //Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Observasjon lagret", resultat.Value);

        }

        //Lagre LoggetInnFeilModel


        //LagreIkkeLoggetInn


        /*LargeIkkeOK  => ?
                         * 
                        [Fact]
                        public async Task LagreIkkeOK()
                        {
                            var innObs = new Observasjon
                            {
                                Id = 1,
                                Navn = "Test Case",
                                Postkode = "0582",
                                Beskrivelse = "OMG, det var en UFO!",
                                Dato = "19.07.21",
                                Tid = "Midnight"
                            };

                            var mock = new Mock<IObservasjonRepository>();
                            mock.Setup(o => o.Lagre(innObs)).ReturnsAsync(false);
                            var obsController = new ObservasjonController(mock.Object);  // Husk å mocke logg her 
                            bool resultat = await obsController.Lagre(innObs);
                            Assert.False(resultat);
                        }
                        */


        //HenteEn OK => HenteEn LoggetInn OK / Ikke endret noe fra EksamenV3

        /* 
        [Fact]
        public async Task HentEnOK()
        {
            var returObs = new Observasjon
            {
                Id = 1,
                Navn = "Test Case",
                Postkode = "0582",
                Beskrivelse = "OMG, det var en UFO!",
                Dato = "19.07.21",
                Tid = "Midnight"
            };

            var mock = new Mock<IObservasjonRepository>();
            mock.Setup(o => o.HentEn(1)).ReturnsAsync(returObs);
            var obsController = new ObservasjonController(mock.Object);  // Husk å mocke logg her 
            Observasjon resultat = await obsController.HentEn(1);
            Assert.Equal<Observasjon>(returObs, resultat);

        }
        */

        //HenteEn IkkeOK => HenteEn LoggetInn IkkeOK / Ikke endret noe fra EksamenV3

        /*
         [Fact]
        public async Task HentEnIkkeOK()
        {
            var returObs = new Observasjon
            {
                Id = 1,
                Navn = "Test Case",
                Postkode = "0582",
                Beskrivelse = "OMG, det var en UFO!",
                Dato = "19.07.21",
                Tid = "Midnight"
            };

            var mock = new Mock<IObservasjonRepository>();
            mock.Setup(o => o.HentEn(1)).ReturnsAsync(returObs);
            var obsController = new ObservasjonController(mock.Object);  // Husk å mocke logg her 
            Observasjon resultat = await obsController.HentEn(1);
            Assert.Equal<Observasjon>(returObs, resultat);

        }
         */

        //Slett OK => SlettLoggetInnOK / Ikke endret noe fra EksamenV3

        /*
         [Fact]
        public async Task SlettOK()
        {
            var mock = new Mock<IObservasjonRepository>();
            mock.Setup(o => o.Slett(1)).ReturnsAsync(true);
            var obsController = new ObservasjonController(mock.Object);  // Husk å mocke logg her
            bool resultat = await obsController.Slett(1);
            Assert.True(resultat);
        }
         */


        //SlettIkkeOK => SlettLoggetInnOk / Ikke endret noe fra EksamenV3

        /*
        [Fact]
        public async Task SlettIkkeOK()
        {
            var mock = new Mock<IObservasjonRepository>();
            mock.Setup(o => o.Slett(1)).ReturnsAsync(false);
            var obsController = new ObservasjonController(mock.Object);  // Husk å mocke logg her
            bool resultat = await obsController.Slett(1);
            Assert.False(resultat);
        } 
        */

        //SletteIkkeLoggetInn 



        //Endre OK => EndreLoggetInnOK / Ikke endret noe fra EksamenV3

        /*
         [Fact]
        public async Task EndreOK()
        {
            var innObs = new Observasjon
            {
                Id = 1,
                Navn = "Test Case",
                Postkode = "0582",
                Beskrivelse = "OMG, det var en UFO!",
                Dato = "19.07.21",
                Tid = "Midnight"
            };

            var mock = new Mock<IObservasjonRepository>();
            mock.Setup(o => o.Endre(innObs)).ReturnsAsync(true);
            var obsController = new ObservasjonController(mock.Object); //Huske logg
            bool resultat = await obsController.Endre(innObs);
            Assert.True(resultat);
        }
         */



        //Endre IkkeOK => EndreLoggetInnIkkeOK / Ikke endret noe fra EksamenV3

        /*
        [Fact]
        public async Task EndreIkkeOK()
        {
            var innObs = new Observasjon
            {
                Id = 1,
                Navn = "Test Case",
                Postkode = "0582",
                Beskrivelse = "OMG, det var en UFO!",
                Dato = "19.07.21",
                Tid = "Midnight"
            };

            var mock = new Mock<IObservasjonRepository>();
            mock.Setup(o => o.Endre(innObs)).ReturnsAsync(false);
            var obsController = new ObservasjonController(mock.Object); //Huske logg
            bool resultat = await obsController.Endre(innObs);
            Assert.False(resultat);
        }

         */


        //EndreLoggetInnFeilModel() 

        //EndreIkkeLoggetInn()

        //LoggInnOK()

        //LoggInnFeilPassordEllerBruker()

        //LoggInnInputFeil()

        //LoggUt()

    }
}

