using System;
using UnityEngine;

namespace BlendShapeMapping {
    
    
    /// <summary>
    /// BlendShape映射集合
    /// </summary>
    public class BlendShapeMapperCollections:MonoBehaviour
    {
        
        [SerializeField]
        private BlendShapeMapper[] BlendShapeMapperArray;

        public BlendShapeMapper[] GetBlendShapeMapperArray
        {
            get
            {
                return BlendShapeMapperArray;
            }
        }

        public int BlendShapeMapperCount
        {
            get
            {
                return BlendShapeMapperArray.Length;
            }
        }

        public BlendShapeMapper GetGetBlendShapeMapper(int index)
        {
            if (index<0 || index>=BlendShapeMapperArray.Length)
            {
                return null;
            }
            return BlendShapeMapperArray[index];
        }
        
        
        public void PlayWeightData(float[] WeightData)
        {
            if (WeightData==null)
                return;
            
            try
            {
                for (int i = 0; i < BlendShapeMapperArray.Length; i++)
                {
                    BlendShapeMapperArray[i].SetWeight(WeightData);
                }
            }
            catch (Exception e)
            {
               Debug.LogError(e.ToString());
            }
        }
        
        
        public void PlayWeightData(LiveLinkFaceProtocol Data)
        {
            if (Data==null)
                return;
            
            try
            {
                for (int i = 0; i < BlendShapeMapperArray.Length; i++)
                {
                    BlendShapeMapperArray[i].SetWeight(Data);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
        
        
        
    }
}
