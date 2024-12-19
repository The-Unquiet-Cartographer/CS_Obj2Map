public static class MarathonFix {

/// <summary>
/// Only checks each face against the one immediate preceeding it in the faces array, which is how Weiland exports .OBJs.
/// </summary>
    public static void Quick (Model _model) {
        for (int i = 1; i < _model.faces.Length; i++) {
            Vector3[] thisFace = _model.GetFaceVertices(i);
            Vector3[] prevFace = _model.GetFaceVertices(i-1);
            if (thisFace.Length != prevFace.Length) continue;
            for (int j = 0; j < thisFace.Length; j++) {
                if (IsCeilingAndFloor(thisFace, prevFace)) {
                    _model.faces[i].ReverseVertexOrder();
                    break;
                }
            }
            continue;
        }
    }

/// <summary>
/// Checks every face against every other face.
/// </summary>
    public static void Thorough (Model _model) {
        for (int i = 0; i < _model.faces.Length; i++) {
            Vector3[] thisFace = _model.GetFaceVertices(i);
            for (int j = 0; j < _model.faces.Length; j++) {
                if (j == i || _model.faces[j].vertexCount != _model.faces[i].vertexCount) continue;
                Vector3[] otherFace = _model.GetFaceVertices(j);
                if (IsCeilingAndFloor(thisFace, otherFace)) {
                    _model.faces[i].ReverseVertexOrder();
                    break;
                }
            }
            continue;
        }
    }

    private static bool IsCeilingAndFloor (Vector3[] ceiling, Vector3[] floor) {
        for (int i = 0; i < ceiling.Length; i++) {
            if (
                ceiling[i].x == floor[i].x
            &&  ceiling[i].z == floor[i].z
            &&  ceiling[i].y > floor[i].y
            ) continue;
            return false;
        }
        return true;
    } 
} 