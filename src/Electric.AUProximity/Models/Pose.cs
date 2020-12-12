namespace Electric.AUProximity.Models
{
    public class Pose
    {
        public float X { get; }
        public float Y { get; }

        public Pose(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
