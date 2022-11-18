using System;
using System.ComponentModel.DataAnnotations;

namespace KundeApp2.Model
{
    public class Observasjon //Vanlig POJO klasse for observasjonene
    {
        public int Id { get; set; }  // Id blir brukt som auto-increment i databasen

        public string Navn { get; set; }
        [RegularExpression(@"[0-9a-zA-ZøæåØÆÅ\\:. ]{2,30}")]
        public string Postkode { get; set; }
        [RegularExpression(@"[0-9a-zA-ZøæåØÆÅ\\:. ]{2,30}")]
        public string Beskrivelse { get; set; }
        [RegularExpression(@"[0-9a-zA-ZøæåØÆÅ\\:. ]{2,30}")]
        public string Dato { get; set; }
        [RegularExpression(@"[0-9a-zA-ZøæåØÆÅ\\:. ]{2,30}")]
        public string Tid { get; set; }

    }
}
