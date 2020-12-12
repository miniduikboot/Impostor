using System.Collections;
using System.Threading.Tasks;
using Impostor.Api.Games;

namespace Electric.AUProximity
{
    public class ProximityGameManager
    {
        public ArrayList TrackedGames { get; }

        public ProximityGameManager()
        {
            TrackedGames = new ArrayList();
        }

        public void StartTracking(GameCode gameCode)
        {
            if (!TrackedGames.Contains(gameCode))
            {
                this.TrackedGames.Add(gameCode);
            }
        }

        public void StopTracking(GameCode gameCode)
        {
            this.TrackedGames.Remove(gameCode);
        }
    }
}
