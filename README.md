# Unity CAVE Package

## Short description

We developed a Unity package for making Unity Projects work on a [CAVE](https://github.com/thechosi/CAVE/wiki/CAVE). To make migration as easy as possible we provide a bunch of tools like synchronization on a cluster, configuration, projection etc. out of the box. For getting started with our package read the following Quick start guide:

## Quick start

To integrate CAVE support into your Unity application there are a few steps to do:

### 1. Integrate our package

Import our unity package into to your project. Just copy the content of the "Package" directory into your assets folder.

### 2. Add NodeManager

In the directory Assets/Cave/Prefabs you should find a prefab called NodeManager. Please add this prefab to your scene.

### 3. Adjust script execution order

Make sure the script Cave.Synchronizer is executed before all other scripts (In Unity go to Edit -> Project Settings -> Script Execution Order):
* Add the script Cave.InstantiateNode and set its value to -200.
* Add the script Cave.Synchronizer and set its value to -100.

### 4. Adjust your scripts

It is very likely that you have to adjust the scripts of your projects. This is necessary as you need to make sure you are using synchronized parameters instead of the ones unity provides which are just local.

You can find all necessary steps in the [synchronization](https://github.com/thechosi/CAVE/wiki/Synchronization) chapter.

### 5. Customize CAVE config

After building your project you can configure how your project should look like in the CAVE. See the [config tool documentation](https://github.com/thechosi/CAVE/wiki/ConfigTool) for further details how to do this.

### 6. Optional: VRPN

Before starting your project, you might also need to start the VRPN server (run VRPN_Server/vrpn_server.exe). This is only necessary, if you use vrpn for tracking.

## More detailed description

For getting more information about a specific topic of our package you can find the documentation on multiple subpages:

* [Synchronization](https://github.com/thechosi/CAVE/wiki/Synchronization)
* [ConfigTool](https://github.com/thechosi/CAVE/wiki/ConfigTool)
* [Jenga (example project)](https://github.com/thechosi/CAVE/wiki/Jenga)

## Technical details

* We developed the package using Unity 5.5.2f1, but our package should also work with future versions.
* This project was originally build for Windows, but it might also work on Linux or Mac.
* As basis for our project we used the [Unity Cluster Package](https://sourceforge.net/projects/unityclusterpackage/). Nevertheless we don't have much in common with it anymore.
* For basic socket communication we used a slightly changed version of the [AwesomeSockets Library](https://github.com/nterry/AwesomeSockets) 
