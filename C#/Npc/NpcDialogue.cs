using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcDialogue : Node
    {

        [Export]
        public AudioStream dialogueAudio;
        [Export]
        public string dialogueText;
        [Export]
        public bool repeat = false;
    }
}