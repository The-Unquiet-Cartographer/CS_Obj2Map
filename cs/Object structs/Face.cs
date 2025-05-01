using System;
using System.Collections;

public struct Face {

    public struct IPair {
        private int _vIndex;
        private int? _uvIndex; 
        public IPair (int vIndex, int? uvIndex = null) {
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


	private IPair[] _indexPairs;
    private string _texture;


	public Face (params Face.IPair[] vertices) {
		this._indexPairs = new IPair[vertices.Length];
		System.Array.Copy(vertices, _indexPairs, vertices.Length);
        this._texture = "";
	}
    public Face (string texture, params Face.IPair[] vertices) {
		this._indexPairs = new IPair[vertices.Length];
		System.Array.Copy(vertices, _indexPairs, vertices.Length);
        this._texture = texture.Trim();
	}


    public IPair[] indexPairs {get{return _indexPairs;}}
    public string texture {get{return _texture;}}
    public int vertexCount {get{return _indexPairs.Length;}}

    public void ReverseVertexOrder () {
        Face.IPair[] vertices_ = new Face.IPair[vertexCount];
        for (int i = 0; i < vertexCount; i++) {
            vertices_[i] = _indexPairs[vertexCount - 1 - i];
        }
        this._indexPairs = vertices_;
    }

    public override string ToString() {
        string s = _texture;
        for (int i = 0; i < this.vertexCount; i++) {
            s += " "+_indexPairs[i].ToString();
        }
        return s;
    }

    public bool ContainsVIndex(int vIndex) {
        foreach (IPair i in _indexPairs) if (i.vIndex == vIndex) return true;
        return false;
    }
    public bool ContainsUVIndex(int uvIndex) {
        foreach (IPair i in _indexPairs) if (i.uvIndex == uvIndex) return true;
        return false;
    }

}