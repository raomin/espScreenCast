# espScreenCast
A project just for fun to cast your screen - well more 8x8 pixels under your mouse cursor- to a ws2812b led matrix on an esp32 over wifi.
You can also scroll text!

![Alt Text](https://github.com/raomin/espScreenCast/blob/master/doc/preview.gif) ![Alt Text](https://github.com/raomin/espScreenCast/blob/master/doc/Screenshot.png) 

Note: actual framerate is much better, just this gif has a low one to keep it small.

## Building the projects
1. open the .sln file with Visual Studio to build it. (or get it in the Release section of this page)
2. open the `espScreenCast-pioProject` folder in Platformio
3. Set the ssid and password in `main.cpp` ; upload to an esp32 and note the IP address from the COM monitoring

## Running
1. run `espScreenCast.exe` (built or downloaded from the realease
2. setup the IP of your esp32 running
3. check the `Sending` chackbox, you should see some image coming!

Have fun and let me know what you do with this.

raomin [at] protonmail.com
