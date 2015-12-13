using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Android.Util;
using Microsoft.Xna.Framework.Audio;

namespace wordel
{
	public class lettere 
	{
		Texture2D _lettera;
		Texture2D _letteraCliccata;
		//SpriteFont sf;
		SoundEffect boom;
		SoundEffectInstance sfx;
		public bool isClicked { get; set; }
		private string _nomeLettera;
		Vector2 vel;

		public int X { get; set; }
		public int Y { get; set; }

		private bool _annullata;

		public bool Annullata {
			get {
				return _annullata;
			}
			set {
				_annullata = value;
			}
		}

		public string NomeLettera {
			get{
				return _nomeLettera;
			}
		}

		public lettere (ContentManager content,string lettera)
		{
			_nomeLettera = lettera;
			_lettera = content.Load<Texture2D> ("wordel\\"+lettera);
			_letteraCliccata = content.Load<Texture2D>("wordel\\"+lettera+"_");
			boom = content.Load<SoundEffect> ("fx_1");
			sfx = boom.CreateInstance ();
			//sf = content.Load<SpriteFont> ("Calibri_1");
			this.isClicked = false;
			_annullata = false;

		}

		public void draw(SpriteBatch sb){
			Vector2 v = new Vector2 (this.X, this.Y);
			if (isClicked) {
				sb.Draw (_letteraCliccata, v, Color.White);
			} else {
				sb.Draw (_lettera, v, Color.White);
			}

			//sb.DrawString (sf, "X:" + vel.X + "   Y:" + vel.Y, new Vector2 (500, 450), Color.Black);

		}

		public void Update (GameTime gt) {
			Vector2 v = GetDesiredVelocityFromInput ();
			Rectangle rClick = new Rectangle ((int)v.X, (int)v.Y, 20, 20);
			Rectangle rBox = new Rectangle (this.X, this.Y, 70, 70);

			if (rBox.Intersects (rClick)) {
				if (sfx.State == SoundState.Stopped)
					sfx.Play ();
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

		public override bool Equals (Object obj)
		{
			var l = obj as lettere;
			if (l.X == this.X && l.Y == this.Y && l.NomeLettera == this.NomeLettera) {
				return true;
			} else { 
				return false;
			}
			
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}


	}
}

