using System;
using System.Collections;

public class Face {

    public struct Vertex {
        private int _vIndex;
        private int? _uvIndex; 
        public Vertex (int vIndex, int? uvIndex = null) {
            this._vIndex = vIndex;
            this._uvIndex = uvIndex < 0 ? null : uvIndex;
        }
        public override string ToString() {
            return vIndex+"/"+uvIndex;
        }
        public int vIndex {get{return _vIndex;} set{vIndex = value;}}
        public int? uvIndex {get{return _uvIndex;} set {_uvIndex = value;}}
        public int uvIntIndex {get{return _uvIndex == null ? -1 : (int)_uvIndex;} set {_uvIndex = value;}}
    }


	private Vertex[] _vertices;
    private string _texture;


	public Face (params Face.Vertex[] vertices) {
		this._vertices = new Vertex[vertices.Length];
		System.Array.Copy(vertices, _vertices, vertices.Length);
        this._texture = "";
	}
    public Face (string texture, params Face.Vertex[] vertices) {
		this._vertices = new Vertex[vertices.Length];
		System.Array.Copy(vertices, _vertices, vertices.Length);
        this._texture = texture.Trim();
	}


    public Vertex[] vertices {get{return _vertices;}}
    public string texture {get{return _texture;}}
    public int vertexCount {get{return _vertices.Length;}}


    public void ReverseVertexOrder () {
        Face.Vertex[] vertices_ = new Face.Vertex[vertexCount];
        for (int i = 0; i < vertexCount; i++) {
            vertices_[i] = _vertices[vertexCount - 1 - i];
        }
        this._vertices = vertices_;
    }

    public override string ToString() {
        string s = _texture;
        for (int i = 0; i < this.vertexCount; i++) {
            s += " "+_vertices[i].ToString();
        }
        return s;
    }

    public bool ContainsVIndex(int vIndex) {
        foreach (Vertex v in _vertices) if (v.vIndex == vIndex) return true;
        return false;
    }
    public bool ContainsUVIndex(int uvIndex) {
        foreach (Vertex v in _vertices) if (v.uvIndex == uvIndex) return true;
        return false;
    }

}