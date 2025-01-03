INCLUDE Utility.ink
->main
===main===
When we boarded the merchant ship, one of my men brought its captain to me. <color="blue">Their most valuable cargo, it seems, was a map detailing a ship's route</color>. The map was in poor condition, so I had a copy made immediately to ensure we wouldn’t lose any vital clues. A treasure hunt is just the thing to keep the crew in high spirits — a shared goal that could lead us to unimaginable riches. How truly wonderful the pirate's life can be!
But we must leave these waters as quickly as possible. <color="blue">Too many pirates linger here</color>, and I have no intention of sharing the treasure with any of them. My navigator has set a course based on the map, and so our journey begins.
We are stopping at a small island. <color="red">We discovered a cave system</color> that might be worth exploring. But some of my men are scared, as they claim to have heard howling from inside. <color="red">They think ghosts live in the cave</color> and we shouldn't disturb their peace. Are they just scared or is there something to it?
*[Explore caves]->Erforschen
*[Continue travelling]->Weiterziehen

===Erforschen===
<color="red">I have sent a squad of my crew to explore the cave system</color>. To take some of the fear out of them, I promised the volunteers an extra ration of rum. Some time later they returned with some crates of gold, as it turns out the cave was an active smuggler's hideout. What a discovery! <color="red">(+50 Booty)</color>
~Unity_Event("booty")
->endScene

===Weiterziehen===
<color="red">I gave in to the complaints of my crew</color>. It wouldn't be a good time to lose people to ghosts or other dangers in the cave. Not now that we're about to embark on such an exciting quest. As we set sail, my crew's relief was obvious.
->endScene

===endScene===
A few days later, what was bound to happen finally did: <color="blue">another pirate ship came after us</color>. In these waters, it’s not uncommon to take what you want — whether from merchants, naval ships, or even fellow pirates. The usual tactic is to wait until a rival ship has plundered a merchant or navy vessel, then strike when their crew is weakened, ensuring there’s treasure to claim. But this doesn’t seem like such an operation; I would’ve noticed if they’d been trailing us since the merchant ship. To the cannons, the battle begins!
~Unity_Event("endScene")
-->END