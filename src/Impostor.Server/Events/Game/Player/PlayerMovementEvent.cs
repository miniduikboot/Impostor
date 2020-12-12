using System.Numerics;
using Impostor.Api.Events.Player;
using Impostor.Api.Games;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;
using Impostor.Server.Net.Inner.Objects;
using Impostor.Server.Net.State;

namespace Impostor.Server.Events.Player
{
    // TODO: Finish and use event, needs to be pooled
    public class PlayerMovementEvent : IPlayerMovementEvent
    {
        public PlayerMovementEvent(IGame game, IClientPlayer clientPlayer, IInnerPlayerControl playerControl, Vector2 targetSyncPosition, Vector2 targetSyncVelocity)
        {
            Game = game;
            ClientPlayer = clientPlayer;
            PlayerControl = playerControl;
            TargetPosition = targetSyncPosition;
            TargetVelocity = targetSyncVelocity;
        }



        public IGame Game { get; }

        public IClientPlayer ClientPlayer { get; }

        public IInnerPlayerControl PlayerControl { get; }

        public Vector2 TargetPosition { get; }

        public Vector2 TargetVelocity { get; }
    }
}
