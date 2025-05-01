public static class TestModels {

    private static Vector3[] vertices_axisAligned = new Vector3[] {
    //Front face
        new Vector3(-64, -64, -64), new Vector3(-64, -64, 64), new Vector3(64, -64, 64), new Vector3(64, -64, -64),
    //Left face
        new Vector3(-64, 64, -64), new Vector3(-64, 64, 64), new Vector3(-64, -64, 64), new Vector3(-64, -64, -64),
    //Rear face
        new Vector3(64, 64, -64), new Vector3(64, 64, 64), new Vector3(-64, 64, 64), new Vector3(-64, 64, -64),
    //Right face
        new Vector3(64, -64, -64), new Vector3(64, -64, 64), new Vector3(64, 64, 64), new Vector3(64, 64, -64),
    //Top face
        new Vector3(-64, -64, 64), new Vector3(-64, 64, 64), new Vector3(64, 64, 64), new Vector3(64, -64, 64),
    //Bottom face
        new Vector3(-64, 64, -64), new Vector3(-64, -64, -64), new Vector3(64, -64, -64), new Vector3(64, 64, -64)
    };
    private static Vector3[] vertices_tiltXPlus = new Vector3[] {
    //Front face
        new Vector3(-64, -128, -64), new Vector3(-64, -64, 128), new Vector3(64, -64, 128), new Vector3(64, -128, -64),
    //Left face
        new Vector3(-64, 64, -128), new Vector3(-64, 128, 64), new Vector3(-64, -64, 128), new Vector3(-64, -128, -64),
    //Rear face
        new Vector3(64, 64, -128), new Vector3(64, 128, 64), new Vector3(-64, 128, 64), new Vector3(-64, 64, -128),
    //Right face
        new Vector3(64, -128, -64), new Vector3(64, -64, 128), new Vector3(64, 128, 64), new Vector3(64, 64, -128),
    //Top face
        new Vector3(-64, -64, 128), new Vector3(-64, 128, 64), new Vector3(64, 128, 64), new Vector3(64, -64, 128),
    //Bottom face
        new Vector3(-64, 64, -128), new Vector3(-64, -128, -64), new Vector3(64, -128, -64), new Vector3(64, 64, -128)
    };
    private static Vector3[] vertices_tiltXMinus = new Vector3[] {
    //Front face
        new Vector3(-64, -64, -128), new Vector3(-64, -128, 64), new Vector3(64, -128, 64), new Vector3(64, -64, -128),
    //Left face
        new Vector3(-64, 128, -64), new Vector3(-64, 64, 128), new Vector3(-64, -128, 64), new Vector3(-64, -64, -128),
    //Rear face
        new Vector3(64, 128, -64), new Vector3(64, 64, 128), new Vector3(-64, 64, 128), new Vector3(-64, 128, -64),
    //Right face
        new Vector3(64, -64, -128), new Vector3(64, -128, 64), new Vector3(64, 64, 128), new Vector3(64, 128, -64),
    //Top face
        new Vector3(-64, -128, 64), new Vector3(-64, 64, 128), new Vector3(64, 64, 128), new Vector3(64, -128, 64),
    //Bottom face
        new Vector3(-64, 128, -64), new Vector3(-64, -64, -128), new Vector3(64, -64, -128), new Vector3(64, 128, -64)
    };
    private static Vector3[] vertices_tiltYPlus = new Vector3[] {
    //Front face
        new Vector3(-128, -64, -64), new Vector3(-64, -64, 128), new Vector3(128, -64, 64), new Vector3(64, -64, -128),
    //Left face
        new Vector3(-128, 64, -64), new Vector3(-64, 64, 128), new Vector3(-64, -64, 128), new Vector3(-128, -64, -64),
    //Rear face
        new Vector3(64, 64, -128), new Vector3(128, 64, 64), new Vector3(-64, 64, 128), new Vector3(-128, 64, -64),
    //Right face
        new Vector3(64, -64, -128), new Vector3(128, -64, 64), new Vector3(128, 64, 64), new Vector3(64, 64, -128),
    //Top face
        new Vector3(-64, -64, 128), new Vector3(-64, 64, 128), new Vector3(128, 64, 64), new Vector3(128, -64, 64),
    //Bottom face
        new Vector3(-128, 64, -64), new Vector3(-128, -64, -64), new Vector3(64, -64, -128), new Vector3(64, 64, -128)
    };
    private static Vector3[] vertices_tiltYMinus = new Vector3[] {
    //Front face
        new Vector3(-64, -64, -128), new Vector3(-128, -64, 64), new Vector3(64, -64, 128), new Vector3(128, -64, -64),
    //Left face
        new Vector3(-64, 64, -128), new Vector3(-128, 64, 64), new Vector3(-128, -64, 64), new Vector3(-64, -64, -128),
    //Rear face
        new Vector3(128, 64, -64), new Vector3(64, 64, 128), new Vector3(-128, 64, 64), new Vector3(-64, 64, -128),
    //Right face
        new Vector3(128, -64, -64), new Vector3(64, -64, 128), new Vector3(64, 64, 128), new Vector3(128, 64, -64),
    //Top face
        new Vector3(-128, -64, 64), new Vector3(-128, 64, 64), new Vector3(64, 64, 128), new Vector3(64, -64, 128),
    //Bottom face
        new Vector3(-64, 64, -128), new Vector3(-64, -64, -128), new Vector3(128, -64, -64), new Vector3(128, 64, -64)
    };
    private static Vector3[] vertices_tiltZPlus = new Vector3[] {
    //Front face
        new Vector3(-128, -64, -64), new Vector3(-128, -64, 64), new Vector3(64, -128, 64), new Vector3(64, -128, -64),
    //Left face
        new Vector3(-64, 128, -64), new Vector3(-64, 128, 64), new Vector3(-128, -64, 64), new Vector3(-128, -64, -64),
    //Rear face
        new Vector3(128, 64, -64), new Vector3(128, 64, 64), new Vector3(-64, 128, 64), new Vector3(-64, 128, -64),
    //Right face
        new Vector3(64, -128, -64), new Vector3(64, -128, 64), new Vector3(128, 64, 64), new Vector3(128, 64, -64),
    //Top face
        new Vector3(-128, -64, 64), new Vector3(-64, 128, 64), new Vector3(128, 64, 64), new Vector3(64, -128, 64),
    //Bottom face
        new Vector3(-64, 128, -64), new Vector3(-128, -64, -64), new Vector3(64, -128, -64), new Vector3(128, 64, -64)
    };
    private static Vector3[] vertices_tiltZMinus = new Vector3[] {
    //Front face
        new Vector3(-64, -128, -64), new Vector3(-64, -128, 64), new Vector3(128, -64, 64), new Vector3(128, -64, -64),
    //Left face
        new Vector3(-128, 64, -64), new Vector3(-128, 64, 64), new Vector3(-64, -128, 64), new Vector3(-64, -128, -64),
    //Rear face
        new Vector3(64, 128, -64), new Vector3(64, 128, 64), new Vector3(-128, 64, 64), new Vector3(-128, 64, -64),
    //Right face
        new Vector3(128, -64, -64), new Vector3(128, -64, 64), new Vector3(64, 128, 64), new Vector3(64, 128, -64),
    //Top face
        new Vector3(-64, -128, 64), new Vector3(-128, 64, 64), new Vector3(64, 128, 64), new Vector3(128, -64, 64),
    //Bottom face
        new Vector3(-128, 64, -64), new Vector3(-64, -128, -64), new Vector3(128, -64, -64), new Vector3(64, 128, -64)
    };

    private static Vector2[] textureVertices_faceAligned = new Vector2[] {
    //Front face
        new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
    //Left face
        new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
    //Rear face
        new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
    //Right face
        new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
    //Top face
        new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0),
    //Bottom face
        new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)            
    };
    private static Vector2[] textureVertices_faceAligned_rotation180 = new Vector2[] {
    //Front face
        new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0), new Vector2(0, 1),
    //Left face
        new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0), new Vector2(0, 1),
    //Rear face
        new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0), new Vector2(0, 1),
    //Right face
        new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0), new Vector2(0, 1),
    //Top face
        new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0), new Vector2(0, 1),
    //Bottom face
        new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0), new Vector2(0, 1),
    };
    private static Vector2[] textureVertices_faceAligned_textureInverted = new Vector2[] {
    //Front face
        new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1),
    //Left face
        new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1),
    //Rear face
        new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1),
    //Right face
        new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1),
    //Top face
        new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1),
    //Bottom face
        new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1)            
    };
    private static Vector2[] textureVertices_rotationPlus45 = new Vector2[] {
    //Front face
        new Vector2(0, 0), new Vector2(-1, 1), new Vector2(0, 2), new Vector2(1, 1),
    //Left face
        new Vector2(0, 0), new Vector2(-1, 1), new Vector2(0, 2), new Vector2(1, 1),
    //Rear face
        new Vector2(0, 0), new Vector2(-1, 1), new Vector2(0, 2), new Vector2(1, 1),
    //Right face
        new Vector2(0, 0), new Vector2(-1, 1), new Vector2(0, 2), new Vector2(1, 1),
    //Top face
        new Vector2(0, 0), new Vector2(-1, 1), new Vector2(0, 2), new Vector2(1, 1),
    //Bottom face
        new Vector2(0, 0), new Vector2(-1, 1), new Vector2(0, 2), new Vector2(1, 1)            
    };

    private static Face[] faces = new Face[] {
    //Front face
        new Face(new Face.IPair(0,0), new Face.IPair(1,1), new Face.IPair(2,2), new Face.IPair(3,3)),
    //Left face
        new Face(new Face.IPair(4,4), new Face.IPair(5,5), new Face.IPair(6,6), new Face.IPair(7,7)),
    //Rear face
        new Face(new Face.IPair(8,8), new Face.IPair(9,9), new Face.IPair(10,10), new Face.IPair(11,11)),
    //Right face
        new Face(new Face.IPair(12,12), new Face.IPair(13,13), new Face.IPair(14,14), new Face.IPair(15,15)),
    //Top face
        new Face(new Face.IPair(16,16), new Face.IPair(17,17), new Face.IPair(18,18), new Face.IPair(19,19)),
    //Bottom face
        new Face(new Face.IPair(20,20), new Face.IPair(21,21), new Face.IPair(22,22), new Face.IPair(23,23))
    };



//All vertices are recorded in clockwise direction per-face, starting from the lower-left relative to the face's facing
    public static Model Cube_AxisAligned = new Model(
        "Test model",
        vertices_axisAligned,
        textureVertices_faceAligned,
        faces
    );
    public static Model Cube_AxisAligned_TextureTilt = new Model(
        "Test model",
        vertices_axisAligned,
        textureVertices_rotationPlus45,
        faces
    );


//Tilted around the X axis
    public static Model Cube_TiltXPlus = new Model(
        "Test model",
        vertices_tiltXPlus,
        textureVertices_faceAligned,
        faces
    );
    public static Model Cube_TiltXMinus = new Model(
        "Test model",
        vertices_tiltXMinus,
        textureVertices_faceAligned,
        faces
    );
    public static Model Cube_TiltXPlus_TextureRot180 = new Model(
        "Test model",
        vertices_tiltXPlus,
        textureVertices_faceAligned_rotation180,
        faces
    );
    public static Model Cube_TiltXMinus_TextureRot180 = new Model(
        "Test model",
        vertices_tiltXMinus,
        textureVertices_faceAligned_rotation180,
        faces
    );


//Tilted around the Y axis
    public static Model Cube_TiltYPlus = new Model(
        "Test model",
        vertices_tiltYPlus,
        textureVertices_faceAligned,
        faces
    );
    public static Model Cube_TiltYMinus = new Model(
        "Test model",
        vertices_tiltYMinus,
        textureVertices_faceAligned,
        faces
    );
    public static Model Cube_TiltYPlus_TextureRot180 = new Model(
        "Test model",
        vertices_tiltYPlus,
        textureVertices_faceAligned_rotation180,
        faces
    );
    public static Model Cube_TiltYMinus_TextureRot180 = new Model(
        "Test model",
        vertices_tiltYMinus,
        textureVertices_faceAligned_rotation180,
        faces
    );


//Tilted around the Z axis
    public static Model Cube_TiltZPlus = new Model(
        "Test model",
        vertices_tiltZPlus,
        textureVertices_faceAligned,
        faces
    );
    public static Model Cube_TiltZMinus = new Model(
        "Test model",
        vertices_tiltZMinus,
        textureVertices_faceAligned,
        faces
    );
    public static Model Cube_TiltZPlus_TextureRot180 = new Model(
        "Test model",
        vertices_tiltZPlus,
        textureVertices_faceAligned_rotation180,
        faces
    );
    public static Model Cube_TiltZMinus_TextureRot180 = new Model(
        "Test model",
        vertices_tiltZMinus,
        textureVertices_faceAligned_rotation180,
        faces
    );



    public static Model CubeyMcCubeFace = new Model(
        "Test model",
        vertices_tiltZMinus,
        textureVertices_rotationPlus45,
        faces
    );
}