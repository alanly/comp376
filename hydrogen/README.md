# README for Hydrogen

COMP 376 Assignment 3

Alan Ly (6293484)

## Game

The game itself differs from the game specifications slightly. Rather than balloons, the game has _atoms_, which grow when struck by a bullet. They grow to a point where they explode in a particle conflagaration. A hot-air balloon passes by and drops spiked bombs at set intervals.

## Code & Configuration

The scripts are separated into two namespaces: `Hydrogen.Core` and `Hydrogen.Entities`, with the latter's files being separated under the `Assets/Scripts/Entities` directory.

### Hydrogen.Core.GameController

The `GameController` maintains the game instance, and provides for a number of configurable parameters via the Unity Editor, under the associated `GameObject`.

1. `TotalNumberOfLives`
	- This integer value specifies the number of life the player has before the game ends.
2. `TotalNumberOfAtoms`
	- This integer value specifies the number of atoms that will be spawned in total during the game. Once the player shoots down a number of atoms/bombs equivalent to this value, the game ends.

### Hydrogen.Entities.Bullet

The `Bullet` script maintains behaviour and configuration for bullets fired by the player.

1. `firingForce`
	- This float value specifies the force at which bullets are shot at, from the player.
2. `maxLifetime`
	- This float value specifies the time in seconds before a bullet is destroyed if it free-flies.

### Hydrogen.Entities.HotAtomBalloon

The `HotAtomBalloon` script maintains behavour for the hot-air balloon which shoots spiked bombs at set intervals during the game.

1. `TranslationSpeed`
	- This float value represents how quickly the hot-air balloon travels across the field.
2. `TimeBetweenAtoms`
	- This float value represents the time between each "bomb" thrown by the balloon, as it travels across the field.

### Hydrogen.Entities.Player

The `Player` script maintains behaviour concerning the player entity.

1. `translationForce`
	- This float value represents the force at which the player can move around the field (via the W A S D keys).
2. `lookAroundSpeed`
	- This float value represents the force at which the player can look around the scene (via their Mouse axis).
3. `maxVerticalAngle`
	- This float value specifies the maximum angle the player can look upwards and downwards via their Mouse axis.
4. `timeBetweenShots`
	- This float value limits the amount of bullets the player can shoot off, imposing a fixed delay in between each shot.

### Hydrogen.Entities.Atom.Atom

The `Atom` script maintains behaviour concerning the basic Atoms.

1. `initialHealth
	- This integer value represents the number of shots that an Atom can sustain, before it explodes.
2. `hitPoint`
	- This integer value represents the value of hitting an Atom with a bullet. The game score is incremented by this amount for a single hit.
3. `killPoint`
	- This integer value represents the value of popping an Atom (i.e. once the player successfully shoots the atom `initialHealth` amount of times.)
4. `growthStep`
	- This float value specifies the size by which the atom grows on each hit.
5. `growthTime`
	- This float value specifies the speed at which the atom grows.
6. `moveSpeed`
	- This float value specifies the speed at which the atom moves around the field.

### Hydrogen.Entities.Atom.HotAtom

The `HotAtom` script maintains the behaviour of the "bombs" dropped by the hot-air balloon.

1. `KillPoint`
	- This integer value represents the value of popping a HotAtom with a bullet. The game score is incremented by this amount for a single hit.

### Hydrogen.Entities.Atom.RevolvingParticles

The `RevolvingParticles` script maintains the animation of the revolving particles associated with an Atom.

1. `rotationSpeed`
	- This float value specifies the speed at which the particle revolves around the atom.
2. `rotationAxis`
	- This float value specifies the axis on which the particle rotates around.


## Running the Game

The game was created with Unity 4.5.5. The `Game` scene file may have to be loaded prior to running.

### Game Controls

- Movement
	* The character may be translated around the field via the `W A S D` keys.
	* The character may look around the field by using their mouse.
- Shooting
	* Shooting is accomplished via the `Ctrl` and `Left-Button` mouse clicks.
	