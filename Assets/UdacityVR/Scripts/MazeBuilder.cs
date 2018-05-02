using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MazeBuilder : MonoBehaviour {

    // Containers for cloned, programmatically added objects, to separate them from regular game objects.
    public Transform mazeContainer;
    public Transform waypointContainer;

    // Prefabs
    public GameObject prefabWallA;
    public GameObject prefabWallB;
    public GameObject prefabPillarExtA;
    public GameObject prefabPillarExtB;
    public GameObject prefabPillar;


    // Other game objects to clone
    public GameObject waypoint;


    // Default origin coordinates. All building blocks will use same y position.
    public float x = 36f;
    public float y = 0.0f;
    public float z = 27.0f;


    // Step widths for pillar extension and wall.
    public float stepExt = 0.9f;
    public float stepWall = 5.1f;

    // Flag to control regeneration in edit mode
    public bool isActive = false;

    // Build a horizontal wall segment.
    protected Vector3 buildHorizontalWall(Vector3 coords)
    {
        Vector3 ret = coords;
        float zAngle = 0.0f;

        GameObject l = (GameObject)Object.Instantiate(prefabPillarExtA, ret, Quaternion.Euler(-90, 0, zAngle), mazeContainer);
        l.tag = "Generated";
        ret.x -= stepExt;

        GameObject w = (GameObject)Object.Instantiate(prefabWallA, ret, Quaternion.Euler(-90, 0, zAngle), mazeContainer);
        w.tag = "Generated";
        ret.x -= stepWall;

        GameObject r = (GameObject)Object.Instantiate(prefabPillarExtB, ret, Quaternion.Euler(-90, 0, zAngle), mazeContainer);
        r.tag = "Generated";

        return ret;
    }


    // Build a vertical wall segment.
    protected Vector3 buildVerticalWall(Vector3 coords)
    {
        Vector3 ret = coords;
        float zAngle = 90.0f;

        // since variables t, w, b are not used anywhere else, can just used Object.Instantiate
        GameObject t = (GameObject)Object.Instantiate(prefabPillarExtA, ret, Quaternion.Euler(-90, 0, zAngle), mazeContainer);
        t.tag = "Generated";
        ret.z += stepExt;

        // tried using the bounds.size to increment coords, but this didn't match up properly
        // using hard-coded increments for the time being
        //Renderer tRenderer = rightExt.GetComponentInChildren<Renderer>();
        //ret.z += (tRenderer.bounds.size.z);

        GameObject w = (GameObject)Object.Instantiate(prefabWallA, ret, Quaternion.Euler(-90, 0, zAngle), mazeContainer);
        w.tag = "Generated";
        ret.z += stepWall;

        GameObject b = (GameObject)Object.Instantiate(prefabPillarExtB, ret, Quaternion.Euler(-90, 0, zAngle), mazeContainer);
        b.tag = "Generated";

        ret.z = ret.z - stepExt - stepWall;
        return ret;
    }


    // Build a pillar.
    protected Vector3 buildPillar(Vector3 coords)
    {
        Vector3 ret = coords;

        GameObject pillar = (GameObject)Object.Instantiate(prefabPillar, ret, Quaternion.Euler(-90, 0, 0), mazeContainer);
        pillar.tag = "Generated";
        return ret;
    }


    // Build empty space to skip over a wall width.
    protected Vector3 buildHorizontalWallSpace(Vector3 coords)
    {
        Vector3 ret = coords;
        ret.x = ret.x - stepExt - stepWall;
        return ret;
    }


    // Add waypoint
    protected void addWaypoint(Vector3 coords)
    {
        coords.x = coords.x + 3.8f;
        coords.y = coords.y + 2.0f;
        coords.z = coords.z + 2.5f;

        GameObject w = (GameObject)Object.Instantiate(waypoint, coords, Quaternion.identity, waypointContainer);
        w.tag = "Generated";
    }


    // Destroy all generated objects
    protected void destroyGenerated()
    {
        GameObject[] generated = GameObject.FindGameObjectsWithTag("Generated");

        for(var i = 0; i < generated.Length; i++)
        {
            DestroyImmediate(generated[i]);
        }
    }


    // Use this for initialization
    void Start () {
        if (!isActive) return;
        else isActive = false;

        destroyGenerated();
        
        Vector3 coords = new Vector3(x, y, z);

        string map = "" +
            "+-+-+-+-+   +-+-+-+-+-+-+\n" +
            "|               |       |\n" +
            "+     +-+-+-+-+ +       +\n" +
            "|     |   |` `| |       |\n" +
            "+ +-+-+   +-+-+ + +   + +\n" +
            "|     |           |   | |\n" +
            "+     +-+-+-+-+-+-+-+-+ +\n" +
            "|     |         |       |\n" +
            "+-+-+-+   +-+-+-+ +-+   +\n" +
            "|     |   |       |     |\n" +
            "+     +   +       +     +\n" +
            "|     |   |       |     |\n" +
            "+ +-+-+   +   +-+-+ +-+ +\n" +
            "|   |     |   |   |   | |\n" +
            "+   +   +-+   +   +   + +\n" +
            "|         |   |   |   | |\n" +
            "+         +   +   +   + +\n" +
            "|         |   |       | |\n" +
            "+-+-+   + +   +-+-+-+-+ +\n" +
            "|   |   | |           | |\n" +
            "+   +   + +-+-+-+-+   + +\n" +
            "|       |   |   |     | |\n" +
            "+       +   +   +   +-+ +\n" +
            "|       |   |   |   |   |\n" +
            "+-+-+   +-+ +   +   +   +\n" +
            "|   |     |     |   |   |\n" +
            "+   + +   +-+-+ +   +-+ +\n" +
            "|     |     |       |   |\n" +
            "+-+-+-+-+-+-+   +-+-+-+-+\n" +
            "";

        bool isPillarRow = true;
        bool isPillarColumn = false;

        foreach (char c in map)
        {
            isPillarColumn = !isPillarColumn;
            switch (c)
            {
                case '+': coords = buildPillar(coords); break;
                case '-': coords = buildHorizontalWall(coords); break;
                case '|': coords = buildVerticalWall(coords); break;
                case ' ':
                case '`':
                    if (!isPillarColumn) {
                        coords = buildHorizontalWallSpace(coords);
                        if (c != '`') addWaypoint(coords);
                    }
                    break;
                case '\n':
                    if (isPillarRow) coords = new Vector3(x, y, coords.z);
                    else coords = new Vector3(x, y, coords.z + stepExt + stepWall);
                    isPillarRow = !isPillarRow;
                    isPillarColumn = false;
                    break;
            }
        }

        // maze container adjustments
        // mazeContainer.localScale = new Vector3(1.5f, 1f, 1.4f);
        // mazeContainer.localScale = new Vector3(2.25f, 1f, 1.92f);
        // mazeContainer.Translate(new Vector3(36f, 0f, -4.9f));

        // additional waypoints
        // Object.Instantiate(waypoint, new Vector3(0f, 3f, 116f), Quaternion.identity, waypointContainer);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
