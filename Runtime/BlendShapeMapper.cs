using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlendShapeMapping {
    
    
    /// <summary>
    /// BlendShape映射对象
    /// </summary>
    public class BlendShapeMapper :MonoBehaviour
    {
        
        /// <summary>
        /// 匹配精度
        /// </summary>
        [Range(0,1)]
        [SerializeField]
        private float BlendShapeTolerance=0.6f;

        /// <summary>
        /// 数值基数
        /// </summary>
        [Range(0.1f,200)]
        [SerializeField]
        private float RideBase = 100;
        
        /// <summary>
        /// MeshRenderer组件
        /// </summary>
        public SkinnedMeshRenderer BlendShapeSkinnedComponent;

        /// <summary>
        /// MeshRenderer 附加组件
        /// 作用：需要和主SkinnedMesh下的BlendShape数量保持一致
        /// </summary>
        [SerializeField]
        private SkinnedMeshRenderer[] SkinnedAdditionalComponents;

        /// <summary>
        /// MeshRenderer 附加组件的BlendShape是否匹配
        /// </summary>
        private bool SkinnedAdditionalBSCountMatch=true;
        
        /// <summary>
        /// BS骨骼预览
        /// </summary>
        [SerializeField]
        public BlendShapePreview BSPreview;

        /// <summary>
        /// 空闲时刻
        /// </summary>
        public Action EventIdelTime;
        
        /// <summary>
        /// 来源绑定类型
        /// 默认：不绑定
        /// </summary>
        [SerializeField]
        public SourceBindingType BlendShapeSourceBindingType=SourceBindingType.None;
        
        /// <summary>
        /// GUID
        /// </summary>
        public string GUID;
        /// <summary>
        /// DeviceName
        /// </summary>
        public string DeviceName;

        void Start()
        {

            if (BlendShapeSkinnedComponent!=null)
            {
                for (int i = 0; i < SkinnedAdditionalComponents.Length; i++)
                {
                    if (SkinnedAdditionalComponents[i]!=null && SkinnedAdditionalComponents[i].sharedMesh.blendShapeCount!=BlendShapeSkinnedComponent.sharedMesh.blendShapeCount)
                    {
                        SkinnedAdditionalBSCountMatch = false;
                        break;
                    }
                }
            }
            
        }
        
        /// <summary>
        /// 获取骨骼信息
        /// </summary>
        /// <param name="index"></param>
        public BonesInfo GetBonesInfo(int index)
        {
            if (BSPreview==null)
            {
                return null;
            }
            
            if (index<BSPreview.BonesInfoList.Count)
            {
                return BSPreview.BonesInfoList[index];
            }
            else
            {
                return null;
            }
        }
        
        
        /// <summary>
        /// 设置权重值
        /// </summary>
        /// <param name="BlendIndex"></param>
        /// <param name="value"></param>
        public void SetWeight(int BlendIndex,float value)
        {
            if (BlendShapeSkinnedComponent==null)
                return;
            //超出范围不执行
            if (BlendIndex>=BlendShapeSkinnedComponent.sharedMesh.blendShapeCount)
                return;
            try
            {
                
                value = Mathf.Clamp(value * this.RideBase, 0, 100);
                BlendShapeSkinnedComponent.SetBlendShapeWeight(BlendIndex,value);

                if (this.SkinnedAdditionalBSCountMatch)
                {
                    for (int i = 0; i < SkinnedAdditionalComponents.Length; i++)
                    {
                        if (SkinnedAdditionalComponents[i]!=null)
                        {
                            SkinnedAdditionalComponents[i].SetBlendShapeWeight(BlendIndex,value);    
                        }
                    }
                }
                
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("{0}//{1}",gameObject.name,e.ToString()));
            }
            
        }

        
        /// <summary>
        /// 设置权重
        /// </summary>
        /// <param name="_blendShapeType"></param>
        /// <param name="value"></param>
        public void SetWeight(LiveLinkFaceBlendShapeType _blendShapeType,float value)
        {
            if (BlendShapeSkinnedComponent==null)
                return;
            try
            {
                SetWeight((int)_blendShapeType,value);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("{0}//{1}",gameObject.name,e.ToString()));
            }
        }
        
        
        public void SetWeight(LiveLinkFaceProtocol Data)
        {
            if (Data.BlendShapeCount<=0)
                return;
            
            switch (BlendShapeSourceBindingType)
            {
                case SourceBindingType.None:
                    SetWeight(Data.BlendShapeValues);
                    break;
                case SourceBindingType.DeviceName:
                    if (Data.DeviceName==DeviceName)
                    {
                        SetWeight(Data.BlendShapeValues);
                    }
                    break;
                case SourceBindingType.GUID:
                    if (Data.GUID==GUID)
                    {
                        SetWeight(Data.BlendShapeValues);
                    }
                    break;
            }
        }
        
        
        
        /// <summary>
        /// 播放权重数值
        /// </summary>
        /// <param name="WeightData"></param>
        public void SetWeight(float[] WeightData)
        {
            if (WeightData.Length==0 || WeightData==null)
            {
                Debug.LogError("数据错误");
                return;
            }
            
            try
            {
                for (int i = 0; i < WeightData.Length; i++)
                {
                    SetWeight(GetBonesInfo(i).SelectKeyIndex,WeightData[i]);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            
        }
        
        
        /// <summary>
        /// 重置权重
        /// </summary>
        public void ResetWeight()
        {
            for (int i = 0; i < this.BlendShapeSkinnedComponent.sharedMesh.blendShapeCount; i++)
            {
                SetWeight(i,0);
            }
        }
        
        
        /// <summary>
        /// 生成MapperData
        /// </summary>
        /// <param name="_bsDatas"></param>
        /// <returns></returns>
        public float[] GenerateMapperData(List<BSData> _bsDatas)
        {
            float[] BSBufferData = new float[this.BSPreview.BonesInfoList.Count];
            for (int i = 0; i < this.BSPreview.BonesInfoList.Count; i++)
            {
                var item = this.BSPreview.BonesInfoList[i];

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
        
        
        public class BSData
        {
            public LiveLinkFaceBlendShapeType BSType;
            public float Value;
        }
        

        [ContextMenu("Debug BlendShapeNames")]
        private void DebugBlendShapeNames()
        {
            if (BlendShapeSkinnedComponent==null)
            {
                Debug.LogError("BlendShapeSkinnedComponent 组件不存在！");
                return;
            }
            string keys = string.Empty;
            foreach (var key in BlendShapeUtility.GetBlendShapeNames(BlendShapeSkinnedComponent.sharedMesh))
            {
                keys += key + "\r\n";
            }
            Debug.Log(keys);
        }
        
        
        
    }
    
}
