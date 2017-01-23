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
    public class Card
    {
        Vector2 position;
        Point sourcePos;
        Point size = new Point(72, 96);

        public bool moving = false;
        public int originPile;
        Vector2 movePos;

        Vector2 offset;

        MouseState mouse;

        public string Name { get; set; }
        public Rectangle Bounds
        {
            get
            {
                if (!moving)
                    return new Rectangle((int)position.X, (int)position.Y, size.X, size.Y);
                else
                    return new Rectangle((int)movePos.X, (int)movePos.Y, size.X, size.Y);
            }
        }

        public int Number
        {
            get
            {
                string[] nameParts = Name.Split('-');

                switch (nameParts[0])
                {
                    case "A":
                        return 1;
                        break;

                    case "Jack":
                        return 11;
                        break;

                    case "Queen":
                        return 12;
                        break;

                    case "King":
                        return 13;
                        break;

                    default:
                        return Convert.ToInt32(nameParts[0]);
                        break;
                }
            }
        }

        public string Suit
        {
            get
            {
                string[] nameParts = Name.Split('-');

                return nameParts[1];
            }
        }
        
        public Rectangle SourceBounds
        {
            get
            {
                return new Rectangle(sourcePos.X, sourcePos.Y, size.X, size.Y);
            }
        }

        public Card(string name, Point sourcePos)
        {
            this.sourcePos = sourcePos;
            Name = name;
        }

        public void SetPosition(Vector2 newPos)
        {
            position = newPos;
        }

        public void Move(int originPile)
        {
            mouse = Mouse.GetState();
            moving = true;
            this.originPile = originPile;

            offset = new Vector2(mouse.X - position.X, mouse.Y - position.Y);
        }

        public void Update()
        {
            mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                movePos = new Vector2(mouse.X, mouse.Y) - offset;
            }
            else
            {
                Pile[] piles = Game1.Instance.piles;

                foreach (Pile p in piles)
                {
                    if (p.CanAcceptCard(this))
                    {
                        p.AddCard(piles[originPile].RemoveTopCard());
                    }
                }

                moving = false;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Game1.spritesheet, Bounds, SourceBounds, Color.White);
        }
    }
}
