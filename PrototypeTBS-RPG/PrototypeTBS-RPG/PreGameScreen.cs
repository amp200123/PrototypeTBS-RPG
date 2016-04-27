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
        public GameScreen game { get; private set; }
        public List<Character> selectedChars { get; private set; }

        private Screen currentScreen;
        private MainPreGameScreen menuScreen;
        private SelectCharactersScreen selectScreen;

        public PreGameScreen(ContentManager content, EventHandler screenEvent, GameScreen game)
            : base(screenEvent)
        {
            this.game = game;

            menuScreen = new MainPreGameScreen(content, new EventHandler(MenuScreenEvent));
            selectScreen = new SelectCharactersScreen(content, new EventHandler(SelectedScreenEvent), game.characterLimit);

            currentScreen = menuScreen;
        }

        public override void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            currentScreen.Draw(gameTime, spritebatch);
        }

        private void MenuScreenEvent(object sender, EventArgs e)
        {
            switch (menuScreen.selectedOption)
            {
                case 1:
                    screenEvent.Invoke(this, new EventArgs());
                    break;
                case 2:
                    currentScreen = selectScreen;
                    break;
                case 3:
                    game = null;
                    screenEvent.Invoke(this, new EventArgs());
                    break;
            }
        }

        private void SelectedScreenEvent(object sender, EventArgs e)
        {
            currentScreen = menuScreen;
        }
    }
}
