# MaiMai-VR
Open Source VR environment for MaiMai DX or above

# Changes in this fork
- Improved Touch Detection
- Improved Captured Window framerate


# About this fork
- This project is a fork of HelloKS's Maimai-VR project. 
- Switched window capture package to: https://github.com/hecomi/uWindowCapture
- Touch communication protocol: https://github.com/Sucareto/Mai2Touch
- !!Currently ONLY SUPPORT OCULUS LINK/AirLink!! It May support Quest 1 controller but the hitbox is made for Quest 2 controller.

Please support your local arcade if you can!

# Declaimer
- Please note this project is non-profit and has no affiliation with SEGA.
- Do not use this in commercial/profitable scenarios.

# Changelog
Please see Changes.md

# How to use
- Get Maimai DX somehow and do some configurations
- Download [latest version of Maimai-VR](https://github.com/xiaopeng12138/MaiMai-VR/releases)
- Download [com0com](https://storage.googleapis.com/google-code-archive-downloads/v2/code.google.com/powersdr-iq/setup_com0com_W7_x64_signed.exe)
- Install com0com
- Configure com0com to bind COM3 and COM5
- Set "DummyTouchPanel=1" to 0 in some file 
- Run maimai in window mod by modifiy some batch file with setting "-screen-fullscreen 0 -screen-width 1170 -screen-height 1050"
- Then start maimaivr
- Enable somehow the maintenance mod of your maimai then exit maintenance mod.

# ToDo
- Add more user-adjustable settings (button audio volume etc.)
- Add support for SteamVR and Oculus Link (IDK how to go back to SteamVR now XD)
- Add support for multiple controllers (I can't debug other controllers so)
- Update model to dx version

Huge thanks to HelloKS and derole1
