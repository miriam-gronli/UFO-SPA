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

        //HentAlle OK/IkkeOK

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


    }
}

