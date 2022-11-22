using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EksamenVersjon3.Model;
using KundeApp2.Controllers;
using KundeApp2.DAL;
using KundeApp2.Model;
using KundeAppTest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UFO_Test
{
    public class ObservasjonTest
    {        
        private readonly Mock<IObservasjonRepository> mockRep = new Mock<IObservasjonRepository>();
        private readonly Mock<ILogger<ObservasjonController>> mockLog = new Mock<ILogger<ObservasjonController>>();

        //Logginn
        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";


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

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await obsController.HentAlle() as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK,resultat.StatusCode);
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


         [Fact]
         public async Task HentAlleIkkeLoggetInn()
         {
             // Arrange
             mockRep.Setup(o => o.HentAlle()).ReturnsAsync(It.IsAny<List<Observasjon>>());

             var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

             mockSession[_loggetInn] = _ikkeLoggetInn;
             mockHttpContext.Setup(s => s.Session).Returns(mockSession);
             obsController.ControllerContext.HttpContext = mockHttpContext.Object;

             // Act
             var resultat = await obsController.HentAlle() as UnauthorizedObjectResult;

             // Assert 
             Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
             Assert.Equal("Ikke logget inn", resultat.Value); //Stemmer ikke oversens med controlleren for dette resultatet. Klarer ikke å 
         }
         

        [Fact]
        public async Task LagreLoggetInnOK()
        {
            //Arrange
            mockRep.Setup(o => o.Lagre(It.IsAny<Observasjon>())).ReturnsAsync(true);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await obsController.Lagre(It.IsAny<Observasjon>()) as OkObjectResult;

            //Assert

            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("En ny observasjon har blitt lagret", resultat.Value);

        }

        [Fact]
        public async Task LagreIkkeOK()
        {            
            mockRep.Setup(o => o.Lagre(It.IsAny<Observasjon>())).ReturnsAsync(false);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);  // Husk å mocke logg her 

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await obsController.Lagre(It.IsAny<Observasjon>()) as BadRequestObjectResult;

            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Observasjon kunne ikke lagres", resultat.Value);
        }


        //Lagre LoggetInnFeilModel //Feiler, usikker på hvorfor.

        [Fact]
        public async Task LagreLoggetInnFeilModel()  
        {
            mockRep.Setup(o => o.Lagre(It.IsAny<Observasjon>())).ReturnsAsync(true);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            obsController.ModelState.AddModelError("", "Feil i inputvalidering"); //Finnes det spesefike feil medlinger avhengig av hva som er feil?

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await obsController.Lagre(It.IsAny<Observasjon>()) as BadRequestObjectResult;

            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }


        //Lagre IkkeLoggetInn

        [Fact]
        public async Task LagreIkkeLoggetInn()
        {
            mockRep.Setup(o => o.Lagre(It.IsAny<Observasjon>())).ReturnsAsync(true);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.Lagre(It.IsAny<Observasjon>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }


        //HenteEn LoggetInn OK
        
        [Fact]
        public async Task HentEnLoggetInnOK()
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
      
            mockRep.Setup(o => o.HentEn(It.IsAny<int>())).ReturnsAsync(returObs);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.HentEn(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<Observasjon>(returObs, (Observasjon)resultat.Value);
        }


        //HenteEn LoggetInn IkkeOK

        [Fact]
        public async Task HentEnLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(o => o.HentEn(It.IsAny<int>())).ReturnsAsync(() => null);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.HentEn(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Fant ikke observasjonen", resultat.Value);
        }


        //HenteEn Ikke Logget inn.

        [Fact]
        public async Task HentEnIkkeLoggetInn()
        {
            mockRep.Setup(o => o.HentEn(It.IsAny<int>())).ReturnsAsync(() => null);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.HentEn(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }


        //Slett LoggetInn OK

        [Fact]
        public async Task SlettLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(o => o.Slett(It.IsAny<int>())).ReturnsAsync(true);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.Slett(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Observasjon slettet", resultat.Value);
        }


        //Slett LoggetInn IkkeOK

        [Fact]
        public async Task SlettLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(o => o.Slett(It.IsAny<int>())).ReturnsAsync(false);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.Slett(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Sletting av observasjon ble ikke utført", resultat.Value);
        }


        //Slette IkkeLoggetInn 

        [Fact]
        public async Task SletteIkkeLoggetInn()
        {
            mockRep.Setup(o => o.Slett(It.IsAny<int>())).ReturnsAsync(true);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.Slett(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }


        //Endre LoggetInn OK 

        [Fact]
        public async Task EndreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(o => o.Endre(It.IsAny<Observasjon>())).ReturnsAsync(true);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.Endre(It.IsAny<Observasjon>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Observasjon endret", resultat.Value);
        }


        //Endre LoggetInn IkkeOK       

        [Fact]
        public async Task EndreLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(o => o.Lagre(It.IsAny<Observasjon>())).ReturnsAsync(false);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.Endre(It.IsAny<Observasjon>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Endringen av observasjonen kunne ikke utføres", resultat.Value);
        }


        //Endre LoggetInn FeilModel 

        [Fact]
        public async Task EndreLoggetInnFeilModel()
        {
            mockRep.Setup(o => o.Endre(It.IsAny<Observasjon>())).ReturnsAsync(true);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            obsController.ModelState.AddModelError("", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.Endre(It.IsAny<Observasjon>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }


        //Endre IkkeLogget Inn

        [Fact]
        public async Task EndreIkkeLoggetInn()
        {
            mockRep.Setup(o => o.Endre(It.IsAny<Observasjon>())).ReturnsAsync(true);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.Endre(It.IsAny<Observasjon>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }


        //LoggInn OK

        [Fact]
        public async Task LoggInnOK()
        {
            mockRep.Setup(o => o.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.True((bool)resultat.Value);
        }

        //LoggInn FeilPassordEllerBruker - Feil fordi outputen er anderledes, usikker på hvordan endre det her. Hvorfor funket ikke denne med It.IsAny?

        [Fact]
        public async Task LoggInnFeilPassordEllerBruker()
        {

            var returBruker = new Bruker
            {
                Brukernavn = "Test Case",
                Passord = "SuperSafe"
            };

            mockRep.Setup(k => k.LoggInn(returBruker)).ReturnsAsync(false);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.LoggInn(returBruker) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.False((bool) resultat.Value);
        }


        //Logg Inn Input Feil

        [Fact]
        public async Task LoggInnInputFeil()
        {
            mockRep.Setup(o => o.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            obsController.ModelState.AddModelError("Brukernavn", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await obsController.LoggInn(It.IsAny<Bruker>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }


        //LoggUt

        [Fact]
        public void LoggUt()
        {
            var obsController = new ObservasjonController(mockRep.Object, mockLog.Object);

            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession[_loggetInn] = _loggetInn;
            obsController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            obsController.LoggUt();

            // Assert
            Assert.Equal(_ikkeLoggetInn, mockSession[_loggetInn]);
        }
    }
}