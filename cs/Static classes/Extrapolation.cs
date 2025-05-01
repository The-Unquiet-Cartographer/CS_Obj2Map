using ExtensionMethods;
using static ExtensionMethods.FloatExtensions;
using static ExtensionMethods.Indexing;

public static class Extrapolation {

//  Transforms face into 2D "transformed" space,
//  uses the vectors between face uvs to create a transform to each "normal" uv {(0,0), (0,1), (1,1), (1,0)},
//  scales each transform by the respective face edge vectors in "transformed" space,
//  showing us where the "normal" uvs are in relation to the face uvs in "transformed" space.
//
//  This allows us to get the texture rotation and scale.

    public static string ExtrapolateMapTextureInfo (in Model _model, int faceIndex, in Material material, bool strictlyAlignTextures, out string debugText) {
        Vector3[] vertices = _model.GetFaceVertices(faceIndex);
        Vector2[] uvs = _model.GetFaceTextureVertices(faceIndex).Select(x => {return x == null ? new Vector2(0,0) : x!.Value;}).ToArray();
        Vector3 faceNormal = _model.GetFaceNormal(faceIndex);

        Vector3 textureAxis_x, textureAxis_y;
        Vector2[] vertices2D = GetFaceVerticesInTransformedSpace(vertices, faceNormal, out textureAxis_x, out textureAxis_y);
        Vector2[] edges2D = GetFaceEdgesInTransformedSpace(vertices2D);
        Vector2[] nuvs_transformed = GetNormalUVsInTransformedSpace(uvs[0], vertices2D[0], uvs, edges2D);
        Vector2 uv_centroid = new Vector2(
            (nuvs_transformed[0].x + nuvs_transformed[1].x + nuvs_transformed[2].x + nuvs_transformed[3].x) / 4,
            (nuvs_transformed[0].y + nuvs_transformed[1].y + nuvs_transformed[2].y + nuvs_transformed[3].y) / 4
        );

    //GET KEY DATA
        float rotation = RadiansToDegrees( Vector2.SignedAngle_L(
                new Vector2(0,1),
                nuvs_transformed[0].DirectionTo(nuvs_transformed[1])
        ) );

        textureAxis_x = textureAxis_x.Rotate_L_OlindeRodrigues(faceNormal, rotation);
        textureAxis_y = textureAxis_y.Rotate_L_OlindeRodrigues(faceNormal, rotation);

        Vector2 scale = new Vector2(
            nuvs_transformed[0].DirectionTo(nuvs_transformed[3]).magnitude/material.width,
            nuvs_transformed[0].DirectionTo(nuvs_transformed[1]).magnitude/material.height
        );

        Vector2 offset;
        Vector2? offset_ = Vector2.Intersection(
            new Vector2(0,0),
            nuvs_transformed[0].DirectionTo(nuvs_transformed[3]),
            nuvs_transformed[0],
            nuvs_transformed[0].DirectionTo(nuvs_transformed[1])
        );
        if (offset_ == null) offset = new Vector2(0,0);
        else {
            offset = new Vector2(
                new Vector2(0,0).DirectionTo(offset_!.Value).magnitude / scale.x % material.width,
                nuvs_transformed[0].DirectionTo(offset_!.Value).magnitude / scale.y % material.height
            );
        }

    //ROUNDING
        if (strictlyAlignTextures) {
            rotation = rotation.Round(1);
            textureAxis_x = textureAxis_x.Round(1);
            textureAxis_y = textureAxis_y.Round(1);
            scale = scale.Round(0.125f);
            offset = offset.Round(1);
        }
        else {
            rotation = rotation.Round(0.01f);
            textureAxis_x = textureAxis_x.Round(0.001f);
            textureAxis_y = textureAxis_y.Round(0.001f);
            scale = scale.Round(0.01f);
            offset = offset.Round(1);
        }

    //DEBUG
    /*
        Debug_MultiColoured(
            new ColStr(ColStr.Type.Text, "Transformed uvs are "), new ColStr(ColStr.Type.Point, nuvs_transformed.Stringify(" ")),
            new ColStr(ColStr.Type.Text, ", centroid is "), new ColStr(ColStr.Type.Point, uv_centroid.ToString())
        );
    */

        debugText = "";
        //debugText = "//uvs = "+uvs.Stringify(" ")+"\n\t//nuvs (rounded) = "+nuvs_transformed.Select(x => x.Round(1)).ToArray().Stringify();

        return(
            "[ "+textureAxis_x.x+" "+textureAxis_x.y+" "+textureAxis_x.z+" "+offset.x+" ] "
            +"[ "+textureAxis_y.x+" "+textureAxis_y.y+" "+textureAxis_y.z+" "+offset.y+" ] "
            +rotation+" "+scale.x+" "+scale.y
        );

    }



    public static Vector2[] GetFaceVerticesInTransformedSpace (in Vector3[] _vertices, in Vector3 faceNormal, out Vector3 textureAxis_x, out Vector3 textureAxis_y) {
        Vector2[] vertices2D = new Vector2[_vertices.Length];
    //Get the first transformed vertex using the face-relative "up" axis
        {
            //vertices2D[0] = new Vector2(0,0);
            GetFaceAxes(faceNormal, out textureAxis_x, out textureAxis_y);
            Vector3 upAxis = textureAxis_y.Invert();                                                    //<== GetFaceAxes() returns the right and down axes. This is correct and necessary.
            Vector3 to3D = _vertices[0].DirectionTo(_vertices[1]);
            float angleInDegrees = RadiansToDegrees(Vector3.SignedAngle_L(faceNormal, upAxis, to3D));
            Vector2 to2D = new Vector2(0,1).Rotate_L(angleInDegrees).Scale1(to3D.magnitude);            //<== To get the angle we measure from upAxis; here we transform from the Vector2 up axis.
            vertices2D[1] = to2D;
        }
    //Get the following transformed vertices
        for (int i = 1; i < _vertices.Length; i++) {
            int h = i.Bwd(_vertices.Length);
            int j = i.Fwd(_vertices.Length);
            Vector3 from3D = _vertices[i].DirectionTo(_vertices[h]);
            Vector3 to3D = _vertices[i].DirectionTo(_vertices[j]);
            float angleInDegrees = RadiansToDegrees(Vector3.SignedAngle_L(faceNormal, from3D, to3D));
            Vector2 from2D = vertices2D[i].DirectionTo(vertices2D[h]);
            Vector2 to2D = from2D.normalized.Rotate_L(angleInDegrees).Scale1(to3D.magnitude);
            vertices2D[j] = vertices2D[i].Add(to2D);
        }

    /*
    //DEBUG
        Console.WriteLine();
        Vector3 dominantFacing = GetDominantFacing(faceNormal);
        if (dominantFacing.x != 0) ConsoleManager.WriteColoredLine(ConsoleColor.DarkRed, "Dominant facing is X:");
        else if (dominantFacing.y != 0) ConsoleManager.WriteColoredLine(ConsoleColor.DarkGreen, "Dominant facing is Y:");
        else if (dominantFacing.z != 0) ConsoleManager.WriteColoredLine(ConsoleColor.DarkBlue, "Dominant facing is Z:");
        Console.WriteLine();
        for (int i = 0; i < _vertices.Length; i++) {
            int h = i.Bwd(_vertices.Length);
            int j = i.Fwd(_vertices.Length);
            Vector3 from3D = _vertices[i].DirectionTo(_vertices[h]);
            Vector3 to3D = _vertices[i].DirectionTo(_vertices[j]);
            Vector2 from2D = vertices2D[i].DirectionTo(vertices2D[h]);
            Vector2 to2D = vertices2D[i].DirectionTo(vertices2D[j]);
            float angleInDegrees3D = RadiansToDegrees(Vector3.SignedAngle_L(faceNormal, from3D, to3D));
            float angleInDegrees2D = RadiansToDegrees(Vector2.SignedAngle_L(from2D, to2D));
            Debug_MultiColoured(
                new ColStr(ColStr.Type.Text, "Vertex "), new ColStr(ColStr.Type.VertInd, i+" "), new ColStr(ColStr.Type.Point, _vertices[i]+" "),
                new ColStr(ColStr.Type.Text, "to vertex "), new ColStr(ColStr.Type.VertInd, j+" "), new ColStr(ColStr.Type.Point, _vertices[j]+" "),
                new ColStr(ColStr.Type.Text, "is vector "), new ColStr(ColStr.Type.Vector, to3D+" "),
                new ColStr(ColStr.Type.Text, "with length "), new ColStr(ColStr.Type.Length, to3D.magnitude.ToString())
            );
            Debug_MultiColoured(
                new ColStr(ColStr.Type.Text, ">>> Transformed vector is "), new ColStr(ColStr.Type.Vector, to2D+" "),
                new ColStr(ColStr.Type.Text, "with length "), new ColStr(ColStr.Type.Length, to2D.magnitude.ToString())
            );
            Debug_MultiColoured(
                new ColStr(ColStr.Type.Text, "Angle at vertex "), new ColStr(ColStr.Type.VertInd, i+" "),
                new ColStr(ColStr.Type.Text, "is "), new ColStr(ColStr.Type.Angle, angleInDegrees3D.ToString())
            );
            Debug_MultiColoured(
                new ColStr(ColStr.Type.Text, ">>> Transformed angle is "), new ColStr(ColStr.Type.Angle, angleInDegrees2D.ToString())
            );
            Console.WriteLine();

        //End iterating vertices for debug
        }
        //END DEBUG
    */

        return vertices2D;
    }

  

    public static Vector2[] GetFaceEdgesInTransformedSpace (in Vector2[] _vertices2D) {
        Vector2[] edges2D = new Vector2[_vertices2D.Length];
        for (int i = 0; i < _vertices2D.Length; i++) {
            int j = i.Fwd(_vertices2D.Length);
            edges2D[i] = _vertices2D[i].DirectionTo(_vertices2D[j]);
        }
        return edges2D;
    }



    public static Vector2[] GetNormalUVsInTransformedSpace (in Vector2 fromPt_uvSpace, in Vector2 fromPt_transformed, in Vector2[] _uvs, in Vector2[] _edges2D) {
        Vector2[] uvs_normal = new Vector2[] {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0)
        };
        Vector2 texEdge_a = _uvs[0].DirectionTo(_uvs[1]);
        Vector2 faceEdge_a_transformed = _edges2D[0];
        Vector2 texEdge_b = _uvs[0].DirectionFrom(_uvs[_uvs.Length-1]);
        Vector2 faceEdge_b_transformed = _edges2D[_edges2D.Length-1];               //<== The face edge needs to match the texture edge, AND be going in the same direction (important). Same with A.
        Vector2[] toPts_transformed = new Vector2[4];

    //Get a transform for each uv
        float GetScalar (Vector2 v1, Vector2 v2) {
            if (v1.magnitude == 0 || v2.magnitude == 0) return 0;
            return v1.magnitude/v2.magnitude
            * MathF.Sign(Vector2.Dot(v1, v2));
        }
        for (int i = 0; i < uvs_normal.Length; i++) {
            if (_uvs[0].Equals(uvs_normal[i])) {
                toPts_transformed[i] = new Vector2(0,0);
                continue;
            }
            Vector2? _interPt = Vector2.Intersection(fromPt_uvSpace, texEdge_a, uvs_normal[i], texEdge_b);
            if (_interPt == null) {
                //Either this face has no uvs, or the program is trying to intersect two lines that are parallel.
                continue;
            }
            Vector2 interPt = _interPt!.Value;
            Vector2 toPt_a = fromPt_uvSpace.DirectionTo(interPt);
            Vector2 toPt_b = interPt.DirectionTo(uvs_normal[i]);
            float scalar_a = GetScalar(toPt_a, texEdge_a);
            float scalar_b = GetScalar(toPt_b, texEdge_b);
            Vector2 toPt_a_transformed = faceEdge_a_transformed.Scale1(scalar_a);
            Vector2 toPt_b_transformed = faceEdge_b_transformed.Scale1(scalar_b);
            toPts_transformed[i] = toPt_a_transformed.Add(toPt_b_transformed).Add(fromPt_transformed);

        /*
        //DEBUG
            Debug_MultiColoured(
                new ColStr(ColStr.Type.Text, "Vectors "), new ColStr(ColStr.Type.Vector, texEdge_a.normalized+" "),
                new ColStr(ColStr.Type.Text, "and "), new ColStr(ColStr.Type.Vector, texEdge_b.normalized+" "),
                new ColStr(ColStr.Type.Text, "from points "), new ColStr(ColStr.Type.Point, fromPt_uvSpace+" "),
                new ColStr(ColStr.Type.Text, "and "), new ColStr(ColStr.Type.Point, uvs_standard[i]+" "),
                new ColStr(ColStr.Type.Text, "intersect at point "), new ColStr(ColStr.Type.Point, interPt.ToString())
            );
            Debug_MultiColoured(
                new ColStr(ColStr.Type.Text, "Resultant transform from "), new ColStr(ColStr.Type.Point, fromPt_uvSpace+" "),
                new ColStr(ColStr.Type.Text, "to "), new ColStr(ColStr.Type.Point, uvs_standard[i]+" "),
                new ColStr(ColStr.Type.Text, "is "), new ColStr(ColStr.Type.Vector, toPt_a+" "),
                new ColStr(ColStr.Type.Text, "+ "), new ColStr(ColStr.Type.Vector, toPt_b+" "),
                new ColStr(ColStr.Type.Text, "or "), new ColStr(ColStr.Type.Vector, toPt_a.Add(toPt_b).ToString())
            );
            //Debug_MultiColoured(
            //    new ColStr(ColStr.Type.Text, "Scalars are "), new ColStr(ColStr.Type.Scalar, scalar_a+" "),
            //    new ColStr(ColStr.Type.Text, "and "), new ColStr(ColStr.Type.Scalar, scalar_b+" ")
            //);
            Debug_MultiColoured(
                new ColStr(ColStr.Type.Text, "Transformed transform for uv "), new ColStr(ColStr.Type.Point, uvs_standard[i]+" "),
                new ColStr(ColStr.Type.Text, "is "), new ColStr(ColStr.Type.Vector, toPt_a_transformed+" "),
                new ColStr(ColStr.Type.Text, "+ "), new ColStr(ColStr.Type.Vector, toPt_b_transformed+" "),
                new ColStr(ColStr.Type.Text, "or "), new ColStr(ColStr.Type.Vector, toPts_transformed[i].ToString())
            );
            Console.WriteLine();
            //END DEBUG
        */

        //End iterating uvs_normal
        }
        return toPts_transformed;
    }



////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  INTERNAL(?) FUNCTIONS
//
////////////////////////////////////////////////////////////////////////////////////////////////////
    public static Vector3 GetDominantFacing (in Vector3 _faceNormal) {
        return _faceNormal.Sign(1);
    }



    private static Vector3
        right3 = new Vector3(1,0,0), forward3 = new Vector3(0,1,0), up3 = new Vector3(0,0,1), 
        left3 = new Vector3(-1,0,0), back3 = new Vector3(0,-1,0), down3 = new Vector3(0,0,-1),
        zero3 = new Vector3(0,0,0)
    ;
    public static void GetFaceAxes (in Vector3 faceNormal, out Vector3 rightAxis, out Vector3 downAxis) {
        Vector3 dominantFacing = faceNormal.Sign(1);
        if (dominantFacing.x != 0) {
            Vector3 pn_xy = new Vector3(faceNormal.x, faceNormal.y, 0).normalized;
            rightAxis = Vector3.Cross(pn_xy, up3);
            Vector3 pn_xz = new Vector3(faceNormal.x, 0, faceNormal.z).normalized;
            if (dominantFacing.x == -1) downAxis = Vector3.Cross_LeftHanded(pn_xz, back3);
            else downAxis = Vector3.Cross_LeftHanded(pn_xz, forward3);
        }
        else if (dominantFacing.y != 0) {
            Vector3 pn_xy = new Vector3(faceNormal.x, faceNormal.y, 0).normalized;
            rightAxis = Vector3.Cross(pn_xy, up3);
            Vector3 pn_yz = new Vector3(0, faceNormal.y, faceNormal.z).normalized;
            if (dominantFacing.y == -1) downAxis = Vector3.Cross_LeftHanded(pn_yz, right3);
            else downAxis = Vector3.Cross_LeftHanded(pn_yz, left3);
        }
        else if (dominantFacing.z != 0) {
            Vector3 pn_xz = new Vector3(faceNormal.x, 0, faceNormal.z).normalized;
            Vector3 pn_yz = new Vector3(0, faceNormal.y, faceNormal.z).normalized;
            if (dominantFacing.z == 1) {
                rightAxis = Vector3.Cross(pn_xz, forward3);
                downAxis = Vector3.Cross_LeftHanded(pn_yz, right3);
            }
            else {
                rightAxis = Vector3.Cross(pn_xz, back3);
                downAxis = Vector3.Cross_LeftHanded(pn_yz, right3);
            }
        }
        else {
            rightAxis = zero3;
            downAxis = zero3;
        }
    }


    
    private struct ColStr {
        public ConsoleColor col {get; private set;}
        public string str {get; private set;}
        public ColStr (ConsoleColor col, string str) {
            this.col = col;
            this.str = str;
        }
        public enum Type {Text, VertInd, Vector, Point, Length, Angle, Scalar}
        private static ConsoleColor[] cols = new ConsoleColor[] {
            ConsoleColor.White, ConsoleColor.Magenta, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Red
        };
        public ColStr (Type type, string str) {
            this.col = cols[(int)type];
            this.str = str;
        }
    }
    private static void Debug_MultiColoured(params ColStr[] colStrs) {
        foreach (ColStr cs in colStrs) {
            Console.ForegroundColor = cs.col;
            Console.Write(cs.str);
            Console.ResetColor();
        }
        Console.WriteLine();
    }



}