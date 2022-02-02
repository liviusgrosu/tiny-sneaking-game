# Tiny Sneaking

##### Table of Contents  
* [Description](#description)
* [Controls](#controls)
* [Interesting Coding-Related Stuff](#interesting-coding-related-stuff)
* [Screenshots](#screenshots)

## Description

Tiny Sneaking is a game where you sneak around and steal loot all while hiding yourself from the guards. Traverse through levels by sneaking through rooms and hallways
that are protected by security grunts.

## Interesting Coding-Related Stuff

### FOV Custom Editor 

Guards have a simple custom editor script relating to their FOV. To guage how much a guard can see and how far they can see, a custom editor was created to indicate to the designer their FOV range. As seen in the screenshot below, a yellow arch represents the guards far sight distance.

![screenshot1](https://raw.githubusercontent.com/liviusgrosu/tiny-sneaking-game/main/Pictures/Enemy%20FOV%20Custom%20Editor.PNG?token=GHSAT0AAAAAABQCBGEN76BKOHZ5B4HGBABWYQDFHOQ)

Players caught in this range will cause the guard to go into 'sighting' stage were they need to assess the situation and consider investigating it. Players who find themselves in the red arch will put the guard into a frenzy by transition into the alert stage where they will attempt to deliever a whack. while in this state, the guards near sight will increase to the far sight range (as soon in the screenshot below)

![screenshot1](https://raw.githubusercontent.com/liviusgrosu/tiny-sneaking-game/main/Pictures/Enemy%20Alert%20FOV%20Custom%20Editor.PNG?token=GHSAT0AAAAAABQCBGEMKCZC43K6NVQN5ACOYQDFHBA)

### Patrol Paths

Patrol paths are designated paths that the guards take during play. Remaining in the patrol state, they will traverse each node and loop back to the first. Designers can configure a patrol path by instantiated new nodes in the prefab.

![screenshot1](https://raw.githubusercontent.com/liviusgrosu/tiny-sneaking-game/main/Pictures/Patrol%20Paths.PNG?token=GHSAT0AAAAAABQCBGEM5JBZG56R3CVQDBG2YQDFMRQ)

### See Through Shader

If the player model is hidden behind a model, a raycast will trigger this interaction and feed the players position into this shader and gets enabled.  

![screenshot1](https://raw.githubusercontent.com/liviusgrosu/tiny-sneaking-game/main/Pictures/Player%20Not%20Covered.PNG?token=GHSAT0AAAAAABQCBGEMLACGRVKMBTNRCEUAYQDFQMQ)
![screenshot1](https://raw.githubusercontent.com/liviusgrosu/tiny-sneaking-game/main/Pictures/PlayerCovered.PNG?token=GHSAT0AAAAAABQCBGENS3BMLXTI2HLYC7KGYQDFRNQ)

## Controls:

| Actions             | Key                   |
| ------------------- | --------------------- |
| W/A/S/D             | Move Around           |
| Shift               | Sprint                |
| Right Click         | Rotate Camera         |
| Control + Aim       | Shift Camera          |
| Left Click          | Grab Loot             |

## Screenshots:

![screenshot1](https://raw.githubusercontent.com/liviusgrosu/tiny-sneaking-game/main/Pictures/Screenshot%201.PNG?token=GHSAT0AAAAAABQCBGENYASHYCG7RHNT4MKEYQDFXXA)

<br/>
<br/>

![screenshot1](https://raw.githubusercontent.com/liviusgrosu/tiny-sneaking-game/main/Pictures/Screenshot%202.PNG?token=GHSAT0AAAAAABQCBGENWXCHMS2E2Q75EYFIYQDFXXA)

<br/>
<br/>

![screenshot1](https://raw.githubusercontent.com/liviusgrosu/tiny-sneaking-game/main/Pictures/Level%202%20Overview.PNG?token=GHSAT0AAAAAABQCBGEMPWIZSNCEZ7AGSQQOYQDFXYA)

