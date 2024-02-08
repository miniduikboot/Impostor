using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impostor.Api.Config;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;
using Impostor.Api.Innersloth;

namespace Impostor.Server.Net.Inner.Objects.Systems.ShipStatus
{
    public class MedScanSystem : ISystemType
    {
        public MedScanSystem(AntiCheatConfig antiCheatConfig)
        {
            _antiCheatConfig = antiCheatConfig;
            UsersList = new List<byte>();
        }

        private readonly AntiCheatConfig _antiCheatConfig;

        public List<byte> UsersList { get; }

        public void Serialize(IMessageWriter writer, bool initialState)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IMessageReader reader, bool initialState)
        {
            UsersList.Clear();

            var num = reader.ReadPackedInt32();

            for (var i = 0; i < num; i++)
            {
                UsersList.Add(reader.ReadByte());
            }
        }

        public async Task<bool> UpdateSystemAsync(IClientPlayer sender, IInnerPlayerControl target, IMessageReader reader)
        {
            var updateType = reader.ReadByte();
            var playerId = (byte)(updateType & 0x1F);

            if (target.PlayerId != playerId)
            {
                if (_antiCheatConfig.EnableOwnershipChecks &&
                    await sender.Client.ReportCheatAsync(SystemTypes.MedBay, "Tried to mess with another players queue position"))
                {
                    return false;
                }
            }

            if ((updateType & 0x80) == 0x80)
            {
                if (UsersList.Contains(playerId))
                {
                    if (_antiCheatConfig.EnableGameFlowChecks &&
                        await sender.Client.ReportCheatAsync(SystemTypes.MedBay, "Tried to add the player to the medbay queue again"))
                    {
                        return false;
                    }
                }
                else
                {
                    UsersList.Add(playerId);
                }
            }
            else if ((updateType & 0x40) == 0x40)
            {
                if (UsersList.Count > 0 && UsersList[0] == playerId)
                {
                    UsersList.Remove(playerId);
                }
                else
                {
                    if (_antiCheatConfig.EnableGameFlowChecks &&
                        await sender.Client.ReportCheatAsync(SystemTypes.MedBay, "Invalid medbay dequeuing operating"))
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (_antiCheatConfig.EnableGameFlowChecks &&
                    await sender.Client.ReportCheatAsync(SystemTypes.MedBay, "Unknown medbay queue operation"))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
