using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlendShapeMapping {
    
    [Serializable]
    public class BlendShapePreview
    {
        public string[] BlendShapeSkinnedKeys;
        public List<BonesInfo> BonesInfoList=new List<BonesInfo>();
    }
    
    
    [Serializable]
    public class BonesInfo
    {
        
        /// <summary>
        /// ARKit BS类型
        /// </summary>
        public LiveLinkFaceBlendShapeType ARKitBSType;
        /// <summary>
        /// 当前模型选择的Keys下标
        /// </summary>
        public int SelectKeyIndex=0;

        public BonesInfo(int BlendShapeType)
        {
            this.ARKitBSType = (LiveLinkFaceBlendShapeType) BlendShapeType;
        }
        
        public BonesInfo(int BlendShapeType,int KeyIndex)
        {
            this.ARKitBSType = (LiveLinkFaceBlendShapeType) BlendShapeType;
            this.SelectKeyIndex = KeyIndex;
        }
        
    }
    
    
}
