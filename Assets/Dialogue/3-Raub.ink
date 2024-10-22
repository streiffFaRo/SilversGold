INCLUDE Utility.ink
->main
===main===
A pirate is a very simple being; he loves his freedom and takes what he wants. He doesn’t need much to live—just rum, gold, and the sea. When we anchored the ship at a trading port to replenish our supplies, some of my men approached me and spoke of a plantation they wanted to raid. They said there was plenty to loot and that the risk was manageable. It’s not uncommon for some of my men to come to me with such a request. Basically, I don't want to stand in the way of my men's wishes, but if things go wrong, it would result in unnecessary losses. However, since we won't set sail again until tomorrow morning anyway, we wouldn't lose any time if they plunder the plantation.
*[Send Raiders]->Raub
*[Continue travelling]->Weiterziehen

===Raub===
~Unity_Event("raid")
I agreed to the plan and pointed out that it would be advantageous for them to strike under the cover of night and try to avoid the guards. While the raiding party set off, I took care of the purchases in town. There were also some repairs to be done after the recent battles. When I was about to retire for the evening, one of my gunners woke me. Shocked, he told me that the raiding group had returned, but not all of them survived, and they hadn’t made off with any loot. Apparently, the whole group had been caught by a patrol, and during the fight, one of our men didn’t make it. What a disaster! (-1 Card)
->endScene

===Weiterziehen===
I decided not to agree to the plan. It’s an unnecessary risk. I made it clear to them that other opportunities will present themselves, and once we have the treasure in our hands, a simple plantation will be nothing in comparison. The raiding party followed my judgment, even though they would have liked to attack the plantation. Instead, they helped with the supplies and repairs to the ship.
->endScene

===endScene===
The next morning, we set off again into the open sea. Soon we would leave the troublesome waters behind. We are heading into a region controlled by the Navy—ships that serve the King. I’m not sure if I’d rather face them than the mostly poorly organized pirate crews that have attacked us so far. It seems we’re about to get one last taste of a pirate attack. A ship with crossed sabers on its banner is heading towards us, preparing for battle. But we will defy this lot as well—attack!
~Unity_Event("endScene")
-->END