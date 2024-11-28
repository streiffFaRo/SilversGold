INCLUDE Utility.ink
->main
===main===
A pirate is a very simple being; he loves his freedom and takes what he wants. He doesn’t need much to live — just rum, gold, and the sea. When we anchored the ship at a trading port to replenish our supplies, <color="red">some of my men approached me and spoke of a plantation they wanted to raid</color>. They said there was plenty to loot and that the risk was manageable. It’s not uncommon for some of my men to come to me with such a request. While I don’t want to stand in the way of their ambitions, I know that if things go wrong, it <color="red">could lead to unnecessary losses</color>. Still, with our ship remaining here until tomorrow morning, it wouldn’t cost us any time to let them plunder the plantation.
*[Send Raiders]->Raub
*[Continue travelling]->Weiterziehen

===Raub===
~Unity_Event("raid")
<color="red">I agreed to the plan</color> and pointed out that it would be advantageous for them to strike under the cover of night and try to avoid the guards. While the raiding party set off, I took care of the purchases in town. There were also some repairs to be done after the recent battles. When I was about to retire for the evening, one of my gunners woke me. Shocked, he told me that the raiding group had returned, but not all of them survived, and they hadn’t made off with any loot. Apparently, the whole group had been caught by a patrol, and during the fight, <color="red">one of our men didn’t make it</color>. What a disaster! <color="red">(-1 Card)</color>
->endScene

===Weiterziehen===
<color="red">I decided not to agree to the plan</color>. It’s an unnecessary risk. I made it clear to them that other opportunities will present themselves, and once we have the treasure in our hands, a simple plantation will be nothing in comparison. The raiding party followed my judgment, even though they would have liked to attack the plantation. Instead, they helped with the supplies and repairs to the ship.
->endScene

===endScene===
The next morning, we set off again into the open sea. Soon we would leave the troublesome waters behind. <color="blue">We are heading into a region controlled by the Navy — ships that serve the King</color>. I’m not sure if I’d rather face them than the mostly poorly organized pirate crews that have attacked us so far. <color="blue">It seems we’re about to get one last taste of a pirate attack</color>. A ship with crossed sabers on its banner is heading towards us, preparing for battle. But we will defy this lot as well. Attack!
~Unity_Event("endScene")
-->END