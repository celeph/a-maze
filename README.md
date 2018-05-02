# Course Project 3 - A Maze

## Introduction

This project is part of [Udacity](https://www.udacity.com "Udacity - Be in demand")'s [VR Developer Nanodegree](https://www.udacity.com/course/vr-developer-nanodegree--nd017) and concludes the VR software development chapter of the first term. 

The objective was to create a maze environment using Unity and the Google VR SDK. The user can navigate through the maze by clicking on and moving from one waypoint to another. The destination is a temple with a treasure locked behind a door. The door can only be opened with a key that's hidden inside the maze and has to be picked up first. On the way the user can collect coins. The number of collected coins is displayed to the user at the destination.

A starter project provided Unity prefabs and wall building blocks, and some of the tasks included:

1) Creating a maze scene
2) Adding VR functionality
3) Building a waypoint navigation system
4) Creating collectable items (coins)
5) Creating a key to pick up
6) Animating a door
7) Display a UI Canvas and restart the game 

I created this project for Android. Tested successfully on a Google Pixel 2XL with Android 8.1.0.


## A Journey - Reflections

### Maze Generation Algorithm

This project probably took me longer than it should have. Because this project was all about software development I had this idea to generate the maze programmatically using something like [Prim's algorithm](https://en.wikipedia.org/wiki/Maze_generation_algorithm). I experimented with a simple static HTML page with canvas and some Javascript because I thought it would be easier if I explored in  a more familiar environment first. This was less trivial than I thought, and I realized that it's not only the maze, I'd also need to port this to Unity's C# and find a way to place waypoints, coins and the key programmatically. I'm sure it's doable, but a fully randomized maze probably would have taken much more time, so I had to scrap that idea (for now).

### Pre-defined Maze Level

My next idea was to predefine a maze level as an array, but a numeric array wasn't very intuitive. So I decided to use a simple ASCII-map instead. The idea was to scan a string and interpret each character as a horizontal wall, vertical wall, pillar, empty space. After some experimentation I came up with some functions that assembled the building blocks pretty accurately. I created a new MazeBuilder.cs that defines the following map and translates this into a proper Unity scene with the given building block prefabs:

	string map = "" +
		"+-+-+-+-+   +-+-+-+-+-+-+\n" +
		"|               |       |\n" +
		"+     +-+-+-+-+ +       +\n" +
		"|     |   |` `| |       |\n" +
		"+ +-+-+   +-+-+ + +   + +\n" +
		"|     |           |   | |\n" +
		"+     +-+-+-+-+-+-+-+-+ +\n" +
		"|     |         |       |\n" +
		"+-+-+-+   +-+-+-+ +-+   +\n" +
		"|     |   |       |     |\n" +
		"+     +   +       +     +\n" +
		"|     |   |       |     |\n" +
		"+ +-+-+   +   +-+-+ +-+ +\n" +
		"|   |     |   |   |   | |\n" +
		"+   +   +-+   +   +   + +\n" +
		"|         |   |   |   | |\n" +
		"+         +   +   +   + +\n" +
		"|         |   |       | |\n" +
		"+-+-+   + +   +-+-+-+-+ +\n" +
		"|   |   | |           | |\n" +
		"+   +   + +-+-+-+-+   + +\n" +
		"|       |   |   |     | |\n" +
		"+       +   +   +   +-+ +\n" +
		"|       |   |   |   |   |\n" +
		"+-+-+   +-+ +   +   +   +\n" +
		"|   |     |     |   |   |\n" +
		"+   + +   +-+-+ +   +-+ +\n" +
		"|     |     |       |   |\n" +
		"+-+-+-+-+-+-+   +-+-+-+-+\n" +
		"";

		
[[https://wessendorf.org/a-maze.png]]
		
		
### Game mode only

The first challenge with this approach was that all the generated walls would only be visible in game mode. If you exit the game mode they'd all disappear and you'd be left with an empty scene again. 

This of course wasn't ideal because I still needed to place some items inside the maze. I also didn't want to let the whole scene regenerate each time you start the game. I wanted to use my script to generate objects permanently and keep them in edit view so I could place objects, bake lights, and make other changes if needed.

I found a handy attribute `[ExecuteInEditMode]` which allowed me to do just that. 

### Bursting with objects

But I encountered new challenges with this approach when I suddenly ended up with multiple mazes of different scales. I added all generated game objects to a `MazeContainer` game object and scaled it to fill the space. Somehow I got a smaller maze, and a scaled version. 

For better control I tagged all generated game objects with 'Generated' and added a boolean flag `isActive`, so it only generates the maze if `isActive` is true, and then set `isActive` to false so it doesn't do it all over again. Before the script generates the maze it also destroys any previously generated objects with the 'Generated' tag. 

### Waypoints

After a while I got it to work and I thought it would be a good idea to let the script place waypoints on the grid automatically as well. Easy, I'll just place a waypoint in every empty space, I thought. But in practice this wasn't a good idea at all. With a tight grid of waypoints all you got to see were waypoints, and navigation was very confusing. I'd easily get lost in the maze or bath of waypoints. So I had to scrap that idea as well.

In the end I deleted a lot of the generated waypoints and positioned them manually trying to maximize the distance between them. Less waypoints made navigation and orientation much more comfortable. 

### A Maze 

The end result is a hybrid of a maze I generated with my MazeBuilder.cs script (once) and waypoints, coins and key I placed inside the maze manually. I added coins on the way to keep it interesting. I couldn't help myself and had to add a different sound. The script counts the number of collected coins and displays them on a UI canvas in the destination room. Also added a few lights (to provide a little hint). The key is animated with a rotation and vertical bounce motion.

A lot of the time I spent trying to create a fancy maze builder probably wasn't necessary, but it was a fun project and journey of discovery, a great learning experience. I'm sure once I learn more about programming in Unity, I may be able to return to my original ideas and use an algorithm to generate a random, properly fitting maze. :)


## Versions and Third Party Assets

- VR Software Development - Starter Project (v5.1.0)

- [Unity 2017.2.0f3 (64-bit Windows)](https://unity3d.com/get-unity/download?thank-you=update&download_nid=48367&os=Win) 

- [GVR Unity SDK v1.70.0](https://github.com/googlevr/gvr-unity-sdk/releases/tag/1.70.0)

- Mario Coin Sound by [svarogg](https://freesound.org/people/svarogg/sounds/344436/), free download from [freesound.org](https://freesound.org/people/svarogg/sounds/344436/), Creative Commons [CC0 1.0](https://creativecommons.org/publicdomain/zero/1.0/)

