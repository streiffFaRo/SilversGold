INCLUDE Utility.ink
->main
===main===
After the soldiers were defeated, we only really realised what kind of ship it was. It was a slave ship. That's nothing unusual for the navy. One of my men claims that slaves can be sold very expensively and that this could be to our advantage. Others of my crew urge me to release the slaves at the next harbour. They say that every man should have the opportunity to choose his own fate. I am faced with another decision. We could certainly use the money from the slave sale to upgrade the ship, which will be necessary for the coming confrontations with other frigates. What should I do?
*[sell them(100 Booty)]->verkaufen
*[free them]->befreien

===verkaufen===
~Unity_Event("sell")
I decided to exchange the slaves for good money at a sugar plantation. It was nothing personal, but we can make good use of the money. If we are defeated, the freed slaves won't be able to help us. So at least we were able to gain an advantage for ourselves and we weren't the ones who drove these people into slavery, rather we passed them on to an interested party.
->endScene

===befreien===
~Unity_Event("free")
Most of the crew wanted to see the slaves free and I saw that as the right thing to do. I, who strive for freedom myself, felt connected to them. Now they can do whatever they want. My decision was very popular with the crew. As a pirate, you don't just rob, sometimes you're also the good guy in the story. (+1 Command Power next Battle)
->endScene

===endScene===
I hope the slaves don't talk too much about us. If the navy finds out that we also defeated a slave ship, that could mean additional trouble. We now need a bit of luck to find the wreck of Captain Silver as soon as possible. My navigator reports another sighting of a royal frigate. Well, I guess that's our fate, let's show those royalists that they'd better leave us alone.
~Unity_Event("endScene")
-->END

