using Microsoft.Azure.Cosmos;

namespace Persistance.Interfaces
{
    public interface ICosmosDbContainer
    {

        Container _container { get; }
    }
}
