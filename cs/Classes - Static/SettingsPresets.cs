public static class SettingsPresets {
    public readonly static GameSettings theWorldIsNotEnough = new GameSettings {
        scaleFactor = 544f,
        reverseVertexOrder = false,
        invertNormals = true,
        subdivideFaces = false,
        swapYZCoordinates = true,
    };
    public readonly static GameSettings goldenEye007 = new GameSettings {
        scaleFactor = 1f,
        reverseVertexOrder = false,
        swapYZCoordinates = false
    };
    public readonly static GameSettings marathon = new GameSettings {
        scaleFactor = 4f,
        reverseVertexOrder = true,
        subdivideFaces = true,
        swapYZCoordinates = true,
        marathonCeilingFix = true
    };
    public readonly static GameSettings spyro = new GameSettings {
        scaleFactor = 1f,
        reverseVertexOrder = false,
        swapYZCoordinates = false
    };
    public readonly static GameSettings starWarsJediKnight = new GameSettings {
        scaleFactor = 320f,
        reverseVertexOrder = true,
        subdivideFaces = true,
        swapYZCoordinates = true
    };
}