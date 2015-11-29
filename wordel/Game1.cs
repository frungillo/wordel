#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input.Touch;

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

		List<lettere> listaLettereInGriglia, listaLettereSelezionate;
		int xVectorScenario ;
		Vector2 vectorScenario ;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = true;	

		}

		private void GeneraListaLettere()
		{
			int letteraX = 250;
			int letteraY = 20;
			Random r = new Random ();
			for (int i = 0; i < 100; i++) {
				char nomeLettera = (char)r.Next (65, 90);
				lettere letteraDaAggiungereAllaLista = new lettere (this.Content, nomeLettera.ToString ().ToLower ());
				letteraDaAggiungereAllaLista.X = letteraX;
				letteraDaAggiungereAllaLista.Y = letteraY;
				listaLettereInGriglia.Add (letteraDaAggiungereAllaLista);
				letteraX += 49;
				if (letteraX > 691) {
					letteraY += 49;
					letteraX = 250;
				}
			}
		}

		/* Lettere maiuscole da 65 a 90*/
		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			scenario = Content.Load<Texture2D> ("scenario_1");


			listaLettereInGriglia= new List<lettere>();
			listaLettereSelezionate = new List<lettere> ();
			GeneraListaLettere ();
			xVectorScenario = 250;
			vectorScenario = new Vector2 (xVectorScenario, 20);


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
			if (GetInput ()) {
				foreach (lettere item in listaLettereInGriglia) {
					System.Threading.Thread.Sleep (7);
					item.isClicked = false;
				}
				listaLettereSelezionate.Clear ();
			}

			foreach (lettere item in listaLettereInGriglia) {
				if (item.isClicked) {
					if (!listaLettereSelezionate.Contains (item))
						listaLettereSelezionate.Add (item);
				}
				item.Update (gameTime);
			}

			

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.LightGray);
			SpriteFont sf = Content.Load<SpriteFont> ("Calibri_1");

			/*Inizio il disegno*/
			spriteBatch.Begin ();

			spriteBatch.Draw (scenario, vectorScenario, Color.White);

			foreach (lettere item in listaLettereInGriglia) {
				item.draw (spriteBatch);
			}
			if (listaLettereSelezionate.Count > 0) {
				string parola="";
				foreach (lettere item in listaLettereSelezionate) {
					parola += item.NomeLettera;
				}
				spriteBatch.DrawString (sf, "Parola: " + parola, new Vector2 (500, 500), Color.Black);
			}

			spriteBatch.End ();
            /*Fine disegno*/
			base.Draw (gameTime);
		}

		bool GetInput()
		{
			bool isUp = false;
			//Vector2 desiredVelocity = new Vector2 ();

			TouchCollection touchCollection = TouchPanel.GetState();

			if (touchCollection.Count > 0 && touchCollection[0].State == TouchLocationState.Released )
			{
				isUp = true;
			}

			return isUp;
		}

	}
}

