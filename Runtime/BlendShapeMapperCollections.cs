using UnityEngine;

namespace BlendShapeMapping {
    
    
    /// <summary>
    /// BlendShape映射集合
    /// </summary>
    public class BlendShapeMapperCollections:MonoBehaviour
    {
        
        public BlendShapeMapper[] BlendShapeMapperArray;
        
        /// <summary>
        /// 设置权重
        /// </summary>
        /// <param name="BlendIndex"></param>
        /// <param name="value"></param>
        public void PlayWeightData(float[] WeightData)
        {
            for (int i = 0; i < BlendShapeMapperArray.Length; i++)
            {
                BlendShapeMapperArray[i].PlayWeightData(WeightData);
            }
        }
        
        
        
    }
}
