using System;
using System.IO;
using System.Text;


namespace BlendShapeMapping
{
	
	public class LiveLinkFaceProtocol
	{

		#region 属性

		/// <summary>
		/// 数据包(帧)版本
		/// </summary>
		public byte PackerVersion;

		/// <summary>
		/// GUID
		/// </summary>
		public string GUID;

		/// <summary>
		/// 设备名称
		/// </summary>
		public string DeviceName;

		/// <summary>
		/// 帧索引
		/// </summary>
		public int FrameNumbser;

		/// <summary>
		/// ???
		/// </summary>
		public float SubFrame;

		/// <summary>
		/// 帧率分子
		/// </summary>
		public int FPSNumerator;

		/// <summary>
		/// 帧率分母
		/// </summary>
		public int FPSDenominator;

		/// <summary>
		/// Blend数量
		/// </summary>
		public int BlendShapeCount;

		/// <summary>
		/// Blend值
		/// </summary>
		public float[] BlendShapeValues;

		protected static LiveLinkFaceProtocol DataProtocol = new LiveLinkFaceProtocol();


		#endregion


		public static LiveLinkFaceProtocol FromBytes(byte[] buffer)
		{

			using (MemoryStream memoryStream = new MemoryStream(buffer))
			{
				BinaryReader binaryReader = new BinaryReader(memoryStream);
				try
				{
					
					//读取包头 1个字节
					DataProtocol.PackerVersion = binaryReader.ReadByte();
					//GUID长度
					int GUIDLength = BitConverter.ToInt32(ArrayReverse(binaryReader.ReadBytes(4)), 0);
					//Debug.Log(GUIDLength);
					DataProtocol.GUID =Encoding.UTF8.GetString(binaryReader.ReadBytes(GUIDLength));
					//设备名称
					int DeviceLength = BitConverter.ToInt32(ArrayReverse(binaryReader.ReadBytes(4)), 0);
					//Debug.Log(DeviceLength);
					DataProtocol.DeviceName = Encoding.UTF8.GetString(binaryReader.ReadBytes(DeviceLength));
					//Debug.Log(DeviceName);
					//FrameTime
					
					//?
					DataProtocol.FrameNumbser = BitConverter.ToInt32(ArrayReverse(binaryReader.ReadBytes(4)), 0);
					//?
					DataProtocol.SubFrame = BitConverter.ToSingle(ArrayReverse(binaryReader.ReadBytes(4)), 0);
					//?
					DataProtocol.FPSNumerator = BitConverter.ToInt32(ArrayReverse(binaryReader.ReadBytes(4)), 0);
					//?
					DataProtocol.FPSDenominator = BitConverter.ToInt32(ArrayReverse(binaryReader.ReadBytes(4)), 0);
					
					DataProtocol.BlendShapeCount = 0;
					//获取Blend数量、如果没有监测到人脸，则报错，丢弃该数据包
					int BlendShapeCount = binaryReader.ReadByte();
					DataProtocol.BlendShapeCount = BlendShapeCount;
					//读取所有的Blend值，并反转
					byte[] BlendShapeBytes = ArrayReverse(binaryReader.ReadBytes(BlendShapeCount * 4));
					DataProtocol.BlendShapeValues = new float[BlendShapeCount];
					//每4个为一个float
					int VIndex = BlendShapeCount - 1;
					for (int i = 0; i < BlendShapeBytes.Length; i += 4)
					{
						DataProtocol.BlendShapeValues[VIndex--] = BitConverter.ToSingle(BlendShapeBytes, i);
					}
					
				}
				catch (Exception e)
				{
					//Debug.LogError(e.ToString());
					//错误，数据丢弃
				}

				//关闭二进制读取器
				binaryReader.Close();
				memoryStream.Close();
			}

			return DataProtocol;
		}

		
		private static byte[] ArrayReverse(byte[] arrayValues)
		{
			Array.Reverse(arrayValues);
			return arrayValues;
		}
		

	}
	
	

}

