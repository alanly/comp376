# ReadMe for Helium

COMP 376 Assignment 2.

Alan Ly (6293484).

## Code & Configuration

The game scripts are separated into two namespaces: `Helium.Core` and `Helium.Entities`, reflected respectively under the `Assets\Scripts\Core` and `Assets\Scripts\Entities` directories.

### Helium.Core.GameController

Under _Core_, we have the **GameController** which is responsible for maintaining the game instance. This class contains three configurable constants:

1. `MAX_BALLOONS`
    - This is an integer field that allows us to set the maximum number of balloons to be spawned for the level.
    - This should be a multiple of `BalloonClusterController.BALLOONS_PER_CLUSTER`, seeing as all balloons are spawned as part of a cluster.
2. `INITIAL_LIVES`
    - This is an integer field that determines the number of lives the player initially has at the start of the game.
3. `SPEED_UP_FACTOR`
    - This is a float field which defines the velocity multiplier for balloons and clusters.
    - When 80% of `MAX_BALLOONS` have been popped, all balloon and clusters instances have their velocity values multiplied by this value.

### Helium.Entities.BalloonClusterController

Under _Entities_, we have the **BalloonClusterController** which is responsible for handling clusters of balloons. This class contains 4 configurable constant values:

1. `BALLOONS_PER_CLUSTER`
    - This is an integer field that defines how many balloons should be spawned for a single cluster instance.
2. `MIN_BALLOONS_PER_CLUSTER`
    - This is an integer field that defines how small a single cluster can be.
    - This value becomes significant when a user shoots a cluster, and that cluster splits apart into smaller clusters, each of which are half-the-size of the original. If the resulting split means that a cluster ends up with a value below this constant, then the balloons are freed from their cluster.
3. `BALLOON_PREFAB_PATH`
    - This is a string field referencing the path of the desired Balloon prefab to use.
4. `CLUSTER_PREFAB_PATH`
    - This is a string field referencing the path of the desired BalloonCluster prefab to use.

### Helium.Entities.Player

Under _Entities_, we have the **Player** entity, representing the player character. This class contains 6 publicly accessible attributes,

1. `rotationForce`
    - This is a float field dictating how fast the character rotates about its axis when the horizontal movement controls are activated.
2. `translationForce`
    - This is a float field dictating how fast the character moves/translates in the world when the the vertical movement controls are activated.
3. `shotDelay`
    - This is a float field dictating the delay between individual shots fired by the player. Particularily, this affects how many "bullets" are fired if the Fire-control is held down.
4. `immunityTime`
    - This is a float field dictating the amount of time the player is immune to balloons upon initial spawning. The default is 3 seconds.
5. `multiShotTime`
    - This is a float field dictating the maximum length of time the player can make use of the multi-shot feature. The default is 3 seconds.
6. `bullet`
    - This is a GameObject reference pointing to the Bullet prefab to be used.
    
## Running the Game

The game was created in **Unity 4.6b17** which seems to lack backwards compatibility with earlier releases of Unity. Please run the game with the latest _beta_ release of Unity to ensure proper functioning.

There is one scene (`Game`) that may have to be loaded upon opening the project.

### Game Controls

- Movement
    + The character may be moved via the traditional `W A S D` keys as well as the directional keys.
- Shooting
    + Shooting is accomplished via the `Space`, `Ctrl`, and `Mouse Left-Click` keys.
    + Further, the special "multi-shot" mode may be activated by holding down the `Left-Shift` key while shooting. Note though, that this mode can only be used **once** and only up to the duration noted by `Helium.Entities.Player.multiShotTime` in seconds (default value is 3 seconds.)