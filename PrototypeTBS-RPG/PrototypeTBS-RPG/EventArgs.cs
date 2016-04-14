using PrototypeTBS_RPG.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    class ProfileEventArgs : EventArgs
    {
        public Character character { get; private set; }

        public ProfileEventArgs(Character character)
        {
            this.character = character;
        }
    }

    class LevelUpEventArgs : EventArgs
    {
        public Character character { get; private set; }

        public LevelUpEventArgs(Character character)
        {
            this.character = character;
        }
    }
}
