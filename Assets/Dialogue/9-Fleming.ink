INCLUDE Utility.ink
->main
===main===
Shortly after the battle, a longboat with two navy soldiers arrived with a white flag. The white flag indicates that they are coming in peace, most likely to negotiate. One of the soldiers introduces himself as a captain in the service of an Admiral Flemming. <color="blue">The admiral demands our immediate and absolute surrender and the handing over of the treasure map</color>. In return, we are to receive a quick death. That doesn't seem like a very good offer to me. He doesn't seem to know who he's threatening. The captain seems to think we would accept this offer. How should I answer him? <color="red">Maybe I should make him the same offer or I could send him back his soldiers without their uniform and their flag with a nice skull and crossbones on it</color>.
*[Make the same offer]->Angebot
*[Mock him]->Verarschen

===Angebot===
<color="red">I have decided to hold up a mirror to this admiral</color>. He probably thinks we're a wild, run-of-the-mill crew of pirates, but he doesn't realise how many dangers we've braved to get our hands on this treasure. Perhaps the admiral will come to his senses and understand this counter-offer. I don't think he'll back down now, these royalists are all the same, but we certainly won't give up so close to the finish line!
->endScene

===Verarschen===
<color="red">I have decided to send this admiral a message in pirate style</color>. It should be the adequate response to such an offer. Perhaps he will reconsider his demands and come to his senses. However, I don't think that this snob will change his mind. These royalists are all the same. But we would be fools to give up so close to our goal.
->endScene

===endScene===
This is not our first battle and it will not be our last. I speak to my crew to encourage them once again. I tell them about our humble beginnings and our goal. <color="blue">Fleming's warship sets off shortly afterwards and sets a direct course for us</color>. He must not have liked my answer. But that won't be the end of us.
~Unity_Event("endScene")
-->END



