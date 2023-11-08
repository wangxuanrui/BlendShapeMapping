using System;
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
        public float BlendShapeTolerance=0.6f;

        /// <summary>
        /// 数值基数
        /// </summary>
        [Range(0.1f,200)]
        public float RideBase = 1;
        
        
        /// <summary>
        /// MeshRenderer组件
        /// </summary>
        public SkinnedMeshRenderer BlendShapeSkinnedComponent;
        
        /// <summary>
        /// BS骨骼预览
        /// </summary>
        [SerializeField]
        public BlendShapePreview BSPreview;

        
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
        /// <param name="_type"></param>
        /// <param name="value"></param>
        public void SetWeight(ARKitBlendShapeType _type, float value)
        {
            SetWeight(GetBonesInfo((int)_type).SelectKeyIndex,value);
        }
        

        /// <summary>
        /// 播放权重数值
        /// </summary>
        /// <param name="WeightData"></param>
        public void PlayWeightData(float[] WeightData)
        {
            if (WeightData.Length != 56)
            {
                Debug.LogError("该长度小于56");
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
        /// 设置权重值
        /// </summary>
        /// <param name="BlendIndex"></param>
        /// <param name="value"></param>
        private void SetWeight(int BlendIndex,float value)
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
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("{0}//{1}",gameObject.name,e.ToString()));
            }
            
        }
        
    }
    
}
