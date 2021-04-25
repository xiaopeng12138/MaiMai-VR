# Changelog

## Current version
0.6
- Unity changes
  - Unity updated to 2019.4.25f1
- Misc VR releated changes
  - Added teleportation. Press thumb sticks to teleport. it might come in handy if you can't play in room scale mode.
  - Fixed player orientation. No longer need to turn back around to see cabinet.
- Adjusted collider for Q2 controller
  - Special thanks to KonaKona on Discord for bug report and fix
  - Because of this, playing this fork on openvr compatible headsets other than Quest 2 will face some issue. See #3 for more info.
- Changed splash image (a.k.a "Made with Unity")
- Implemented more LED hooks
  - Now button lights on player 2 side and both body lights works.

## Old versions

0.5
- Unity and VR libraries changes
  - Unity updated to 2019.4.19f1
  - VR library changed to OpenVR Unity XR plugin (v1.1.4) + SteamVR unity plugin (v2.7.2)
- Changed cabinet model
  - Model is based on [Club SEGA](https://www.pm3d-animation.fr/pages/3d-space/club-sega.html#3dspace) project, changed some textures
- Adjusted button & touch trigger area
  - Button trigger area is smaller than previous version
  - Touch trigger is more arcade accurate
- Changed touch area keymap for MaiSense
  - For this fork, you need to use [MaiSense](https://github.com/SirusDoma/MaiSense) otherwise touch will not work at all.
- Changes for Oculus Quest 2
  - Controller vibration no longer go crazy
  - Adjusted controller collider position for Q2 controller model
  - Other openvr compatible headsets are still works
- Button LED hook reworked
  - Buttons are now sync with game, just like as it should be
- Some feature removed
  - Maimai camera and its recording function (RockVR)
  - Multiplayer function (P2 is not working at all - MaiSense limitation)
  - Graphics quality selection window at startup (Unity removed this in recent version)
  - Decoration cabs removed

0.4
- Oculus headsets should work now

0.3
- Switched support from GreeN to FiNALE
- Updated cabinet artwork to fit the FiNALE theme
- Touch input now mostly works (can sometimes be a bit dodgy)
- LED hooks implemented (may or may not lag the environment, didnt lag too bad for me)
- You can now use any part of the controller to hit buttons
  
0.2
- Input lag greatly reduced
- VJoy/UCR no longer required, virtual buttons can now interface directly with the game
- 2 player is supported HOWEVER you would need to somehow get a 2nd VR headset in the game or something unless you wanna try play 
  doubles with yourself

0.1
- First version