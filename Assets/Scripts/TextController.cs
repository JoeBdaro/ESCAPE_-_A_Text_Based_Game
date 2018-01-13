using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    //declarations of variables
    enum States {menu, cell, mirror0, mirror1, mirrorBroken, toilet0, toilet1, diedByCreature, killedCreature, sheets0, sheets1, door0, door1, freedom};
    States myState = States.menu;
    public Text text;
    public static List<string> playerInventory = new List<string>();
    float timeTillBleedout = 80.0f;



    // Use this for initialization
    void Start()
    {
        //font alignment to center
        text.alignment = TextAnchor.MiddleCenter;

        //beginning text
        text.text = "Press Space to wake up";

        //if user hits space it begins the game
        if (Input.GetKeyDown(KeyCode.Space))
        {
            text.fontSize = 30;
            myState = States.cell;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //player states will determine what state function needs to be called
        print(myState);
        if (myState == States.menu)
        {
            Start();
        }

        else if (myState == States.cell)
        {
            state_Cell();
        }
        else if (myState == States.sheets0)
        {
            state_sheet0();
        }
        else if (myState == States.sheets1)
        {
            state_sheet1();
        }
        else if (myState == States.mirror0)
        {
            state_mirror0();
        }
        else if (myState == States.mirror1)
        {
            state_mirror1();
        }
        else if (myState == States.mirrorBroken)
        {
            state_mirrorBroken();
        }
        else if( myState == States.toilet0)
        {
            state_toilet0();
        }
        else if (myState == States.toilet1)
        {
            state_toilet1();
        }
        else if(myState == States.door0)
        {
            state_door0();
        }
        else if(myState == States.door1)
        {
            state_door1();
        }
        else if(myState == States.diedByCreature)
        {
            state_diedByCreature();
        }
        else if(myState == States.freedom)
        {
            state_freedom();
        }
        else if(myState == States.killedCreature)
        {
            state_stabbedCreature();
        }

        /*special if statement that is only triggered if the player obtains the bloody shiv in his inventory in which his bleed out timer begins, 
        once the bleed out timer finishes the player dies, his inventory and progress is reset and is asked if they'd like to try again*/
        if (playerInventory.Contains("bloodyShiv"))
        {
            print(timeTillBleedout);
            timeTillBleedout -= Time.deltaTime;
            if(timeTillBleedout <= 0)
            {
                timeTillBleedout = 0;

                //If the player happens to have picked up the cell key and is not the door1 state nor killedcreature state then if the timer runs out the player dies from infection
                if (playerInventory.Contains("cellKey") && myState != States.door1 && myState != States.killedCreature)
                {
                    text.text = "YOU HAVE passed out of infection from your wounds \n\n" +
                                "Press R to continue";
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        playerInventory.Clear();
                        timeTillBleedout = 80.0f;
                        myState = States.menu;
                    }
                }
                //else if the player is in door1 state or killedcreature and timer goes to 0 then he dies from his collective wounds 
                else if (myState == States.door1 || myState ==States.killedCreature)
                {
                    text.text = "Although you beat the creature you passed out from your collective wounds \n\n" +
                                "Press R to continue";
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        playerInventory.Clear();
                        timeTillBleedout = 80.0f;
                        myState = States.menu;

                    }
                }
                else
                {
                    //if neither of theses outcomes the player just bleeds out
                    text.text = "YOU HAVE BLED OUT \n\n" +
                                "Press R to continue";
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        playerInventory.Clear();
                        timeTillBleedout = 80.0f;
                        myState = States.menu;

                    }
                }
            }
        }
    }

    #region state methods
    //main state hub that will decided what the state of the player is based on the key presses of the user and the items in the player's inventory
    void state_Cell()
    {
        text.fontSize = 20;
        text.text = "You wake up to find yourself in a dark cell, completely unaware of how you wound up there to begin with no memory and a freshly stitched wound on your neck and stomach as well as your fists. " +
                       "You get up on your feet with one clear goal, no matter how you got here, you NEED to escape. " +
                       "You notice a few items in the room some dirty sheets on a bed, a mirror on the wall, a toilet that seems to be clogged up with dirty liquid, " +
                       "and the door standing between you and freedom \n\n" +
                       "Press S to Look at the sheets \n" +
                       "Press M to look at the mirror \n" +
                       "Press T to look at the toilet \n" +
                       "Press D to look at the cell door";

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (playerInventory.Contains("rag"))
            {
                myState = States.sheets1;
            }
            else
            {
                myState = States.sheets0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            if(playerInventory.Contains("bloodyShiv") || playerInventory.Contains("shiv"))
            {
                myState = States.mirrorBroken;
            }
            else
            {
                myState = States.mirror0;

            }
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            if (playerInventory.Contains("cellKey"))
            {
                myState = States.toilet1;
            }
            else
            {
                myState = States.toilet0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            myState = States.door0;
        }
    }

    //State methods that will be called based on the state of the player
    void state_sheet0()
    {
        text.text = "You walk over to the dirty sheet stained with blood, and reeks of mildew. You notice however that there is a rectangular lump under the sheets pressed between the sheet and the mattress. \n\n" +
                    "Press I to inspect sheets \n" +
                    "Press R to return";
        if (Input.GetKeyDown(KeyCode.I))
        {
            timeTillBleedout += 30.0f;
            playerInventory.Add("rag");
            myState = States.sheets1;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            myState = States.cell;
        }
    }

    void state_sheet1()
    {
        if (!playerInventory.Contains("bloodyShiv"))
        {
            text.text = "You uncover the dirty sheets from the stained mattress and find a somewhat clean enough rag, you pick it up \n \n" +
                        "Press R to return";
            print(playerInventory.Count);
            if (Input.GetKeyDown(KeyCode.R))
            {
                myState = States.cell;
            }
        }
        else if (playerInventory.Contains("bloodyShiv"))
        {
            text.text = "You uncover the dirty sheets from the stained mattress and find a somewhat clean enough rag, quickly you pick it up and wrap your bloody fist in it, you just bought yourself some more time \n \n" +
                        "Press R to return";
            if (Input.GetKeyDown(KeyCode.R))
            {
                myState = States.cell;
            }
        }
    }

    void state_mirror0()
    {
        text.text = "You walk up to the mirror and get an idea, " +
            "\"if I am to break out this prison who knows what might be waiting out there for me, better break this mirror to make a makeshift shiv, sure hope I don't open my stitches in the process\" \n\n" +
            "Press B to take the risk and break the glass \n" +
            "Press R to return, it's too risky";
        if (Input.GetKeyDown(KeyCode.B))
        {
            myState = States.mirror1;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            myState = States.cell;
        }
    }

    void state_mirror1()
    {
        if (playerInventory.Contains("rag"))
        {
            text.text = "You pulled out your rag, wrapped up your fist in it and punched the mirror with as much force as your exhausted body can muster, good thinking in wrapping up your fist in that rag before you punched a hole in the mirror. " +
                        "You kneel down and pick up a pointy and large shard of glass and put it in your pocket just in case \n\n" +
                        "Press R to return";
            playerInventory.Add("shiv");
            if (Input.GetKeyDown(KeyCode.R))
            {
                myState = States.cell;
            }
        }
        else
        {
            text.text = "SHIT! you messed up big time, by breaking the glass with your bare hands many of the sharp glass shards were implanted in your bare fists, your stitches are open and you're bleeding profusely, IF ONLY YOU HAD SOMETHING TO WRAP YOUR HANDS IN BEFORE PUNCHING GLASS \n" +
                        "You picked up a pointy and bloody glass shard as a shiv, Hurry now I'd say you got about 2 minutes before bleeding out \n\n" + 
                        "Press R to return";
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerInventory.Add("bloodyShiv");
                myState = States.cell;
            }
        }
    }

    void state_mirrorBroken()
    {
        if (playerInventory.Contains("shiv"))
        {
            text.text = "You look at the fist sized hole in the mirror and the glass shards crackling below you. There's nothing to see here \n\n" +
                        "Press R to return";
            if (Input.GetKeyDown(KeyCode.R))
            {
                myState = States.cell;
            }
        }
        else if (playerInventory.Contains("bloodyShiv"))
        {
            text.text = "You look at the bloody fist sized hole in the mirror and the bloody glass shards crackling below you, then down at your bleeding fist, there's no time any minute now you might collapse \n\n" +
                        "Press R to return";
            if (Input.GetKeyDown(KeyCode.R))
            {
                myState = States.cell;
            }
        }
    }
    void state_toilet0()
    {
        text.text = "You approach the toilet and see what seems to be something shiny in the bile and filled and backed up toilet, should you reach in there to see what it is ? \n\n" +
                    "Press I to inspect \n" +
                    "Press R for \"NO THANKS\" and return";
        if (Input.GetKeyDown(KeyCode.I))
        {
            myState = States.toilet1;   
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            myState = States.cell;
        }
    }

    void state_toilet1()
    {
        if (!playerInventory.Contains("cellKey"))
        {
            if (playerInventory.Contains("bloodyShiv") && !playerInventory.Contains("rag"))
            {
                text.text = "You dip your bloodied bleeding hand in the backed up toilet full of bile and remains of feces probably not a good idea since your wounds are now infected and you have less time to live. " +
                            "On the bright side! you found a KEY THAT MIGHT LIKELEY BE THE KEY TO FREEDOM \n\n" +
                            "Press R to return";
                if (Input.GetKeyDown(KeyCode.R))
                {
                    playerInventory.Add("cellKey");
                    timeTillBleedout -= 30.0f;
                    myState = States.cell;
                }
            }
            else if (playerInventory.Contains("bloodyShiv") && playerInventory.Contains("rag"))
            {
                text.text = "You make sure your wound is sealed well in the rag before reaching in the toilet of bile, then you reach in and VOILA! A key most likely the key to your freedom, however your rag covering your wound is soiled with the bile from the toilet, this wont be good for your wound \n\n" +
                            "Press R to return";
                if (Input.GetKeyDown(KeyCode.R))
                {
                    playerInventory.Add("cellKey");
                    timeTillBleedout -= 10.0f;
                    myState = States.cell;
                }
            }
            else if (!playerInventory.Contains("bloodyShiv"))
            {
                
                text.text = "You take a deep breath and dip your hands in the toilet filled with bile and feces and VOILA a KEY TO YOUR FREEDOM! \n\n" +
                            "Press R to return";
                if (Input.GetKeyDown(KeyCode.R))
                {
                    playerInventory.Add("cellKey");
                    myState = States.cell;
                }
            }
        }
        else if (playerInventory.Contains("cellKey"))
        {
            text.text = "It's pretty much an overflown toilet of bile, can't believe you dunked your hand in there. Then again freedom has it's price \n\n" +
                        "Press R to return";
            if (Input.GetKeyDown(KeyCode.R))
            {
                myState = States.cell;
            }
        }
    }

    void state_door0()
    {
        if (!playerInventory.Contains("cellKey")){
            text.text = "Seems like the door is locked and won't budge, but there seems to be a keyhole from inside the cell, whoever must've locked you up in here wanted you to try and escape \n\n" +
                        "Press R to return";
            if (Input.GetKeyDown(KeyCode.R))
            {
                myState = States.cell;
            }
        }
        else
        {
            text.text = "Looks like that key fits the keyhole perfectly, before you turn the key and make the escape you think back if you need to do anything else before you make your escape \n\n" +
                        "Press E to escape \n" +
                        "Press R to return one more time";
            if (Input.GetKeyDown(KeyCode.R))
            {
                myState = States.cell;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                myState = States.door1;
            }
        }
    }

    void state_door1()
    {
        text.text = "You turn the key and shove the door open and start running for your life, sweet freedom or so you thought" +
                    "A strange disfigured creature with gaping holes for eyes and a circular mouth with multiple sets of razor sharp teeth stumbles out from the cell next door" +
                    "The creature unfortunately sees you lunges at you with inhuman speed and overwhelming strength sending you to the floor with him on top of you. " +
                    "You struggle as the creature's mouth gets closer and closer to your neck, as you lose your grip on fending the creature off, the creature plunges it's teeth into your shoulders, you can feel it's jaw's clench. \n\n" +
                    "Press Space to continue";

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (playerInventory.Contains("bloodyShiv") || playerInventory.Contains("shiv"))
            {
                timeTillBleedout -= 30.0f;
                myState = States.killedCreature;      
            }
            else
            {
                myState = States.diedByCreature;
            }
        }
    }
    void state_stabbedCreature()
    {
        text.text = "You suddenly get an idea and pull out your shiv from your pocket and with all your fury you plunge it into the beast's temple. " +
                           "The beast let's out a deep yelp and rolls off of you wringing in pain. unfortunately your wounds are really bad and you have very little time, IT'S TIME TO GE THE HELL OUT OF HERE! \n\n" +
                           "Press Space to get up and RUN!";

        if (Input.GetKeyDown(KeyCode.Space))
        {

            myState = States.freedom;
        }
    }

    void state_diedByCreature()
    {
        text.text = "you try punching the beast but it grabs your arm after the second blow and breaks it, it then lunges it's mouth at your throat and rips out your jugular, as you're bleeding profusely from your throat " +
                            "you start blacking out with the last image before everything fading to black is of the creature tearing into your stomach... and then darkness \n\n" +
                            "Press R to continue";
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerInventory.Clear();
            myState = States.menu;
        }
    }
    void state_freedom()
    {
        playerInventory.Clear();
        timeTillBleedout = 80.0f;
        text.text = "The creature is on the ground you run and notice a door, you push through it, this might be your escape. " +
                    "As the door flings open you see a stage, people are everywhere watching some screen... COULD IT BE .... THESE PEOPLE WERE WATCHING YOU ALL ALONG. " +
                    "You notice people in the crowd cheering, some are angry. before you can react you feel something pierce your skin. " +
                    "It seems you've been tranquilized. As everything starts to go dark, you hear the host shout \"THAT'S ALL FOR THIS RUN...GET YOUR BETS READY FOR ROUNDS 2!!\".....darkness falls upon you \n\n" +
                    "Press Space to continue";
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myState = States.menu;
        }
    }
#endregion
}

