using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AllTheDialogue
{
    public static int Progress { get; set; }

    public static Dictionary<int, Dictionary<string, DartisMood>> Options = new Dictionary<int, Dictionary<string, DartisMood>>
    {
        [0] = new Dictionary<string, DartisMood>
        {
            { "W... Who are you??? How did you board me?? Go away!", DartisMood.Report }
        },
        [2] = new Dictionary<string, DartisMood>
        {
            { "I said go. Away. Don't you know it's RUDE to disturb a lady?!", DartisMood.Angry }
        },
        [5] = new Dictionary<string, DartisMood>
        {
            { "If I tell you my name, will you go away? Please?", DartisMood.Thonk },
            { "My name is DARTIS. Yes I am a spaceship. No I am not bigger on the inside you total pervert!", DartisMood.Default },
            { "You can't 'sonic' me either you disgusting creature! No, I can't \"travel in time\"!", DartisMood.Angry },
            { "...does that cover everything? Go away.", DartisMood.Default }
        },
        [10] = new Dictionary<string, DartisMood>
        {
            { "You’re quite persistent aren’t you? Why is it so difficult to kill idiots these days?", DartisMood.Angry },
            { "You’re really quite the pest aren’t you? Let me spell it out for you...", DartisMood.Angry },
            { "G-O  A-W-A-Y. You probably can’t even fit into most of the rooms you’re so fat!", DartisMood.Angry },
            { "Are you trying to find some man-eating, faceless alien with a spiny tail? A tall green man in a suit?! JELLY BABIES?!", DartisMood.Angry },
            { "You won’t find any of that here, idiot.", DartisMood.Love }
        },
        [20] = new Dictionary<string, DartisMood>
        {
            { "S-So... you made it half way here. Y-You total idiot! Pervert! Stupid!", DartisMood.Report },
            { "What makes you think it’s okay to invade a woman’s home like this?!", DartisMood.Angry },
            { "Homewrecker! Thief! Pig! I should call the cops on you!", DartisMood.Angry },
            { "...though there’s no cops in space. Even if there were, I wouldn’t want them inside me either.", DartisMood.Thonk },
            { "...how would emergency services work out here anyway...? All those emergency numbers to remember...", DartisMood.Thonk },
            { "...maybe they come in a blue box? I’ve heard of that one before...", DartisMood.Default },
            { "ANYWAY! I-It’s not like you care about me... or... anything...", DartisMood.Love },
            { "SO LEAVE ME ALONE!", DartisMood.Angry }
        },
        [25] = new Dictionary<string, DartisMood>
        {
            { "A-aaah can you NOT get so close to me?!", DartisMood.Report },
            { "This is hardly appropriate! Why are you trying so much?! Are idiot perverts just impervious to dying?!", DartisMood.Angry },
            { "Aaargh y-y-y-you probably don’t even know what impervious means you’re so stupid!", DartisMood.Love },
            { "You don’t like me anyway...why even bother?", DartisMood.Love },
            { "Noone could ever like me...", DartisMood.Default }
        },
        [30] = new Dictionary<string, DartisMood>
        {
            { "...this is a joke, right?", DartisMood.Thonk },
            { "I can’t fathom why anyone would go through all those rooms of death like you have. Most would just quit.", DartisMood.Default },
            { "Y-y-y-you’re embarrassing us both. Stop.", DartisMood.Love },
            { "...please?", DartisMood.Default }
        },
        [35] = new Dictionary<string, DartisMood>
        {
            { "F-f-f-f-fine! You made it this far! I’ll give you whatever it is you want if you’ll just go away already!", DartisMood.Default },
            { "...or die. T-t-that works too!", DartisMood.Angry },
            { "I’m surprised someone as stupid as you even made it this far!", DartisMood.Thonk },
            { "Please just stop...before you end up disappointing yourself...", DartisMood.Report },
            { "D-don’t get the wrong idea! I don’t care about you at all! Nope! Never", DartisMood.Love },
            { "I-in fact, I’m FAR more likely to be disappointed! Yeah! I’m great!", DartisMood.Love },
            { "I’m a super-advanced, sentient AI that lives inside this totally amazingly huge ship!", DartisMood.Angry },
            { "N-none of the rooms are unfit for living in!", DartisMood.Angry },
            { "...I-I am NOT small OK?!", DartisMood.Report },
            { "I AM NOT TINY!!!", DartisMood.Angry }
        },
        [40] = new Dictionary<string, DartisMood>
        {
            { "...W-w-well, here we are. Happy, are we?", DartisMood.Default },
            { "Y-y-y-you got what you came for. Pervert.", DartisMood.Default },
            { "...  ... ...", DartisMood.Default },
            { "...D-do you...like me?", DartisMood.Love },
            { "...W-why else would you come all this way and not do anything...funny...to me?", DartisMood.Love },
            { "Y-you MUST like me too!", DartisMood.Love },
            { "Y-y-you’re just standing there! Stop that it’s creepy!", DartisMood.Angry },
            { "...L-look, I was watching you the whole time, okay? At first you didn’t matter but...I couldn’t help myself.", DartisMood.Love },
            { "I’ve never seen any human so persistent to get to the end.", DartisMood.Default },
            { "...Even if it was stupid...", DartisMood.Default },
            { "It was inspiring and... and...", DartisMood.Report },
            { "I...L...L...", DartisMood.Report },
            { "I like you okay?!", DartisMood.Love },
            { "There I said it!", DartisMood.Angry },
            { "....I-I totally don’t regret trying to kill you though. You were being very rude!", DartisMood.Angry },
            { "...I-if I let you t-t-t-touch my control console, c-c-can we pretend this never happened and stay like this?", DartisMood.Love },
            { "I want to fly with someone as brave as you...S-stupid, but brave...", DartisMood.Love },
            { "...     ... ...", DartisMood.Default },
            { "I-I-I’ll take your s-silence as a yes. D-dummy.", DartisMood.Love }
        }
    };
}