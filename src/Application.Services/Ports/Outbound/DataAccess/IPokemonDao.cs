using Domain.Model;
using System.Collections.Generic;

namespace Application.Services.Ports.Outbound.DataAccess
{
    public interface IPokemonDao
    {
        List<Pokemon> GetAll();
    }
}
