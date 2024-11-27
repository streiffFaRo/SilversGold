INCLUDE Utility.ink
->main
===main===
Today, I took a look at the map with my navigator, and we have almost covered the entire route of Silver's ship. So, <color="blue">it won't be long before we find the wreck and can recover the treasure</color>. I don't want to reveal it to the other men just yet, as it could cause unrest. The greed of a pirate should not be underestimated. Now, I face a choice. <color="red">The waters ahead are not very deep, and the ship could suffer damage</color> if we strictly follow the map. However, <color="red">the detour would take several days</color>, and without a doubt, some of my men will question my decision.
*[Sail ahead]->Schaden
*[Take the detour]->Umfahren

===Schaden
~Unity_Event("damage")
As expected, the water is just barely deep enough to follow the route. We are making progress, but <color="red">the hull has suffered damage</color>. The nearest port is still a few days away. We've managed to stabilize the situation, but it's not ideal. With all the battles we've already fought, it's only a matter of time before the next Navy ship comes to attack us. <color="red">(-3 Shiphealth next Battle)</color>
->endScene

===Umfahren
<color="red">I ordered the detour</color>. It would be foolish to take unnecessary risks now. Maybe it would have worked out, maybe not. <color="red">The crew doesn't understand my decision</color>, but I'm certain it's the best for everyone. However, unrest is spreading. Some are already openly questioning if I'm still the right person for the position of captain. I will show them that I am worthy. Once we have the treasure on board, no one will question me anymore. <color="red">(-1 Command Power next Battle)</color>
~Unity_Event("detour")

->endScene

===endScene===
This time the navy really seems to mean business, with <color="blue">a frigate and a warship heading towards us</color>. A warship is the largest class of ship that the navy has. A beast of the seas, more cannons than a man can count. Some of my men are starting to shiver. But the warship doesn't seem to be looking for a confrontation. <color="blue">Only the frigate seeks battle with us</color>. Let's give the warship a show. Everyone to battle stations!
~Unity_Event("endScene")
-->END