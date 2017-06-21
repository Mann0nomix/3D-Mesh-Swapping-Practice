using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSwapper : MonoBehaviour {
    public GameObject swapObject;
    float rotatedAngleY = 180f;
    SkinnedMeshRenderer thisRenderer;

    // Use this for initialization
    void Start()
    {
        //SkinnedMeshRenderer targetRenderer = swapObject.GetComponent<SkinnedMeshRenderer>();
        //Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
        //foreach (Transform bone in targetRenderer.bones)
        //{
        //    boneMap[bone.name] = bone;
        //    Debug.Log(bone.name);
        //}

        //SkinnedMeshRenderer thisRenderer = GetComponent<SkinnedMeshRenderer>();
        //Transform[] boneArray = thisRenderer.bones;
        //for (int idx = 0; idx < boneArray.Length; ++idx)
        //{
        //    //string boneName = boneArray[idx].name;
        //    string boneName = "Guard_Body_02";
        //    if (false == boneMap.TryGetValue(boneName, out boneArray[idx]))
        //    {
        //        Debug.LogError("failed to get bone: " + boneName);
        //        Debug.Break();
        //    }
        //}
        //thisRenderer.bones = boneArray; //take effect


        SkinnedMeshRenderer targetRenderer = swapObject.GetComponent<SkinnedMeshRenderer>();
        //    thisRenderer = GetComponent<SkinnedMeshRenderer>();
        //    thisRenderer.transform.Rotate(34, 0, 0);
        //    //RotateMesh(targetRenderer);
        gameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterial = targetRenderer.sharedMaterial;

        //Mesh[] jacketArray = Resources.LoadAll<Mesh>("Guard");
        //foreach(Mesh m in jacketArray)
        //{
        //    Debug.Log(m.name);
        //    if (m.name.Equals("Guard_head_white"))
        //    {
        //        gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
        //        gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = m;
        //    }
        //}

        Mesh[] jacketArray = Resources.LoadAll<Mesh>("Lu");
        foreach (Mesh m in jacketArray) {
            Debug.Log(m.name);
            if (m.name.Equals("Jacket_clean")) {
                gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = m;
            }
        }

        //AddLimb(gameObject, swapObject);
    }

    void RotateMesh(SkinnedMeshRenderer renderer)
    {
        Vector3[] originalVerts = renderer.sharedMesh.vertices;
        Vector3[] rotatedVerts = new Vector3[originalVerts.Length];
        Quaternion qAngle = Quaternion.AngleAxis(rotatedAngleY, Vector3.up);
        for (int vert = 0; vert < originalVerts.Length; vert++){
            rotatedVerts[vert] = qAngle * originalVerts[vert];
        }

        renderer.sharedMesh.vertices = rotatedVerts;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void AddLimb(GameObject swapObject, GameObject player)
    {
        SkinnedMeshRenderer[] BonedObjects = swapObject.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer sr in BonedObjects)
        {
            ProcessBonedObject(sr, player);
        }
    }

    private void ProcessBonedObject(SkinnedMeshRenderer renderer, GameObject player)
    {
        /*      Create the SubObject        */
        GameObject NewObj = new GameObject(renderer.gameObject.name);
        NewObj.transform.parent = player.transform;
        /*      Add the renderer        */
        NewObj.AddComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer NewRenderer = NewObj.GetComponent<SkinnedMeshRenderer>();
        /*      Assemble Bone Structure     */
        Transform[] MyBones = new Transform[renderer.bones.Length];
        for (int i = 0; i < renderer.bones.Length; i++)
            MyBones[i] = FindChildByName(renderer.bones[i].name, player.transform);
        /*      Assemble Renderer       */
        NewRenderer.bones = MyBones;
        NewRenderer.sharedMesh = renderer.sharedMesh;
        NewRenderer.materials = renderer.materials;
    }

    private Transform FindChildByName(string name, Transform obj)
    {
        Transform returnObject;

        if(obj.name == name)
        {
            return obj.transform;
        }
        
        foreach (Transform child in obj)
        {
            returnObject = FindChildByName(name, child );
            if(returnObject)
                return returnObject;
        }

        return null;
    }
}
