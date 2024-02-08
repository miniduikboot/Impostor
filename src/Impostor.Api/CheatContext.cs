using Impostor.Api.Net.Inner;

namespace Impostor.Api
{
    public class CheatContext
    {
        public CheatContext(string name)
        {
            Name = name;
        }

        public static CheatContext Deserialize { get; } = new CheatContext(nameof(Deserialize));

        public static CheatContext Serialize { get; } = new CheatContext(nameof(Serialize));

        public string Name { get; }

        public static implicit operator CheatContext(RpcCalls rpcCalls) => new CheatContext(rpcCalls.ToString());

        public static implicit operator CheatContext(Innersloth.SystemTypes systemType) => new CheatContext(systemType.ToString());
    }
}
