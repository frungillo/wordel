#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

#endregion

namespace wordel
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Texture2D scenario;
		SoundEffect boom;
		lettere l_1, l_2;
		int xVectorScenario ;
		Vector2 vectorScenario ;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = true;	

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			scenario = Content.Load<Texture2D> ("scenario_1");
			boom = Content.Load<SoundEffect> ("fx_1");
			l_1 = new lettere (Content);
			l_2 = new lettere (Content);

			xVectorScenario = 250;
			vectorScenario = new Vector2 (xVectorScenario, 20);
			l_1.X = 250; l_1.Y = 20;
			l_2.X = 299; l_2.Y = 20;

			base.Initialize ();
				
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			//TODO: use this.Content to load your game content here 
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState ().IsKeyDown (Keys.Escape)) {
				Exit ();
			}
			#endif
			// TODO: Add your update logic here
			l_1.Update (gameTime);
			l_2.Update (gameTime);

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
			SpriteFont sf = Content.Load<SpriteFont> ("Calibri_1");

			/*Inizio il disegno*/
			spriteBatch.Begin ();

			spriteBatch.Draw (scenario, vectorScenario, Color.White);

			spriteBatch.DrawString (sf, "X:" + l_2.X + "   Y:" + l_2.Y, new Vector2 (500, 500), Color.Black);

			l_1.draw (spriteBatch);
			l_2.draw (spriteBatch);

			spriteBatch.End ();
            /*Fine disegno*/
			base.Draw (gameTime);
		}
	}
}

