namespace GRisk.Engine
{
    public static class GRTypes
    {
        public enum Player : uint
        {
            NONE = 0, // Territoires neutres
            PLAYER = 1, // Territoires du joueur
            ENTITY = 666 // Territoires de l'IA
        }

        public enum Phase : uint
        {
            ITEMS = 0, // Obtenir des bonus
            REINFORCE = 1, // Mouvement de troupes
            ATTACK = 2, // Attaque
            LAST = ATTACK,
            END = 4 // Fin
        }
    }
}