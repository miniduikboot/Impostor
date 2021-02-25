using Impostor.Api.Events.Player;
using Impostor.Api.Games;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;
using Microsoft.Extensions.ObjectPool;
using System.Numerics;

namespace Impostor.Server.Events.Player
{
    public class PlayerMovementEvent : IPlayerMovementEvent
    {
        public IGame Game { get; private set; }

        public IClientPlayer ClientPlayer { get; private set; }

        public IInnerPlayerControl PlayerControl { get; private set; }

        public Vector2 TargetPosition { get; private set; }

        public Vector2 TargetVelocity { get; private set; }

        public void Reset(IGame game, IClientPlayer clientPlayer, IInnerPlayerControl playerControl, Vector2 targetPosition, Vector2 targetVelocity)
        {
            Game = game;
            ClientPlayer = clientPlayer;
            PlayerControl = playerControl;
            TargetPosition = targetPosition;
            TargetVelocity = targetVelocity;
        }

        public void Reset()
        {
            Game = null;
            ClientPlayer = null;
            PlayerControl = null;
            TargetPosition = Vector2.Zero;
            TargetVelocity = Vector2.Zero;
        }
    }

    public class PlayerMovementEventObjectPolicy : IPooledObjectPolicy<PlayerMovementEvent>
    {
        public PlayerMovementEvent Create()
        {
            return new PlayerMovementEvent();
        }

        public bool Return(PlayerMovementEvent obj)
        {
            obj.Reset();
            return true;
        }
    }
}
