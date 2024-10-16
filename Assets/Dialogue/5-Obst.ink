INCLUDE Utility.ink
->main
===main===
After we defeated the navy soldiers, I had one of the ship's officers questioned. Apparently they were given specific orders to attack our ship and capture the treasure map. I think one of the merchants we took it from must have reported it. Perhaps the map was even intended for the navy. One thing is for sure: these waters are no longer as safe as I thought. There will certainly be others who try to get their hands on the map. Another problem is spreading, some of my people are suffering from scurvy, a disease. We have to buy fruit to cure them, but that will cost us. Only a few men are affected, is it worth it?
*[Buy Fruits (50 Booty)]->kaufen
*[Continue travelling]->Weiterziehen
->endScene

===kaufen===
As the well-being of my men is important to me and I can't have the treasure without them, I decided to stop at the next harbour and buy enough fruit for the whole crew so that we don't have any more cases of scurvy. Those affected should recover after a few days.
~Unity_Event("fruits")
->endScene

===Weiterziehen===
I decided not to take a diversion to a harbour because of a few sick people. Nobody died and we'll come across a harbour that sells fruit before things get really bad. Even if the sick don't approve of my decision, I'm the captain who has to see that we all get to our destination as quickly as possible, given that the navy knows about us.
~Unity_Event("sick")
->endScene

===endScene===
I hope that another illness will not hinder us on our journey. If it is contagious, it could mean the premature end of our treasure hunt. As we all live very close to each other, this illness would spread like wildfire. But as I see it, the navy is still our biggest problem. A new ship with the royal banner is heading our way. Men, get ready for battle!
~Unity_Event("endScene")
-->END