TrekMe  ---Windows Phone 8 and higher tracking app

source code at GIT: https://github.com/zbabac/TrekMe.git


or at SF: git clone ssh://zbabac@git.code.sf.net/p/trekme/code trekme-code

Windows Phone Store location:
http://www.windowsphone.com/s?appid=f519dba9-5601-4691-a614-2df33604452c

Latest version 1.3.3

Content:
 - Change logs
 - Known issues
 - Prerequisities
 - Features

Licensed under MIT license, so you are free to use the source code for any purpose you like, open source or commercial.
Please, don't forget to mention me in your work. I hope that you will learn a lot, as I have learned from others.

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
- settings page options: map pitch (0-70), map auto rotate option (or fixed north heading), use miles and feet (instead of m and km)
- settings are saved automatically so that they are not lost after app exits
- no ads, only map, location and data capabilities needed