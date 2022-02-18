using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum Object
{
    grabbingObject,
    grabbingTarget,
    mainCamera,
    segmentedCam,
    groundPlane, 
    directionalLight,
}
public class DomainController : MonoBehaviour
{
    public Object Object;
    private TaskFileTemplate currentTask;
    private bool randomRobotInitialization;

    public void Init(Object Object, TaskFileTemplate currentTask)
    {
        this.Object = Object;
        changeSize(Object);
        changeColor(Object);
        changeLight(Object);
        changeRobotInitPose(currentTask, randomRobotInitialization);
        changeCameraPose(Object);
        changeGroundPlaneTexture(Object);
        changeSky();
    }

    public void changeSize(Object Object)
    {                
        float rd =  Random.Range(-2f, 2f);
        Vector3 scaleVector = new Vector3(rd, rd, rd);
        if(this.Object == Object.grabbingObject)
        {
            this.transform.localScale = new Vector3(3 + scaleVector.x, 3 + scaleVector.y,3 + scaleVector.z);
        }
        if(this.Object == Object.grabbingTarget)
        {
            this.transform.localScale = new Vector3(15 + scaleVector.x * 2, 0.1f, 15 + scaleVector.z * 2);
        } 
    }
    public void changeColor(Object Object)
    {
        this.Object = Object;
        if(this.Object == Object.grabbingObject || this.Object == Object.grabbingTarget)
        {
            this.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
    }
    public void changeCameraPose(Object Object)
    {
        this.Object = Object;
        var camCoordinates = new List<(Vector3 position, Vector3 rotation)>
        {
            (new Vector3(53, 25, 24), new Vector3(14, -118, 0)),
            (new Vector3(53, 25, -24), new Vector3(14, -58, 0)),
            (new Vector3(53, 7.7f, -24), new Vector3(0.5f, -58, 0)),
            (new Vector3(53, 7.7f, 24), new Vector3(0.5f, -125, 0)),
            (new Vector3(62, 7.7f, 0), new Vector3(0.5f, -90, 0)),
            (new Vector3(62, 14.7f, 0), new Vector3(6.2f, -90, 0)),

        };
        int index = Random.Range(0, camCoordinates.Count);
        Vector3 newPosition = camCoordinates[index].Item1;
        Vector3 newRotation = camCoordinates[index].Item2;
        if(this.Object == Object.mainCamera /*|| this.Object == Object.segmentedCam*/)
        {
            this.transform.position = newPosition;
            this.transform.eulerAngles = newRotation;
        }
    }
    public void changeLight(Object Object)
    {
        this.Object = Object;
        if(this.Object == Object.directionalLight)
        {
            this.GetComponent<Light>().color = Color.Lerp(Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), Mathf.PingPong(Time.time, 1));
        }
    }
    public void changeRobotInitPose(TaskFileTemplate currentTask, bool randomRobotInitialization)
    {
        currentTask.randomRobotInitialization = true;
    }
    public void changeGroundPlaneTexture(Object Object)
    {
        this.Object = Object;   
        if(this.Object == Object.groundPlane)
        {
            var textureList = new List<Texture2D>();
            foreach(Texture2D go in Resources.LoadAll("Textures", typeof(Texture2D)))
            {
                textureList.Add(go);
            }
            Texture2D randomTexture = (Texture2D)textureList[Random.Range(0,textureList.Count)];
            this.GetComponent<Renderer>().material = null;
            this.GetComponent<Renderer>().material.EnableKeyword ("_NORMALMAP");
            this.GetComponent<Renderer>().material.EnableKeyword ("_METALLICGLOSSMAP");
            this.GetComponent<Renderer>().material.SetTexture("_MainTex", randomTexture);
            this.GetComponent<Renderer>().material.mainTextureScale = new Vector2(100, 100);
        }
    }
    public void changeSky()
    {
        var skyMaterialList = new List<Material>();
        foreach(Material x in Resources.LoadAll("Sky", typeof(Material)))
        {
            skyMaterialList.Add(x);
        }
        Material randomSkyMaterial = (Material)skyMaterialList[Random.Range(0,skyMaterialList.Count)];
        RenderSettings.skybox = randomSkyMaterial;
    }
}