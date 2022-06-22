***TrekMe  ---Windows Phone 8 and higher tracking app***  

This is final version 1.4.6.2 as released on Windows Phone Store (available only for Windows Phone 10 devices).  

You can sideload here released xap on Windows Phone 8 and 8.1 devices, if you're still in possesion of one :)  

The is no more Microsoft store for Windows 8 and 8.1 devices, please check some threads like this one guide to sideload on Windows Phone 8.1:
https://stackoverflow.com/questions/41239377/install-xap-file-locally-in-windows-phone-8-1-device

The point is that you need SDK and registered account to be able to sideload. On some older Windows Phone 8 devices it may be possible just to copy the XAP file locally and install.

Windows Phone Store location (only for WP10):
http://www.windowsphone.com/s?appid=f519dba9-5601-4691-a614-2df33604452c)](http://www.windowsphone.com/s?appid=f519dba9-5601-4691-a614-2df33604452c

Other app locations:

source code at GIT: https://github.com/zbabac/TrekMe.git

or at SF: git clone ssh://zbabac@git.code.sf.net/p/trekme/code trekme-code

The app uses maps stored on the windows phone, so it can be used offline (no Internet connection).
It uses language that it set on the phone (if supported, by the app localization), otherwise it is set to English.
Localizations implemented:
- German
- Spanish
- French
- Italian
- Norwegian (Bokmål)
- Polish
- Portuguese
- Serbian

Latest version 1.4.6.2

Content:
 - Change logs
 - Known issues
 - Prerequisities
 - Features

'''Licensed under MIT license, so you are free to use the source code for any purpose you like, open source or commercial.
Please, don't forget to mention me in your work. I hope that you will learn a lot, as I have learned from others.'''

Change log since v1.4.6.2:
	- added 3 new features
	- long press on map shows pin with coordinates (longitude, latitude)
	- tap on that pin copies coordinates to the clipboard, tap anywhere else removes the pin
	- added page that shows statistics for every run and saves it
	- fixed coordinates decimal format
	- added language Italian

Change log since v1.3.6:
	- added new Privacy policy regarding location (required by Microsoft)
	- fixed crash issues for some language localizations

Change log since v1.3.3:
	- added automatic settings save, even when app exits
	- added settings option to use miles and feet instead of km & m
	- localization for French, Spanish, Portuguese, Polish, German
	- minor UI fixes

Change log since v1.2.0:
	- added localization for Norwegian
	- Settings: Map pitch and rotation
	- fixed UI when light theme used

Change log since v1.1.0:
	- added About and Settings page: Map Pitch configurable
	- UI improvements and fixes

Change log since v1.0.0:
	- this is first stable version, tested and working on Samsung Ativ S
	
Known issues:
At start, first couple of lines may show inaccurate location (several red lines drawn on the map missing the target by few hundred meters). Accordingly, speed and distance may be inaccurate. This happens when app is started indoors, so no good GPS signal exists.

So, to have accurate reading start app only when you are outdoors!

Prerequisities:

http://www.windowsphone.com/s?appid=f519dba9-5601-4691-a614-2df33604452c

Features:
- 2 pivot items are displayed: one with details, other with map
- swiping switches between pivot items
- details pivot shows GPS location data, speed, avg. speed, distance covered, run time, break time
- run can be paused
- map pivot shows the map where line is drawn showing your movements
- starting point is shown with red or blue circle
- pause point is shown with gray circle
- stopping point is shown with green circle
- long press on the map shows the pin with geo coordinates, tap on that pin copies the coordinates to the clipboard, you can share them for example via SMS or another application
- every run is saved to the file and shown at "runs" page (click at button located at app bar)
- settings page options: map pitch (0-70), map auto rotate option (or fixed north heading), use miles and feet (instead of m and km)
- settings are saved automatically so that they are not lost after app exits
- no ads, only map, location and data capabilities needed
