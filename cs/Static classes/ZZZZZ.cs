static class ZZZZZ {

//GET THE MOST HORIZONTAL / MOST VERTICAL OUT OF A SELECTION OF VECTORS
    const float piDiv2 = MathF.PI/2;
    private static float Orientation (Vector2 _line) {
        float line_atan2 = MathF.Atan2(_line.y, _line.x);
    //If result is close to piDiv2, line is horizontal.
    //If result is close to zero, line is vertical.
        return MathF.Abs(MathF.Abs(line_atan2)-piDiv2);
    }
    public static int MostHorizontal(params Vector2[] candidates) {
        float value_ = Orientation(candidates[0]);
        int index = 0;
        for (int i = 1; i < candidates.Length; i++) {
            float orientation = Orientation(candidates[i]);
            if (orientation > value_) {
                value_ = orientation;
                index = i;
            }
        }
        return index;
    }
    public static int MostVertical(params Vector2[] candidates) {
        float value_ = Orientation(candidates[0]);
        int index = 0;
        for (int i = 1; i < candidates.Length; i++) {
            float orientation = Orientation(candidates[i]);
            if (orientation < value_) {
                value_ = orientation;
                index = i;
            }
        }
        return index;
    }



    public delegate void IterationFunction(int i, int j);
    public static void IteratePairs<T> (T[] array, IterationFunction function) {
        for (int i = 0; i < array.Length-1; i++) {
            int j = i+1;
            function(i, j);
        }
        function(array.Length-1, 0);
    }

}