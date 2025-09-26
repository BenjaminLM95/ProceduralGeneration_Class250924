using UnityEngine;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]


public class MeshGenerator : MonoBehaviour
{
    // vertices
    // triangles
    // uvs
    // normals

    Mesh mesh;

    int height = 10;
    int width = 10;

    float xCoord;
    float yCoord;
    float noise; 

    Vector3[] verts;
    Vector2[] uvs;


    int[] tris;
    //position in tris array
    int tri = 0;
    // position in verts
    int vert = 0;

    public Material _material; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        



    }

    private void OnDrawGizmos()
    {
        /*
        if (Application.isPlaying)
        {

            for (int i = 0; i < verts.Length; i++)
            {
                Gizmos.DrawSphere(verts[i], 0.2f);
            }
        } */
    }

    // Update is called once per frame
    void Update()
    {
                

        mesh = new Mesh();

        SetCoordinator();

        AssignVerts();

        // assign tris

        AssignTris();

        Renderer renderer = GetComponent<Renderer>();
        renderer.material = _material;




    }

    private void SetCoordinator() 
    {
        verts = new Vector3[(width + 1) * (height + 1)];

        uvs = new Vector2[verts.Length];

        int pos = 0;
        for (int i = 0; i < height + 1; i++)
        {
            for (int j = 0; j < width + 1; j++)
            {
                xCoord = (float)j / width;
                yCoord = (float)i / height;

                if ((i < 4 && j < 4) || (i > height - 4 && j > width - 4))
                {
                    noise = Mathf.PerlinNoise(xCoord, yCoord) * 3;
                }
                else
                {
                    noise = Mathf.Sin((Mathf.PerlinNoise(xCoord, yCoord) - 0.5f) * 3 * Time.time) * (1 + Random.Range(1, 100) / 100);
                }
                

                verts[pos] = new Vector3(j, noise, i);
                uvs[pos] = new Vector2((float)j / width, (float)i / height);

                pos++;
            }
        }
    }

    private void AssignVerts() 
    {
        tris = new int[width * height * 6];

        //position in tris array
        tri = 0;
        // position in verts
        vert = 0;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //We will do six points (two triangle
                // First triangle
                tris[tri + 0] = vert + 0;
                tris[tri + 1] = vert + width + 1;
                tris[tri + 2] = vert + 1;

                // Second triangle
                tris[tri + 3] = vert + 1;
                tris[tri + 4] = vert + width + 1;
                tris[tri + 5] = vert + width + 2;

                tri += 6;
                vert++;
            }
            vert++;

        }
    }

    private void AssignTris() 
    {
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
