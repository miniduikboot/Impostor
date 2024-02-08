using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;

namespace Impostor.Server.Net.Inner.Objects.Systems.ShipStatus
{
    public class SecurityCameraSystemType : ISystemType
    {
        private HashSet<byte> PlayersUsing = new HashSet<byte>();

        public void Serialize(IMessageWriter writer, bool initialState)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IMessageReader reader, bool initialState)
        {
            PlayersUsing.Clear();
            int num = reader.ReadPackedInt32();
            for (int i = 0; i < num; i++)
            {
                PlayersUsing.Add(reader.ReadByte());
            }
        }

        public Task<bool> UpdateSystemAsync(IClientPlayer sender, IInnerPlayerControl target, IMessageReader reader)
        {
            var updateType = reader.ReadByte();
            if (updateType == 1)
            {
                PlayersUsing.Add(target.PlayerId);
            }
            else
            {
                PlayersUsing.Remove(target.PlayerId);
            }
            return Task.FromResult(true);
        }
    }
}
