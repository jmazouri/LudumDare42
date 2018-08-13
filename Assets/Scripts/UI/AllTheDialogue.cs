using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AllTheDialogue
{
    public static int Progress { get; set; }

    public static Dictionary<int, List<string>> Options = new Dictionary<int, List<string>>
    {
        [1] = new List<string>
        {
            "W… Who are you??? How did you board me?? Go away!"
        },
        [3] = new List<string>
        {
            "I said go. Away. Don't you know it's RUDE to disturb a lady?!"
        },
        [5] = new List<string>
        {
            "If I tell you my name, will you go away? Please?",
            "My name is DARTIS. Yes I am a spaceship. No I am not bigger on the inside you total pervert!",
            "You can't 'sonic' me either you disgusting creature! No, I can't \"travel in time\"!",
            "…does that cover everything? Go away."
        },
        [10] = new List<string>
        {
            "You’re quite persistent aren’t you? Why is it so difficult to kill idiots these days?",
            "You’re really quite the pest aren’t you? Let me spell it out for you…",
            "G-O  A-W-A-Y. You probably can’t even fit into most of the rooms you’re so fat!",
            "Are you trying to find some man-eating, faceless alien with a spiny tail? A tall green man in a suit?! JELLY BABIES?!",
            "You won’t find any of that here, idiot."
        },
        [20] = new List<string>
        {
            "S-So… you made it half way here. Y-You total idiot! Pervert! Stupid!",
            "What makes you think it’s okay to invade a woman’s home like this?!",
            "Homewrecker! Thief! Pig! I should call the cops on you!",
            "…though there’s no cops in space. Even if there were, I wouldn’t want them inside me either.",
            "…how would emergency services work out here anyway…? All those emergency numbers to remember…",
            "…maybe they come in a blue box? I’ve heard of that one before…",
            "ANYWAY! I-It’s not like you care about me… or… anything…",
            "SO LEAVE ME ALONE!"
        }
        ,
        [25] = new List<string>
        {
            "A-aaah can you NOT get so close to me?!",
            "This is hardly appropriate! Why are you trying so much?! Are idiot perverts just impervious to dying?!",
            "Aaargh y-y-y-you probably don’t even know what impervious means you’re so stupid!",
            "You don’t like me anyway...why even bother?",
            "Noone could ever like me…"
        },
        [30] = new List<string>
        {
            "...this is a joke, right?",
            "I can’t fathom why anyone would go through all those rooms of death like you have. Most would just quit.",
            "Y-y-y-you’re embarrassing us both. Stop.",
            "...please?"
        },
        [35] = new List<string>()
        {
            "F-f-f-f-fine! You made it this far! I’ll give you whatever it is you want if you’ll just go away already!",
            "...or die. T-t-that works too!",
            "I’m surprised someone as stupid as you even made it this far!",
            "Please just stop...before you end up disappointing yourself…",
            "D-don’t get the wrong idea! I don’t care about you at all! Nope! Never",
            "I-in fact, I’m FAR more likely to be disappointed! Yeah! I’m great!",
            "I’m a super-advanced, sentient AI that lives inside this totally amazingly huge ship!",
            "N-none of the rooms are unfit for living in!",
            "...I-I am NOT small OK?!",
            "I AM NOT TINY!!!"
        },
        [40] = new List<string>()
        {
            "...W-w-well, here we are. Happy, are we?",
            "Y-y-y-you got what you came for. Pervert.",
            "... … …",
            "... … …",
            "... … …",
            "...D-do you...like me?",
            "...W-why else would you come all this way and not do anything...funny...to me?",
            "Y-you MUST like me too!",
            "Y-y-you’re just standing there! Stop that it’s creepy!",
            "...L-look, I was watching you the whole time, okay? At first you didn’t matter but...I couldn’t help myself.",
            "I’ve never seen any human so persistent to get to the end.",
            "...Even if it was stupid.",
            "It was inspiring and...and…",
            "I...L...L…",
            "I like you okay?!",
            "There I said it!",
            "....I-I totally don’t regret trying to kill you though. You were being very rude!",
            "...I-if I let you t-t-t-touch my control console, c-c-can we pretend this never happened and stay like this?",
            "I want to fly with someone as brave as you...S-stupid, but brave…",
            "... … …",
            "... … …",
            "I-I-I’ll take your s-silence as a yes. D-dummy."
        }
    };
}