using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impostor.Api;
using Impostor.Api.Innersloth;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;

namespace Impostor.Server.Net.Inner.Objects.Systems.ShipStatus
{
    public class SecurityCameraSystemType : ISystemType
    {
        private readonly HashSet<byte> playersUsing = new HashSet<byte>();

        public void Serialize(IMessageWriter writer, bool initialState)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IMessageReader reader, bool initialState)
        {
            playersUsing.Clear();
            var num = reader.ReadPackedInt32();
            for (var i = 0; i < num; i++)
            {
                playersUsing.Add(reader.ReadByte());
            }
        }

        public async Task<bool> UpdateSystemAsync(IClientPlayer sender, IInnerPlayerControl target, IMessageReader reader)
        {
            var updateType = reader.ReadByte();
            if (updateType == 1)
            {
                playersUsing.Add(target.PlayerId);
            }
            else if (updateType == 2)
            {
                playersUsing.Remove(target.PlayerId);
            }
            else
            {
                if (await sender.Client.ReportCheatAsync(SystemTypes.Security, CheatCategory.Sabotage, "Unknown update type sent"))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
