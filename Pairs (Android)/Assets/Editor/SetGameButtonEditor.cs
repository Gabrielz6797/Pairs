using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SetGameButton))]
[CanEditMultipleObjects]
[System.Serializable]
public class SetGameButtonEditor : Editor
{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        SetGameButton myScript = target as SetGameButton;
        switch(myScript.ButtonType){
            case SetGameButton.EButtonType.PuzzleCategoryButton:
                myScript.puzzleCategories = (GameSettings.EPuzzleCategories) EditorGUILayout.EnumPopup("Puzzle Categories",myScript.puzzleCategories);
                break; 
        }
        if(GUI.changed){
            EditorUtility.SetDirty(target);
        }
    }
}
