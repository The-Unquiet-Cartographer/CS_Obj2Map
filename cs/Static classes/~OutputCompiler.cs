// https://en.wikipedia.org/wiki/Wavefront_.obj_file
// https://quakewiki.org/wiki/Quake_Map_Format

using static ExtensionMethods.FloatExtensions;
using static ExtensionMethods.CollectionExtensions;

public static class OutputCompiler {

////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  .MAP FORMAT
//
////////////////////////////////////////////////////////////////////////////////////////////////////
    public static string Compile_MAP (in Model model, in MaterialLibrary materialLibrary, bool invertNormals, int brushThickness, bool strictTextureAlignment, GeneralSettings.Optimise_MAP optimiser) {
        string output = "// entity 0"+NewLine(0)+"{"+NewLine(1)+"\"classname\"\"worldspawn\"";
        if (optimiser == GeneralSettings.Optimise_MAP.WorldCraft_Valve220 || optimiser == GeneralSettings.Optimise_MAP.Radiant_Valve220) output += NewLine(1)+"\"mapversion\"\"220\"";
        for (int i = 0; i < model.faces.Length; i++) {
            Face face = model.faces[i];
        //Brush vertices
            Vector3 _faceNormal = model.GetFaceNormal(i);
            Vector3 unitNormal = _faceNormal.Sign(3).Scale1(brushThickness);
            if (!invertNormals) unitNormal = unitNormal.Invert();
            Vector3[] _vertices = model.GetFaceVertices(i);
            Vector3[] brushVertices = new Vector3[face.vertexCount*2];
            for (int j = 0; j < _vertices.Length; j++) {
                brushVertices[j*2] = _vertices[j];
                brushVertices[j*2+1] = Vector3.Add(_vertices[j], unitNormal);
            }

        /*
        //For debugging purposes, eliminate any faces that don't match these criteria:
            if (
                !_faceNormal.Equals(new Vector3(0,0,-1))
            ||  _vertices.Any(v => {
                    if (v.x < 353 || v.x > 1123
                    ||  v.y < 385 || v.y > 1541
                    ||  v.z < 0 || v.z > 0
                    ) return true;
                    return false;
                })
            ) continue;
        */

        //Get texture info, optimising for WorldCraft or Radiant
            Material? material = materialLibrary.FindMaterial(model.faces[i].texture);
            string textureInfo_hidden = "";
            switch (optimiser) {
                case GeneralSettings.Optimise_MAP.WorldCraft_Legacy:
                    if (material == null) material = MaterialLibrary.defaultMaterial_WorldCraft;
                    textureInfo_hidden = "#DEFAULT 0 0 0 1 1";
                    break;
                case GeneralSettings.Optimise_MAP.WorldCraft_Valve220:
                    if (material == null) material = MaterialLibrary.defaultMaterial_WorldCraft;
                    textureInfo_hidden = "#DEFAULT [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1";
                    break;
                case GeneralSettings.Optimise_MAP.Radiant_Legacy:
                    if (material == null) material = MaterialLibrary.defaultMaterial_Radiant;
                    textureInfo_hidden = "common/caulk 0 0 0 1 1";
                    break;
                case GeneralSettings.Optimise_MAP.Radiant_Valve220:
                    if (material == null) material = MaterialLibrary.defaultMaterial_Radiant;
                    textureInfo_hidden = "common/caulk [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1";
                    break;
            }
            string textureName = material!.Value.texture == "" ? material!.Value.name : material!.Value.texture;
            string debugText;
            string info_face = Extrapolation.ExtrapolateMapTextureInfo(model, i, material!.Value, strictTextureAlignment, out debugText);
            string textureInfo_face = textureName+" "+info_face;
        //Write faces
            string brush = NewLine(1)+"// brush "+i+NewLine(1)+"{";
            //if (debugText.Length > 0) brush += NewLine(1)+""+debugText;
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

    //Format for a brush plane for a .map file
    /*
    *   I think we need to invert the plane by swapping two of the coordinate values, due to the way QBSP processes .MAP files.
    */
        string WriteBrushPlane(Vector3 a, Vector3 b, Vector3 c, string textureInfo) {
            return WriteCoord(b)+" "+WriteCoord(a)+" "+WriteCoord(c)+" "+textureInfo;
        }
    //Take a vertex and return a .map readable string
        string WriteCoord(Vector3 v) {
            return "( "+v.x+" "+v.y+" "+v.z+" )";
        }
    }


    private static string NewLine(byte indent = 0) {
        string s = "\r\n";
        for (int i = 0; i < indent; i++) s+="\t";
        return s;
    }

}