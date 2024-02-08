using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;

namespace Impostor.Server.Net.Inner.Objects.Systems.ShipStatus
{
    public class ElectricalDoors : ISystemType
    {
        private readonly Dictionary<int, bool> _doors;

        public ElectricalDoors(Dictionary<int, bool> doors)
        {
            _doors = doors;
        }

        public void Serialize(IMessageWriter writer, bool initialState)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IMessageReader reader, bool initialState)
        {
            var num = reader.ReadUInt32();
            for (var i = 0; i < _doors.Count; i++)
            {
                _doors[i] = (num & (ulong)(1L << (i & 31))) > 0UL;
            }
        }

        public Task<bool> UpdateSystemAsync(IClientPlayer sender, IInnerPlayerControl target, IMessageReader reader)
        {
            return Task.FromResult(true);
        }
    }
}
