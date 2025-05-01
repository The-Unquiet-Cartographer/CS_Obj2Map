public class Model {

    private string _name;
    private Vector3[] _vertices;
    private Vector2[] _textureVertices;
    private Face[] _faces;
    //private static ProgressReporter PROGRESS_REPORTER = new ProgressReporter();


//
//  CONSTRUCTORS
//
// Empty model
    public Model () {
        this._name = "";
        this._vertices = new Vector3[0];
        this._textureVertices = new Vector2[0];
        this._faces = new Face[0];
    }
// Basic
    public Model (Vector3[] vertices_, Vector2[] textureVertices_, Face[] faces_) {
        this._name = "";
        this._vertices = vertices_;
        this._textureVertices = textureVertices_;
        this._faces = faces_;
    }
    public Model (string name, Vector3[] vertices_, Vector2[] textureVertices_, Face[] faces_) {
        this._name = name;
        this._vertices = vertices_;
        this._textureVertices = textureVertices_;
        this._faces = faces_;
    }
// Using FileData struct
    public Model (in InputManager.FileData fileData, in MaterialLibrary materialLibrary, string[]? excludeTextures = null) {
        List<Vector3> vertices_ = new List<Vector3>();
        List<Vector2> textureVertices_ = new List<Vector2>();
        List<Face> faces_ = new List<Face>();
        Switch_FileType(fileData, materialLibrary, ref vertices_, ref textureVertices_, ref faces_, excludeTextures);
        this._name = fileData.fileName;
        this._vertices = vertices_.ToArray();
        this._textureVertices = textureVertices_.ToArray();
        this._faces = faces_.ToArray();
    }


//
//  HELPER FUNCTIONS
//
    private static void Switch_FileType (in InputManager.FileData fileData, in MaterialLibrary materialLibrary, ref List<Vector3> vertices_, ref List<Vector2>textureVertices_, ref List<Face> faces_, string[]? excludeTextures = null) {
        Switch_FileType(fileData.fileContents.Split('\n'), fileData.fileType, materialLibrary, ref vertices_, ref textureVertices_, ref faces_, excludeTextures);
    }
    private static void Switch_FileType (in string[] splitContents, InputManager.FileType fileType, in MaterialLibrary materialLibrary, ref List<Vector3> vertices_, ref List<Vector2>textureVertices_, ref List<Face> faces_, string[]? excludeTextures = null) {
        //if (!PROGRESS_REPORTER.processingMultiple) PROGRESS_REPORTER.Reset();
        //PROGRESS_REPORTER.AddStage(splitContents.Length);

        switch (fileType) {
            case InputManager.FileType.obj:
                Decompile_OBJ(splitContents, materialLibrary, ref vertices_, ref textureVertices_, ref faces_, excludeTextures);
                break;
            default:
                return;
        }
    }


//
//  PROPERTIES
//
    public string name {get{return this._name;} set{this._name = value;}}
    public Vector3[] vertices {get{return this._vertices;}}
    public Vector2[] textureVertices {get{return this._textureVertices;}}
    public Face[] faces {get{return this._faces;}}
    public int vertexCount {get{return this._vertices.Length;}}
    public int textureVertexCount {get{return this._textureVertices.Length;}}
    public int faceCount {get{return this._faces.Length;}}
    public bool isEmpty{get{
        if (vertexCount == 0 && textureVertexCount == 0 && faceCount == 0) return true;
        return false;
    }}



//////////////////////////////////////////////////
//
//  DECOMPILE INPUT FORMAT
//
//////////////////////////////////////////////////
    private static void Decompile_OBJ (in string[] _splitContents, in MaterialLibrary materialLibrary, ref List<Vector3> _vertices_, ref List<Vector2> _textureVertices_, ref List<Face> _faces_, string[]? excludeTextures = null) {
        string texture = "";
        bool skipMaterial = false;
        for (int i = 0; i < _splitContents.Length; i++) {
            if (_splitContents[i].StartsWith("v ")) {
                string[] line = _splitContents[i].Split(' ');
                _vertices_.Add( new Vector3( float.Parse(line[1]), float.Parse(line[2]), float.Parse(line[3]) ) );
            }
            else if (_splitContents[i].StartsWith("vt ")) {
                string[] line = _splitContents[i].Split(' ');
                _textureVertices_.Add( new Vector2( float.Parse(line[1]), float.Parse(line[2]) ) );
            }
            else if (_splitContents[i].StartsWith("f ")) {
                if (!skipMaterial) {
                    string[] line = _splitContents[i].Split(' ');
                    Face.IPair[] faceVertices = new Face.IPair[line.Length-1];
                    for (int j = 1; j < line.Length; j++) {
                        string[] vertexComponents = line[j].Split('/');
                        int vIndex = int.Parse(vertexComponents[0]);
                        int uvIndex = -1;
                        if (vertexComponents.Length > 1 && vertexComponents[1].Length > 0) {
                            uvIndex = int.Parse(vertexComponents[1]);
                            uvIndex -=1;
                        }
                        vIndex -=1;
                            // ^^^ -1 because .OBJ uses a 1-based index, but c# uses a 0-based index.
                        faceVertices[j-1] = new Face.IPair(
                            vIndex,
                            uvIndex
                        );
                    }
                    _faces_.Add(new Face(texture, faceVertices));
                }
            }
            else if (_splitContents[i].StartsWith("usemtl ")) {
                string useMaterial = _splitContents[i].Substring(_splitContents[i].IndexOf(' ')+1);
                Material? material = materialLibrary.FindMaterial(useMaterial);
                texture = (material == null ? useMaterial : material!.Value.texture);
                if (excludeTextures != null
                && (
                    (useMaterial != "" && excludeTextures.Contains(useMaterial))      //<== Empty material/texture entries are something to look for; I imagine it could lead to some hair-pulling if the program outputs an empty file because of this.
                    || (texture != "" && excludeTextures.Contains(texture))
                )) {
                    skipMaterial = true;
                }
                else {
                    skipMaterial = false;
                }
            }
            //PROGRESS_REPORTER.IncrementProgress();
            //PROGRESS_REPORTER.WriteProgressPerStep();
        }
    }



//////////////////////////////////////////////////
//
//  CONSTRUCT A MODEL FROM MULTIPLE SOURCES
//
//////////////////////////////////////////////////
    public static Model Amalgamate (in InputManager.FileData[] fileData, in MaterialLibrary materialLibrary, string[]? excludeTextures = null) {
        //PROGRESS_REPORTER.Reset();
        //PROGRESS_REPORTER.processingMultiple = true;
        Model a = new Model();
        foreach (InputManager.FileData fD in fileData) {
            Model b = new Model(fD, materialLibrary, excludeTextures);
            a = Combine(a,b);
        }
        //PROGRESS_REPORTER.processingMultiple = false;
        return a;
    }
    public static Model Combine (Model a, Model b) {
        List<Face> faces_ = a._faces.ToList();
        List<Vector3> vertices_ = a._vertices.ToList();
        List<Vector2> textureVertices_ = a._textureVertices.ToList();
    //ITERATE THROUGH B FACES
    //Discard faces that are exact duplicates of those in A.
        for (int i = 0; i < b._faces.Length; i++) {
            Vector3[] _faceVertices = b.GetFaceVertices(i);
            if (a.ContainsFace(_faceVertices)) continue;
            Vector2?[] _faceTextureVertices = b.GetFaceTextureVertices(i);
            Face.IPair[] faceVertices_ = new Face.IPair[b._faces[i].vertexCount];
        //CREATE NEW FACEDATA
        //* Might as well ADD VERTICES and ADD TEXTURE VERTICES while we're at it.
        //* Skip adding texture vertices that are null.
        //* If we use vertices_.Count when adding the vertex indices to faceVertices_, THEN add the vertex to vertices_, the index should be correct.
            for (int j = 0; j < faceVertices_.Length; j++) {
                if (_faceTextureVertices[j] == null) {
                    faceVertices_[j] = new Face.IPair(
                        vertices_.Count,
                        -1
                    );
                    vertices_.Add(_faceVertices[j]);
                }
                else {
                    faceVertices_[j] = new Face.IPair(
                        vertices_.Count,
                        textureVertices_.Count
                    );
                    vertices_.Add(_faceVertices[j]);
                    textureVertices_.Add(_faceTextureVertices[j]!.Value);
                }
            }
        //ADD FACE
            faces_.Add(new Face(b._faces[i].texture, faceVertices_));
        }
    //CREATE NEW MODEL
        return new Model("CombinedModel", vertices_.ToArray(), textureVertices_.ToArray(), faces_.ToArray());
    }
    private bool ContainsFace (Vector3[] faceVertices) {
        for (int i = 0; i < this.faceCount; i++) {
            if (this._faces[i].vertexCount != faceVertices.Length) return false;
            bool Match () {
                for (int j = 0; j < this._faces[i].vertexCount; j++) {
                    if (!this._vertices[ this._faces[i].indexPairs[j].vIndex ].Equals(faceVertices[j])) return false;
                }
                return true;
            }
            if (Match()) return true;
        }
        return false;
    }



//////////////////////////////////////////////////
//
//  MODIFY SELF
//
//////////////////////////////////////////////////
    public void Scale (float scaleFactor) {
        for (int i = 0; i < _vertices.Length; i++) {
            _vertices[i] = _vertices[i].Scale1(scaleFactor);
        }
    }
    public void RoundVertices (float rounding) {
        for (int i = 0; i < _vertices.Length; i++) {
            _vertices[i] = _vertices[i].Round(rounding);
        }
    }
    public void ReverseVertexOrder () {
        for (int i = 0; i < _faces.Length; i++) {
            _faces[i].ReverseVertexOrder();
        }
    }
    public void SubdivideFaces () {
        List<Face> faces_ = new List<Face>();
        for (int i = 0; i < _faces.Length; i++) {
            for (int j = 2; j < _faces[i].vertexCount; j++) {
                faces_.Add(new Face(_faces[i].texture, _faces[i].indexPairs[0], _faces[i].indexPairs[j-1], _faces[i].indexPairs[j]));
            }
        }
        this._faces = faces_.ToArray();
    }
    public void SwapYZCoordinates () {
        for (int i = 0; i < _vertices.Length; i++) {
            _vertices[i] = new Vector3(_vertices[i].x, -_vertices[i].z, _vertices[i].y);
        }
    }
    public void TrimUnused () {
        List<Vector3> vertices_ = this._vertices.ToList();
        List<Vector2> textureVertices_ = this._textureVertices.ToList();
    //Trim vertices
        for (int i = vertices_.Count-1; i > -1; i--) {
            bool vIndexFound = false;
            foreach (Face f in this._faces) if (f.ContainsVIndex(i)) {
                vIndexFound = true;
                break;
            }
            if (!vIndexFound) {
                vertices_.RemoveAt(i);
            //Adjust vIndices in faces
                foreach (Face f in this._faces) {

                //Is this modifying the faces like it's supposed to?
                    for (int k = 0; k < f.vertexCount; k++) if (f.indexPairs[k].vIndex > i) f.indexPairs[k].vIndex = f.indexPairs[k].vIndex-1;

                }
            }
        }
    //Trim texture vertices
        for (int i = textureVertices_.Count-1; i > -1; i--) {
            bool uvIndexFound = false;
            foreach (Face f in this._faces) if (f.ContainsUVIndex(i)) {
                uvIndexFound = true;
                break;
            }
            if (!uvIndexFound) {
                textureVertices_.RemoveAt(i);
            //Adjust uvIndices in faces
                foreach (Face f in this._faces) {
                    for (int k = 0; k < f.vertexCount; k++) if (f.indexPairs[k].uvIndex > i) f.indexPairs[k].uvIndex = f.indexPairs[k].uvIndex-1;
                }
            }
        }
        this._vertices = vertices_.ToArray();
        this._textureVertices = textureVertices_.ToArray();
    //end function
    }



//////////////////////////////////////////////////
//
//  UTILITIES
//
//////////////////////////////////////////////////
    public Vector3[] GetFaceVertices (int faceIndex) {
        Face _face = _faces[faceIndex];
        Vector3[] faceVertices = new Vector3[_face.vertexCount];
        for (int i = 0; i < _face.vertexCount; i++) {
            faceVertices[i] = this._vertices[_face.indexPairs[i].vIndex];
        }
        return faceVertices;
    }
    public Vector2?[] GetFaceTextureVertices (int faceIndex) {
        Face _face = _faces[faceIndex];
        List<Vector2?> faceTextureVertices = new List<Vector2?>(_face.vertexCount);
        for (int i = 0; i < _face.vertexCount; i++) {
            if (_face.indexPairs[i].uvIndex < 0) faceTextureVertices.Add(null);
            else faceTextureVertices.Add(this._textureVertices[(int)_face.indexPairs[i].uvIntIndex]);
        }
        return faceTextureVertices.ToArray();
    }
    public Vector3 GetFaceNormal (int faceIndex) {
        Face _face = _faces[faceIndex];
        Vector3 normal = Vector3.Cross_LeftHanded(
            this._vertices[_face.indexPairs[0].vIndex].DirectionTo(this._vertices[_face.indexPairs[1].vIndex]),
            this._vertices[_face.indexPairs[1].vIndex].DirectionTo(this._vertices[_face.indexPairs[2].vIndex])
        ).normalized;
        if (float.IsNaN(normal.x)) normal.x = 0;
        if (float.IsNaN(normal.y)) normal.y = 0;
        if (float.IsNaN(normal.z)) normal.z = 0;
        return normal;
    }

}