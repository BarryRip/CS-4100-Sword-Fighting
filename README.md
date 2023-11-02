# Sword Fighting Bots
This repo contains all the files and source code related to this CS 4100 project.
Project guidelines can be found [here](https://rajagopalvenkat.com/teaching/CS4100/CS4100_Project_Guidelines.pdf).

## Repo Structure
The repository is structured into two main directories:
- **SwordFightUnity**: Contains the whole Unity project including source code, which can be opened as a Unity project by UnityHub.
- **PythonFiles**: Contains all files that are used by or related to the Python virtual environment side of the project.

Unfortunately, this repo can only hold up to 2 GB for our source code. Because of this, storing the entire virtual environment will be infeasable. So generally, we will be storing the Unity project in this repo, as well as any configurations or other files that can be placed directly into one's own virtual environment. The virtual environment must be set up on one's own machine. Information on setting up this virtual environment can be found in [the below section](#Setup).

*(Theoretically I'm sure something like a Docker container or something is the correct way to handle this but idk that seems like a lot to figure out for such a short scope so meh)*

## Setup
This is an overview for setting up Unity ML Agents for the project. This setup somewhat follows the tutorial video [here](https://www.youtube.com/watch?v=zPFU30tbyKs) when relevant, which also serves as a useful guide for using the Unity ML Agents package in general.

**NOTE: I did this on Windows 10. These steps may vary for other machines.**

### Step 1: Python Setup
This first step sets up the Python side of the project, including all relevant installations and creating the virtual environment.

I would highly recommend looking at the [Unity ML Agents Github repo](https://github.com/Unity-Technologies/ml-agents), specifically the [Installation instructions](https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Installation.md) in the docs directory. This contained most of the information I needed to set up this section.

The first step is to download Python 3.10.12 or higher, **HOWEVER** from my testing this only works with versions 3.10.XX and under. I had trouble trying to get the dependencies right for Python 3.11. As such, I would suggest getting [Python 3.10.11](https://www.python.org/ftp/python/3.10.11/) and using that version for the following steps. As always, the full description can be found on the [Installation instructions](https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Installation.md#install-python-31012-or-higher).

Next, I created a virtual environment in the Demo directory. This can be done with the command:
```
python -m venv ENVIRONMENT_NAME
```
Where "ENVIRONMENT_NAME" can be whatever you want the directory containing your environment to be named.

For my purposes, I had to run this command using Python 3.10. This makes it so your virtual environment runs in Python 3.10. Again, dependencies were failing when I tried this in 3.11. If you do not have Python 3.10 as your default, you can run:
```
python3.10 -m venv ENVIRONMENT_NAME
```
or
```
PATH -m venv ENVIRONMENT_NAME
```
Where "PATH" is the path to your Python 3.10 executable.

To enter this virtual environment, run the 'activate' file in the Scripts directory. From the location we created our environment, we can run:
```
ENVIRONMENT_NAME/Scripts/activate
```
This environment ensures we are separating this Python project from other projects.

**NOTE: Make sure you are in that virtual environment for these next steps. You should see (venv) on the command line.**

To make sure that your Python version is indeed 3.10, within your virtual environment, run:
```
python --version
```
Next, make sure you have pip installed and updated in the virtual environment. To do this, run:
```
python -m pip install --upgrade pip
```
Then, install pytorch in the virtual environment like so:
```
pip3 install torch~=1.13.1 -f https://download.pytorch.org/whl/torch_stable.html
```
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

For this project, you will want Unity 2022.3 downloaded. The specific version I used was 2022.3.11f1.

Open UnityHub and click the "Open" button on the top right. Navigate to the location that you cloned this Github repo, and select the SwordFightingUnity or the UnityMLAgentDemo directory to open. This should launch Unity and load the Unity project, with all the necessary project settings and packages.

### Step 3(?): Running The Demo
At this point, you should be able to run the ML agents package on the python side (this can be checked by running mlagents-learn --help in your virtual environment) and you should be able to open up the Unity project.

#### So, How Does The Demo Work?
The demo is set up as a 2D simulation of a single agent (blue circle), a goal (yellow square), and walls (red rectangles). The agent can move in 4 directions. If the agent reaches the goal, it receives a reward and a new episode is started. If the agent hits a wall, a negative reward is received and a new episode is started. If the agent takes too long (5000 timesteps to be precise), the episode ends with no reward. The agent is only given information about its current position and the position of the goal. Also, every time a new episode begins, the position of the agent and goal is semi-randomized.

#### How Can I Run The Demo Myself?
To run trials, simply press the play button in the Unity editor. You will probably see the agent do a moderately decent job reaching the goal. In this state, **the agent is only using a neural network to make decisions, and is not learning through reinforcement learning.** In order to enable reinforcement learning, you will need to run mlagents-learn on the Python side.

```
mlagents-learn
```

Note that **if you want to run this command multiple times, you need to give it a new id each time.** To do this, run:

```
mlagents-learn --run-id=ID_NAME_HERE
```

where ID_NAME_HERE is replaced with the name you want to give it. Alternatively, if you don't care about saving / overwriting your results, you can force it to overwrite by running:

```
mlagents-learn --force
```

If this command works, you should see an ASCII art Unity logo appear. Now, if you run the Unity side simulation by pressing the play button in the editor, you will see that it is actually doing reinforcement learning (notice the more random behavior as it is trying different random movements). This agent is working under pretty simple rules, so after some time you will probably see it get better at reaching the goal at certain corners, but fail to reach it in other corners. 

Once the training ends (either by you manually stopping Unity in the editor, ctrl-C on the Python cmd prompt, or the simulation will just stop after a while), the results of the training are output into the results folder on the Python side, saved under the id you gave it (or "ppo" if no id was given). The main points of interest in this folder are the .onnx and configuration.yaml files it creates. The configuration file contains the various settings tweaked during the run, and it can be fed back into the Python side when running more tests. The .onnx file is the machine learning model created by the run. This can be placed into our agent object in Unity to act as our neural network model. In fact, this is what the agent already had been using. In the assets folder of Unity, there are two models I already created: MoveToGoal and ReallyGood. The agent is currently using ReallyGood as a model, which is why even if no reinforcement learning is happening, the agent can still find the goal.

#### Agent Behavior
The behavior of the agent is encapsulated in a single script, MoveToGoalAgent, which is attached to the agent object. This script extends the Agent class predefined by the ML Agents package. This script is where all the magic happens, so to speak. I'm not going to get into all the minute details here, but almost all of the stuff in this script comes straight from the [Youtube tutorial I linked earlier](https://www.youtube.com/watch?v=zPFU30tbyKs), so take a look at the latter half of that video for more information. The three important methods to override from the Agent class are:
- OnEpisodeBegin(), where we implement the logic for restarting the environment upon a new episode beginning
- CollectObservations(), where we give the agent the input we want it to receive from the Unity environment
- OnActionReceived(), where we convert the output from the agent into actual movement in our Unity environment

There are also other important methods we need to use, mainly located in the OnTriggerEnter2D method, that are used to handle rewards and ending the episode:
- SetReward() will give the agent a positive or negative reward
- EndEpisode() will end the current episode and automatically start a new episode, consequently calling OnEpisodeBegin()

The actions and observations can be customized in the "Behavior Parameters" script that comes with the ML Agents package and is automatically attached to an object when an Agent script is attached. Specifically, Vector Observation and Actions determine the type and number of observations and actions respectively, while Model allows you to insert your own .onnx model, as was done with ReallyGood. Again, you can find a better detailed explanation by watching the [Youtube tutorial](https://www.youtube.com/watch?v=zPFU30tbyKs).

If this isn't making much sense, I would highly recommend just following the Unity part of the Youtube tutorial step by step to create your own demo. The tutorial goes through the process of actually building it from scratch, which might be more helpful for you than looking at my already made demo.

#### Other Resources
The Unity documentation for the ML Agents package can be found [here](https://docs.unity3d.com/Packages/com.unity.ml-agents@1.0/api/Unity.MLAgents.html), although I'd mostly use this if you need to know more about a specific method or class that is in the ML Agents package.

While there is a LOT to sift through, the ML Agents github repo has a [documentation folder](https://github.com/Unity-Technologies/ml-agents/tree/develop/docs) that contains a lot of info about both the Unity and Python sides. Again, there is a lot there, but this is probably the location with the most accurate information about the package.

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
- [Project Tasks](https://docs.google.com/document/d/1ON1UFuPIz32DkRHPDysAGG57dz48_XiHCx6mdlsMTZc/edit?usp=sharing)
- [Final Report (TBD)]()
