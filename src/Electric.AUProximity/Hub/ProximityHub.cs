using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Electric.AUProximity.Hub
{
    public class ProximityHub : Hub<IProximityHub>
    {
        public async Task TrackGame(string gameCode)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameCode);
        }
    }
}
