using System.Numerics;

namespace Impostor.Api.Events.Player
{
    public interface IPlayerMovementEvent : IPlayerEvent
    {
        public Vector2 TargetPosition { get; }

        public Vector2 TargetVelocity { get; }
    }
}
