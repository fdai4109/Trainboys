using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace PinballProjekt.Game1.BoxCollider
{
    class BoxCollider : Game1
    {
        public Vector2 Size { get; set;  }
        public Vector2 Offset { get; set; }
        public Game1 Parent { get; set; }
        #region Constructors
        /// <summary>
        /// Creates a new BoxCollider
        /// </summary>
        /// <param name="parent">The GameObject of which the BoxCollider will be placed on</param>
        /// <param name="offset">How the BoxCollider should be offset from the GameObject point of origin</param>
        /// <param name="size">The dimensions of the BoxCollider</param>
        
        public BoxCollider (Game1 parent, Vector2 offset, Vector2 size)
            : base(parent, "BoxCollider")
        {
            Size = size;
            Offset = offset;
            Parent = parent;
        }
        #endregion


        public int LinkeKante()
        {
            return (int)Math.Round(Parent.Position.X + Offset.X - (Size.X * 0.5));
        }

        public int RechteKante()
        {
            return (int)Math.Round(Parent.Position.X + Offset.X + (Size.X * 0.5));
        }

        public int ObereKante()
        {
            return (int)Math.Round(Parent.Position.Y + Offset.Y - (Size.Y * 0.5));
        }
        public int UntereKante()
        {
            return (int)Math.Round(Parent.Position.Y + Offset.Y + (Size.Y * 0.5));
        }
    }

}
