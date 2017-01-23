using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Beleagured_Castle
{
    public class Pile
    {
        List<Card> cards;
        Point position;
        public bool middle;
        int cardSpacing = 15;

        MouseState mouse;
        MouseState oldMouse;

        public int PileId { get; set; }

        public Pile(Point position, bool middle, int pileId)
        {
            this.position = position;
            this.middle = middle;

            PileId = pileId;

            cards = new List<Card>();

            mouse = Mouse.GetState();
        }

        public void AddCard(Card c)
        {
            int middleScreen = Game1.Instance.GraphicsDevice.Viewport.Width / 2;

            if (position.X < middleScreen && !middle)
            {
                c.SetPosition(new Vector2(position.X - (cards.Count * cardSpacing), position.Y));
            }
            else if (middle)
            {
                c.SetPosition(new Vector2(position.X, position.Y));
            }
            else
            {
                c.SetPosition(new Vector2(position.X + (cards.Count * cardSpacing), position.Y));
            }

            cards.Add(c);
        }

        public int CardCount()
        {
            return cards.Count;
        }

        public List<Card> Cards()
        {
            return cards;
        }

        public Card GetTopCard()
        {
            return cards[cards.Count - 1];
        }

        public Card RemoveTopCard()
        {
            Card c = GetTopCard();
            cards.RemoveAt(cards.Count - 1);
            return c;
        }

        public bool CanAcceptCard(Card c)
        {
            int cNum = c.Number;
            int topNum = GetTopCard().Number;
            string cSuit = c.Suit;
            string topSuit = GetTopCard().Suit;

            if (GetTopCard().Bounds.Contains(c.Bounds.Center))
            {
                if (middle)
                {
                    if (cNum == topNum + 1 && cSuit.Equals(topSuit))
                        return true;
                }
                else
                {
                    if (cNum + 1 == topNum || cards.Count == 0)
                        return true;
                }
            }

            return false;
        }

        public void Update()
        {
            oldMouse = mouse;
            mouse = Mouse.GetState();

            Card c = GetTopCard();

            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton != ButtonState.Pressed && !middle)
            {
                if (c.Bounds.Contains(new Point(mouse.X, mouse.Y)))
                {
                    c.Move(PileId);
                }
            }

            if (c.moving) c.Update();
        }
    }
}
