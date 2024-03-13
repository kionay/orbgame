# orbgame
a recreation of a certain non-specific game of spheres

I wanted to learn more about Godot, and find that projects like recreating a known and popular game serves as a much better learning experience for me than going through popular tutorials.

This repository is something I intend to use to document my personal trials and progress learning bits of Godot.

For example, colliding two identical objects, each with a script that itends to emit a Signal that triggers the merging of the objects is challenging. Most tutorials I could find to do with Collision referred to a short-living enemy object and a long-living player object. The persisting player object could be relied upon, but the same cannot be said here.
It is too easy to end up processing both sets of collisions. A colliding with B causes A to merge with B at the same time B tries to merge with A. If this is allowed then two duplicates are placed at the same merge location, causing another merge and a runaway explosion of elements until they hit their limit.
To prevent this I leaned on two aspects of Godot.
1. simultaneous signal activation may both _try_ to happen in the same frame, but they're processed synchronously, allowing me to earmark subsequent merges as forbidden.
2. Godot allows, during physics processing, objects to be queued for deletion on the next game frame. Combine this with checking if objects are queued for deletion make for a gate to prevent both sets of collision from creating objects.

I'd also like to mention that I had a lot of issues managing delegated functions that might happen at the end of physics processing, which required more null-checking than I would've liked in the core logic.

If I make future games I'll more-than-likely try to do them in GDScript, to avoid the exceptions with signal processing and emitting in C# scripts. Also, content online tends to refer to GDScript. Godot is still at the point where C# feels like a second-class citizen compared to GDScript.

![The current state simple shapes and mechanics.](https://github.com/kionay/orbgame/Example%20Images/example_board.png)

I find that it is tempting to add far too much to global autoloaded scripts and letting one file handle too much.

Uses of Godot after this project I should be mindful to allow objects to handle decent amounts of their own logic.

Also, there seems like a great amount of potential in organizing global handlers for events, and creating an efficient event network using it could be either a really good or really bad idea for a larger project.

![The current state of GAME OVER.](https://github.com/kionay/orbgame/Example%20Images/example_gameover_screen.png)

