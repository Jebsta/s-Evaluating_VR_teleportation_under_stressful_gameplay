# DM2905 - StressPort: A VR Study Design for Teleportation Under Game-Like Stressful Conditions
<img width="100%" alt="image" src="Assets/Art/Images/cover_vr_stressport.jpg">

# DISCLAIMER
The following readme is from the previous iteration of the project, and it is not up to date with the current state of the project. The readme will be updated in the next iteration, but for now, it serves as a reference for the overall architecture and design of the project.

## About the study
### Objective
The experiment is designed to investigate how exposure to game-like stressors affects teleportation performance, perceived stress, and workload in VR environments.

### Motivation
Gathering reliable results from user evaluations is crucial for informing design decisions. While prior studies have explored stress responses during gameplay in fully developed or modified games, the intermediate step between isolated lab-based evaluations and in-game scenarios remains underexplored. This research seeks to bridge that gap by designing a test environment that enables the researcher to examine the impact of stressors in a controlled VR teleportation evaluation task, simulating high-arousal states typical of fast-paced video games.

### Experience Overview
In this experimental design, the subjects have to teleport between 6 hexagon- layout platforms. Each platform represented a different color (red, blue, green, yellow, orange, and purple). Subjects are given one teleportation instruction at a time in the form of color displayed on a VR GUI to move to a destination. Once at the destination platform, they activate a mechanism to cue the system for the next platform.

This instance of the prototype includes two teleportation methods and three game stressors, but further methods and stressors can be added according to the research’s needs. Sound feedback, dynamic Stroop rules, and adjustments to attention-demanding stimuli are critical next steps for enhancing the prototype’s functionality. The design hopes to help advance the understanding of VR teleportation evaluation under game-like stressors and serve as a starting point for future researchers and VR game developers.

## Dependencies
- Platfrom: Meta Quest 3 VR headset using Meta’s Interaction SDK 
- Unity version 2022.3.46f1 using the Built-in Render Pipeline
- OpenXR SDK and the virtual simulator (powered using Vulkan)
*When the application is installed onto the HMD, the viewport can be streamed to a MacBook Pro via the Meta Quest Developer Hub for real-time monitoring.

## Directories
The important files can be found in the following locations, any other files are dependencies from pluggings and they must not be removed or edited unless you know what you are doing. The **main scene** is `TeleportationOnRightHandOnly` inside the `/Scenes/Experiment`.
```bash
Project/
├── Assets/
│   ├── Art/
│   │   ├── Images/
│   │   ├── Materials/
│   │   ├── Mesh/
│   │   └── Textures/
│   ├── Prefabs/
│   ├── Scenes/
│   │    ├── Experiment/ TeleportationOnRightHandOnly
│   │    └── Playgrounds/
│   ├── Scriptable Objects/
│   │   └── Platforms/
│   └── Scripts/
│       ├── Controllers/
│       ├── Experiment Variables/
│       ├── Manager/
│       ├── Persistence/
│       └── States/

```
**About Scripts:**
`Controllers`: Manages player and object interactions.
`Experiment Variables`: Handles specific experimental conditions and variables.
`Manager`: Oversees overarching game logic or systems.
`Persistence`: Scripts for saving/loading data.
`States`: State machine implementation for controlling various behaviors.

## Overall architecture

`ExperimentManager`
1. `OnConditionChanged`: Triggered when a new experimental condition is set, notifying subscribers about the change.
2. `OnConditionTerminated`: Invoked when the current experimental condition is forcibly terminated, notifying subscribers about the termination.
3. `OnConditionFulfilled`: Raised when tasks for the current condition are completed successfully, allowing subscribers to react to the condition's fulfillment.
4. `OnExperimentCompleted`: Fired when all experimental conditions are fulfilled, signaling the end of the experiment.
5. `OnExperimentReset`: Activated when the experiment is reset, notifying subscribers to reinitialize or prepare for a new round.

`GameplayManager`
1. `OnPracticeStandby`: Triggered when the player is in a standby state, waiting to manually start the practice tasks.
2. `OnPracticeBegin`: Raised when the player begins performing the practice tasks.
3. `OnPracticeEnd`: Fired when all practice tasks are completed.
4. `OnPracticeEndAndTrialStandby`: Activated after completing all practice tasks, signaling the player can manually begin the trial tasks.
5. `OnTrialStandby`: Triggered when the player is in a standby state, waiting to manually start the trial tasks.
6. `OnTrialBegin`: Invoked when the player starts performing the trial tasks.
7. `OnTrialEnd`: Fired when all trial tasks are completed.
8. `OnGameOver`: Signals the end of the gameplay session, used to clean up or reset state.

`GameState`

**Events**
1. `OnNewSequence`: Raised when a new sequence of task colors is created. Observers can subscribe to this event to receive the stack of colors forming the new sequence.
OnNewNextColor:

2. `Description`: Triggered whenever the next task color is determined during a sequence. Observers receive details about the stimulus type, next color, and whether the previous task was completed successfully.
**Observer-Dependent Methods**
1. `Subscribe(IObserver<GameStateData> observer)`:Adds a new observer to the list of observers. Returns an IDisposable object to allow the observer to unsubscribe later.
2. `NotifyObservers()`:Notifies all registered observers about the current game state change. Sends the updated GameStateData.
3. `NotifyObserversForTheLastTime()`:Sends a final notification to all observers and ensures they are informed of the game's end. It traverses the observer list in reverse to prevent issues when removing observers.

## Features
- **Start:** The primary button on `GameplayGUI` children call `BeginGame` on GameplayManager
**Platforms:** `EnterGameplay` in `ExprimentManager` and in `GameplayManager` triggers the setup of the platforms.
<img width="50%" alt="image" src="Assets/Art/Images/vr_scene_layout.jpg">

- **Hard exit:** Press both primary buttons for 5 seconds to terminate the task.
- **Color generation:** The colors are generated using a Graph, and a set of coordinates `{ -3, -2, -1, 1, 2, 3 }` that represent the number of steps from the player current position and the direction: `-` left and `+` right. 
`GenerateRandomCoordinateList` inside `GameState`, creates the Graph. Read *Session 2024-11-05: Adding Graph and Distances* for more details.
<img width="50%" alt="image" src="Assets/Art/Images/vr_color_gui.jpg"> 

- **Participant and variables Data:** The information about what stimuli are active in a task is in `ParticipantData` Scriptable Object. The GUI writes directly to its field to toggle variables.
<img width="50%" alt="image" src="Assets/Art/Images/vr_new_gui.jpg"> 

- **Timer:** This variables in controlled by the GameplayManager via the `CountdownToReachPlaform()` method.
- **Biased instruction:** This stimuli is controll inside the `ColorPromptController` class, in the `UpdateColorPromptDisplay()` method. A memeber variable `m_participantData` referencing `ParticipantData` game object expose the property `.GameStressorBiasedInstruction`.
- **Shrinking platforms:** This stimuli is controlled by `ShrinkPlatformController`, this component is attached to a GameObject ShrinkController inside the parent prefab FloatingPlatform. The GameObject TeleportationBlocker is what causes the teleportation area to apparently shrink. It is a torus that bloacks the Nav Mesh.
<img width="50%" alt="image" src="Assets/Art/Images/turus.png">

## Backlog
- [ ]  Logging variables in CSV and store the file in the Headset
- [ ]  Sound feedback when color prompt changes
- [ ]  Sound feedback when a platform gets activated
- [ ]  Sound feedback when arriving at the wrong platform
- [x] Finish gameloop
    - [x] Connect the condition with the game state
    - [x] Update the game loop to have a practice and trial color sequence
    - [x] GUI for *Start Practice*, then for "Practice complete now" *Start Trial*.
    - [x] GUI once the Trial is completed (Condition fulfilled), *Back to conditions*
    - [x] The color prompt shows "COLOR" or "WORD" depending on the presence of the congnitive interference
- [x] Menu to hide the conditions
- [x] Confirmation feature to manually reset, terminate and fulfill each condition and the experiment
- [x] Test the conditions manu in VR
- [x] Hide and show the conditions menu
- [x] Begin the experiment with a subject ID, set the ID either by keyboard input, or random.

## Known issues
- The color prompter sometimes gives a color that corresponds to the current platform the player is standing.
- The teleportation orientation feature disables object grabbing when both functionalities are active in the same scene.

### Devlogs
<img width="1014" alt="image" src="https://github.com/user-attachments/assets/0547c2ad-07c8-4b45-baf4-780802d84d71">

