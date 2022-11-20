//Denne koden er hentet fra "Model" mappen som igjen ligger under mappen "KundeApp2-med-hash-logginn" hentet fra canvas

using System;
using System.ComponentModel.DataAnnotations;

namespace EksamenVersjon3.Model
{
    public class Bruker
    {
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,20}$")]
        public String Brukernavn { get; set; }
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$")]
        public String Passord { get; set; }
    }
}

