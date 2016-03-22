using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeTBS_RPG
{
    abstract class Screen
    {
        protected EventHandler screenEvent;

        public Screen(EventHandler screenEvent)
        {
            this.screenEvent = screenEvent;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gametime">Provides a snapshot of timing values.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="spritebatch">Spritebatch object to draw objects with</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spritebatch);
    }
}
