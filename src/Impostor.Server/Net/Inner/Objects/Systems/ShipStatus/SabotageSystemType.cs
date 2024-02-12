using System;
using System.Threading.Tasks;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;
using Impostor.Api.Innersloth;

namespace Impostor.Server.Net.Inner.Objects.Systems.ShipStatus
{
    public class SabotageSystemType : ISystemType
    {
        private readonly IActivatable[] _specials;

        public SabotageSystemType(IActivatable[] specials)
        {
            _specials = specials;
        }

        public float Timer { get; set; }

        public void Serialize(IMessageWriter writer, bool initialState)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IMessageReader reader, bool initialState)
        {
            Timer = reader.ReadSingle();
        }

        public Task<bool> UpdateSystemAsync(IClientPlayer sender, IInnerPlayerControl target, IMessageReader reader)
        {
            // TODO
            // if (sender.IsHost && _antiCheatConfig)
            throw new NotImplementedException();
        }
    }
}
