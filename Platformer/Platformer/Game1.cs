﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;

namespace Platformer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Location slimeCity, testArea, slimeHut;
        private static List<Menu> menuList;
        private static Player player;
        private int screenGridWidth, screenGridHeight, screenWidth, screenHeight;
        public static int time;
        private SpriteFont font;
        KeyboardState previousKeyboardState;
        private OrthographicCamera camera;
        private PlayerInfoBar playerInfoBar;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            screenWidth = (int)(graphics.PreferredBackBufferWidth);
            screenHeight = (int)(graphics.PreferredBackBufferHeight);
            screenGridWidth = (int)(graphics.PreferredBackBufferWidth / 50) + 1;
            screenGridHeight = (int)(graphics.PreferredBackBufferHeight / 50) + 1;

            player = new Player(new Vector2(1050, 1100), screenWidth, screenHeight);
            testArea = new Locations.TestArea(player, screenGridWidth, screenGridHeight, screenWidth, screenHeight, GraphicsDevice);
            slimeCity = new Locations.SlimeCity(player, screenGridWidth, screenGridHeight, screenWidth, screenHeight, GraphicsDevice);
            slimeHut = new Locations.SlimeHut(player, screenGridWidth, screenGridHeight, screenWidth, screenHeight, GraphicsDevice);
            testArea.AddPortals();
            slimeCity.AddPortals();
            slimeHut.AddPortals();
            player.Travel(slimeCity, new Vector2(1050, 1100));

            menuList = new List<Menu>();

            IsMouseVisible = true;

            playerInfoBar = new PlayerInfoBar(player, screenWidth, screenHeight);




            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ItemManager.RegisterItems();

            player.AddToInventory(new Items.HealthPotion(3));
            player.AddToInventory(new Items.ManaPotion(3));
            player.AddToInventory(new Items.SwordItem(1));
            player.AddToInventory(new Items.ScytheItem(1));

            previousKeyboardState = Keyboard.GetState();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1280, 720);
            camera = new OrthographicCamera(viewportAdapter);

            // TODO: use this.Content to load your game content here

            Player.LoadTextures(Content, GraphicsDevice);

            //player.GetCurrentLocation().LoadTextures(Content);
            testArea.LoadTextures(Content);
            slimeCity.LoadTextures(Content);
            slimeHut.LoadTextures(Content);
            InfoBox.LoadTextures(Content);
            InventoryItem.LoadTextures(Content);
            Tiles.Wall.LoadTextures(Content);

            font = Content.Load<SpriteFont>("NPCText");

            ItemManager.LoadTextures(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            
            player.GetCurrentLocation().Update(keyboardState, mouseState, camera);

            if (keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape) && menuList.Count > 0)
            {
                menuList.RemoveAt(menuList.Count - 1);
            }

            for (int i = 0; i < menuList.Count; i++)
            {
                menuList[i].Update(mouseState);
            }

            time++;
            if (time > 2000)
            {
                time = 0;
            }
            previousKeyboardState = keyboardState;
            playerInfoBar.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (time < 1600)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }
            else
            {
                GraphicsDevice.Clear(Color.DarkBlue);
            }

            // TODO: Add your drawing code here

            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
            player.GetCurrentLocation().Draw(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin();
            playerInfoBar.Draw(spriteBatch);
            foreach (Menu menu in menuList)
            {
                menu.Draw(spriteBatch);
            }
            spriteBatch.DrawString(font, "time: " + time, new Vector2(10, 10), Color.White);
            spriteBatch.End();



            base.Draw(gameTime);
        }
        public static void ToggleMenu(Menu menu)
        {
            if (menuList.Contains(menu))
            {
                menuList.Remove(menu);
            }
            else
            {
                menuList.Add(menu);
            }
        }
        public static void BringToFrontOfMenuList(Menu menu)
        {
            menuList.Remove(menu);
            menuList.Add(menu);
        }

        public static Player GetPlayer()
        {
            return player;
        }
    }
}