using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyClone.API.Services
{
    public interface ISpotifyService
    {
        Task<IEnumerable<object>> BuscarContenidoAsync(string query);
    }
}