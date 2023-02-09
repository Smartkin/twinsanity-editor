## Crash TwinSanity Editor

## Description
This is an editor of game files for the games Crash TwinSanity (PS2/XBOX) and Super Monkey Ball Adventure (PSP).  
(Editor originally created by NeoKesha)  
  
The following formats are supported:  
- .BD/.BH (PS2 file archives) extraction and rebuilding  
- .MB/.MH (PS2 music archives) extraction and rebuilding  
- .RM2 (PS2 level resources) editing  
- .SM2 (PS2 scenery resources) editing  
- .RMX (XBOX level resources) editing  
- .SMX (XBOX scenery resources) editing  
- .RM2/.SM2 editing support of the Crash TwinSanity demo  
- .RM (PSP level resources) limited editing  
- .SM (PSP level resources) limited editing  
  
## Current roadmap
Here's the list of what needs to be implemented:
- [x] Model viewing
- [x] Collision viewing
- [ ] Scenery viewing
- [x] Sound playback
- [x] Fully functional editing GUI
- [ ] Ditch any unnecessery dependencies (OpenTK should suffice for sounds and graphics)
- [ ] General code refactoring and rewriting (having 6k lines of code doesn't seem practical)
- [ ] More API optimizations?
- [ ] A lot of other smaller stuff

## Compiling
Just open your project in Visual Studio 2017 and compile it. (requires .NET Framework 4.7)
