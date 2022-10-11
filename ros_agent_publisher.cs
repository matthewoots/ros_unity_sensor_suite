/*
* agent_publisher.cs
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

using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

using ros_sensor_pcl = RosMessageTypes.Sensor.PointCloudMsg;
using ros_geometry_point = RosMessageTypes.Geometry.Point32Msg;
using ros_sensor_image = RosMessageTypes.Sensor.ImageMsg;
using ros_geometry_pose = RosMessageTypes.Geometry.PoseStampedMsg;

using sensors_suite;

public class ros_agent_publisher : MonoBehaviour
{
    public ROSConnection ros;
    public GameObject obj;
    // Message types and list for single agent
    // 0. imu
    // 1. pose
    // 2. gps
    // 3. rgb camera
    // 4. depth
    // 5. lidar
    
    [Header("Publishing Parameters")]
    public string prefix = "unity/agent_0_";
    public string[] sensor_list = 
        {"imu", "pose", "gps", "rgb", "depth", "lidar"};
    public int[] message_rate_hz = 
        {30, 30, 5, 5, 5, 5};
    
    [Header("Insert Sensors from Scene")]
    public sensors_suite.lidar_sensor lidar;
    public sensors_suite.image_sensor rgb;
    public sensors_suite.depth_sensor depth;

    [Header("Private Parameters")]
    private bool initialized;
    private string[] topic_list = new string[6];
    private float[] time_elapsed = 
        {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};

    private void initialization()
    {
        for (int i = 0; i < sensor_list.Length; i++) 
            topic_list[i] = prefix + sensor_list[i];

        if (rgb != null)
            ros.RegisterPublisher<ros_sensor_image>(topic_list[3]);

        if (depth != null)
            ros.RegisterPublisher<ros_sensor_image>(topic_list[4]);

        if (lidar != null)
            ros.RegisterPublisher<ros_sensor_pcl>(topic_list[5]);

        initialized = true;
    }

    private void handler(string type)
    {
        int index = Array.IndexOf(sensor_list, type);
        if (index == -1)
            return;

        time_elapsed[index] += Time.deltaTime;
        if (time_elapsed[index] > 1/(float)message_rate_hz[index])
        {
            time_elapsed[index] = 0.0f;

            switch(index) 
            {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;

            case 3:
                ros_sensor_image image_msg = rgb.RosDataHandler();
                ros.Send(topic_list[index], image_msg);
                break;

            case 4:
                ros_sensor_image depth_msg = new ros_sensor_image();
                depth_msg = depth.msg;
                ros.Send(topic_list[index], depth_msg);
                break;

            case 5:
                lidar.initialize();
                ros_sensor_pcl pcl = lidar.RosDataHandler();
                ros.Send(topic_list[index], pcl);
                break;

            default:
                break;
            }
        }
    }

    private void Update()
    {
        if (!initialized)
        {
            if (Time.time < 2.0f)
                initialization();
            return;
        }
        
        if (rgb != null) handler("rgb");
        if (depth != null) handler("depth");    
        if (lidar != null) handler("lidar");    
    }

}