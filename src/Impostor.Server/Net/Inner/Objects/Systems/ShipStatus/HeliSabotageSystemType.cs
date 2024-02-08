using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impostor.Api.Config;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;
using Impostor.Api.Innersloth;

namespace Impostor.Server.Net.Inner.Objects.Systems.ShipStatus
{
    public class HeliSabotageSystemType : ISystemType, IActivatable
    {
        public HeliSabotageSystemType(AntiCheatConfig antiCheatConfig)
        {
            _antiCheatConfig = antiCheatConfig;
            Countdown = 10000f;
            ActiveConsoles = new HashSet<Tuple<byte, byte>>();
            CompletedConsoles = new HashSet<byte>();
        }

        private readonly AntiCheatConfig _antiCheatConfig;

        public float Countdown { get; private set; }

        public float Timer { get; private set; }

        public HashSet<Tuple<byte, byte>> ActiveConsoles { get; }

        public HashSet<byte> CompletedConsoles { get; }

        public bool IsActive => Countdown < 10000.0;

        public void Serialize(IMessageWriter writer, bool initialState)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IMessageReader reader, bool initialState)
        {
            Countdown = reader.ReadSingle();
            Timer = reader.ReadSingle();
            ActiveConsoles.Clear(); // TODO: Thread safety
            CompletedConsoles.Clear(); // TODO: Thread safety

            var activeCount = reader.ReadPackedUInt32();

            for (var i = 0; i < activeCount; i++)
            {
                ActiveConsoles.Add(new Tuple<byte, byte>(reader.ReadByte(), reader.ReadByte()));
            }

            var completedCount = reader.ReadPackedUInt32();

            for (var i = 0; i < completedCount; i++)
            {
                CompletedConsoles.Add(reader.ReadByte());
            }
        }

        public async Task<bool> UpdateSystemAsync(IClientPlayer sender, IInnerPlayerControl target, IMessageReader reader)
        {
            var update = reader.ReadByte();
            var consoleId = (byte)(update & 0x0F);
            switch (update & 0xF0)
            {
                case 0x80:
                    // Trigger the sabotage
                    if (_antiCheatConfig.EnableHostPrivilegeChecks &&
                        await sender.Client.ReportCheatAsync(SystemTypes.HeliSabotage, "Non-host player attempted to start sabotage"))
                    {
                        return false;
                    }
                    break;
                case 0x40:
                // Player opens a console
                case 0x20:
                // Player closes a console
                case 0x10:
                    // Player completes a console
                    if (_antiCheatConfig.EnableOwnershipChecks &&
                        await sender.Client.ReportCheatAsync(SystemTypes.HeliSabotage, $"Player tried to attempt action {update} on other player"))
                    {
                        return false;
                    }
                    if (consoleId >= 2 && _antiCheatConfig.EnableSabotageChecks &&
                        await sender.Client.ReportCheatAsync(SystemTypes.HeliSabotage, $"Player tried to interact with nonexistent console {consoleId}"))
                    {
                        return false;
                    }
                    break;

            }
            return true;
        }
    }
}
