# Sword Fighting Bots
This repo contains all the files and source code related to this CS 4100 project.
Project guidelines can be found [here](https://rajagopalvenkat.com/teaching/CS4100/CS4100_Project_Guidelines.pdf).

## Important Notes
### About Comments
The [project guidelines](https://rajagopalvenkat.com/teaching/CS4100/CS4100_Project_Guidelines.pdf) state:

> Source code must be submitted through a GitHub repository, with detailed comments and documentation to help with reproducibility. Part of your grade will also be based on this.

As such, we should make sure to leave detailed comments and keep up on documentation for our code.

## Relevant Links
### Google Docs
- [Project Abstract](https://docs.google.com/document/d/15YgOB4iAarVcja2aBHLCG4l0LHIrKNnMfNQofseEgoQ/edit?usp=sharing)
- [Project Proposal](https://docs.google.com/document/d/1RttSZunMfO3l-zYLLr1a9zKFXj9-zJl461Nsug4n2Ds/edit?usp=sharing)
- [Proposal Notes](https://docs.google.com/document/d/1bjnws28TNjxzjJyIBMW0jlY4n7hWPXb2DV5LNOszm2c/edit?usp=sharing)
- [Final Report (TBD)]()

## Unity ML Demo
This demo is included in the repo as an exploration and sample for basic RL in Unity. Essentially, the files in this directory were created to test out the Unity ML Agents library. This is to be used solely as a point of reference and is otherwise unrelated to the actual contents of the project, meaning it can be removed from the main branch in the future before submission. This demo tries to follow the tutorial video [here](https://www.youtube.com/watch?v=zPFU30tbyKs) when relevant, but the actual steps I had to take are listed below to make it easier to follow.

**NOTE: This is being run on Windows, so steps may vary for other machines.**

### Step 1: Python Setup
On the actual [Unity ML Agents Github repo](https://github.com/Unity-Technologies/ml-agents), the first point of interest is the [Installation instructions](https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Installation.md) in the docs directory.

The first step is to download Python 3.10.12 or higher, **HOWEVER** from my testing this only works with 3.10 and under. I had trouble trying to get the dependencies right for Python 3.11. As such, I would suggest getting Python 3.10.11 and using that version for the following steps. As always, the full description can be found on the [Installation instructions](https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Installation.md#install-python-31012-or-higher).

Next, I created a virtual environment in the Demo directory, called oldenv. This can be done with the command:
```
python -m venv oldenv
```
For my purposes, I had to run this command using Python 3.10. This makes it so your virtual environment runs in Python 3.10. Again, dependencies were failing when I tried this in 3.11. If you do not have Python 3.10 as your default, you can run:
```
python3.10 -m venv oldenv
```
or
```
[PATH/TO/PYTHON3.10/EXECUTABLE] -m venv oldenv
```
To enter this virtual environment, run the 'activate' file in the Scripts directory. From the location we created venv, we can run:
```
venv/Scripts/activate
```
This environment ensures we are separating this Python project from other projects.

**NOTE: Make sure you are in that virtual environment for these next steps. You should see (venv) on the command line.**

Next, make sure you have pip installed and updated in the virtual environment. To do this, run:
```
python -m pip install --upgrade pip
```
Then, install pytorch in the virtual environment like so:
```
pip3 install torch~=1.13.1 -f https://download.pytorch.org/whl/torch_stable.html
```
This is just the command that works for my machine, but the command in the video and the Github documentation did not work for me. I got this installation line by going to the official [Pytorch website](https://pytorch.org/get-started/locally/) and following the instructions there.

Now, we can install the ML agents package in our virtual environment. My recommendation is to first clone the ML agents repo:
```
git clone --branch release_21 https://github.com/Unity-Technologies/ml-agents.git
```
And then navigate to the cloned repo (should be under the directory \ml-agents that was just cloned) and run these commands in the virtual environment:
```
python -m pip install ./ml-agents-envs
python -m pip install ./ml-agents
```
To check that the ML agents package and pytorch have been installed successfully on your virtual environment, run:
```
mlagents-learn --help
```
And a command help menu should appear on your command line. If an error occurs instead, check that pytorch was installed correctly or that your python version inside the virtual environment is correct.

If you can run this help command successfully, the Python side of things is all set up!

### Step 2: Unity Setup
If you don't have Unity installed, the process for that is pretty simple. I'd recommend getting started with [this](https://docs.unity3d.com/hub/manual/index.html) tutorial on installing Unity Hub, which allows you to manage your Unity projects and install different versions of Unity.

We want Unity 2022.3 specifically for this demo (I used Unity 2022.3.11f1).
