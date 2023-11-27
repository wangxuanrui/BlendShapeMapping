using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace BlendShapeMapping.Example
{
    
    public class ExampleFace : MonoBehaviour
    {

        private UdpClient UdpServer;
        private int Port = 11111;
        private Thread ReceiveThread;
        private IPEndPoint RemoveIpEndPoint;
        private byte[] RevceBuffer;
        private SynchronizationContext Context;

        private BlendShapeMapperCollections BSMapperCollections;
        private LiveLinkFaceProtocol ProtocolData;

        private GUIStyle FontStyle;
        
        
        void Start()
        {
            BSMapperCollections = GetComponent<BlendShapeMapperCollections>();
            
            ReceiveThread = new Thread(
                            new ThreadStart(StartUDPServer));
            ReceiveThread.IsBackground = true;
            ReceiveThread.Start();
            RemoveIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Context = SynchronizationContext.Current;

            FontStyle = new GUIStyle();
            FontStyle.normal.textColor = Color.white;
            FontStyle.fontSize = 30;
        }
        
        private void OnDestroy()
        {
            UdpServer.Close();
            ReceiveThread.Abort();
        }
        
        private void StartUDPServer()
        {
            
            UdpServer = new UdpClient(Port);
            while (true)
            {
                try
                {
                    RevceBuffer = UdpServer.Receive(ref RemoveIpEndPoint);
                    ProtocolData=LiveLinkFaceProtocol.FromBytes(RevceBuffer);
                    Context.Post(_ =>
                    {
                        //Debug.Log(FaceData.BlendShapeValues.Length);
                        //BSMapperCollections.PlayWeightData(ProtocolData.BlendShapeValues);    
                        BSMapperCollections.PlayWeightData(ProtocolData);
                    },null);
                }
                catch (Exception e)
                {
                    
                }
            }
            
        }
        
        

        private void OnGUI()
        {
            if (ProtocolData!=null)
            {
                GUI.Label(new Rect(new Vector2(0,Screen.height-80),new Vector2(200,30)),$"GUID:{ProtocolData.GUID}\nDeviceName:{ProtocolData.DeviceName}",this.FontStyle);
            }
        }
        
        
        
    }
}
