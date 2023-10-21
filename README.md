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

The first step is to download Python 3.10.12 or higher, **HOWEVER** from my testing this only works with 3.10 and under. I had trouble trying to get the dependencies right for Python 3.11. As such, I would suggest getting [Python 3.10.11](https://www.python.org/ftp/python/3.10.11/) and using that version for the following steps. As always, the full description can be found on the [Installation instructions](https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Installation.md#install-python-31012-or-higher).

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

Open UnityHub and click the "Open" button on the top right. Navigate to the location that you cloned this Github repo, and select the SwordFightingUnity directory to open. This should launch Unity and load the Unity project, with all the necessary project settings and packages.

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
