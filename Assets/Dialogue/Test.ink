INCLUDE Utility.ink
EXTERNAL poker(betAmount)

->main

===main
Blah Things Happen, Check Rounds
{Get_State("round") ==1: ->r1g} -> r1v

===r1g
We are in RoundsG
~Add_State("round",5)
->endScene

===r1v
We are in RoungsV
~Add_State("round",1)
->endScene

===endScene
It's all over now, watch me!
You won {Get_State("round")}c with two jacks in your hand!
~Unity_Event("endScene")
-->END
