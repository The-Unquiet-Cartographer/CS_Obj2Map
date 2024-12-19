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
           return f/f;
        }
        public static float Round (float f, float rounding) {
            return MathF.Round(f/rounding) * rounding;
        }
    }

    public static class CollectionExtensions {
        public static string Stringify<T> (this List<T> list, string divider = ", ") {
            if (list.Count == 0) return "";
            string s = list[0]!.ToString()!;
            if (list.Count > 1) for (int i = 1; i < list.Count; i++) {
                s += divider+list[i];
            }
            return s;
        }
    }
}