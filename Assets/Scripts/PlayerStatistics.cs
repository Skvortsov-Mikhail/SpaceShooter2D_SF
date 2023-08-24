namespace SpaceShooter
{
    public class PlayerStatistics
    {
        public int numKills;
        public int score;
        public int time;
        public int extraBonus;

        public void Reset()
        {
            numKills = 0;
            score = 0;
            time = 0;
            extraBonus = 0;
        }
    }
}