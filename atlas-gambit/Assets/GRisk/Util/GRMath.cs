using System;

namespace GRisk.Util
{
    public class GRMath
    {
        private static readonly Random random = new Random();

        public static uint dice(uint sides = 6, uint amount = 1)
        {
            // catch zero-rolls
            if (sides == 0 || amount == 0)
                return 0;

            uint acc = 0;

            for (uint i = 0; i < amount; i++)
            {
                acc += (uint)random.Next(1, (int)(sides + 1));
            }

            return acc;
        }

        public static float toAdvantage(
            uint diceRoll,
            uint diceSides,
            uint diceAmount,
            float width,
            float bias
        )
        {
            // `width` is how wide of a span around 1 the advantage should stretch
            // e.g. width of 1 would go from x0.5 to x1.5

            // `bias` is an offset from 1 for the advantage span
            // e.g. a width of 1 and a bias of -0.5 would go from x0 to x1
            // e.g. a width of 1 and a bias of 0.2 would go from x0.7 to x1.7

            // catch zero-rolls
            if (diceSides == 0 || diceAmount == 0)
                return 1f;

            // Min and max possible roll
            float min = diceAmount * 1f;
            float max = diceAmount * diceSides;

            // catch "ball dice"
            if (max == min)
                return 1f;

            // Normalize roll to 0–1
            float normalized = (diceRoll - min) / (max - min);

            // Advantage range centered around 1 + bias
            float halfWidth = width / 2f;
            float center = 1f + bias;

            float minAdv = center - halfWidth;
            float maxAdv = center + halfWidth;

            // Interpolate within range
            return minAdv + (normalized * (maxAdv - minAdv));
        }

        public static float advantageDice(uint sides, uint amount, float width, float bias)
        {
            return toAdvantage(dice(sides, amount), sides, amount, width, bias);
        }

        public static uint scaleTo(uint n, float advantage)
        {
            return (uint)MathF.Round(n * advantage);
        }

        public static uint scaleFrom(uint n, float advantage)
        {
            return (uint)MathF.Round(n / advantage);
        }

        public static uint difference(uint a, uint b)
        {
            return a > b ? a - b : b - a;
        }
    }
}
