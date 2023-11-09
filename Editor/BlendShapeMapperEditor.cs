using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace BlendShapeMapping
{
	
	[CustomEditor(typeof(BlendShapeMapper))]
	public class BlendShapeMapperEditor : Editor
	{
		private SerializedProperty m_BlendShapeSkinnedComponent;
		private SerializedProperty m_BlendShapeTolerance;
		private SerializedProperty m_RideBase;

		private BlendShapeMapper Component;

		private bool ShowBSPreview = false;
		private string ShowBSPreviewStatus = "MapperPreview";

		private void OnEnable()
		{
			m_BlendShapeSkinnedComponent = serializedObject.FindProperty("BlendShapeSkinnedComponent");
			m_BlendShapeTolerance = serializedObject.FindProperty("BlendShapeTolerance");
			m_RideBase = serializedObject.FindProperty("RideBase");
		}


		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			Component = target as BlendShapeMapper;

			EditorGUILayout.PropertyField(m_BlendShapeTolerance);
			EditorGUILayout.PropertyField(m_RideBase);
			EditorGUILayout.PropertyField(m_BlendShapeSkinnedComponent);


			if (Component.BlendShapeSkinnedComponent != null)
			{
				if (Component.BSPreview.BonesInfoList.Count <= 0)
				{
					//创建映射
					if (GUILayout.Button("CreateMapper"))
					{
						CreateMapper();
					}
				}
				else
				{
					BlendShapeTestGUI();
					BSPreviewGUI();
					//清除映射
					if (GUILayout.Button("RemoveMapper"))
					{
						Component.BSPreview.BonesInfoList.Clear();
					}
				}
			}


			if (GUI.changed)
			{
				serializedObject.ApplyModifiedProperties();
				EditorUtility.SetDirty(target);
			}
		}


		private void BlendShapeTestGUI()
		{
			
			GUILayout.BeginVertical(GUI.skin.textField);

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Reset", GUILayout.Width(310)))
			{
				Component.ResetWeight();
			}

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("呲牙", GUILayout.Width(100)))
			{
				List<BSData> TMPData = new List<BSData>();
				
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.MouthUpperUpLeft,
								Value = 1f
				});
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.MouthUpperUpRight,
								Value = 1f
				});
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.MouthLowerDownLeft,
								Value = 1f
				});
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.MouthLowerDownRight,
								Value = 1f
				});
				Component.PlayWeightData(CreateMapperData(TMPData));
			}

			if (GUILayout.Button("嘟嘟嘴", GUILayout.Width(100)))
			{
				List<BSData> TMPData = new List<BSData>();
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.MouthPucker,
								Value = 1
				});
				// TMPData.Add(new BSData()
				// {
				//                 BSType = ARKitBlendShapeType.MouthFunnel,
				//                 Value = 0.45f
				// });
				// TMPData.Add(new BSData()
				// {
				//                 BSType = ARKitBlendShapeType.MouthStretchLeft,
				//                 Value = 1
				// });
				// TMPData.Add(new BSData()
				// {
				//                 BSType = ARKitBlendShapeType.MouthStretchRight,
				//                 Value = 1
				// });
				Component.PlayWeightData(CreateMapperData(TMPData));
			}

			if (GUILayout.Button("张嘴", GUILayout.Width(100)))
			{
				List<BSData> TMPData = new List<BSData>();
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.MouthStretchLeft,
								Value = 0.1f
				});
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.MouthStretchLeft,
								Value = 0.1f
				});
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.JawOpen,
								Value = 1
				});
				Component.PlayWeightData(CreateMapperData(TMPData));
			}

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("瞪眼", GUILayout.Width(100)))
			{
				List<BSData> TMPData = new List<BSData>();
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.EyeWideLeft,
								Value = 1f
				});
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.EyeWideRight,
								Value = 1f
				});

				Component.PlayWeightData(CreateMapperData(TMPData));
			}

			if (GUILayout.Button("挑眉", GUILayout.Width(100)))
			{
				List<BSData> TMPData = new List<BSData>();
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.BrowInnerUp,
								Value = 1f
				});
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.EyeWideRight,
								Value = 1f
				});
				Component.PlayWeightData(CreateMapperData(TMPData));
			}

			if (GUILayout.Button("闭眼", GUILayout.Width(100)))
			{
				List<BSData> TMPData = new List<BSData>();
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.EyeBlinkLeft,
								Value = 1f
				});
				TMPData.Add(new BSData()
				{
								BSType = ARKitBlendShapeType.EyeBlinkRight,
								Value = 1f
				});
				Component.PlayWeightData(CreateMapperData(TMPData));
			}

			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}


		/// <summary>
		/// 创建映射
		/// </summary>
		private void CreateMapper()
		{
			if (Component == null)
				return;
			//获取BlendShaped Keys
			Component.BSPreview.BlendShapeSkinnedKeys =
							BlendShapeUtility.GetBlendShapeNames(Component.BlendShapeSkinnedComponent.sharedMesh);
			var tempList = Component.BSPreview.BlendShapeSkinnedKeys.ToList();
			tempList.Add("Valid");
			Component.BSPreview.BlendShapeSkinnedKeys = tempList.ToArray();
			//创建56个
			for (int i = 0; i < 56; i++)
			{
				BonesInfo Bone = new BonesInfo(i);
				int index = BlendShapeUtility.FindMatch(Bone.ARKitBSType.ToString(),
								Component.BSPreview.BlendShapeSkinnedKeys, m_BlendShapeTolerance.floatValue);
				//无效
				if (index == -1)
				{
					Bone.SelectKeyIndex = Component.BSPreview.BlendShapeSkinnedKeys.Length - 1;
				}
				else
				{
					Bone.SelectKeyIndex = index;
				}

				Component.BSPreview.BonesInfoList.Add(Bone);
			}
		}


		private void BSPreviewGUI()
		{
			ShowBSPreview = EditorGUILayout.Foldout(ShowBSPreview, ShowBSPreviewStatus);
			if (ShowBSPreview)
			{
				for (int i = 0; i < Component.BSPreview.BonesInfoList.Count; i++)
				{
					BonesInfo info = Component.BSPreview.BonesInfoList[i];

					GUILayout.BeginHorizontal(GUI.skin.box);
					GUILayout.Label(i.ToString());
					GUILayout.Label(info.ARKitBSType.ToString());
					info.SelectKeyIndex = EditorGUILayout.Popup("", info.SelectKeyIndex,
									Component.BSPreview.BlendShapeSkinnedKeys);
					GUILayout.EndHorizontal();
				}
			}
		}


		private float[] CreateMapperData(List<BSData> _bsDatas)
		{
			float[] BSBufferData = new float[56];
			for (int i = 0; i < Component.BSPreview.BonesInfoList.Count; i++)
			{
				var item = Component.BSPreview.BonesInfoList[i];

				BSData[] FindBSData = _bsDatas.Where((v) => v.BSType == item.ARKitBSType).ToArray();

				if (FindBSData.Length > 0)
				{
					BSBufferData[i] = FindBSData[0].Value;
				}
				else
				{
					BSBufferData[i] = 0;
				}
			}

			return BSBufferData;
		}
	}


	public class BSData
	{
		public ARKitBlendShapeType BSType;
		public float Value;
	}
}