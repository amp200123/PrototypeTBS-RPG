using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeTBS_RPG
{
    class PreGameScreen : Screen
    {
        public List<Character> selectedChars { get; private set; }

        private Screen currentScreen;
        private SelectCharactersScreen selectScreen;

        private GameScreen game;

        public PreGameScreen(ContentManager content, EventHandler screenEvent, GameScreen game)
            : base(screenEvent)
        {
            this.game = game;

            selectScreen = new SelectCharactersScreen(content, new EventHandler(SelectedScreenEvent));
        }

        public override void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            currentScreen.Draw(gameTime, spritebatch);
        }

        private void SelectedScreenEvent(object sender, EventArgs e)
        {

        }
    }
}
