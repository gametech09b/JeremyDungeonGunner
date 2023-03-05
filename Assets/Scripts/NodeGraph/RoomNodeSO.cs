using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomNodeSO : ScriptableObject
{
    [HideInInspector] public string id;
    [HideInInspector] public List<string> parrentRoomNodeIDList = new List<string>();
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>();
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
    public RoomNodeTypeSO roomNodeType;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;

    #region Editor Code

    // the following code should only be run in the unity editor
#if UNITY_EDITOR

    [HideInInspector] public Rect rect;

    /// <summary>
    /// Initialize node
    /// </summary>
    public void Initialise(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.id = Guid.NewGuid().ToString();
        this.name = "Room Node";
        this.roomNodeGraph = nodeGraph;
        this.roomNodeType = roomNodeType;

        //Load room node type list
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    /// <summary>
    /// Draw node with the nodestyle
    /// </summary>

    public void Draw(GUIStyle nodeStyle)
    {
        // Draw node Box using begin area
        GUILayout.BeginArea(rect, nodeStyle);

        // Start Region To Detect Popup Selection Changes
        EditorGUI.BeginChangeCheck();

        // Display Popup using the RoomNodeType name values that can be selected from (default to the currently set roomNodeType)
        int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);
        int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());
        roomNodeType = roomNodeTypeList.list[selection];
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(this);
        GUILayout.EndArea();
    }

    /// <summary>
    /// Populate a string array with the room node type to display that can be selected
    /// </summary>
    public string[] GetRoomNodeTypesToDisplay()
    {
        string[] roomArray = new string [roomNodeTypeList.list.Count];

        for (int i = 0; i < roomNodeTypeList.list.Count; i++)
        {
            if (roomNodeTypeList.list[i].displayInNodeGraphEditor)
            {
                roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
            }
        }
        return roomArray;
    }


#endif

    #endregion Editor Code
}
