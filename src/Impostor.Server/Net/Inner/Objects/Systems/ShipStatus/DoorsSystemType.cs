using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impostor.Api.Config;
using Impostor.Api.Innersloth;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;

namespace Impostor.Server.Net.Inner.Objects.Systems.ShipStatus
{
    public class DoorsSystemType : ISystemType
    {
        private readonly Dictionary<SystemTypes, float> _timers = new Dictionary<SystemTypes, float>();
        private readonly AntiCheatConfig _antiCheatConfig;
        private readonly Dictionary<int, bool> _doors;

        public DoorsSystemType(AntiCheatConfig antiCheatConfig, Dictionary<int, bool> doors)
        {
            _antiCheatConfig = antiCheatConfig;
            _doors = doors;
        }

        public void Serialize(IMessageWriter writer, bool initialState)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IMessageReader reader, bool initialState)
        {
            var num = reader.ReadByte();
            for (var i = 0; i < num; i++)
            {
                var systemType = (SystemTypes)reader.ReadByte();
                var value = reader.ReadSingle();

                _timers[systemType] = value;
            }

            for (var j = 0; j < _doors.Count; j++)
            {
                _doors[j] = reader.ReadBoolean();
            }
        }

        public async Task<bool> UpdateSystemAsync(IClientPlayer sender, IInnerPlayerControl target, IMessageReader reader)
        {
            var update = reader.ReadByte();
            var doorId = update & 0x1F;
            if ((update & 0xC0) != 0x40)
            {
                if (_antiCheatConfig.EnableSabotageChecks &&
                    await sender.Client.ReportCheatAsync(SystemTypes.Doors, $"Performed unknown action {update & 0xC0} on door {doorId}"))
                {
                    return false;
                }

                return true;
            }
            if (_doors.ContainsKey(doorId))
            {
                _doors[doorId] = true;
            }
            else
            {
                if (!_antiCheatConfig.AllowProtocolExtensions &&
                    await sender.Client.ReportCheatAsync(SystemTypes.Doors, $"Tried to open unknown door {doorId}"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
