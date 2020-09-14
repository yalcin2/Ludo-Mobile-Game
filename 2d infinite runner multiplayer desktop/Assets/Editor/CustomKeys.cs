using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[ExecuteInEditMode]
public class CustomKeys :  ScriptableObject  {
#if UNITY_EDITOR
    
    
//    https://docs.unity3d.com/ScriptReference/MenuItem.html
// #
    [MenuItem ("Custom/Back &_1")]
    static void SetCameraBack(){
//        Debug.Log("cam back");

//        Vector3 position = SceneView.lastActiveSceneView.pivot;
//        position.z -= 10.0f;
//        SceneView.lastActiveSceneView.pivot = position;
//        SceneView.lastActiveSceneView.Repaint()

        if (SceneView.lastActiveSceneView == null) return;
        
        SceneView.lastActiveSceneView.rotation = Quaternion.Euler(0,0,0);
        SceneView.lastActiveSceneView.pivot = SceneView.lastActiveSceneView.pivot.magnitude*Vector3.back;
        SceneView.lastActiveSceneView.Repaint();

    }
   
    [MenuItem ("Custom/Play #_F10")]
    static void PlayGame(){
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }
    [MenuItem ("Custom/Pause #_F11")]
    static void PauseGame(){
        EditorApplication.ExecuteMenuItem("Edit/Pause");
    }
    [MenuItem ("Custom/Scene &_LEFT")]
    static void SceneTab(){
        EditorApplication.ExecuteMenuItem("Window/Scene");
    }
    [MenuItem ("Custom/Game &_RIGHT")]
    static void GameTab(){
        EditorApplication.ExecuteMenuItem("Window/Game");
    }

    
    
    
#endif
}
