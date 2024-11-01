INCLUDE Utility.ink
->main
===main===
When we boarded the merchant ship, one of my men brought the captain of the ship to me. Apparently their most valuable cargo was a map describing a ship's route. The condition of the map is not very good, so I immediately had a copy made so as not to lose any clues. The hunt for treasure is just the thing to keep the crew happy. A common thread that we can follow and gain enormous wealth in the process, just how wonderful the pirate life can be.
But we have to leave our current waters as quickly as possible. There are many pirates here and I don't want to share the treasure with any of them. My navigator sets course according to the map and our journey begins.
We are stopping at a small island. We discovered a cave system that might be worth exploring. But some of my men are scared, as they claim to have heard howling from inside. They think ghosts live in the cave and we shouldn't disturb their peace. Are they just scared or is there something to it?
*[Explore caves]->Erforschen
*[Continue travelling]->Weiterziehen

===Erforschen===
~Unity_Event("booty")
I have sent a squad of my crew to explore the cave system. To take some of the fear out of them, I promised the volunteers an extra ration of rum. Some time later they returned with some crates of gold, as it turns out the cave was an active smuggler's hideout. My men managed to steal a few crates undetected. What a discovery! (+50 Booty)
->endScene

===Weiterziehen===
I gave in to the complaints of my crew. It wouldn't be a good time to lose people to ghosts or other dangers in a cave. Not now that we're about to embark on such an exciting quest. As we set sail, my crew's relief was obvious.
->endScene

===endScene===
A few days later, what was bound to happen happened: another pirate ship came after us. It's not unusual to take what you want in these waters. We don't even stop at other pirates. Usually you wait before a pirate ship takes a merchant ship or naval cruiser and then strikes. You can then always be sure that their crew has been hit and there is something to take. But this isn't likely to be such an operation, I would have noticed if they had been following us since the merchant ship. To the cannons, the battle begins!
~Unity_Event("endScene")
-->END