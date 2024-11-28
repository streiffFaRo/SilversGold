INCLUDE Utility.ink
->main
===main===
~Unity_Event("chooseRdmCard")
Our journey is going better than expected so far. I was expecting more encounters with pirates. Besides, we only had to stand our ground against individual ships, there are also many pirates who command entire fleets. But there's something coming up today that I'm not looking forward to at all. <color="red">Last night my navigator fell asleep during his night shift</color> and we sailed the wrong course all night because of it. It's now up to me to decide whether to <color="red">send him over the plank or spare him</color>. If I let this go unpunished, it may make me look weak, and it could encourage further misbehavior among the crew. But is it truly fair to send him to his death for something as simple as falling asleep once? What should I do?
*[Punish]->Planke
*[Spare]->Weiterziehen

===Planke===
~Unity_Event("plank")
<color="red">I have decided to judge strictly</color>. It will show my crew that I'm prepared to be tough so that we can get our hands on this treasure. But I also feel a little sorry for my navigator, he was a good man and loyal to our cause, but he knew the rules. <color="red">(-1 Card)</color>
->endScene

===Weiterziehen===
~Unity_Event("spare")
<color="red">I have decided to spare him</color>. I want to give him a second chance. He is very loyal to our cause and he begged me for mercy, which I granted him. However, some of the crew didn't seem happy with this decision. I must expect that they will try to push the limits of what is allowed in the following days. I have to be careful. <color="red">(-1 Command Power next Battle)</color>
->endScene

===endScene===
The ship is back on track, but it has cost us unnecessary time and supplies to take this diversion. Soon we will have to set course for a harbour. <color="blue">A ship of the royal navy is following us</color>. I wonder what they want? It's not usual for them to attack neutral ships, even if they are in their waters. There is something fishy about that. <color="blue">They're catching up and seem to be heading for a confrontation</color>. So let's give them what they deserve: A watery grave!
~Unity_Event("endScene")
-->END