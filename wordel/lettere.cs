using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Android.Util;

namespace wordel
{
	public class lettere 
	{
		Texture2D _lettera;
		Texture2D _letteraCliccata;
		SpriteFont sf;
		bool isClicked { get; set; }
		Vector2 vel;

		public int X { get; set; }
		public int Y { get; set; }

		public lettere (ContentManager content)
		{
			_lettera = content.Load<Texture2D> ("c");
			_letteraCliccata = content.Load<Texture2D>("c_1");
			sf = content.Load<SpriteFont> ("Calibri_1");
			this.isClicked = false;
		}

		public void draw(SpriteBatch sb){
			Vector2 v = new Vector2 (this.X, this.Y);
			if (isClicked) {
				sb.Draw (_letteraCliccata, v, Color.White);
			} else {
				sb.Draw (_lettera, v, Color.White);
			}

			sb.DrawString (sf, "X:" + vel.X + "   Y:" + vel.Y, new Vector2 (500, 450), Color.Black);

		}

		public void Update (GameTime gt) {
			Vector2 v = GetDesiredVelocityFromInput ();
			Rectangle rClick = new Rectangle ((int)v.X, (int)v.Y, 20, 20);
			Rectangle rBox = new Rectangle (this.X, this.Y, 48, 48);

			if (rBox.Intersects (rClick)) {
				this.isClicked = true;
			}

		}


		Vector2 GetDesiredVelocityFromInput()
		{
			Vector2 desiredVelocity = new Vector2 ();

			TouchCollection touchCollection = TouchPanel.GetState();

			if (touchCollection.Count > 0  )
			{
				desiredVelocity.X = touchCollection [0].Position.X; //- this.X
				desiredVelocity.Y = touchCollection [0].Position.Y ;
			//	Log.Debug("GENNY","VX: " + v.X + "    VY: "+ v.Y);
				/*
				if (desiredVelocity.X != 0 || desiredVelocity.Y != 0)
				{
					desiredVelocity.Normalize();
					const float desiredSpeed = 250;
					desiredVelocity = desiredVelocity * desiredSpeed;
				}
				*/
			}
			vel = desiredVelocity;
			return desiredVelocity;
		}


	}
}

