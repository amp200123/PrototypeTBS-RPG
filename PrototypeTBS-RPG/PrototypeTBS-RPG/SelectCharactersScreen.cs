using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    class SelectCharactersScreen : Screen
    {
        public List<Character> selectedChars { get; private set; }

        private List<CharacterBar> characterBars;

        public SelectCharactersScreen(ContentManager content, EventHandler screenEvent)
            : base(screenEvent)
        {
            selectedChars = new List<Character>();
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {

        }
    }
}
