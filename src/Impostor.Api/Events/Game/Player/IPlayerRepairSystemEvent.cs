using Impostor.Api.Innersloth;

namespace Impostor.Api.Events.Player
{
    public interface IPlayerRepairSystemEvent : IPlayerEvent
    {
        /// <summary>
        ///     System that the player sabotaged.
        /// </summary>
        SystemTypes SystemType { get; }
        /// <summary>
        ///     Amount that the system was sabotaged by.
        /// </summary>
        byte Amount { get; }
    }
}
