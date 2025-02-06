Authors: Simon Whidden and Yohan Kwak, Completed 12/8/2022.

Design:
Powerups - Powerups spawn on a timer with a maximum of 20 in the world. If the extra game mode is enabled, then
collected powerups increase snake speed in addition to their length.
Snakes - Snakes are 120 units long initially, every powerup collected grows them by 12 frame's worth of units.
Snake speed is set to 3 units by default, if the extra game mode is enabled, then it can be increased to a maximum of six
units per frame.
Walls - Our walls follow standard implementation as described in the PS9 Canvas Assignment page.

Features:
We have implemented proper world wrapping for snakes. When a snake's head reaches the world boundary,
we create a vertex at said boundary, one at the opposite boundary, 
and another vertex one frame's worth of snake's movement away from the opposite boundary.

We have implemented all forms of collsion, including self-collisions. Zig-zagging is allowed, 
and snakes are forbidden from making rapid 180 degree turns.

For snake collisions with other snakes, itself, and walls, a line precedes the head of the snake,
if this line intersects with any of the above objects, the snake dies. If two snakes collide at the same time,
the snake that joined sooner dies first.

We have implemented a robust server that can handle client disconnects and joins at any time.
We have implemented a proper spawn system that checks for open areas before spawning game objects.
This system also prevents snakes from spawning too close to the world border. This was done to prevent
possible bugs with world wrapping.

We have implemented an extra game mode. This gamemode is activated with the ExtraGameMode setting in the 
external settings file. When activated, powerups increase snake speed in addition to increasing their length.
Snake speed is 3 by default, and each powerup increases it by 0.1 up to 6 units per frame.

