using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KundeApp2.Model;

namespace KundeApp2.DAL
{
    public interface IObservasjonRepository
    {
        Task<bool> Lagre(Observasjon innObservasjon);
        Task<List<Observasjon>> HentAlle();
        Task<bool> Slett(int id);
        Task<Observasjon> HentEn(int id);
        Task<bool> Endre(Observasjon endreObservasjon);
    }
}
