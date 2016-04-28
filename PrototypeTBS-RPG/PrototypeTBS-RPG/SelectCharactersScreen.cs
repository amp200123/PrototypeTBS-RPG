using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private int characterLimit;
        private bool movingScreen;

        private MouseState oldMouseState = Mouse.GetState();

        public SelectCharactersScreen(ContentManager content, EventHandler screenEvent, int characterLimit)
            : base(screenEvent)
        {
            this.characterLimit = characterLimit;

            selectedChars = new List<Character>();
            characterBars = new List<CharacterBar>();

            foreach (Character ch in Game1.playerParty)
            {
                characterBars.Add(new CharacterBar(content, ch));
            }

            for (int i = characterBars.Count; i < 12; i++)
            {
                characterBars.Add(new CharacterBar(content));
            }

            characterBars.Add(new CharacterBar(content));
            characterBars.Add(new CharacterBar(content));
            characterBars.Add(new CharacterBar(content));

            for (int i = 0; i < characterBars.Count; i++)
            {
                int row = (int)Math.Floor(i / 3f);
                int column = i % 3;

                int width = new CharacterBar(content).texture.Width;
                int height = new CharacterBar(content).texture.Height;

                characterBars[i].position = new Vector2(column *  width + width / 2, row * height + height / 2);
            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();

            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released
                && !movingScreen)
            {
                foreach (CharacterBar bar in characterBars)
                {
                    if (bar.character != null && bar.boundingRectangle.Contains(newMouseState.Position))
                    {
                        if (bar.selected)
                            bar.selected = false;
                        else if (selectedChars.Count < characterLimit)
                            bar.selected = true;

                        if (bar.selected)
                            selectedChars.Add(bar.character);
                        else selectedChars.Remove(bar.character);
                    }
                }
            }

            if (newMouseState.Position.X <= Game1.WINDOW_WIDTH && newMouseState.Position.X >= 0 &&
                newMouseState.Position.Y <= Game1.WINDOW_HEIGHT && newMouseState.Position.Y >= 0 &&
                newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed &&
                newMouseState.Position.Y != oldMouseState.Position.Y)
            {
                movingScreen = true;

                float movement = newMouseState.Position.Y - oldMouseState.Position.Y;
                
                if ((movement > 0 && characterBars[0].boundingRectangle.Top < 0) ||
                    (movement < 0 && characterBars[characterBars.Count - 1].boundingRectangle.Bottom > Game1.WINDOW_HEIGHT)) 
                {
                    if (characterBars[0].boundingRectangle.Top + movement > 0)
                        movement = -characterBars[0].boundingRectangle.Top;
                    else if (characterBars[characterBars.Count - 1].boundingRectangle.Bottom + movement < Game1.WINDOW_HEIGHT)
                        movement = Game1.WINDOW_HEIGHT - characterBars[characterBars.Count - 1].boundingRectangle.Bottom;

                    foreach (CharacterBar bar in characterBars)
                    {
                        bar.position.Y += movement;
                    }
                }
            }

            if (newMouseState.LeftButton == ButtonState.Released)
                movingScreen = false;

            if (newMouseState.RightButton == ButtonState.Pressed)
                screenEvent.Invoke(this, new EventArgs());

            oldMouseState = newMouseState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            foreach (CharacterBar bar in characterBars) 
            {
                bar.Draw(spritebatch);
            }
        }
    }
}
