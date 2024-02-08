using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impostor.Api;
using Impostor.Api.Innersloth;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;

namespace Impostor.Server.Net.Inner.Objects.Systems.ShipStatus
{
    public class ReactorSystemType : ISystemType, IActivatable
    {
        public ReactorSystemType()
        {
            Countdown = 10000f;
            UserConsolePairs = new HashSet<Tuple<byte, byte>>();
        }

        public float Countdown { get; private set; }

        public HashSet<Tuple<byte, byte>> UserConsolePairs { get; }

        public bool IsActive => Countdown < 10000.0;

        public void Serialize(IMessageWriter writer, bool initialState)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IMessageReader reader, bool initialState)
        {
            Countdown = reader.ReadSingle();
            UserConsolePairs.Clear(); // TODO: Thread safety

            var count = reader.ReadPackedInt32();

            for (var i = 0; i < count; i++)
            {
                UserConsolePairs.Add(new Tuple<byte, byte>(reader.ReadByte(), reader.ReadByte()));
            }
        }

        public async Task<bool> UpdateSystemAsync(IClientPlayer sender, IInnerPlayerControl target, IMessageReader reader)
        {
            var updateType = reader.ReadByte();
            if (updateType == 128)
            {
                // Start sabotage
            }
            else if (updateType == 16)
            {
                // Repaired
            }
            else if ((updateType & 64) == 64)
            {
                // Console completed
            }
            else if ((updateType & 32) == 32)
            {
                // Console cleared
            }
            else
            {
                if (await sender.Client.ReportCheatAsync(SystemTypes.LifeSupp, CheatCategory.Sabotage, $"Player performed unknown update type {updateType}"))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
