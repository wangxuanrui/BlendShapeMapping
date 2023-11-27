using System;
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
		private SerializedProperty m_SkinnedAdditionalComponents;
		private SerializedProperty m_BlendShapeTolerance;
		private SerializedProperty m_RideBase;
		private SerializedProperty m_BlendShapeSourceBindingType;
		private SerializedProperty m_SourceGUID;
		private SerializedProperty m_SourceDeviceName;
		
		private BlendShapeMapper Component;

		private bool ShowBSPreview = false;
		private string ShowBSPreviewStatus = "MapperPreview";
		
		private void OnEnable()
		{
			m_BlendShapeSkinnedComponent = serializedObject.FindProperty("BlendShapeSkinnedComponent");
			m_SkinnedAdditionalComponents=serializedObject.FindProperty("SkinnedAdditionalComponents");
			m_BlendShapeTolerance = serializedObject.FindProperty("BlendShapeTolerance");
			m_RideBase = serializedObject.FindProperty("RideBase");
			m_BlendShapeSourceBindingType = serializedObject.FindProperty("BlendShapeSourceBindingType");
			m_SourceGUID = serializedObject.FindProperty("GUID");
			m_SourceDeviceName = serializedObject.FindProperty("DeviceName");
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
				EditorGUILayout.PropertyField(m_SkinnedAdditionalComponents);
				
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
					SourceBindingGUI();
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
								BSType = LiveLinkFaceBlendShapeType.MouthUpperUpLeft,
								Value = 1f
				});
				TMPData.Add(new BSData()
				{
								BSType = LiveLinkFaceBlendShapeType.MouthUpperUpRight,
								Value = 1f
				});
				TMPData.Add(new BSData()
				{
								BSType = LiveLinkFaceBlendShapeType.MouthLowerDownLeft,
								Value = 1f
				});
				TMPData.Add(new BSData()
				{
								BSType = LiveLinkFaceBlendShapeType.MouthLowerDownRight,
								Value = 1f
				});
				
				Component.SetWeight(CreateMapperData(TMPData));
			}

			if (GUILayout.Button("嘟嘟嘴", GUILayout.Width(100)))
			{
				List<BSData> TMPData = new List<BSData>();
				TMPData.Add(new BSData()
				{
								BSType = LiveLinkFaceBlendShapeType.MouthPucker,
								Value = 1
				});
				Component.SetWeight(CreateMapperData(TMPData));
			}


			if (GUILayout.Button("张嘴", GUILayout.Width(100)))
			{
				List<BSData> TMPData = new List<BSData>();
				TMPData.Add(new BSData()
				{
								BSType = LiveLinkFaceBlendShapeType.MouthStretchLeft,
								Value = 0.1f
				});
				TMPData.Add(new BSData()
				{
								BSType = LiveLinkFaceBlendShapeType.MouthStretchLeft,
								Value = 0.1f
				});
				TMPData.Add(new BSData()
				{
								BSType = LiveLinkFaceBlendShapeType.JawOpen,
								Value = 1
				});
				Component.SetWeight(CreateMapperData(TMPData));
			}

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("瞪眼", GUILayout.Width(100)))
			{
				List<BSData> TMPData = new List<BSData>();
				TMPData.Add(new BSData()
				{
								BSType = LiveLinkFaceBlendShapeType.EyeWideLeft,
								Value = 1f
				});
				TMPData.Add(new BSData()
				{
								BSType = LiveLinkFaceBlendShapeType.EyeWideRight,
								Value = 1f
				});

				Component.SetWeight(CreateMapperData(TMPData));
			}

			if (GUILayout.Button("挑眉", GUILayout.Width(100)))
			{
				List<BSData> TMPData = new List<BSData>();
				TMPData.Add(new BSData()
				{
								BSType = LiveLinkFaceBlendShapeType.BrowInnerUp,
								Value = 1f
				});
				TMPData.Add(new BSData()
				{
								BSType = LiveLinkFaceBlendShapeType.EyeWideRight,
								Value = 1f
				});
				Component.SetWeight(CreateMapperData(TMPData));
			}

			if (GUILayout.Button("闭眼", GUILayout.Width(100)))
			{
				List<BSData> TMPData = new List<BSData>();
				TMPData.Add(new BSData()
				{
								BSType = LiveLinkFaceBlendShapeType.EyeBlinkLeft,
								Value = 1f
				});
				TMPData.Add(new BSData()
				{
								BSType = LiveLinkFaceBlendShapeType.EyeBlinkRight,
								Value = 1f
				});
				Component.SetWeight(CreateMapperData(TMPData));
			}

			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}

		
		private void SourceBindingGUI()
		{
			EditorGUILayout.BeginVertical(GUI.skin.box);
			EditorGUILayout.PropertyField(m_BlendShapeSourceBindingType);
			
			switch ((SourceBindingType)m_BlendShapeSourceBindingType.enumValueIndex)
			{
				case SourceBindingType.None:
					break;
				case SourceBindingType.DeviceName:
					EditorGUILayout.PropertyField(m_SourceDeviceName);
					break;
				case SourceBindingType.GUID:
					EditorGUILayout.PropertyField(m_SourceGUID);
					break;
			}
			
			EditorGUILayout.EndVertical();
		}
		
		
		/// <summary>
		/// 创建映射
		/// </summary>
		private void CreateMapper()
		{
			if (Component == null)
				return;
			//获取模型的BlendShaped Keys
			Component.BSPreview.BlendShapeSkinnedKeys =
							BlendShapeUtility.GetBlendShapeNames(Component.BlendShapeSkinnedComponent.sharedMesh);
			//添加一个无效选择项
			var tempList = Component.BSPreview.BlendShapeSkinnedKeys.ToList();
			tempList.Add("Valid");
			Component.BSPreview.BlendShapeSkinnedKeys = tempList.ToArray();
			//创建LiveLinkFaceBlendShape数量
			for (int i = 0; i < Enum.GetValues(typeof(LiveLinkFaceBlendShapeType)).Length-1; i++)
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
					GUILayout.Label((i+1).ToString(),GUILayout.Width(30));
					GUILayout.Label(info.ARKitBSType.ToString(), GUILayout.Width(150));
					
					info.SelectKeyIndex = EditorGUILayout.Popup("", info.SelectKeyIndex,
									Component.BSPreview.BlendShapeSkinnedKeys,GUILayout.Width(200));
					
					if (info.SelectKeyIndex==Component.BSPreview.BlendShapeSkinnedKeys.Length-1)
					{
						//匹配映射
						if (GUILayout.Button("Match", GUILayout.Width(60)))
						{
							int findTmp= BlendShapeUtility.FindMatch(info.ARKitBSType.ToString(),
											Component.BSPreview.BlendShapeSkinnedKeys,
											m_BlendShapeTolerance.floatValue);
							info.SelectKeyIndex = findTmp == -1
											? Component.BSPreview.BlendShapeSkinnedKeys.Length - 1
											: findTmp;
						}
					}
					else
					{
						//解除映射
						if (GUILayout.Button("X",GUILayout.Width(30)))
						{
							Component.BSPreview.BonesInfoList[i].SelectKeyIndex =
											Component.BSPreview.BlendShapeSkinnedKeys.Length - 1;
						}
					}
					
					GUILayout.EndHorizontal();
					
				}
			}
			
		}
		
		
		private float[] CreateMapperData(List<BSData> _bsDatas)
		{
			float[] BSBufferData = new float[Component.BSPreview.BonesInfoList.Count];
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
		public LiveLinkFaceBlendShapeType BSType;
		public float Value;
	}
	
	
	
}