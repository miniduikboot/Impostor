using System;
using System.Threading.Tasks;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;
using Impostor.Api.Innersloth;
using Impostor.Api.Config;

namespace Impostor.Server.Net.Inner.Objects.Systems.ShipStatus
{
    public class SabotageSystemType : ISystemType
    {
        private readonly IActivatable[] _specials;
        private readonly AntiCheatConfig _antiCheatConfig;

        public SabotageSystemType(IActivatable[] specials, AntiCheatConfig antiCheatConfig)
        {
            _specials = specials;
            _antiCheatConfig = antiCheatConfig;
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
            // if (sender.IsHost && _antiCheatConfig)
            throw new NotImplementedException();
        }
    }
}
