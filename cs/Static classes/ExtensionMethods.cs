namespace ExtensionMethods {
    public static class FloatExtensions {
        public static float PositiveZero (float f) {
            if (f == -0f) f = 0f;
            return f;
        }
        public static float Squared (this float f) {
            return f * f;
        }
        public static float SquareRoot (float f) {
           return MathF.Sqrt(f);
        }
        public static float Round (this float f, float rounding) {
            return MathF.Round(f/rounding) * rounding;
        }
        public static float RoundF (float f, float rounding) {
            return MathF.Round(f/rounding) * rounding;
        }
        public static float DegreesToRadians (float xDeg) {
            return xDeg / 57.29578f;
        }
        public static float RadiansToDegrees (float xRad) {
            return xRad * 57.29578f;
        }
    }


/*
    public static class DoubleExtensions {
        public static double PositiveZero (double d) {
            if (d == -0) d = 0;
            return d;
        }
        public static double Squared (this double d) {
            return d * d;
        }
        public static double SquareRoot (double d) {
           return Math.Sqrt(d);
        }
        public static double Round (this double d, double rounding) {
            return Math.Round(d/rounding) * rounding;
        }
        public static double RoundD (double d, double rounding) {
            return Math.Round(d/rounding) * rounding;
        }
        public static double DegreesToRadians (double xDeg) {
            return xDeg / 57.29577951308232;
        }
        public static double RadiansToDegrees (double xRad) {
            return xRad * 57.29577951308232;
        }
    }

    public static class DecimalExtensions {
        public static decimal PositiveZero (decimal d) {
            if (d == -0) d = 0;
            return d;
        }
        public static decimal Squared (this decimal d) {
            return d * d;
        }
        public static decimal SquareRoot (decimal d) {
            decimal guess = d / 2;
            decimal previousGuess;
            do {
                previousGuess = guess;
                guess = (previousGuess + d / previousGuess) / 2;
            } while (Math.Abs(previousGuess - guess) > 0.0m);
            return guess;
        }
        public static decimal Round (this decimal d, decimal rounding) {
            return Math.Round(d/rounding) * rounding;
        }
        public static decimal RoundM (decimal d, decimal rounding) {
            return Math.Round(d/rounding) * rounding;
        }
        public static decimal DegreesToRadians (decimal xDeg) {
            return xDeg / 57.295779513082320876798154814105m;
        }
        public static decimal RadiansToDegrees (decimal xRad) {
            return xRad * 57.295779513082320876798154814105m;
        }
    }
*/



    public static class CollectionExtensions {
        public static string Stringify<T> (this List<T> list, string divider = ", ") {
            if (list.Count == 0) return "";
            string s = list[0]!.ToString()!;
            if (list.Count > 1) for (int i = 1; i < list.Count; i++) {
                s += divider+list[i];
            }
            return s;
        }
        public static string Stringify<T> (this T[] arr, string divider = ", ") {
            if (arr.Length == 0) return "";
            string s = arr[0]!.ToString()!;
            if (arr.Length > 1) for (int i = 1; i < arr.Length; i++) {
                s += divider+arr[i];
            }
            return s;
        }
    }

    public static class Indexing {
        public static int Fwd (this int i, int max) {if (i==max-1) return 0; return i+1;}
        public static int Bwd (this int i, int max) {if (i==0) return max-1; return i-1;}
        public static int Fwd (this int i, int increment, int max) {i+=increment; while (i>=max) i-=max; return i;}
        public static int Bwd (this int i, int increment, int max) {i-=increment; while (i<0) i+=max; return i;}
    }
}