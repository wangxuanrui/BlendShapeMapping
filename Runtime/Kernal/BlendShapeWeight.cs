using System;
using UnityEngine;


namespace BlendShapeMapping {
    
    
    [RequireComponent(typeof(BlendShapeMapper))]
    public class BlendShapeWeight : MonoBehaviour
    {
        
        /// <summary>
        /// BlendShape映射对象
        /// </summary>
        private BlendShapeMapper _BlendShapeMapperComponent;

        /// <summary>
        /// BlendShape映射对象
        /// </summary>
        public BlendShapeMapper BlendShapeMapperComponent
        {
            get
            {
                if (_BlendShapeMapperComponent==null)
                {
                    _BlendShapeMapperComponent = GetComponent<BlendShapeMapper>();
                }

                return _BlendShapeMapperComponent;
            }
            private set { }
        }


        private void Start()
        {
            
        }

        /// <summary>
        /// 播放权重数据
        /// </summary>
        /// <param name="AIData"></param>
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
                     //BlendShapeMapperComponent.BSPreview.BonesInfoList[i].SelectKeyIndex
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
           
        }
        
        
    }
}
