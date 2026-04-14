namespace GRisk.Engine
{
    public static class GRTypes
    {
        public enum Player : uint
        {
            NONE = 0,
            PLAYER = 1,
            ENTITY = 666
        }

        public enum Phase : uint
        {
            ITEMS = 0,
            REINFORCE = 1,
            ATTACK = 2,
            LAST = ATTACK,
            END = 4
        }
    }
}