/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Soomla
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	/// <summary>
	/// This class holds the store's configurations.
	/// </summary>
	public class SoomlaEditorScript : ScriptableObject
	{
		public static string AND_PUB_KEY_DEFAULT = "YOUR GOOGLE PLAY PUBLIC KEY";
		public static string ONLY_ONCE_DEFAULT = "SET ONLY ONCE";

		const string soomSettingsAssetName = "SoomlaEditorScript";
		const string soomSettingsPath = "Soomla/Resources";
		const string soomSettingsAssetExtension = ".asset";

		private static SoomlaEditorScript instance;

		public static SoomlaEditorScript Instance
		{
			get
			{
				if (instance == null)
				{
          instance = Resources.Load(soomSettingsAssetName) as SoomlaEditorScript;

					if (instance == null)
					{
            // If not found, autocreate the asset object.
						instance = CreateInstance<SoomlaEditorScript>();
#if UNITY_EDITOR
						string properPath = Path.Combine(Application.dataPath, soomSettingsPath);
						if (!Directory.Exists(properPath))
						{
							AssetDatabase.CreateFolder("Assets/Soomla", "Resources");
						}

						string fullPath = Path.Combine(Path.Combine("Assets", soomSettingsPath),
						                               soomSettingsAssetName + soomSettingsAssetExtension);
						AssetDatabase.CreateAsset(instance, fullPath);
#endif
					}
				}
				return instance;
			}
		}

	#if UNITY_EDITOR

		private static List<ISoomlaSettings> mSoomlaSettings = new List<ISoomlaSettings>();
		public static void addSettings(ISoomlaSettings spp) {
			mSoomlaSettings.Add(spp);
		}

		public static void OnEnable() {
			foreach(ISoomlaSettings settings in mSoomlaSettings) {
				settings.OnEnable();
			}
			GetVersion();
		}

		public static void OnInspectorGUI() {
			foreach(ISoomlaSettings settings in mSoomlaSettings) {
				settings.OnSoomlaGUI();
				EditorGUILayout.Space();
			}
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			foreach(ISoomlaSettings settings in mSoomlaSettings) {
				settings.OnModuleGUI();
			}
			EditorGUILayout.Space();
			foreach(ISoomlaSettings settings in mSoomlaSettings) {
				settings.OnInfoGUI();
				EditorGUILayout.Space();
			}
		}

		[MenuItem("Window/Soomla/Edit Settings")]
	    public static void Edit()
	    {
	        Selection.activeObject = Instance;
	    }

		[MenuItem("Window/Soomla/Delete Soomla")]
		public static void Delete()
		{
			if (EditorUtility.DisplayDialog ("Confirmation", "Are you sure you want to delete SOOMLA?", "Yes", "No")) {
				string[] allPackages = System.IO.Directory.GetFiles("Assets/Soomla/", "*_file_list");;
				foreach(string filename in allPackages){
                    RemoveModule(filename);
				}
				AssetDatabase.Refresh();
			}

		}

		[MenuItem("Window/Soomla/Framework Page")]
	    public static void OpenFramework()
	    {
	        string url = "https://www.github.com/soomla/unity3d-store";
	        Application.OpenURL(url);
	    }

		[MenuItem("Window/Soomla/Report an issue")]
	    public static void OpenIssue()
	    {
			string url = "https://answers.soom.la";
	        Application.OpenURL(url);
	    }

	#endif

	    public static void DirtyEditor()
	    {
	#if UNITY_EDITOR
	        EditorUtility.SetDirty(Instance);
	#endif
	    }

		[SerializeField]
		public ObjectDictionary SoomlaSettings = new ObjectDictionary();

		public void setSettingsValue(string key, string value) {
			SoomlaSettings[key] = value;
		}




		/** SOOMLA Core UI **/
#if UNITY_EDITOR
		public static GUILayoutOption FieldHeight = GUILayout.Height(16);
		public static GUILayoutOption FieldWidth = GUILayout.Width(120);
		public static GUILayoutOption SpaceWidth = GUILayout.Width(24);
		public static GUIContent EmptyContent = new GUIContent("");

		public static JSONObject versionJson;
		public static WWW www;
		public static string status;

		public static void GetVersion(){
			www = new WWW ("http://library.soom.la/fetch/info");
		}

        public static void RemoveModule(string fileList)
        {
            string line;
            List<string> folders = new List<string>();
            string filename = "Assets/Soomla/" + fileList;
            StreamReader reader = new StreamReader(filename);
            do
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    FileUtil.DeleteFileOrDirectory(line);
                    string folderPath = Path.GetDirectoryName(line);
                    do
                    {
                        if (!folders.Contains(folderPath))
                        {
                            folders.Add(folderPath);
                        }
                        folderPath = Path.GetDirectoryName(folderPath);
                    } while (folderPath != "");
                }
            } while (line != null);
            reader.Close();
            FileUtil.DeleteFileOrDirectory(filename);
            AssetDatabase.Refresh();
        }

        public static void LatestVersionField(string moduleId, string currentVersion, string versionPrompt, string downloadLink)
		{
			string latestVersion = "";
			if (versionJson == null) {
				status = "Checking version...";
				if (www.isDone) {
					versionJson = new JSONObject (www.text);
				}
			} else {
				latestVersion = versionJson.GetField (moduleId).GetField ("latest").str;
			}
			GUIStyle style = new GUIStyle (GUI.skin.label);
			if (currentVersion != latestVersion) {
				status = versionPrompt;
				style.normal.textColor = Color.blue;
			} else {
				status = "";
			}
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button (status, style, GUILayout.Width (170), FieldHeight)) {
				if (currentVersion != latestVersion && latestVersion != "") {
					Application.OpenURL(downloadLink);
				}
			}			
			EditorGUILayout.EndHorizontal ();
		}

		public static void RemoveSoomlaModuleButton(GUIContent label, string value, string fileList, string moduleName)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(label, GUILayout.Width(140), FieldHeight);
			EditorGUILayout.SelectableLabel(value, GUILayout.Width(40), FieldHeight);
			
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.normal.textColor = Color.blue;
			if (GUILayout.Button("Remove", style, GUILayout.Width(60), FieldHeight)) {
                if (EditorUtility.DisplayDialog("Confirmation", "Are you sure you want to delete? "+ moduleName, "Yes", "No"))
                {
                    RemoveModule(fileList);
                }
			}
			EditorGUILayout.EndHorizontal();
		}

		public static void SelectableLabelField(GUIContent label, string value)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(label, GUILayout.Width(140), FieldHeight);
			EditorGUILayout.SelectableLabel(value, FieldHeight);
			EditorGUILayout.EndHorizontal();
		}

#endif
	}
}
