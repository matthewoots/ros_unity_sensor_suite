# ROS UNITY SENSOR SUITE

## Introduction

This repository contains a sensor suite for simulating multi agents in Unity3d, using the `ROS-TCP-Connector` from `Unity-Technologies`.

This package is used with https://github.com/matthewoots/ros_tcp_connector which the server endpoint, and provides the bridge from `Unity` to `ROS`.

## Installation

### To download Unity3d
For `Ubuntu 20.04 LTS` or other linux variants:
1. Download Unity Hub from https://unity3d.com/get-unity/download
    - Afterwards, remember to `sudo chmod +x UnityHub.AppImage`
    - And launch with `./UnityHub.AppImage`

### To download ROS-TCP-Connector
1. Using `Unity 2020.2 or later`, open the Package Manager from `Window` -> `Package Manager`.
2. In the Package Manager window, find and click the + button in the upper lefthand corner of the window. Select `Add package from git URL....`

    ![image](https://user-images.githubusercontent.com/29758400/110989310-8ea36180-8326-11eb-8318-f67ee200a23d.png)

3. Add 2 packages (ROS-TCP-Connector and Visualizations) Enter the git URLs
    - `https://github.com/Unity-Technologies/ROS-TCP-Connector.git?path=/com.unity.robotics.ros-tcp-connector#v0.6.0`
    - `https://github.com/Unity-Technologies/ROS-TCP-Connector.git?path=/com.unity.robotics.visualizations#v0.6.0`

