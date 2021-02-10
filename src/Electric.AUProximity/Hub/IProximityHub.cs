using System.Threading.Tasks;
using Electric.AUProximity.Models;
using Impostor.Api.Innersloth;

namespace Electric.AUProximity.Hub
{
    public interface IProximityHub
    {
        Task GameStarted();
        Task PlayerMove(string playerName, Pose pose);
        Task MeetingCalled();
        Task MeetingEnded();
        Task PlayerExiled(string playerName);
        Task GameEnd();
        Task CommsSabotage(bool fix);
        Task MapChange(MapTypes id);
        Task HostChange(string name);
    }
}
