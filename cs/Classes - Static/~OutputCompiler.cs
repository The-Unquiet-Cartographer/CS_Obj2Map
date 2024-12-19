public static class OutputCompiler {

    public static string Compile_MAP (Model model, bool invertNormals, int brushThickness, GeneralSettings.Optimise_MAP optimiser = GeneralSettings.Optimise_MAP.WorldCraft) {
        string output = "// entity 0"+NewLine(0)+"{"+NewLine(1)+"\"classname\"\"worldspawn\"";
        for (int i = 0; i < model.faces.Length; i++) {
            Face face = model.faces[i];
        //Texture
            string textureInfo_face = "";
            string textureInfo_hidden = "";
            switch (optimiser) {
                case GeneralSettings.Optimise_MAP.WorldCraft:
                textureInfo_face = (model.faces[i].texture == "") ? "#DEFAULT" : model.faces[i].texture;
                textureInfo_hidden = "#DEFAULT";
                break;
                case GeneralSettings.Optimise_MAP.Radiant:
                textureInfo_face = (model.faces[i].texture == "") ? "common/caulk" : model.name+"/"+model.faces[i].texture;
                textureInfo_hidden = "common/caulk";
                break;
            }
            textureInfo_face = textureInfo_face+" 0 0 0 1 1";
            textureInfo_hidden = textureInfo_hidden+" 0 0 0 1 1";
        //Brush vertices
            Vector3[] _vertices = model.GetFaceVertices(i);
            Vector3[] brushVertices = new Vector3[face.vertexCount*2];
            Vector3 unitNormal = Vector3.Cross(
                Vector3.GetDirection(_vertices[0], _vertices[1]),
                Vector3.GetDirection(_vertices[0], _vertices[_vertices.Length-1])
            ).Sign(3).Scale1(brushThickness);
            if (invertNormals) unitNormal = unitNormal.Invert();
            for (int j = 0; j < _vertices.Length; j++) {
                brushVertices[j*2] = _vertices[j];
                brushVertices[j*2+1] = Vector3.Add(_vertices[j], unitNormal);
            }
        //Write faces
            string brush = NewLine(1)+"// brush "+i+NewLine(1)+"{";
        //Front
            brush += NewLine(2)+WriteBrushPlane(brushVertices[0], brushVertices[2], brushVertices[4], textureInfo_face);
        //Rear
            brush += NewLine(2)+WriteBrushPlane(brushVertices[1], brushVertices[5], brushVertices[3], textureInfo_hidden);
        //Sides
            for (int j = 0; j < brushVertices.Length-2; j+=2) {
                brush += NewLine(2)+WriteBrushPlane(brushVertices[j], brushVertices[j+1], brushVertices[j+2], textureInfo_hidden);
            }
            brush += NewLine(2)+WriteBrushPlane(brushVertices[brushVertices.Length-2], brushVertices[brushVertices.Length-1], brushVertices[0], textureInfo_hidden);
        //FINISH
            brush += NewLine(1)+"}";
            output += brush;
        }
        output += NewLine()+"}\r\n";
        return output;
    }

//Format for a brush plane for a .map file
/*
*   I think we need to invert the plane by swapping two of the coordinate values, due to the way QBSP processes .MAP files.
*/
    private static string WriteBrushPlane(Vector3 a, Vector3 b, Vector3 c, string textureInfo) {
        return WriteCoord(b)+" "+WriteCoord(a)+" "+WriteCoord(c)+" "+textureInfo;
    }

//Take a vertex and return a .map readable string
    private static string WriteCoord(Vector3 v) {
        return "( "+v.x+" "+v.y+" "+v.z+" )";
    }

    private static string NewLine(byte indent = 0) {
        string s = "\r\n";
        for (int i = 0; i < indent; i++) s+="\t";
        return s;
    }

}