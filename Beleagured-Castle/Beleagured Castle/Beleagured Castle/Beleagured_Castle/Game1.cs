using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Beleagured_Castle
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Texture2D spritesheet;

        public Pile[] piles;

        List<Card> deck;

        Random rand;

        public static Game1 Instance;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            Instance = this;
        }

        protected override void Initialize()
        {
            piles = new Pile[12];
            deck = new List<Card>();

            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;
            rand = new Random();

            for (int i = 0; i < piles.Length; i++)
            {
                if (i < 4)
                {
                    piles[i] = new Pile(new Point(width / 2 - 128, height / 4 * i + 15), false, i);
                }
                else if (i < 8)
                {
                    piles[i] = new Pile(new Point(width / 2 + 56, height / 4 * (i - 4) + 15), false, i);
                }
                else
                {
                    piles[i] = new Pile(new Point(width / 2 - 36, height / 4 * (i - 8) + 15), true, i);
                }
            }

            base.Initialize();

            foreach (Pile p in piles)
            {
                while (p.CardCount() < 6 && !p.middle)
                {
                    int index = rand.Next(deck.Count);
                    p.AddCard(deck[index]);
                    deck.RemoveAt(index);
                }
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadCards();

            spritesheet = Content.Load<Texture2D>("card_sheet");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            foreach (Pile p in piles)
            {
                p.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LimeGreen);

            spriteBatch.Begin();
            foreach (Pile p in piles)
            {
                p.Cards().ForEach(c => c.Draw(spriteBatch));
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void LoadCards()
        {
            try
            {
                using (StreamReader reader = new StreamReader(@"Content/card_sheet.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] parts = line.Split(' ');

                        if (parts[0].Contains('A'))
                        {
                            if (parts[0].Contains("club")) piles[8].AddCard(new Card(parts[0], new Point(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]))));
                            if (parts[0].Contains("diamond")) piles[9].AddCard(new Card(parts[0], new Point(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]))));
                            if (parts[0].Contains("heart")) piles[10].AddCard(new Card(parts[0], new Point(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]))));
                            if (parts[0].Contains("spade")) piles[11].AddCard(new Card(parts[0], new Point(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]))));
                            continue;
                        }

                        deck.Add(new Card(parts[0], new Point(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]))));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
