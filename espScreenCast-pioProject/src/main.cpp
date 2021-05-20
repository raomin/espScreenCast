#include "WiFi.h"
#include "AsyncUDP.h"

const char * ssid = "**ssid**";
const char * password = "**pass**";

AsyncUDP udp;

#include <FastLED.h>
#define NUM_LEDS 64

CRGBArray<NUM_LEDS> leds;

void display(AsyncUDPPacket packet){
  byte bmp[packet.length()] = {0};
  memcpy(bmp,packet.data(),packet.length());
  FastLED.setBrightness(bmp[packet.length()-1]);
  // Serial.printf("got packet with brightness %d",bmp[packet.length()-1]);
  
  for(int i = 0; i < NUM_LEDS; i++) {
    leds[i]=CRGB(bmp[i*3+2],bmp[i*3+1],bmp[i*3]);   
  }
  FastLED.show();

}

void setup() { 
    Serial.begin(115200);
    WiFi.mode(WIFI_STA);
    WiFi.begin(ssid, password);
    if (WiFi.waitForConnectResult() != WL_CONNECTED) {
        Serial.println("WiFi Failed");
        while(1) {
            delay(1000);
        }
    }
    if(udp.listen(2812)) {
        Serial.print("UDP Listening on IP: ");
        Serial.println(WiFi.localIP());
        udp.onPacket(display);
    }

 
  FastLED.addLeds<NEOPIXEL,32>(leds, NUM_LEDS);
  FastLED.setBrightness(4);
  
}

void loop(){ 

}
