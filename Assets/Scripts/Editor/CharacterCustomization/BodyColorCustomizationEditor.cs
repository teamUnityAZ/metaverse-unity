using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(BodyColorCustomization))]
public class BodyColorCustomizationEditor : Editor
{
    string eyeballTexturePath;
    string skinTonePath;
    string lipsTexturePath;
    string makeupTexturePath;

    BodyColorCustomization myTarget;

    public override void OnInspectorGUI()
    {
        
        myTarget = (BodyColorCustomization)target;

        EditorGUILayout.Space(10f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("eyeball"), new GUIContent("Eyeball Renderer: "));

        EditorGUILayout.Space(10f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("eyeBallSprites"), new GUIContent("EyeBall Sprites: "));

        EditorGUILayout.Space(10f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("characterBodyParts"), new GUIContent("Character Body Parts: "));

        EditorGUILayout.Space(10f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("head"), new GUIContent("Head Parts: "));

        EditorGUILayout.Space(20f);

        DrawEyeBallTextureUI();

        EditorGUILayout.Space();
        EditorGUILayout.Space(10f);

        GUILayout.BeginVertical("Skin Tone", "window");

        EditorGUILayout.Space(10f);

        DrawSkinToneUI();

        GUILayout.EndVertical();

        EditorGUILayout.Space();

        GUILayout.BeginVertical("Lips Textures", "window");

        EditorGUILayout.Space(10f);

        DrawLipsTextureUI();

        GUILayout.EndVertical();

        EditorGUILayout.Space();

        GUILayout.BeginVertical("Makeup Textures", "window");

        EditorGUILayout.Space(10f);

        DrawMakeupTextureUI();

        GUILayout.EndVertical();

        EditorGUILayout.Space();

        EditorUtility.SetDirty(myTarget);
    }

    void DrawEyeBallTextureUI()
    {
        GUILayout.BeginVertical("Eye Textures", "window");

        //EditorGUILayout.Space(10f);
        //EditorGUILayout.LabelField("Eye Textures");
        EditorGUILayout.Space(10f);

        if (eyeballTexturePath != "" && eyeballTexturePath != "/")
        {
            //EditorGUILayout.LabelField(eyeballTexturePath);
            EditorGUILayout.TextField("Eye Ball Path: ", eyeballTexturePath);
        }

        if (GUILayout.Button("Choose Path"))
        {
            eyeballTexturePath = EditorUtility.OpenFolderPanel("Load Eye Ball Textures", "", "");
            eyeballTexturePath += "/";
        }


        EditorGUILayout.PropertyField(serializedObject.FindProperty("eyeBallTextures"), new GUIContent("Eye Ball Textures: "));

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(10f);

        if (GUILayout.Button("Get All Eyeball Textures"))
        {
            Texture2D[] _textures = GetAtPath<Texture2D>(eyeballTexturePath);

            myTarget.eyeBallTextures = _textures;
        }

        GUILayout.EndVertical();
    }

    void DrawSkinToneUI()
    {
        serializedObject.Update();

        if (skinTonePath != "" && skinTonePath != "/")
        {
            //EditorGUILayout.LabelField(eyeballTexturePath);
            EditorGUILayout.TextField("Skin Tone Path: ", skinTonePath);
        }

        if (GUILayout.Button("Choose Path"))
        {
            skinTonePath = EditorUtility.OpenFolderPanel("Load Skin Tone Properties", "", "");
            skinTonePath += "/";
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(10f);

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bodyTextures"), new GUIContent("Skin Tone Customizations: "));
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(10f);

        if (GUILayout.Button("Get All Skin Textures"))
        {
            myTarget.bodyTextures.Clear();

            string[] folders = Directory.GetDirectories(skinTonePath);

            foreach (string folder in folders)
            {
                string folderPath = folder.ToString() + "/";
                Texture2D[] _texture = GetAtPath<Texture2D>(folderPath);

                Debug.Log(_texture.Length);

                BodyTextures _bodyTextures = new BodyTextures();
                _bodyTextures.baseColor = _texture[0];
                //_bodyTextures.heightMap = _texture[1];
                //_bodyTextures.metallicMap = _texture[2];
                //_bodyTextures.normalMap = _texture[3];
                //_bodyTextures.roughnessMap = _texture[4];

                myTarget.bodyTextures.Add(_bodyTextures);
                
            }

            Sprite[] _textures = GetAtPath<Sprite>(skinTonePath);

            Debug.Log(_textures.Length);

            for (int i = 0; i < _textures.Length; i++)
            {
                myTarget.bodyTextures[i].attributeIcon = _textures[i];
            }

            serializedObject.ApplyModifiedProperties();
        }

        

        serializedObject.ApplyModifiedProperties();
    }

    void DrawLipsTextureUI()
    {
        serializedObject.Update();

        if (lipsTexturePath != "" && lipsTexturePath != "/")
        {
            //EditorGUILayout.LabelField(eyeballTexturePath);
            EditorGUILayout.TextField("Lips Texture Path: ", lipsTexturePath);
        }

        if (GUILayout.Button("Choose Path"))
        {
            lipsTexturePath = EditorUtility.OpenFolderPanel("Load Lips Properties", "", "");
            lipsTexturePath += "/";
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(10f);

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lipsTextures"), new GUIContent("Lips Texture Customizations: "));
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(10f);

        if (GUILayout.Button("Get All Lips Textures"))
        {
            myTarget.lipsTextures.Clear();

            string[] folders = Directory.GetDirectories(lipsTexturePath);

            foreach (string folder in folders)
            {
                string folderPath = folder.ToString() + "/";
                Texture2D[] _texture = GetAtPath<Texture2D>(folderPath);

                Debug.Log(_texture.Length);

                BodyTextures _bodyTextures = new BodyTextures();
                _bodyTextures.baseColor = _texture[0];
                //_bodyTextures.heightMap = _texture[1];
                //_bodyTextures.metallicMap = _texture[2];
                //_bodyTextures.normalMap = _texture[3];
                //_bodyTextures.roughnessMap = _texture[4];

                myTarget.lipsTextures.Add(_bodyTextures);
            }

            Sprite[] _textures = GetAtPath<Sprite>(lipsTexturePath);

            Debug.Log(_textures.Length);

            for (int i = 0; i < _textures.Length; i++)
            {
                myTarget.lipsTextures[i].attributeIcon = _textures[i];
            }

            serializedObject.ApplyModifiedProperties();
        }



        serializedObject.ApplyModifiedProperties();
    }

    void DrawMakeupTextureUI()
    {
        serializedObject.Update();

        if (makeupTexturePath != "" && makeupTexturePath != "/")
        {
            //EditorGUILayout.LabelField(eyeballTexturePath);
            EditorGUILayout.TextField("MakeUp Texture Path: ", makeupTexturePath);
        }

        if (GUILayout.Button("Choose Path"))
        {
            makeupTexturePath = EditorUtility.OpenFolderPanel("Load Makeup Properties", "", "");
            makeupTexturePath += "/";
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(10f);

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("makeupTextures"), new GUIContent("Makeup Texture Customizations: "));
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(10f);

        if (GUILayout.Button("Get All Makeup Textures"))
        {
            myTarget.makeupTextures.Clear();

            string[] folders = Directory.GetDirectories(makeupTexturePath);

            foreach (string folder in folders)
            {
                string folderPath = folder.ToString() + "/";
                Texture2D[] _texture = GetAtPath<Texture2D>(folderPath);

                Debug.Log(_texture.Length);

                BodyTextures _bodyTextures = new BodyTextures();
                _bodyTextures.baseColor = _texture[0];
                //_bodyTextures.heightMap = _texture[1];
                //_bodyTextures.metallicMap = _texture[2];
                //_bodyTextures.normalMap = _texture[3];
                //_bodyTextures.roughnessMap = _texture[4];

                myTarget.makeupTextures.Add(_bodyTextures);
            }

            //Sprite[] _textures = GetAtPath<Sprite>(makeupTexturePath);

            //Debug.Log(_textures.Length);

            //for (int i = 0; i < _textures.Length; i++)
            //{
            //    myTarget.makeupTextures[i].attributeIcon = _textures[i];
            //}

            serializedObject.ApplyModifiedProperties();
        }



        serializedObject.ApplyModifiedProperties();
    }

    public static T[] GetAtPath<T>(string path)
    {

        ArrayList al = new ArrayList();
        //string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
        string[] fileEntries = Directory.GetFiles(path);

        if (path.StartsWith(Application.dataPath))
        {
            path = "Assets" + path.Substring(Application.dataPath.Length);
        }

        foreach (string fileName in fileEntries)
        {
            int index = fileName.LastIndexOf("/");
            string localPath = "Assets/" + path;
            localPath = path;
            localPath.Remove(localPath.Length - 1);
            Debug.Log(localPath);


            if (index > 0)
                localPath += fileName.Substring(index);

            Debug.Log(localPath);

            Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

            if (t != null)
                al.Add(t);
        }
        T[] result = new T[al.Count];
        for (int i = 0; i < al.Count; i++)
            result[i] = (T)al[i];

        return result;
    }

}
