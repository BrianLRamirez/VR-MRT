![Imgur](https://i.imgur.com/HWHM5K0.png)

## About 

The VR Mental Rotation test is a project that was born at the [MIT STEP Lab](https://education.mit.edu/) where I worked with the [Collaborative Learning Environments in Virtual Reality](https://education.mit.edu/project/clevr/) Team to create this research project under the supervision of Dr. Meredith Thompson, PhD. 
 

The VR-MRT is a virtual reality application that aims to  reproduce the popular mental rotation test created by Shepard and Metzler's in 1971, but rather than using paper to test spatial abilities, we use virtual reality headsets. 

## Video Demo

[![Watch the video](https://i.imgur.com/SH29clf.png)](https://www.youtube.com/watch?v=HMa2PaHXDqs&feature=emb_title)

## Installation
Installing this experience is pretty straight forward, clone this repo by running: 

```
git clone https://github.com/BrianLRamirez/VR-MRT.git
```

This project was developed for the Oculus Quest on Unity so you’ll need to make sure you have Unity installed. In the root of the source code folder there is a file called `Build.apk`, you can side load this app into any Oculus Quest and start using the VR experience in no time. 

If you would like to edit the experience to better fit your research, then you will need to build the project with Unity. This project was built with `Unity Version 2019.1.14f.1` so we recommend you download this version of unity to avoid build errors.  

## Implementation Details 
The experience was created in a simple and coherent fashion. There is only one scene called `Main` and can be found in `Assets/Scenes/Main.unity`. This scene is made up of the environment which could be described as a small futuristic-looking room with no ceiling revealing a colorful sky.

In front of the player there are two objects (which we call puzzles), one of which has been rotated along its X and Y axis. All puzzles can be found in `Assets/Resources/`, these 3D models were made in blender. Rotating an object is done by rotating the thumbsticks in the oculus touch controllers, this input is handled by the rotation script which can be found in `Assets/Scripts/Rotateable.cs`. 

The game mechanics are straightforward: the user uses the thumbsticks to rotate the puzzles and then clicks the confirmation button to check their answer, if their answer is wrong they’ll need to rotate the object further and try again, if their answer is correct a new puzzle will be loaded and their completion time will be saved in the time board (located right behind the user in the VR experience). The experience ends when the user solves all 8 puzzles. 



## License

MIT License
