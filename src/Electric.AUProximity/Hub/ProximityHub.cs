using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Impostor.Api.Games;
using Impostor.Api.Games.Managers;

namespace Electric.AUProximity.Hub
{
    public class ProximityHub : Hub<IProximityHub>
    {
        private readonly IGameManager _gameManager;

        public ProximityHub(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public async Task TrackGame(string gameCode)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameCode);
            IGame? game = _gameManager.Find(gameCode);
            if (game != null)
            {
                await this.Clients.Group(gameCode).MapChange(game.Options.Map);
                await this.Clients.Group(gameCode).HostChange(game.Host.Client.Name);
            }
        }
    }
}
