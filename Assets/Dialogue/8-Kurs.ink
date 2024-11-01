INCLUDE Utility.ink
->main
===main===
Today, I took a look at the map with my navigator, and we have almost covered the entire route of Silver's ship. So, it won't be long before we find the wreck and can recover the treasure. I don't want to reveal it to the other men just yet, as it could cause unrest. The greed of a pirate should not be underestimated. Now, I face a choice. The waters ahead are not very deep, and the ship could suffer damage if we strictly follow the map. However, the detour would take several days, and without a doubt, some of my men will question my decision.
*[Sail ahead]->Schaden
*[Take the detour]->Umfahren

===Schaden
~Unity_Event("damage")
As expected, the water is just barely deep enough to follow the route. We are making progress, but the hull has suffered damage. The nearest port is still a few days away. We've managed to stabilize the situation, but it's not ideal. With all the battles we've already fought, it's only a matter of time before the next Navy ship comes to attack us. (-3 Shiphealth next Battle)
->endScene

===Umfahren
I ordered the detour. It would be foolish to take unnecessary risks now. Maybe it would have worked out, maybe not. The crew doesn't understand my decision, but I'm certain it's the best for everyone. However, unrest is spreading. Some are already openly questioning if I'm still the right person for the position of captain. I will show them that I am worthy. Once we have the treasure on board, no one will question me anymore. (-1 Command Power next Battle)
~Unity_Event("detour")

->endScene

===endScene===
This time the navy really seems to mean business, with a frigate and a warship heading towards us. A warship is the largest class of ship that the navy has. A beast of the seas, more cannons than a man can count. Some of my men are starting to shiver. But the warship doesn't seem to be looking for a confrontation. Only the frigate seeks battle with us. Let's give the warship a show. Everyone to battle stations!
~Unity_Event("endScene")
-->END