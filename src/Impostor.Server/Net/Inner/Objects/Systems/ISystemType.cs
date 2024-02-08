using System.Threading.Tasks;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;

namespace Impostor.Server.Net.Inner.Objects.Systems
{
    public interface ISystemType
    {
        void Serialize(IMessageWriter writer, bool initialState);

        void Deserialize(IMessageReader reader, bool initialState);

        Task<bool> UpdateSystemAsync(IClientPlayer sender, IInnerPlayerControl target, IMessageReader reader);
    }
}
