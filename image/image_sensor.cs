/*
* image_sensor.cs
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

using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using ros_sensor_image = RosMessageTypes.Sensor.ImageMsg;

namespace sensors_suite {
    public class image_sensor : MonoBehaviour
    {
        public Camera cam;
        public int cameraWidth = 640, cameraHeight = 480;

        void initialize(){}

        public ros_sensor_image RosDataHandler()
        {
            // Image Format
            Texture2D imageTexture = ReturnRGBImage(cam, cam.pixelWidth, cam.pixelHeight);
            // Use GetRawTextureData since EncodeToPNG or JPG does not import the whole size of the data
            return CopyData(cam, imageTexture.GetRawTextureData(),"rgb8", true); 
        }

        private Texture2D ReturnRGBImage(Camera cam, int imageWidth, int imageHeight)
        {
            RenderTexture rt = new RenderTexture(imageWidth, imageHeight, 0);
            cam.targetTexture = rt;
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = cam.targetTexture;
            cam.Render();

            Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, false);
            image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
            image.Apply();

            RenderTexture.active = currentRT;
            Destroy(rt);
            Destroy(image);
            return image;
        }

        private ros_sensor_image CopyData(Camera cam, byte[] Data, string format, bool isrgb)
        {
            byte[] tmp = new byte[Data.Length];

            ros_sensor_image ImageData = new ros_sensor_image();
            ImageData.height = (uint)cam.pixelHeight;
            ImageData.width = (uint)cam.pixelWidth;
            ImageData.encoding = format;
            ImageData.is_bigendian = 0;

            if (isrgb) ImageData.step = (uint)cam.pixelWidth * 3;
            else ImageData.step = (uint)cam.pixelWidth;

            ImageData.data = Data;
            
            return ImageData;      
        }

    }
}