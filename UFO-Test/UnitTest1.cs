using System;
using System.Threading.Tasks;
using KundeApp2.Controllers;
using KundeApp2.DAL;
using KundeApp2.Model;
using Moq;
using Xunit;

namespace UFO_Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Lagre()
        {
            //Arrange
            var innObservasjon = new Observasjon
            {
                Id = 1,
                Navn = "Test Case",
                Postkode = "0582",
                Beskrivelse = "OMG, det var en UFO!",
                Dato = "19.07.21",
                Tid = "Midnight",
            };

            var mock = new Mock<IObservasjonRepository>();
            mock.Setup(o => o.Lagre(innObservasjon)).ReturnsAsync(true);
            var observasjonController = new ObservasjonController(mock.Object);

            //Act
            bool resultat = await observasjonController.Lagre(innObservasjon);

            //Assert
            Assert.True(resultat);


        }
    }
}

