﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Platformer.Enemies
{
    class SlimeDrip : Slime
    {
        private static Texture2D regularTexture, hurtTexture;

        public SlimeDrip(Vector2 location, Player player, Location currentLocation) : base(currentLocation)
        {
            this.location = location;
            this.currentLocation = currentLocation;
            this.player = player;
            jumpCooldown = 20;
            width = 40;
            height = 40;
            hitBox = new Rectangle((int)location.X, (int)location.Y, width, height);
            health = 40;
            drops.Add(new Items.SlimeItem());
            jumpHeight = .5f;
        }
        override public Enemy Create(Vector2 location, Location currentLocation)
        {
            return new SlimeDrip(location, player, currentLocation);
        }
        public static void LoadTextures(ContentManager content)
        {
            regularTexture = content.Load<Texture2D>("drip");
            hurtTexture = content.Load<Texture2D>("drip-hurt");
        }

        override public void Draw(SpriteBatch spriteBatch, int offsetX, int offsetY)
        {
            Rectangle sourceRectangle = new Rectangle(currentFrame * 40, 0, 40, 40);
            Texture2D texture = regularTexture;
            if (this.state == "hurt")
            {
                texture = hurtTexture;
            }
            spriteBatch.Draw(texture, new Vector2(location.X - offsetX, location.Y - offsetY), sourceRectangle, Color.White);
        }
    }
}