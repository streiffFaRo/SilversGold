INCLUDE Utility.ink
->main
===main===
~Unity_Event("checkBooty")
After we defeated the navy soldiers, I had one of the ship's officers questioned. <color="blue">Apparently they were given specific orders to attack our ship and capture the treasure map</color>. I think one of the merchants we took it from must have reported it. Perhaps the map was even intended for the navy. One thing is for sure: these waters are no longer as safe as I thought. There will certainly be others who try to get their hands on the map. Another problem is spreading, <color="red">some of my people are suffering from scurvy</color>, a disease. We have to buy fruit to cure them, but that will cost us. Only a few men are affected, is it worth it?
*[Buy Fruits (50 Booty)]->Markt
*[Continue travelling]->Weiterziehen
->endScene

===Markt===
As the well-being of my men is important to me and I can't have the treasure without them, I decided to stop at the next harbour and made my way to the marketplace. Fruit is very popular here. The prices are relatively high.
{Get_State("gotBooty") ==1: ->kaufen}->NoBooty

===NoBooty===
When I saw the prices, I realised that <color="red">we no longer had enough booty</color> to buy the fruit. But I was determined to do everything I could for my crew. So I spoke to the trader and tried to bargain him down. But he didn't give in and because there were a lot of guards at this market, it wouldn't have been worth attacking. So I had to return to my men empty-handed. The men were disappointed and some questioned my abilities as a leader <color="red">(-1 Command Power next Battle)</color>
~Unity_Event("sick")
->endScene

===kaufen===
Despite the high prices, <color="red">it was worth spending the money on my men</color>.  So I bought enough fruits for the whole crew , that we don't have any more cases of scurvy. Those affected should recover after a few days.
~Unity_Event("fruits")
->endScene

===Weiterziehen===
<color="red">I decided not to take a diversion to a harbour because of a few sick people</color>. Nobody died and we'll come across a harbour that sells fruit before things get really bad. Even if the sick don't approve of my decision, I'm the captain who has to see that we all get to our destination as quickly as possible, given that the navy knows about us. <color="red">(-1 Command Power next Battle)</color>
~Unity_Event("sick")
->endScene

===endScene===
I hope that another illness will not hinder us on our journey. If it is contagious, it could mean the premature end of our treasure hunt. As we all live very close to each other, this illness would spread like wildfire. But as I see it, the navy is still our biggest problem. <color="blue">A new ship with the royal banner is heading our way</color>. Men, get ready for battle!
~Unity_Event("endScene")
-->END