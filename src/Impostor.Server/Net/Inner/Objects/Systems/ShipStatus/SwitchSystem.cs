using System;
using System.Threading.Tasks;
using Impostor.Api;
using Impostor.Api.Innersloth;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;

namespace Impostor.Server.Net.Inner.Objects.Systems.ShipStatus
{
    public class SwitchSystem : ISystemType, IActivatable
    {
        public byte ExpectedSwitches { get; set; }

        public byte ActualSwitches { get; set; }

        public byte Value { get; set; } = byte.MaxValue;

        public bool IsActive { get; }

        public void Serialize(IMessageWriter writer, bool initialState)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IMessageReader reader, bool initialState)
        {
            ExpectedSwitches = reader.ReadByte();
            ActualSwitches = reader.ReadByte();
            Value = reader.ReadByte();
        }

        public async Task<bool> UpdateSystemAsync(IClientPlayer sender, IInnerPlayerControl target, IMessageReader reader)
        {
            if (sender.Character != target)
            {
                if (await sender.Client.ReportCheatAsync(SystemTypes.Electrical, CheatCategory.Ownership, "Attempted to change switches as another player")) {
                    return false;
                }
            }

            return true;
        }
    }
}
