public class Model {
    private static bool _PROCESSING_MULTIPLE = false;
    private static ProgressReporter _PROGRESS_REPORTER = new ProgressReporter();
    private static void RESET_PROGRESS_REPORTER () {_PROGRESS_REPORTER.Reset();}
    //The Amalgamate() function will set _PROCESSING_MULTIPLE to true. The constructor or one of its helper functions will check this bool. If FALSE, it will call RESET_PROGRESS_REPORTER().
    //This circumvents the need to call RESET_PROGRESS_REPORTER() from outside the class. 



    private string _name;
    private Vector3[] _vertices;
    private Vector2[] _textureVertices;
    private Face[] _faces;

    public enum InputFormat {
        obj, //map
    }


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
// Using Model.FileData struct
    public struct FileData {
        public string fileName, fileType, fileContents;
        public FileData (string fileName, string fileType, string fileContents) {
            this.fileName = fileName;
            this.fileType = fileType;
            this.fileContents = fileContents;
        }
    }
    public Model (FileData fileData, string[]? excludeTextures = null) {
        List<Vector3> vertices_ = new List<Vector3>();
        List<Vector2> textureVertices_ = new List<Vector2>();
        List<Face> faces_ = new List<Face>();
        InputFormat? inputFormat;
        if (InputFormatIsValid(fileData.fileType, out inputFormat)) {
            string[] splitContents = fileData.fileContents.Split('\n');
            Switch_FileType(inputFormat, splitContents, ref vertices_, ref textureVertices_, ref faces_, excludeTextures);
        }
        this._name = fileData.fileName;
        this._vertices = vertices_.ToArray();
        this._textureVertices = textureVertices_.ToArray();
        this._faces = faces_.ToArray();
    }
// Using a direct path to the source file
    public Model (string filePath, string[]? excludeTextures = null) {
        string fileName = "";
        string fileType = "";
        string fileContents = FileReader.File_ReadContents(filePath, out fileName, out fileType);
        List<Vector3> vertices_ = new List<Vector3>();
        List<Vector2> textureVertices_ = new List<Vector2>();
        List<Face> faces_ = new List<Face>();
        InputFormat? inputFormat;
        if (InputFormatIsValid(fileType, out inputFormat)) {
            string[] splitContents = fileContents.Split('\n');
            Switch_FileType(inputFormat, splitContents, ref vertices_, ref textureVertices_, ref faces_, excludeTextures);
        }
        this._name = fileName;
        this._vertices = vertices_.ToArray();
        this._textureVertices = textureVertices_.ToArray();
        this._faces = faces_.ToArray();
    }


//
//  HELPER FUNCTIONS
//
    public static bool InputFormatIsValid (string fileExtension) {
        if (fileExtension[0] == '.') fileExtension = fileExtension.Remove(0,1);
        InputFormat _out;
        return Enum.TryParse<InputFormat>(fileExtension, out _out);
    }
    public static bool InputFormatIsValid (string fileExtension, out InputFormat? inputFormat) {
        if (fileExtension[0] == '.') fileExtension = fileExtension.Remove(0,1);
        InputFormat _out;
        bool b = Enum.TryParse<InputFormat>(fileExtension, out _out);
        if (!b) inputFormat = null;
        else inputFormat = _out;
        return b;
    }
    private static void Switch_FileType (InputFormat? inputFormat, string[] splitFileContents, ref List<Vector3> vertices_, ref List<Vector2>textureVertices_, ref List<Face> faces_, string[]? excludeTextures = null) {
        if (!_PROCESSING_MULTIPLE) RESET_PROGRESS_REPORTER();
        _PROGRESS_REPORTER.AddStage(splitFileContents.Length);
        switch (inputFormat) {
            case InputFormat.obj:
                Decompile_OBJ(splitFileContents, ref vertices_, ref textureVertices_, ref faces_, excludeTextures);
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
    private static void Decompile_OBJ (string[] _splitContents, ref List<Vector3> _vertices_, ref List<Vector2> _textureVertices_, ref List<Face> _faces_, string[]? excludeTextures = null) {
        string currentMaterial = "";
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
                    Face.Vertex[] faceVertices = new Face.Vertex[line.Length-1];
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
                        faceVertices[j-1] = new Face.Vertex(
                            vIndex,
                            uvIndex
                        );
                    }
                    _faces_.Add(new Face(currentMaterial, faceVertices));
                }
            }
            else if (_splitContents[i].StartsWith("usemtl ")) {
                currentMaterial = _splitContents[i].Substring(_splitContents[i].IndexOf(' ')+1);
                if (excludeTextures != null && excludeTextures.Contains(currentMaterial)) {
                    skipMaterial = true;
                }
                else {
                    skipMaterial = false;
                }
            }
            _PROGRESS_REPORTER.IncrementProgress();
            _PROGRESS_REPORTER.WriteProgressPerStep();
        }
    }



//////////////////////////////////////////////////
//
//  CONSTRUCT A MODEL FROM MULTIPLE SOURCES
//
//////////////////////////////////////////////////
    public static Model Amalgamate (FileData[] fileData, string[]? excludeTextures = null) {
        RESET_PROGRESS_REPORTER();
        _PROCESSING_MULTIPLE = true;
        Model a = new Model();
        foreach (FileData fD in fileData) {
            Model b = new Model(fD, excludeTextures);
            a = Combine(a,b);
        }
        _PROCESSING_MULTIPLE = false;
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
            Face.Vertex[] faceVertices_ = new Face.Vertex[b._faces[i].vertexCount];
        //CREATE NEW FACEDATA
        //* Might as well ADD VERTICES and ADD TEXTURE VERTICES while we're at it.
        //* Skip adding texture vertices that are null.
        //* If we use vertices_.Count when adding the vertex indices to faceVertices_, THEN add the vertex to vertices_, the index should be correct.
            for (int j = 0; j < faceVertices_.Length; j++) {
                if (_faceTextureVertices[j] == null) {
                    faceVertices_[j] = new Face.Vertex(
                        vertices_.Count,
                        -1
                    );
                    vertices_.Add(_faceVertices[j]);
                }
                else {
                    faceVertices_[j] = new Face.Vertex(
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
                    if (!this._vertices[ this._faces[i].vertices[j].vIndex ].Equals(faceVertices[j])) return false;
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
                faces_.Add(new Face(_faces[i].texture, _faces[i].vertices[0], _faces[i].vertices[j-1], _faces[i].vertices[j]));
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
                    for (int k = 0; k < f.vertexCount; k++) if (f.vertices[k].vIndex > i) f.vertices[k].vIndex = f.vertices[k].vIndex-1;
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
                    for (int k = 0; k < f.vertexCount; k++) if (f.vertices[k].uvIndex > i) f.vertices[k].uvIndex = f.vertices[k].uvIndex-1;
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
            faceVertices[i] = this._vertices[_face.vertices[i].vIndex];
        }
        return faceVertices;
    }
    public Vector2?[] GetFaceTextureVertices (int faceIndex) {
        Face _face = _faces[faceIndex];
        List<Vector2?> faceTextureVertices = new List<Vector2?>(_face.vertexCount);
        for (int i = 0; i < _face.vertexCount; i++) {
            if (_face.vertices[i].uvIndex == null) faceTextureVertices.Add(null);
            else faceTextureVertices.Add(this._textureVertices[(int)_face.vertices[i].uvIntIndex]);
        }
        return faceTextureVertices.ToArray();
    }

}