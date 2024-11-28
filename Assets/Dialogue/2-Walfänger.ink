INCLUDE Utility.ink
->main
===main===
~Unity_Event("presentWhaler")
On our trip today we came across <color="blue">a lone sailor whose boat had loaded a whale</color>. I ordered the ship to stop and take a longboat to him. He was very defensive at first, which was probably due to the fact that it's not exactly difficult to realise that we are pirates. But when I was able to make it clear to him that we didn't want to harm him and were just amazed at what a catch he had made. He says that he has also caught sharks and then sells the meat in the harbours. As he talks like this, <color="red">I wonder if it wouldn't be a good idea to recruit him</color> for our cause. Of course, I would also have to offer him a decent wage.
*[Recruit (30 Booty)]->TEST
*[Continue travelling]->Weiterziehen

===TEST===
I waited patiently until he had finished his story and then politely offered him the chance to become part of our crew.
{Get_State("gotBooty") ==1: ->Rekrutieren}->NoBooty

===Rekrutieren===
He replied hesitantly, but <color="red">when I offered him certain payment</color>, he agreed. I'm not sure but I think it was more the camaraderie than the gold that convinced him to join us. In any case, I'm very happy to have him with us. In bad times, we can rely on his stories to give us new courage. <color="blue">(+Whaler)</color>
  ~Unity_Event("whaler")
->endScene

===NoBooty===
He hesitated, when I offered him the rest of our booty he said that the offer honoured him but that <color="red">he made more money hunting whales</color>. I respected his decision. Despite this, it was a very interesting encounter. To see how a single man can conquer such a beast of the sea was impressive.
->endScene

===Weiterziehen===
<color="red">I decided against recruiting him</color>. It just wasn't worth the gold. Besides, I don't really trust him either. He's quite a talkative fellow and people with a loose mouth can be dangerous when you're looking for treasure. Despite this, it was a very interesting encounter. To see how a single man can conquer such a beast of the sea was impressive.
->endScene

===endScene===
The whaler was not the only one we met that day. A ship with a black banner chased us and caught up. <color="blue">It was another band of pirates who were after us</color>. My men were determined and unimpressed. All hands on deck, the battle begins!
~Unity_Event("endScene")
-->END