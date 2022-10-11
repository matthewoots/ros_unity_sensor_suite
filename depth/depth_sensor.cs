/*
* depth_sensor.cs
*
* ---------------------------------------------------------------------
* Copyright (C) 2022 Matthew (matthewoots at gmail.com)
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU General Public License
*  as published by the Free Software Foundation; either version 2
*  of the License, or (at your option) any later version.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
* ---------------------------------------------------------------------
*/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using ros_sensor_image = RosMessageTypes.Sensor.ImageMsg;

using UnityEngine;

namespace sensors_suite {

    public class depth_sensor : MonoBehaviour {
    
        private int cam_width, cam_height;
        private Material material;
        private Camera cam;
        private RenderTexture cam_rt;
        private RenderTexture currentRT;
        private Texture2D tex2d;
        // private Color data;

        public Byte[] data32;
        public ros_sensor_image msg;

        // Creates a private material used to the effect
        void Awake ()
        {
            material = new Material(Shader.Find("Hidden/depth"));
            cam_width = GetComponent<Camera>().pixelWidth;
            cam_height = GetComponent<Camera>().pixelHeight;

            cam = GetComponent<Camera>();
            cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;

            currentRT = new RenderTexture(cam_width, cam_height, 0);

            cam_rt = new RenderTexture(cam_width, cam_height, 32, RenderTextureFormat.RFloat);

            cam.targetTexture = cam_rt;
            currentRT = RenderTexture.active;
        }

        void Update()
        {
            // We destroy the render texture so and reset the values again
            Destroy(cam_rt);  
                      
            cam_rt = new RenderTexture(cam_width, cam_height, 32, RenderTextureFormat.RFloat);
            cam.targetTexture = cam_rt;
            RenderTexture.active = cam.targetTexture;
        }

        void OnPostRender()
        {   
            // Image starts from bottom left corner

            // Test right bottom Corner
            // Debug.Log("RFloat sample @ " + BitConverter.ToSingle( data32, cam_width * 4 - 4));
            // Test left bottom Corner
            // Debug.Log("RFloat sample @ " + BitConverter.ToSingle( data32, 0)); 

            msg = CopyData(data32, "RGB");
        }
        
        // Postprocess the image
        void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit (source, destination, material);
            
            // Make a new texture and read the active Render Texture into it.
            tex2d = new Texture2D(cam_width, cam_height, TextureFormat.RFloat, false);
            tex2d.ReadPixels(new Rect (0, 0, cam_width, cam_height), 0, 0);

            Color colors = new Color(cam.farClipPlane, 0, 0);            

            Color[] cols = tex2d.GetPixels(0);
            for (int i = 0; i < cols.Length; ++i)
            {
                cols[i] = cols[i] * colors;
            }
            tex2d.SetPixels(cols, 0);

            data32 = tex2d.GetRawTextureData();
            // data = tex2d.GetPixel(cam_width/2, cam_height/2);

            Destroy(tex2d);
            RenderTexture.active = currentRT;
        }

        private ros_sensor_image CopyData(byte[] data, string format)
        {
            byte[] tmp = new byte[data.Length];

            ros_sensor_image image_data = new ros_sensor_image();
            image_data.height = (uint)cam_height;
            image_data.width = (uint)cam_width;
            image_data.is_bigendian = 0;

            image_data.step = (uint)cam_width * 4;
            image_data.encoding = "32FC1";
            image_data.data = data;

            return image_data;      
        }
        
    }
}
